using System;

namespace Benchy.Runner
{
    /// <summary>
    /// An abstract logger implementation.
    /// </summary>
    public abstract class Logger : ILogger, IDisposable
    {
        private readonly LogLevel _loggingStrategy;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="loggingStrategy">The level(s) to log.</param>
        protected Logger(LogLevel loggingStrategy)
        {
            _loggingStrategy = loggingStrategy;
        }

        /// <summary>
        /// Standard WriteEntry Method.
        /// Note: This method filters calls to the Write method based on the LogLevel passed to the constructor.
        /// </summary>
        /// <param name="text">The log text to write.</param>
        /// <param name="level">The level of the item to log.</param>
        public void WriteEntry(string text, LogLevel level)
        {
            if (_loggingStrategy.HasFlag(level))
            {
                Write(DateTime.Now.ToString("[HH:mm:ss.fffff] ") + text);
            }
        }

        /// <summary>
        /// Method to override.
        /// </summary>
        /// <param name="text">Text to write.</param>
        protected abstract void Write(string text);

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Method to allow implementers to override.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing) GC.SuppressFinalize(this);
        }
    }
}