using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchy.Internal
{
    class DataBreakout : IDataBreakout
    {
        public TimeSpan RangeMinValue { get; set; }
        public TimeSpan RangeMaxValue { get; set; }
        public int Occurences { get; set; }

        public string GetText()
        {
            return string.Format("{0} - {1} : {2}", RangeMinValue, RangeMaxValue, Occurences);
        }

        public static IDataBreakout[] GetBreakouts(TimeSpan[] input, int countOfBreakouts)
        {
            var min = input.Min(m => m.Ticks);
            var max = input.Max(m => m.Ticks);
            var width = ((max - min) + 1)/countOfBreakouts;

            var l = new List<IDataBreakout>();
            for (var i = countOfBreakouts; i > 0; i--)
            {
                var rMax = width*i > max ? max : (width*i);
                var rMin = width*(i - 1) < min ? min : (width*(i - 1));

                var line = new DataBreakout
                    {
                        RangeMaxValue = TimeSpan.FromTicks(rMax),
                        RangeMinValue = TimeSpan.FromTicks(rMin)
                    };
                line.Occurences = input.Count(m => m <= line.RangeMaxValue && m >= line.RangeMinValue);
                l.Insert(0, line);
            }
            return l.ToArray();
        }
    }
}
