using System;

namespace Benchy
{
    /// <summary>
    /// Indicates the method that should run before every BenchMark method in the fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SetupAttribute : Attribute
    {
        /// <summary>
        /// An array representing the parameters to pass to the method.
        /// </summary>
        public object[] MethodParameters { get; set; }

        public SetupAttribute(params object[] methodParameters)
        {
            MethodParameters = methodParameters;
        }
    }
}