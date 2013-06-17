using System.Collections.Generic;

namespace Benchy.Internal
{
    interface IFixture
    {
        void Setup();

        IEnumerable<IBenchmarkTest> BenchmarkTests { get; }

        void Teardown();
    }
}