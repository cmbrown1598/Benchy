using System.Collections.Generic;

namespace Benchy.Framework
{
    interface IFixture
    {
        void Setup();

        IEnumerable<IBenchmarkTest> BenchmarkTests { get; }

        void Teardown();
    }
}