using System;

namespace Benchy.Runner
{
    internal class ConsoleLogger : Logger
    {
        public ConsoleLogger(LogLevel loggingStrategy) : base(loggingStrategy)
        {
        }

        protected override void Write(string text)
        {
            Console.WriteLine(text);
        }
    }
}