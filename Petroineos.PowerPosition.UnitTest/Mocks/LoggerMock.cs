using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace Petroineos.PowerPosition.Tests.Mocks
{
    public interface ILoggerMock<T> : ILoggerMock, ILogger<T>
    {

    }

    public interface ILoggerMock : ILogger, IDisposable
    {
        List<string> DebugLog { get; } 
        List<string> InfoLog { get; } 
        List<string> ErrorLog { get; } 
        List<Exception> ExceptionLog { get; } 
        List<LogMessage> LogMessages { get; }
    }


    public class LogMessage
    {
        public LogMessage() { }
        public LogMessage(
                 LogLevel logLevel,
                 EventId eventId,
                 Type type,
                 Exception exception,
                 string Message)
        {
            this.LogLevel = logLevel;
            this.EventId = eventId;
            this.Type = type;
            this.Exception = exception;
            this.Message = Message;
        }
        public LogLevel LogLevel { get; set; }
        public EventId EventId { get; set; }
        public Exception Exception { get; set; } 
        public Type Type { get; set; }
        public string Message { get; set; }

    }
      
    public class LoggerMock<T> : LoggerMock, ILoggerMock<T>
    {
        public LoggerMock() : base()
        {

        } 
    } 

    public class LoggerMock : ILoggerMock
    {
        
        public void Dispose()
        { }
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public LoggerMock()
        {
            this.LogMessages = new List<LogMessage>();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            this.LogMessages.Add(new LogMessage
            {
                LogLevel = logLevel,
                EventId = eventId,
                Type = typeof(TState),
                Exception = exception,
                Message = formatter(state, exception)
            }); 
        }

        public List<LogMessage> LogMessages { get; private set; }


        public List<string> DebugLog
        {
            get
            {
                return this.LogMessages.Where(a => a.LogLevel == LogLevel.Debug).Select(s => s.Message).ToList<string>();
            }
        }
         
        public List<string> InfoLog
        {
            get
            {
                return this.LogMessages.Where(a => a.LogLevel == LogLevel.Information).Select(s => s.Message).ToList<string>();
            }
        }
         
        public List<string> ErrorLog
        {
            get
            {
                return this.LogMessages.Where(a => a.LogLevel == LogLevel.Error).Select(s => s.Message).ToList<string>();
            }
        }

        public List<Exception> ExceptionLog
        {
            get
            {
                return this.LogMessages.Where(a => a.LogLevel == LogLevel.Error).Select(s => s.Exception).ToList<Exception>();
            }
        } 

    }
}
