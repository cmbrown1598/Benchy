using System;

namespace Benchy.Framework
{
    class ExternalBenchmarkTest : IBenchmarkTest
    {
        public Action PerPassSetupAction { get; set; }
        public Action SetupAction { get; set; }
        public Action ExecuteAction { get; set; }
        public Action TeardownAction { get; set; }
        public Action PerPassTeardownAction { get; set; }

        public bool CollectGarbage { get; set; }
        public uint ExecutionCount { get; set; }
        public string Name { get; set; }
        public TimeSpan? WarnTime { get; set; }
        public TimeSpan? FailTime { get; set; }

        public string Category { get;
            set;
        }

        public bool HasSetup(ExecutionScope scope)
        {
            if (scope == ExecutionScope.OncePerMethod)
            {
                return SetupAction != null;
            }
            return PerPassSetupAction != null;
        }
        public bool HasTeardown(ExecutionScope scope)
        {
            if (scope == ExecutionScope.OncePerMethod)
            {
                return TeardownAction != null;
            }
            return PerPassTeardownAction != null;
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

        public void PerPassSetup()
        {
            PerPassSetupAction();
        }

        public void PerPassTeardown()
        {
            PerPassTeardownAction();
        }
    }
}
