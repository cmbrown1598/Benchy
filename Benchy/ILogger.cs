namespace Benchy
{
    public interface ILogger
    {
        void WriteEntry(string text, LogLevel level);
    }
}