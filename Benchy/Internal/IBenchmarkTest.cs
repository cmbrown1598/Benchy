using System;

namespace Benchy.Internal
{
    interface IBenchmarkTest
    {
        uint ExecutionCount { get; }

        TimeSpan? WarnTime { get; }
        TimeSpan? FailTime { get; }

        string Name { get; }

        void Setup();
        void Execute();
        void Teardown();
    }
}
