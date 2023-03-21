using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace src.Algorithms
{
    class dfsUtil
    {   
        private  List<List<string>> _map;
        private const string TREASURE = "T";
        private const string PATH = "R";
        private const string BLOCK = "X";
        private const string START = "K";


        public dfsUtil(List<List<string>> map)
        {
            this._map = map;
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
            return this._map[p.Y][p.Y] == START;
        }

        private Point? getStartPoint() {
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

        private int getNumberOfTreasure() {
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

        private string stringify(List<Point> path)
        {
            foreach (Point point in path)
            {
                string s = String.Format("({0},{1})",point.Y, point.X);
                Console.WriteLine(s);
            }

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
                else if (path[i].Y > path[i - 1].Y) 
                {
                    result += "U";
                    continue;
                }
                else 
                {
                    //
                }
            }
            return result;
        }

        private bool findPathDFSHelper(Point point, ref bool[,] state, ref List<Point> solution, ref List<Point> candidate,
                                        ref int numberOfTreasureFound, ref int numberOfTreasureAvail, ref bool tsp)
        {
            bool anyTreasureFound = false;

            visit(point, ref state);
            candidate.Add(point);

            if (isTreasure(point)) 
            {
                numberOfTreasureFound++;
                anyTreasureFound = true;
                solution.AddRange(candidate);
                candidate.Clear();
            }
            if (numberOfTreasureFound == numberOfTreasureAvail)
            {
                return true;
            }

            Point up = new Point(point.X, point.Y + 1);
            if (isIdxValid(up)) 
            {   
                if (!isVisited(up, ref state) && !isBlock(up))
                {
                    bool anyTreasureFound1  = findPathDFSHelper(up, ref state, ref solution, ref candidate, ref numberOfTreasureFound, ref numberOfTreasureAvail, ref tsp);
                    if (numberOfTreasureFound == numberOfTreasureAvail)
                    {
                        if (tsp)
                        {
                            solution.Add(point);
                        }
                        return true;
                    }
                    if (anyTreasureFound1)
                    {
                        anyTreasureFound = true;
                        solution.Add(point);
                    }
                }
            }
            Point down = new Point(point.X, point.Y - 1);
            if (isIdxValid(down)) 
            {
                if (!isVisited(down, ref state) && !isBlock(down))
                {
                    bool anyTreasureFound2 = findPathDFSHelper(down, ref state, ref solution, ref candidate, ref numberOfTreasureFound, ref numberOfTreasureAvail, ref tsp);
                    if (numberOfTreasureFound == numberOfTreasureAvail)
                    {
                        if (tsp)
                        {
                            solution.Add(point);
                        }
                        return true;
                    }
                    if (anyTreasureFound2)
                    {

                        anyTreasureFound = true;
                        solution.Add(point);
                    }
                    anyTreasureFound = anyTreasureFound || anyTreasureFound2;
                }
            }
            Point left = new Point(point.X - 1, point.Y);
            if (isIdxValid(left)) 
            {
                if (!isVisited(left, ref state) && !isBlock(left))
                {
                    bool anyTreasureFound3 = findPathDFSHelper(left, ref state, ref solution, ref candidate, ref numberOfTreasureFound, ref numberOfTreasureAvail, ref tsp);
                    if (numberOfTreasureFound == numberOfTreasureAvail)
                    {
                        if (tsp)
                        {
                            solution.Add(point);
                        }
                        return true;
                    }
                    if (anyTreasureFound3)
                    {
                        anyTreasureFound = true;
                        solution.Add(point);
                    }
                }
            }
            Point right = new Point(point.X + 1, point.Y);
            if (isIdxValid(right)) 
            {
                if (!isVisited(right, ref state) && !isBlock(right))
                {
                    bool anyTreasureFound4 = findPathDFSHelper(right, ref state, ref solution, ref candidate, ref numberOfTreasureFound, ref numberOfTreasureAvail, ref tsp);
                    if (numberOfTreasureFound == numberOfTreasureAvail)
                    {
                        if (tsp)
                        {
                            solution.Add(point);
                        }
                        return true;
                    }
                    if (anyTreasureFound4)
                    {
                        anyTreasureFound = true;
                        solution.Add(point);
                    }
                }
            }
            
            if (!anyTreasureFound)
            {
                candidate.Clear();
            }

            return anyTreasureFound;
        }

        public string findPathDFS(bool tsp = false) 
        {
            int numberOfTreasureAvail = getNumberOfTreasure();
            if (numberOfTreasureAvail == 0) 
            {
                return ""; // end
            }

            Point? startPoint = getStartPoint();
            if (startPoint == null) {
                return ""; // throw exception
            }

            List<Point> solution = new List<Point>();
            List<Point> candidate = new List<Point>();
            bool[,] state = new bool[_map.Count, _map[0].Count];
            int numberOfTreasureFound = 0;

            findPathDFSHelper((Point) startPoint, ref state, ref solution, ref candidate, ref numberOfTreasureFound, ref numberOfTreasureAvail, ref tsp);
            if (numberOfTreasureFound == numberOfTreasureAvail)
            {
                
                return stringify(solution);
            }
            else 
            {
                return "";    
            }
        }
            
        
    };


    class dfs
    {
        public string doDFS(List<List<string>> map)
        {
            dfsUtil pathfinder = new dfsUtil(map);
            return pathfinder.findPathDFS();
        }
    }
}
