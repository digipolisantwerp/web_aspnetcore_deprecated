using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Digipolis.Web.UnitTests.Utilities
{
    public class TestLogger<T> : ILogger<T>
    {
        public List<LogMessage> LoggedMessages { get; set; }

        public TestLogger()
        {
            LoggedMessages = new List<LogMessage>();
        }

        public TestLogger(List<LogMessage> loggedMessages)
        {
            LoggedMessages = loggedMessages;
        }

        public IDisposable BeginScopeImpl(object state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            LoggedMessages.Add(new LogMessage { Level = logLevel, Message = state.ToString() });
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LoggedMessages.Add(new LogMessage { Level = logLevel, Message = state.ToString() });
        }

        public static TestLogger<T> CreateLogger()
        {
            return new TestLogger<T>();
        }

        public static TestLogger<T> CreateLogger(List<LogMessage> loggedMessages)
        {
            return new TestLogger<T>(loggedMessages);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }

    public class LogMessage
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }
    }
}
