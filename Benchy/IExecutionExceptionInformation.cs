namespace Benchy
{
    public interface IExecutionExceptionInformation
    {
        /// <summary>
        /// The type name of the exception thrown.
        /// </summary>
        string ExceptionTypeName { get; }

        /// <summary>
        /// The number of occurrences the exception was thrown.
        /// </summary>
        int Occurances { get; }
    }
}