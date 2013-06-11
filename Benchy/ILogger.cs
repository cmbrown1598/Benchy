using System;

namespace Benchy
{
    public interface ILogger : IDisposable
    {
        void WriteEntry(string text, LogLevel level);
    }
}