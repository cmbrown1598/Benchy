using System;

namespace Benchy
{
    public abstract class Logger : ILogger
    {
        private readonly LogLevel _loggingStrategy;

        protected Logger(LogLevel loggingStrategy)
        {
            _loggingStrategy = loggingStrategy;
        }

        public void WriteEntry(string text, LogLevel level)
        {
            if (_loggingStrategy.HasFlag(level))
            {
                Write(DateTime.Now.ToString("[HH:mm:ss.fffff] ") + text);
            }
        }

        protected abstract void Write(string text);

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing) GC.SuppressFinalize(this);
        }
    }
}