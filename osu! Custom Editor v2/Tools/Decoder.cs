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
            var a = timestamp.Split(':');
            var times = a.ToList().ConvertAll(e => Convert.ToInt32(e));

            if (a.Length < 3)
                throw new FormatException();

            int totalMS = (times[0] * 60000) + (times[1] * 1000) + times[2];
            return totalMS;
        }
    }
}
