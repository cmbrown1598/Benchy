using System.IO;
using Benchy.Framework;

namespace Benchy.Runner
{
    internal class FileLogger : Logger
    {
        public string FilePath { get; set; }
        private TextWriter _textWriter;
        private bool _created = false;

        public FileLogger(string filePath, LogLevel loggingStrategy)
            : base(loggingStrategy)
        {
            FilePath = filePath;
        }

        protected override void Write(string text)
        {
            if (!_created)
            {
                File.Delete(FilePath);
                _textWriter = File.CreateText(FilePath);
                _created = true;
            }
            _textWriter.WriteLine(text);
            _textWriter.Flush();
        }
        protected override void Dispose(bool disposing)
        {
            _textWriter.Close();
            _textWriter.Dispose();
            _textWriter = null;
            base.Dispose(disposing);
        }
    }
}