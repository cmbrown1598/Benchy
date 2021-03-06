﻿namespace Benchy.Framework
{
    /// <summary>
    /// Interface that defines a logger
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes to the log.
        /// </summary>
        /// <param name="text">The text to write to the log.</param>
        /// <param name="level">The <see cref="LogLevel" /> represented by this entry.</param>
        void WriteEntry(string text, LogLevel level);
    }
}