using System;

namespace Benchy.Internal
{
    interface IBenchmarkTest
    {
        uint ExecutionCount { get; }

        TimeSpan? WarnTime { get; }
        TimeSpan? FailTime { get; }

        string Name { get; }
        string TypeName { get; }
        string Category { get; }

        void Setup();
        void Execute();
        void Teardown();
    }
}
