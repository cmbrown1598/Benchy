using System;

namespace Benchy.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var tg = new TestGetter();
            var  tests =  tg.GetTests(args);
            var runner = new TestRunner(new Logger(LoggingStrategy.Full));
            runner.ExecuteTests(tests);
        
        }
    }
}
