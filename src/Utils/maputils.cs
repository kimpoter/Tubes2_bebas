using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public static int countVisitedNodes(List<string> steps, int startI, int startJ, int rowCount, int colCount)
        {
            List<List<int>> visitedNodes = new();

            // Populate with 0s
            for (int row = 0; row < rowCount; row++)
            {
                List<int> cols = new();
                for (int col = 0; col < colCount; col++)
                {
                    cols.Add(0);
                }
                visitedNodes.Add(cols);
            }

            // Count times visited for every nodes
            int i = startI, j = startJ, max = 0, currPos = 0;

            foreach (string step in steps)
            {
                if (step == "R" || step == "L" || step == "D" || step == "U" || step == "T")
                {
                    visitedNodes[i][j]++;
                }
                if (visitedNodes[i][j] > max)
                {
                    max = visitedNodes[i][j];
                }

                if (step == "L")
                {
                    j--;
                }
                else if (step == "R")
                {
                    j++;
                }
                else if (step == "U")
                {
                    i--;
                }
                else if (step == "D")
                {
                    i++;
                }
                else if (step == "T")
                {
                    List<string> targetNode = steps[currPos + 1].Split(",").ToList();
                    j = Int32.Parse(targetNode[0]);
                    i = Int32.Parse(targetNode[1]);
                }

                currPos++;
            }

            visitedNodes[i][j]++;
            if (visitedNodes[i][j] > max)
            {
                max = visitedNodes[i][j];
            }

            return max;
        }

        public static List<string> convertStringToList(string s)
        {
            List<string> listOfString = new();
            foreach (char ch in s)
            {
                listOfString.Add(ch.ToString());
            }

            return listOfString;
        }
    }
}
