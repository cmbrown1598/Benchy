using System.Diagnostics;

namespace Benchy.Framework
{
    /// <summary>
    /// A default logger implementation.
    /// </summary>
    public sealed class Logger : ILogger
    {
        private readonly LogLevel _loggingStrategy;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="loggingStrategy">The level(s) to log.</param>
        public Logger(LogLevel loggingStrategy)
        {
            _loggingStrategy = loggingStrategy;
            if (Trace.Listeners.Count == 1 && Trace.Listeners[0].GetType() == (typeof(DefaultTraceListener)))
                Trace.Listeners.Add(new ConsoleTraceListener());

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
                Trace.WriteLine(text);
            }
        }
        
    }
}