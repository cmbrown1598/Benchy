using System;

namespace Benchy.Runner
{
    class Program
    {
        static int Main(string[] args)
        {
            // Options and filepaths of assemblies to load.
            var parser = new CommandArgumentParser();
            var options = parser.Parse(args);
            using (var engine = new Engine(options))
            {
                engine.Execute();
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
            return 0;
        }
    }
}
