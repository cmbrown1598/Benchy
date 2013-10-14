// ReSharper disable CheckNamespace
namespace Benchy.Framework
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Status of a test execution.
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// Status has yet to be determined.
        /// </summary>
        Indeterminate = -1,
        /// <summary>
        /// Test executed successfully.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Test executed, but in a time longer than the warning time specified.
        /// </summary>
        Warning = 10,
        /// <summary>
        /// The test failed, either by throwing an unhandled exception, or by being over the failure time specified.
        /// </summary>
        Failed = 20
    }
}