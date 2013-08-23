using System;

// ReSharper disable CheckNamespace
namespace Benchy.Framework
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Exception that occurs during the Teardown method of a test fixture.
    /// </summary>
    [Serializable]
    public class TeardownException : InvalidOperationException
    {
        /// <summary>
        /// Constructor for the exception
        /// </summary>
        /// <param name="innerException">The inner exception</param>
        public TeardownException(Exception innerException)
            : base("An exception was thrown during the call to the teardown method on the Performance test.", innerException)
        {
        }
    }
}