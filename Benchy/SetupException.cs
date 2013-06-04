using System;

namespace Benchy
{
    [Serializable]
    public class SetupException : InvalidOperationException
    {
        public SetupException(Exception innerException)
            : base("An exception was thrown during the call to the Setup method on the Performance test.", innerException)
        {
        }
    }
}
