using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchy
{
    public class DataBreakout
    {
        private const int Digits = 4;

        public double RangeMinValue { get; set; }
        public double RangeMaxValue { get; set; }
        public int Occurences { get; set; }

        public string GetText()
        {
            return string.Format("{0} - {1} : {2}", Math.Round(RangeMinValue, Digits), Math.Round(RangeMaxValue, Digits), Occurences);
        }

        public static DataBreakout[] GetBreakouts(double[] input, int countOfBreakouts)
        {
            var min = input.Min();
            var max = input.Max();
            var width = ((max - min) + 1)/countOfBreakouts;

            var l = new List<DataBreakout>();
            for (var i = countOfBreakouts; i > 0; i--)
            {
                var line = new DataBreakout
                    {
                        RangeMaxValue = (width * i) > max ? max : (width * i),
                        RangeMinValue = (width * (i - 1)) < min ? min : (width * (i - 1))
                    };
                line.Occurences = input.Count(m => m <= line.RangeMaxValue && m >= line.RangeMinValue);
                l.Insert(0, line);
            }
            return l.ToArray();
        }
    }
}
