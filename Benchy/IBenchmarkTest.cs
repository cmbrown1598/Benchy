namespace Benchy
{
    public interface IBenchmarkTest
    {
        string Name { get; }

        /// <summary>
        /// Setup your test here.
        /// </summary>
        void Setup();
        
        /// <summary>
        /// This is the method that is timed.
        /// </summary>
        void Execute();

        /// <summary>
        /// Teardown your setup items here.
        /// </summary>
        void Teardown();
    }
}
