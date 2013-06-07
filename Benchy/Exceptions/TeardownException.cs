using System;

namespace Benchy
{
    [Serializable]
    public class TeardownException : InvalidOperationException
    {
        public TeardownException(Exception innerException)
            : base("An exception was thrown during the call to the teardown method on the Performance test.", innerException)
        {
        }
    }
}