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
                Write(text);
            }
        }

        protected abstract void Write(string text);
    }
}