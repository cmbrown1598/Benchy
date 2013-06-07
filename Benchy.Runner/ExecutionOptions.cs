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
        /// Standard constructor.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="logger"></param>
        public ExecutionOptions(string[] files, ILogger logger)
        {
            Files = files;
            Logger = logger;
        }
    }
}
