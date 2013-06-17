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

        bool HasSetup(ExecutionScope scope);
        bool HasTeardown(ExecutionScope scope);

        void Setup();
        void Execute();
        void Teardown();
        void PerPassSetup();
        void PerPassTeardown();
    }
}
