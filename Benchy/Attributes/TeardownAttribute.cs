using System;

// ReSharper disable CheckNamespace
namespace Benchy
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attribute indicating the method that gets called after every BenchMark method in the fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TeardownAttribute : Attribute
    {
        /// <summary>
        /// An array representing the parameters to pass to the method.
        /// </summary>
        public object[] MethodParameters { get; set; }

        /// <summary>
        /// Attribute constructor.
        /// </summary>
        /// <param name="methodParameters">An array representing the parameters to pass to the method during execution.</param>
        public TeardownAttribute(params object[] methodParameters)
        {
            MethodParameters = methodParameters;
        }

    }
}