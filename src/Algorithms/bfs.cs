using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace src.Algorithms
{
    class bfs
    {
        class bfsUtil
        {
            private List<List<string>> _map;
            private const string TREASURE = "T";
            private const string PATH = "R";
            private const string BLOCK = "X";
            private const string START = "K";

            public bfsUtil(List<List<string>> map)
            {
                this._map = new List<List<string>>();
                foreach (List<string> row in map)
                {
                    List<string> tempRow = new List<string>(row);
                    _map.Add(tempRow);
                }
            }

            private int getMaxX()
            {
                return _map[0].Count - 1;
            }

            private int getMaxY()
            {
                return _map.Count - 1;
            }

            private bool isIdxValid(Point p)
            {
                bool xOutOfBound = p.X < 0 || p.X > getMaxX();
                bool yOutOfBound = p.Y < 0 || p.Y > getMaxY();

                return !(xOutOfBound || yOutOfBound);
            }

            private bool isPath(Point p)
            {
                return this._map[p.Y][p.Y] == PATH;
            }

            private bool isTreasure(Point p)
            {
                return this._map[p.Y][p.X] == TREASURE;
            }

            private bool isBlock(Point p)
            {
                return this._map[p.Y][p.X] == BLOCK;
            }

            private bool isStart(Point p)
            {
                return this._map[p.Y][p.X] == START;
            }

            private Point? getStartPoint()
            {
                Point? startPoint = null;
                bool startPointFound = false;
                for (int y = 0; y <= getMaxY() && !startPointFound; y++)
                {
                    for (int x = 0; x <= getMaxX() && !startPointFound; x++)
                    {
                        if (isStart(new Point(x, y)))
                        {
                            startPoint = new Point(x, y);
                            startPointFound = true;
                        }
                    }
                }

                return startPoint;
            }

            private int getNumberOfTreasure()
            {
                int numberOfTreasure = 0;
                for (int y = 0; y <= getMaxY(); y++)
                {
                    for (int x = 0; x <= getMaxX(); x++)
                    {
                        if (isTreasure(new Point(x, y)))
                        {
                            numberOfTreasure++;
                        }
                    }
                }

                return numberOfTreasure;
            }

            private bool isVisited(Point point, ref bool[,] state)
            {
                return state[point.Y, point.X] == true;
            }

            private void visit(Point point, ref bool[,] state)
            {
                state[point.Y, point.X] = true;
            }

            private void forget(Point point, ref bool[,] state)
            {
                state[point.Y, point.X] = false;
            }

            private void enqueueNeighbour(List<Point> currentPath, ref Queue<List<Point>> queue, ref bool[,] state)
            {
                Point point = currentPath[^1];

                Point up = new Point(point.X, point.Y + 1);
                if (isIdxValid(up))
                {
                    if (!isVisited(up, ref state) && !isBlock(up))
                    {
                        List<Point> temp1 = new List<Point>(currentPath);
                        temp1.Add(up);
                        queue.Enqueue(temp1);
                        visit(point, ref state);
                    }
                }
                Point down = new Point(point.X, point.Y - 1);
                if (isIdxValid(down))
                {
                    if (!isVisited(down, ref state) && !isBlock(down))
                    {
                        List<Point> temp2 = new List<Point>(currentPath);
                        temp2.Add(down);
                        queue.Enqueue(temp2);
                        visit(point, ref state);
                    }
                }
                Point left = new Point(point.X - 1, point.Y);
                if (isIdxValid(left))
                {
                    if (!isVisited(left, ref state) && !isBlock(left))
                    {
                        List<Point> temp3 = new List<Point>(currentPath);
                        temp3.Add(left);
                        queue.Enqueue(temp3);
                        visit(point, ref state);
                    }
                }
                Point right = new Point(point.X + 1, point.Y);
                if (isIdxValid(right))
                {
                    if (!isVisited(right, ref state) && !isBlock(right))
                    {
                        List<Point> temp4 = new List<Point>(currentPath);
                        temp4.Add(right);
                        queue.Enqueue(temp4);
                        visit(point, ref state);
                    }
                }

            }

            private string stringify(List<Point> path)
            {
                if (path.Count <= 1)
                {
                    return "";
                }
                string result = "";
                for (int i = 1; i < path.Count; i++)
                {
                    if (path[i].X > path[i - 1].X)
                    {
                        result += "R";
                        continue;
                    }
                    else if (path[i].X < path[i - 1].X)
                    {
                        result += "L";
                        continue;
                    }
                    else if (path[i].Y < path[i - 1].Y)
                    {
                        result += "D";
                        continue;
                    }
                    else
                    {
                        result += "U";
                        continue;
                    }
                }
                return result;
            }

            public string findPathBFS(bool tsp = false)
            {
                bool doneTsp = false;
                int numberOfTreasureAvail = getNumberOfTreasure();
                if (numberOfTreasureAvail == 0)
                {
                    return ""; // end
                }

                Point? startPoint = getStartPoint();
                if (startPoint == null)
                {
                    return ""; // throw exception
                }

                bool[,] state = new bool[_map.Count, _map[0].Count];

                Queue<List<Point>> queue = new Queue<List<Point>>(); // queue of active active path (end-node still active)
                int numberOfTreasureFound = 0;


                visit((Point)startPoint, ref state);
                queue.Enqueue(new List<Point> { (Point)startPoint });
                while (queue.Count != 0)
                {
                    List<Point> currentPath = queue.Dequeue();
                    Point currentPoint = currentPath[^1];
                    if (isTreasure(currentPoint))
                    {
                        numberOfTreasureFound++;
                        if (numberOfTreasureFound >= numberOfTreasureAvail)
                        {

                            if (!tsp || doneTsp)
                            {
                                return (stringify(currentPath));

                            }
                            else
                            {
                                Point temp = (Point)startPoint;
                                _map[temp.Y][temp.X] = TREASURE;
                                doneTsp = true;
                            }
                        }
                        // set the currentPoint (treasure) as regular path
                        _map[currentPoint.Y][currentPoint.X] = PATH;

                        // forget all node visited
                        foreach (List<Point> path in queue)
                        {
                            foreach (Point point in path)
                            {
                                forget(point, ref state);
                            }
                        }
                        foreach (Point point in currentPath)
                        {
                            forget(point, ref state);
                        }
                        queue.Clear();
                        visit(currentPoint, ref state);
                    }
                    enqueueNeighbour(currentPath, ref queue, ref state);
                }
                return "";
            }
        };

        public static string doBFS(List<List<string>> map, bool tsp = false)
        {
            foreach (var row in map)
            {
                foreach (var col in row)
                {
                    Trace.Write(col);
                }
                Trace.Write('\n');
            }
            bfsUtil pathfinder = new bfsUtil(map);
            return pathfinder.findPathBFS(tsp);
        }
    }
}