namespace Benchy
{
    /// <summary>
    /// The scope of the method's execution.
    /// </summary>
    public enum ExecutionScope
    {
        /// <summary>
        /// The method will run once for each benchmark test pass.
        /// </summary>
        OncePerPass,
        /// <summary>
        /// The method will run once for each benchmark test in the fixture.
        /// </summary>
        OncePerMethod,
        /// <summary>
        /// The method will run once for all Benchmark tests in the the fixture.
        /// </summary>
        OncePerFixture
    }
}