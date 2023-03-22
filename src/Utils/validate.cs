using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace src.Utils
{
    class validate
    {
        public bool validateData(string fileName)
        {
            List<string> lines = File.ReadAllLines(fileName).ToList();

            int i = 0;
            int totalElement = 0, totalStart = 0, totalTreasures = 0;

            foreach (var line in lines)
            {
                string[] elements = line.Trim().Split(' ');

                int j = 0;
                foreach (var element in elements)
                {
                    if (element != "K" && element != "R" && element != "X" && element != "T")
                    {
                        return false;
                    }

                    if (element == "K")
                    {
                        totalStart++;
                        if (totalStart > 1)
                        {
                            return false;
                        }
                    }
                    if (element == "T")
                    {
                        totalTreasures++;
                    }
                    totalElement++;
                }
                i++;
            }

            if (totalStart < 1 || totalElement < 2 || totalTreasures < 1)
            {
                return false;
            }
            return true;
        }
    }
}
