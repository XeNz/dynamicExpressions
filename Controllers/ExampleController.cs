using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExpressFuncStuff.DataAccess;
using ExpressFuncStuff.Extensions;
using Microsoft.AspNetCore.Mvc;
using ExpressFuncStuff.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace ExpressFuncStuff.Controllers
{
    public class ExampleController : Controller
    {
        private readonly EntityContext _entityContext;

        public ExampleController(EntityContext entityContext)
        {
            _entityContext = entityContext;
        }
        
        public async Task<IActionResult> Expressions()
        {
            // where text == ble || where  id  < 2
            var result = await GetCommentsWithOrWhereClauses();
            var result2 = await GetCommentsWithPredicateBuilder();

            var toReturn = new Dictionary<string, (List<Comment> results, string sqlQuery)>
                {{"ExpressionFuncWay", result}, {"PredicateBuilderWay", result2}};
            return new OkObjectResult(toReturn);
        }

        private async Task<(List<Comment> result, string sqlQuery)> GetCommentsWithPredicateBuilder()
        {
            var query = _entityContext.Comments.AsExpandable();
            var predicate = PredicateBuilder.New<Comment>();

            predicate = predicate.Or(comment => comment.Id < 2);
            predicate = predicate.Or(comment => string.Equals(comment.Text, "ble"));

            query = query.Where(predicate);
            var result = await query.ToListAsync();
            var sqlQueryString = _entityContext.Comments.AsQueryable().Where(predicate).ToSql();
            
            return (result, sqlQueryString);
        }

        private async Task<(List<Comment> result, string sqlQuery)> GetCommentsWithOrWhereClauses()
        {
            var query = _entityContext.Comments.AsQueryable();

            Expression<Func<Comment, bool>> filter = comment => comment.Id < 2;
            Expression<Func<Comment, bool>> filter2 = comment => string.Equals(comment.Text, "ble");

            query = query.Where(OrTheseFiltersTogether(new List<Expression<Func<Comment, bool>>> {filter, filter2}));
            var result = await query.ToListAsync();
            var sqlQueryString = query.ToSql();
            
            return (result, sqlQueryString);
        }

        private static Expression<Func<T, bool>> OrTheseFiltersTogether<T>(
            IEnumerable<Expression<Func<T, bool>>> filters)
        {
            var expressions = filters.ToList();
            var firstFilter = expressions.FirstOrDefault();
            if (firstFilter == null)
            {
                Expression<Func<T, bool>> alwaysTrue = x => true;
                return alwaysTrue;
            }

            var body = firstFilter.Body;
            var param = firstFilter.Parameters.ToArray();
            body = expressions
                .Skip(1)
                .Select(nextFilter => Expression.Invoke(nextFilter, param))
                .Aggregate(body, Expression.OrElse);
            var result = Expression.Lambda<Func<T, bool>>(body, param);
            return result;
        }
    }
}