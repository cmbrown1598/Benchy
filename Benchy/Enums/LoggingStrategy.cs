using System;

namespace Benchy
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Results = 1,
        Setup = 2,
        Teardown = 4,
        Execution = 8,
        Exception = 16,
        FixtureSetup = 32,
        Full = Results | Setup | Teardown | Execution | Exception | FixtureSetup
    }
}
