using System;
using Benchy.Attributes;

// ReSharper disable CheckNamespace
namespace Benchy
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Indicates the method that should run before every BenchMark method in the fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SetupAttribute : Attribute, IBenchyAttribute
    {
        /// <summary>
        /// An array representing the parameters to pass to the method.
        /// </summary>
        public object[] Parameters { get; set; }

        public SetupAttribute(params object[] parameters)
        {
            Parameters = parameters;
        }
    }
}