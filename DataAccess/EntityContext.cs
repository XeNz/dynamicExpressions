using ExpressFuncStuff.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ExpressFuncStuff.DataAccess
{
    public class EntityContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        protected EntityContext()
        {
        }

        public EntityContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            _loggerFactory.AddProvider(new TraceLoggerProvider());
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        public DbSet<Comment> Comments { get; set; }
    }
}