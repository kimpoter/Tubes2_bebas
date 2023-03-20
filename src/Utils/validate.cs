using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace src.Utils
{
    class validate
    {
        public bool validateData(string fileName)
        {
            List<string> lines = File.ReadAllLines(fileName).ToList();

            int i = 0;
            int totalElement = 0, totalStart = 0;

            foreach (var line in lines)
            {
                string[] elements = line.Split(' ');

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
                    totalElement++;
                }

                if (totalElement < 2)
                {
                    return false;
                }

                i++;
            }
            return true;
        }
    }
}
