using System;
using Benchy.Attributes;

// ReSharper disable CheckNamespace
namespace Benchy
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attribute indicating the method that gets called after every BenchMark method in the fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TeardownAttribute : Attribute, IBenchyAttribute, IScopedAttribute
    {
        private ExecutionScope _executionScope = ExecutionScope.OncePerMethod;

        /// <summary>
        /// An array representing the parameters to pass to the method.
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// The scope of the teardown method.  Defaults to ExecutionScope.Fixture
        /// 
        /// If set to ExecutionScope.Method, will execute after each Benchmark method test pass completes.
        /// If set to ExecutionScope.Fixture, will execute after all Benchmark methods test pass completes.
        /// </summary>
        public ExecutionScope ExecutionScope
        {
            get { return _executionScope; }
            set { _executionScope = value; }
        }

        /// <summary>
        /// Attribute constructor.
        /// </summary>
        /// <param name="parameters">An array representing the parameters to pass to the method during execution.</param>
        public TeardownAttribute(params object[] parameters)
        {
            Parameters = parameters;
        }

    }
}