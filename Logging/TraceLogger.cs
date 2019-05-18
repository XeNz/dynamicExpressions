using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ExpressFuncStuff.Logging
{
    public class TraceLogger : ILogger
    {
        private readonly string _categoryName;

        public TraceLogger(string categoryName) => _categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            Trace.WriteLine($"{DateTime.Now.ToString("o")} {logLevel} {eventId.Id} {_categoryName}");
            Trace.WriteLine(formatter(state, exception));
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }

    public class TraceLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new TraceLogger(categoryName);

        public void Dispose()
        {
        }
    }
}