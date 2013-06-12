using System.Collections.Generic;
using System.Reflection;

namespace Benchy.Internal
{
    internal interface ITestBuilder
    {
        IEnumerable<ExternalBenchmarkTest> BuildTests(Assembly assembly);
    }
}