using System;

namespace Benchy
{
    // Why NOT use log4net?
    public class Logger : ILogger
    {
        private readonly LoggingStrategy _loggingStrategy;

        public Logger(LoggingStrategy loggingStrategy)
        {
            _loggingStrategy = loggingStrategy;
        }

        public void WriteEntry(string text, LoggingStrategy level)
        {
            if (_loggingStrategy.HasFlag(level))
            {
                Console.WriteLine(text);
            }
        }
    }
}