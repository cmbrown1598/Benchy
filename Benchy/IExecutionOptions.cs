namespace Benchy
{
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
    }
}