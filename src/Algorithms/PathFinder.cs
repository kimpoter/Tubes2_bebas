using System.Drawing;
using System;
using System.Collections.Generic;

namespace src
{
    class PathFinder
    {
        protected List<List<string>> _map;
        protected bool[,] _visited;
        protected List<Point> _solution;
        protected readonly int _numberOfTreasureAvail;
        protected readonly Point _startPoint;
        protected readonly bool _tsp;
        
        protected const string TREASURE = "T";
        protected const string PATH = "R";
        protected const string BLOCK = "X";
        protected const string START = "K";


        public PathFinder(List<List<string>> map, bool tsp)
        {
            int maxRowLength = 0;
            foreach(List<string> row in map)
            {
                maxRowLength = row.Count > maxRowLength ? row.Count : maxRowLength;
            }

            _map = new List<List<string>>();
            foreach(List<string> row in map)
            {
                List<string> tempRow = new List<string>(row);
                for (int i = 0; i < maxRowLength - row.Count; i++)
                {
                    tempRow.Add(BLOCK);
                }
                _map.Add(tempRow);
            }

            _visited = new bool[_map.Count, _map[0].Count];

            _solution = new List<Point>();

            _numberOfTreasureAvail = 0;
            for (int y = 0; y <= _getMaxY(); y++) 
            {
                for (int x = 0; x <= _getMaxX(); x++) 
                {
                    if (_map[y][x] == TREASURE) 
                    {
                        _numberOfTreasureAvail++;
                    }
                }
            }
            
            int numberOfStartPoint = 0;
            for (int y = 0; y <= _getMaxY(); y++)
            {
                for (int x = 0; x <= _getMaxX(); x++)
                {
                    if (_map[y][x] == START)
                    {
                        numberOfStartPoint++;
                        _startPoint = new Point(x, y);
                    }
                }
            }

            if (numberOfStartPoint == 0)
            {
                throw new StartPointNotFoundException();
            }
            else if (numberOfStartPoint > 1)
            {
                throw new MultipleStartPointException(numberOfStartPoint);
            }

            _tsp = tsp;
        }

        protected int _getMaxX() 
        {
            return _map[0].Count - 1;
        }

        protected int _getMaxY() 
        {
            return _map.Count - 1;
        }

        protected bool _isIdxValid(Point p) 
        {
            bool xOutOfBound = p.X < 0 || p.X > _getMaxX();
            bool yOutOfBound = p.Y < 0 || p.Y > _getMaxY();
            
            return !(xOutOfBound || yOutOfBound);
        }

        protected bool _isPath(Point p) 
        {
            return this._map[p.Y][p.X] == PATH;
        }

        protected bool _isTreasure(Point p) 
        {
            return this._map[p.Y][p.X] == TREASURE;
        }

        protected bool _isBlock(Point p) 
        {
            return this._map[p.Y][p.X] == BLOCK;
        }

        protected bool _isStart(Point p)
        {
            return this._map[p.Y][p.X] == START;
        }

        protected Point? _getStartPoint() {
            Point? startPoint = null;
            bool startPointFound = false;
            for (int y = 0; y <= _getMaxY() && !startPointFound; y++) 
            {
                for (int x = 0; x <= _getMaxX() && !startPointFound; x++) 
                {
                    if (_isStart(new Point(x, y))) 
                    {
                        startPoint = new Point(x, y);
                        startPointFound = true;
                    }
                }
            }

            return startPoint;
        } 

        protected bool _isVisited(Point point)
        {
                return _visited[point.Y, point.X] == true;
        }

        protected void _remember(Point point)
        {
            _visited[point.Y, point.X] = true;
        }

        protected void _forgetAll()
        {
            for (int y = 0; y <= _getMaxY(); y++)
            {
                for (int x = 0; x <= _getMaxX(); x++)
                {
                    _visited[y, x] = false;
                }
            }
        }

        protected string _stringify(List<Point> path)
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
                    result += "U";
                    continue;
                }
                else 
                {
                    result += "D";
                    continue;
                }
            }
            return result;
        }

    }

    class StartPointNotFoundException : Exception
    {
        public StartPointNotFoundException() : base("no start point found!")
        {
        }
    };

    class MultipleStartPointException : Exception
    {
        private readonly int _numberOfStartPoint;
        
        public MultipleStartPointException(int numberOfStartPoint) : base("multiple start point found!")
        {
            _numberOfStartPoint = numberOfStartPoint;
        }
    };

    class SolutionNotFoundExeption : Exception
    {
        public SolutionNotFoundExeption() : base("no solution found for given map!")
        {
        }
    };
}