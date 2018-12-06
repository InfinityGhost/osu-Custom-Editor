using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace osu__Custom_Editor_v2.Tools
{
    public class Images
    {
        public static ImageSource GetSource(string value) => new ImageSourceConverter().ConvertFromString(value) as ImageSource;

        public static string GetPath(Image source) => ((BitmapFrame)source.Source).Decoder.ToString();
    }
}
