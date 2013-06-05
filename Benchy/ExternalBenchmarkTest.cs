using System;

namespace Benchy
{
    class ExternalBenchmarkTest : IBenchmarkTest
    {
        private readonly Action _doSetup;
        private readonly Action _doExecute;
        private readonly Action _doTeardown;

        public ExternalBenchmarkTest(Action doSetup, Action doExecute, Action doTeardown)
        {
            _doSetup = doSetup;
            _doExecute = doExecute;
            _doTeardown = doTeardown;
        }

        public uint ExecutionCount { get; set; }
        public string Name { get; set; }


        public TimeSpan? WarnBy { get; set; }
        public TimeSpan? FailBy { get; set; }
        
        public void Setup()
        {
            _doSetup();
        }

        public void Execute()
        {
            _doExecute();
        }

        public void Teardown()
        {
            _doTeardown();
        }
    }
}
