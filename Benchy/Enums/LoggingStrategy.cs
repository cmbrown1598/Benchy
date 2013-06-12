using System;

// ReSharper disable CheckNamespace
namespace Benchy
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// The level of detail to log to.
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// No detail.
        /// </summary>
        None = 0,
        /// <summary>
        /// The results of a test execution.
        /// </summary>
        Results = 1,
        /// <summary>
        /// The setup of a test execution.
        /// </summary>
        Setup = 2,
        /// <summary>
        /// The teardown of a test execution
        /// </summary>
        Teardown = 4,
        /// <summary>
        /// The execution itself.
        /// </summary>
        Execution = 8,
        /// <summary>
        /// Exceptions that occur during execution, set and teardown.
        /// </summary>
        Exception = 16,
        /// <summary>
        /// Setup of the fixture itself during test build.
        /// </summary>
        FixtureSetup = 32,
        /// <summary>
        /// All events.
        /// </summary>
        Full = Results | Setup | Teardown | Execution | Exception | FixtureSetup
    }
}
