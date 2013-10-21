using System;

// ReSharper disable CheckNamespace
namespace Benchy.Framework
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Indicates the method that should run before every BenchMark method in the fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SetupAttribute : Attribute, IBenchyAttribute, IScopedAttribute
    {
        private ExecutionScope _executionScope = ExecutionScope.OncePerMethod;

        /// <summary>
        /// An array representing the parameters to pass to the method.
        /// </summary>
        public object[] Parameters { get; set; }


        /// <summary>
        /// The scope of the setup method.  Defaults to ExecutionScope.OnePerMethod
        /// 
        /// If set to ExecutionScope.OnePerMethod, will execute after each Benchmark method test pass completes.
        /// If set to ExecutionScope.Fixture, will execute after all Benchmark methods test pass completes.
        /// </summary>
        public ExecutionScope ExecutionScope
        {
            get { return _executionScope; }
            set { _executionScope = value; }
        }

        public SetupAttribute(params object[] parameters)
        {
            Parameters = parameters;
        }
    }
}