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
        public IExecutionResultsWriter ResultsWriter { get; set; }


        /// <summary>
        /// Standard constructor.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="logger"></param>
        /// <param name="writer"></param>
        public ExecutionOptions(string[] files, ILogger logger, IExecutionResultsWriter writer = null)
        {
            Files = files;
            Logger = logger;
            ResultsWriter = writer ?? new ExecutionResultsWriter(logger);
        }
    }
}
