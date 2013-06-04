using System;

namespace Benchy
{
    [Flags]
    public enum LoggingStrategy
    {
        None = 0,
        Results = 1,
        Setup = 2,
        Teardown = 4,
        Execution = 8,
        Exception = 16
    }
}
