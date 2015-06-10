using System;
using System.IO;
using System.Linq;

namespace Benchy.Framework
{
    class ExecutionOptionsValidator : IExecutionOptionsValidator
    {
        public bool Validate(IExecutionOptions options)
        {
            if (options.Files == null)
                throw new ApplicationException("Files not specified.");
            if (options.Files.Length == 0)
                throw new ApplicationException("Files not specified.");
            if (options.Logger == null)
                throw new ApplicationException("Logger not specified.");
            if (options.ResultsFormatter == null)
                throw new ApplicationException("Results formatter not specified.");

            foreach (var file in options.Files.Where(file => !File.Exists(file)))
            {
                throw new FileNotFoundException(string.Format("Assembly file {0} not found.", file));
            }
            return true;
        }
    }
}
