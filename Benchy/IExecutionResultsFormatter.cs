namespace Benchy.Framework
{
    /// <summary>
    /// A string formatter for an <see cref="IExecutionResults"/> item.
    /// </summary>
    public interface IExecutionResultsFormatter
    {
        /// <summary>
        /// Method which returns a textual result of an <see cref="IExecutionResults"/> instance.
        /// </summary>
        /// <param name="item">An execution test result</param>
        /// <returns>A string representation of that result.</returns>
        string FormatResult(IExecutionResults item);
    }
}