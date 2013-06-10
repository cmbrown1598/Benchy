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

        public static IDataBreakout[] GetBreakouts(TimeSpan[] input)
        {
            var min = input.Min(m => m.Ticks);
            var max = input.Max(m => m.Ticks);
            var countOfBreakouts = Math.Min((input.Count() / 5), 8);

            
            var width = ((max - min) + 1)/countOfBreakouts;
            var l = new List<IDataBreakout>();
            for (var i = 0; i < countOfBreakouts; i++)
            {
                var rMin = min + (width*i);
                var rMax = (i + 1 == countOfBreakouts) ? max : min + (width*(i + 1));
                var line = new DataBreakout
                    {
                        RangeMaxValue = TimeSpan.FromTicks(rMax),
                        RangeMinValue = TimeSpan.FromTicks(rMin)
                    };
                line.Occurences = input.Count(m => m <= line.RangeMaxValue && m >= line.RangeMinValue);
                l.Add(line);
            }
            return l.ToArray();
        }
    }
}
