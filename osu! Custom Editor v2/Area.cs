using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace osu__Custom_Editor_v2
{
    public class Area
    {
        public Area() { }

        public Area(float width, float height)
        {
            Width = width;
            Height = height;
            X = 0;
            Y = 0;
        }

        public Area(float width, float height, float x, float y)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Width of the area.
        /// </summary>
        public double Width { set; get; }

        /// <summary>
        /// Height of the area.
        /// </summary>
        public double Height { set; get; }

        /// <summary>
        /// Horizontal offset of the area.
        /// </summary>
        public double X { set; get; }

        /// <summary>
        /// Vertical offset of the area.
        /// </summary>
        public double Y { set; get; }

        /// <summary>
        /// Returns a Rectangle that represents the area.
        /// </summary>
        /// <returns>System.Windows.Shapes.Rectangle</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle
            {
                Width = Width,
                Height = Height,
            };
        }

    }
}
