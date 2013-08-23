using System;

namespace Benchy.Framework
{
    /// <summary>
    /// Represents a breakdown of TimeSpan values.
    /// </summary>
    public interface IDataBreakout
    {
        /// <summary>
        /// The minimum value of the breakout.
        /// </summary>
        TimeSpan RangeMinValue { get; set; }
        /// <summary>
        /// The maximum value of the breakout.
        /// </summary>
        TimeSpan RangeMaxValue { get; set; }
        /// <summary>
        /// The number of items that fit between that breakout.
        /// </summary>
        int Occurences { get; set; }
        /// <summary>
        /// Represents a way to render it textually.
        /// </summary>
        /// <returns>A string representing the Breakout.</returns>
        string GetText();
    }
}