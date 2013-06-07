using System;

namespace Benchy
{
    public interface IDataBreakout
    {
        TimeSpan RangeMinValue { get; set; }
        TimeSpan RangeMaxValue { get; set; }
        int Occurences { get; set; }
        string GetText();
    }
}