using System;

namespace Benchy.Internal
{
    class ExternalBenchmarkTest : IBenchmarkTest
    {
        public Action SetupAction { get; set; }
        public Action ExecuteAction { get; set; }
        public Action TeardownAction { get; set; }

        public uint ExecutionCount { get; set; }
        public string Name { get; set; }
        public TimeSpan? WarnTime { get; set; }
        public TimeSpan? FailTime { get; set; }

        public string Category { get;
            set;
        }

        public string TypeName { get; set; }

        public void Setup()
        {
            SetupAction();
        }

        public void Execute()
        {
            ExecuteAction();
        }

        public void Teardown()
        {
            TeardownAction();
        }
    }
}
