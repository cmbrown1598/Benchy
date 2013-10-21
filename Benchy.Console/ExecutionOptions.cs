using Benchy.Framework;

namespace Benchy.Runner
{
    class ExecutionOptions : IExecutionOptions
    {
        /// <summary>
        /// The files to examine / run found tests upon.
        /// </summary>
        public string[] Files { get; set; }
        /// <summary>
        /// The logger to use.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// The results writer to use.
        /// </summary>
        public IExecutionResultsFormatter ResultsFormatter { get; set; }


        /// <summary>
        /// Standard constructor.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="logger"></param>
        public ExecutionOptions(string[] files, 
            ILogger logger = null)
        {
            Files = files;
            Logger = logger ?? new Logger(LogLevel.Full);
            ResultsFormatter = new ExecutionResultsFormatter();
        }
    }
}
