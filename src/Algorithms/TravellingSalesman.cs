using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace src.Algorithms
{
    class TravellingSalesman
    {
        // Class for Pair
        class Pair
        {
            public int y;
            public int x;

            public Pair(int y, int x)
            {
                this.y = y;
                this.x = x;
            }
        };

        // Class for path
        class Path
        {
            public Pair point;
            public String path;
            public int distance;

            public Path(Pair point, string path, int distance)
            {
                this.point = point;
                this.path = path;
                this.distance = distance;
            }

            public Path(Pair point, string path)
            {
                this.point = point;
                this.path = path;
            }
        };

        // Direction vectors
        private Path[] direction =
        {
            new Path(new Pair(-1, 0), "U"),
            new Path(new Pair(0, -1), "L"),
            new Path(new Pair(1, 0), "D"),
            new Path(new Pair(0, 1), "R"),
        };

        // Map width and height
        private int height;
        private int width;

        // Keep track of visited array
        private bool[,]? visited;

        // List of treasure/start indexes
        private Pair[]? index;

        // Adjacency Matrix
        private int[,]? distance;

        // Save path for each treasure/start
        List<List<String>>? stringPaths;

        // List of permutations
        List<List<int>>? permutation;

        // Reset visited matrix
        private void refreshVisited(List<List<String>> map)
        {
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (map[i][j] == "X")
                        this.visited[i, j] = true;
                    else
                        this.visited[i, j] = false;
                }
            }
        }

        // Check if index is valid
        private bool isValidIndex(int row, int col)
        {
            if (row < 0 || col < 0)
                return false;
            if (row >= this.height|| col >= this.width)
                return false;
            if (this.visited[row, col] == true)
                return false;
            return true;
        }

        private List<String> transformToGraph(List<List<String>> map, Pair start)
        {
            List<String> stringPath = new List<String>( new String[this.index.Count()]);

            Queue<Path> q = new Queue<Path>();
            q.Enqueue(new Path(start, "", 0));

            int startY = start.y;
            int startX = start.x;

            this.visited[startY, startX] = true;

            int startIndex = 0;
            for (int i = 0; i < this.index.Count(); i++)
                if (this.index[i].y == startY && this.index[i].x == startX)
                    startIndex = i;

            while (q.Count > 0)
            {
                Path p = q.Dequeue();

                int y = p.point.y;
                int x = p.point.x;
                String path = p.path;
                int distance = p.distance;

                if (map[y][x] == "K" || map[y][x] == "T")
                {
                    for (int i = 0; i < this.index.Count(); i++)
                    {
                        if (this.index[i].y == y && this.index[i].x == x)
                        {
                            this.distance[i, startIndex] = distance;
                            this.distance[startIndex, i] = distance;

                            stringPath[i] = path;

                            break;
                        }
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    int adjY = y + this.direction[i].point.y;
                    int adjX = x + this.direction[i].point.x;
                    String direction = this.direction[i].path;

                    if (isValidIndex(adjY, adjX))
                    {
                        q.Enqueue(new Path(new Pair(adjY, adjX), path + direction, distance + 1));
                        this.visited[adjY, adjX] = true;
                    }
                }
            }

            return stringPath;
        }
        private void generatePermutation(List<int> numbers, int size)
        {
            if (size == 1)
            {
                List<int> temp = new List<int>();

                for (int i = 0; i < numbers.Count(); i++)
                    temp.Add(numbers[i]);

                this.permutation.Add(temp);
            }

            for (int i = 0; i < size; i++)
            {
                generatePermutation(numbers, size - 1);

                if (size % 2 == 1)
                {
                    int temp = numbers[0];
                    numbers[0] = numbers[size - 1];
                    numbers[size - 1] = temp;
                }

                else
                {
                    int temp = numbers[i];
                    numbers[i] = numbers[size - 1];
                    numbers[size - 1] = temp;
                }
            }
        }

        private int calculateDistance(List<int> numbers)
        {
            int distance = 0;

            for (int i = 0; i < numbers.Count-1; i++)
            {
                distance += this.distance[numbers[i], numbers[i + 1]];
            }

            distance += this.distance[numbers[numbers.Count - 1], numbers[0]];

            return distance;
        }

        // Main solve function
        public String doTSP(List<List<String>> map)
        {
            // Initialize constant variables
            this.height = map.Count;
            this.width = map[0].Count;

            int amount = 0;
            for (int i = 0; i < this.height; i++)
                for (int j = 0; j < this.width; j++)
                    if (map[i][j] == "K" || map[i][j] == "T")
                        amount++;

            // Initialize visited array
            this.visited = new bool[this.height, this.width];

            // Save K and T indexes
            this.index = new Pair[amount];

            int len = 1;
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (map[i][j] == "K")
                    {
                        this.index[0] = new Pair(i, j);
                    }

                    else if (map[i][j] == "T")
                    {
                        index[len] = new Pair(i, j);
                        len++;
                    }
                }
            }

            // Calculate distance matrix between two points & save path
            this.distance = new int[amount, amount];
            this.stringPaths = new List<List<String>>();
            for (int i = 0; i < amount; i++)
            {
                refreshVisited(map);

                this.stringPaths.Add(transformToGraph(map, index[i]));
            }

            this.permutation = new List<List<int>>();

            List<int> numbers = new List<int>();
            for (int i = 0; i < amount; i++)
                numbers.Add(i);

            generatePermutation(numbers, numbers.Count);

            int shortestIndex = 0;
            for (int i = 1; i < this.permutation.Count; i++)
            {
                if (permutation[i][0] != 0)
                    continue;

                if (calculateDistance(permutation[i]) < calculateDistance(permutation[shortestIndex]))
                    shortestIndex = i;
            }

            String finalPath = "";
            for (int i = 0; i < amount - 1; i++)
                finalPath += this.stringPaths[this.permutation[shortestIndex][i]][this.permutation[shortestIndex][i+1]];

            finalPath += this.stringPaths[this.permutation[shortestIndex][amount - 1]][this.permutation[shortestIndex][0]];

            return finalPath;
        }
    }
}
