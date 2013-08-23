using System.Collections.Generic;
using System.Reflection;

namespace Benchy.Framework
{
    internal interface ITestBuilder
    {
        IEnumerable<IFixture> BuildTests(Assembly assembly);
    }
}