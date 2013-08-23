using System;
using System.Collections.Generic;
using System.Linq;
using Benchy.Framework;
using CommandLine;
using CommandLine.Text;

namespace Benchy.Runner
{
    class CommandLineOptions
    {
        public static ExecutionOptions GetCommandLineOptions(string[] args)
        {
            var options = new CommandLineOptions();
            if (Parser.Default.ParseArguments(args, options))
            {
                if(!options.AssemblyFiles.Any())
                {
                    
                    Console.WriteLine(options.GetHelpText());

                    return null;
                }

                var logger = string.IsNullOrWhiteSpace(options.LogFileName)
                                 ? new ConsoleLogger(options.LogLevel)
                                 : (ILogger) new FileLogger(options.LogFileName, options.LogLevel);

                
                var exOptions = new ExecutionOptions(
                    (from m in options.AssemblyFiles select m).ToArray(),
                    logger);
                return exOptions;
            }


            Console.WriteLine(options.GetHelpText());

            return null;
        }


        [ValueList(typeof(List<string>), MaximumElements = -1)]
        public IList<string> AssemblyFiles { get; set; }

        [Option('f', DefaultValue = null, HelpText = "File to log to. Logs are written to console if not specified.")]
        public string LogFileName { get; set; }

        [Option('l', DefaultValue = LogLevel.Full, HelpText = "Logging message level.")]
        public LogLevel LogLevel { get; set; }

        public string GetHelpText()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
