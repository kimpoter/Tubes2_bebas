using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace src.Utils
{
    class maputils
    {
        public static Point getStartPoint(List<List<string>> map)
        {
            Point start = new Point(0, 0);
            int i = 0;
            foreach (var row in map)
            {
                int j = 0;
                foreach (var col in row)
                {
                    if (col == "K")
                    {
                        return new Point(i, j);
                    }
                    j++;
                }
                i++;
            }
            return start;
        }
    }
}
