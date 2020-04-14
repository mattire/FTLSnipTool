using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapboard
{
    public static class Utils
    {
        public static Point Add(this Point p1, Point p2)
        {
            return new Point() { X = p1.X + p2.X, Y = p1.Y + p2.Y };
        }

        public static Point Add(params Point[] points)
        {
            return points.Aggregate((p1, p2) => p1.Add(p2));
        }
    }
}
