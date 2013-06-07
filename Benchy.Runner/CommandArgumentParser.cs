namespace Benchy.Runner
{
    internal class CommandArgumentParser
    {
        public ExecutionOptions Parse(string[] args)
        {
            return new ExecutionOptions(args, new ConsoleLogger(LogLevel.Full));
        }
    }
}