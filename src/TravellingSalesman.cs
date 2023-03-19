using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace src
{
    public class TravellingSalesman
    {
        // Class for pair
        class Pair
        {
            public int x;
            public int y;

            public Pair(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public Pair()
            {
                this.x = 0;
                this.y = 0;
            }
        };

        // Direction vectors
        private Pair[] direction = {
            new Pair(-1, 0),
            new Pair(0, -1),
            new Pair(1, 0),
            new Pair(0, 1)
        };
        
        private Pair[]? index;

        // Original map
        private List<List<String>>? map;

        // Map width / height
        private int width;
        private int height;

        // Keep track of visited array
        private bool[,]? visited;

        // Adjacency matrix
        private int[,]? distance;

        // Make an (n+1)*2^(n+1) array
        private int[,]? memo;

        // List of index taken
        private List<Pair> path = new List<Pair>();

        private bool isValidIndex(int row, int col)
        {
            if (row < 0 || col < 0)
                return false;
            if (row > this.width | col > this.height)
                return false;
            if (this.visited[row, col] == true)
                return false;
            return true;
        }

        private void InitializeAdjacentMatrix(int index, int x, int y)
        {
            Queue<Pair> q = new Queue<Pair>();

            q.Enqueue(new Pair(x, y));
            this.visited[x, y] = true;

            int distance = 0;

            while (q.Count > 0)
            {
                Pair p = q.Dequeue();

                if (this.map[p.x][p.y] == "K" || this.map[p.x][p.y] == "T")
                {
                    for (int i = 0; i < this.index.Count(); i++)
                    {
                        if (this.index[i].x == p.x && this.index[i].y == p.y)
                        {
                            this.distance[index, i] = distance;
                            this.distance[i, index] = distance;
                        }
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    int adjX = x + this.direction[i].x;
                    int adjY = y + this.direction[i].y;

                    if (isValidIndex(adjX, adjY))
                    {
                        q.Enqueue(new Pair(adjX, adjY));
                        this.visited[adjX, adjY] = true;
                    }
                }

                distance++;
            }
        }
        private int TravellingSalesmanSolve(int index, int mask)
        {
            // Best case, if only 0th and indexth is 1, then we have visited all other nodes
            if (mask == 1 << index)
                return distance[1, index];

            // Memoization
            if (memo[index, mask] != 0)
                return memo[index, mask];

            int res = int.MaxValue;

            for (int i = 0; i < this.distance.Length; i++)
            {
                if ((mask & (1 << i)) != 0 && i != index && i != 0)
                {
                    res = Math.Min(
                        res,
                        TravellingSalesmanSolve(i, mask & (~(1 << i))) + distance[i, index]
                    );
                }
            }

            memo[index, mask] = res;

            return res;
        }

        public List<List<String>> Solve(List<List<String>> map, int amount)
        {
            // Initialize attributes
            this.map = map;
            this.width = this.map.Count;
            this.height = this.map[0].Count;
            this.visited = new bool[this.width, this.height];

            // Save K and T indexes, K always in index 0
            this.index = new Pair[amount];

            int len = 1;
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    if (this.map[i][j] == "K")
                        index[0] = new Pair(i, j);

                    if (this.map[i][j] == "T")
                    {
                        index[len] = new Pair(i, j);
                        len++;
                    }
                }
            }

            this.distance = new int[amount, amount];

            // Calculate distances between each node with BFS; save to distance matrix
            for (int i = 0; i < index.Count(); i++)
                InitializeAdjacentMatrix(i, index[i].x, index[i].y);

            // Solve using TSP
            this.memo = new int[amount, 1 << amount];

            int shortest = int.MaxValue;
            for (int i = 0; i < amount; i++)
                shortest = TravellingSalesmanSolve(i, (1 << amount) - 1) + distance[i, 0];


            return map;
        }
    }
}
