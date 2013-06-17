using System;
using System.Collections.Generic;

namespace Benchy.Internal
{
    class ExternalBenchmarkFixture : IFixture
    {
        private readonly ILogger _logger;

        public ExternalBenchmarkFixture(ILogger logger)
        {
            _logger = logger;
        }

        public Action SetupAction { get; set; }
        public Action TeardownAction { get; set; }

        public void Setup()
        {
            if (SetupAction == null) return;

            _logger.WriteEntry("PER FIXTURE SETUP START", LogLevel.Setup);
            SetupAction();
            _logger.WriteEntry("PER FIXTURE SETUP COMPLETE", LogLevel.Setup);
        }

        public IEnumerable<IBenchmarkTest> BenchmarkTests { get; set; }

        public void Teardown()
        {
            if (TeardownAction == null) return;
            _logger.WriteEntry("PER FIXTURE TEARDOWN START", LogLevel.Setup);
            TeardownAction();
            _logger.WriteEntry("PER FIXTURE TEARDOWN COMPLETE", LogLevel.Setup);
        }
    }
}
