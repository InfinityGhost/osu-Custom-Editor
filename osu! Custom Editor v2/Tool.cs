using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace osu__Custom_Editor_v2
{
    public class Tool
    {
        public static Brush StringToBrush(string RGB)
        {
            var owo = RGB.Split(',').ToList().ConvertAll(e => float.Parse(e));
            return RGBToBrush(owo[0], owo[1], owo[2]);
        }

        public static Brush RGBToBrush(float R, float G, float B)
        {
            Color color = Color.FromRgb((byte)R, (byte)G, (byte)B);
            return new BrushConverter().ConvertFrom(color) as Brush;
        }

        public static Brush HexToBrush(string hex)
        {
            return new BrushConverter().ConvertFrom(hex) as Brush;
        }
    }
}
