namespace Benchy.Framework
{
    /// <summary>
    /// Test pass exception information.
    /// </summary>
    class ExecutionExceptionInformation : IExecutionExceptionInformation
    {
        /// <summary>
        /// The type name of the exception thrown.
        /// </summary>
        public string ExceptionTypeName { get; internal set; }
        /// <summary>
        /// The number of occurrences the exception was thrown.
        /// </summary>
        public int Occurances { get; internal set; }
    }
}
