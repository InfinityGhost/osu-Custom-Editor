using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu__Custom_Editor_v2.Tools
{
    public class Decoder
    {
        public static int Timestamp(string timestamp)
        {
            string final = string.Empty;
            if (timestamp.Contains("-"))
            {
                int dash = timestamp.IndexOf('-') - 1;
                final = timestamp.Remove(dash, timestamp.Length - dash);
            }
            if (final.Contains("(") || final.Contains(")"))
            {
                int x = final.IndexOf("(") - 1;
                final = final.Remove(x, final.Length - x);
            }

            var a = final.Split(':');
            var times = a.ToList().ConvertAll(e => Convert.ToInt32(e));

            if (a.Length < 3)
                throw new FormatException();

            int totalMS = (times[0] * 60000) + (times[1] * 1000) + times[2];
            return totalMS;
        }
    }
}
