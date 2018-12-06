using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu__Custom_Editor_v2.Tools
{
    public class OsuCalc
    {
        public static double FadeIn(int ar)
        {
            if (ar < 5)
                return 800 + 400 * (5 - ar) / 5;
            else if (ar == 5)
                return 800;
            else if (ar > 5)
                return 800 - 500 * (ar - 5) / 5;
            else
                throw new ArgumentException();
        }

        public static double Preempt(int ar)
        {
            if (ar < 5)
                return 1200 + 600 * (5 - ar) / 5;
            else if (ar == 5)
                return 1200;
            else if (ar > 5)
                return 1200 - 750 * (ar - 5) / 5;
            else
                throw new ArgumentException();
        }
    }
}
