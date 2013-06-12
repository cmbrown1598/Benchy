using System;

namespace Benchy
{
    /// <summary>
    /// An exception that occurred during the Setup Method on a test.
    /// </summary>
    [Serializable]
    public class SetupException : InvalidOperationException
    {
        /// <summary>
        /// Standard constructor.
        /// </summary>
        /// <param name="innerException">The inner exception</param>
        public SetupException(Exception innerException)
            : base("An exception was thrown during the call to the Setup method on the Performance test.", innerException)
        {
        }
    }
}
