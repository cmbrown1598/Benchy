namespace Benchy
{
    /// <summary>
    /// An interface representing Engine options.
    /// </summary>
    public interface IExecutionOptions
    {
        /// <summary>
        /// The files to examine / run found tests upon.
        /// </summary>
        string[] Files { get; set; }

        /// <summary>
        /// The logger to use.
        /// </summary>
        ILogger Logger { get; set; }

        /// <summary>
        /// The formatter of results to use.
        /// </summary>
        IExecutionResultsFormatter ResultsFormatter { get; set; }
    }
}