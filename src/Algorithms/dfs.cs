using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace src.Algorithms
{
    class dfs
    {
        class DFSUtil : PathFinder
        {   
            public DFSUtil(List<List<string>> map, bool tsp) : base(map, tsp)
            {
            }

            private bool _visit(Point point, ref int numberOfTreasureFound)
            {
                bool anyTreasureFound = false;

                _remember(point);
                _solution.Add(point);

                if (_isTreasure(point)) 
                {
                    numberOfTreasureFound++;
                    anyTreasureFound = true;
                }

                if (numberOfTreasureFound == _numberOfTreasureAvail)
                {
                    return true;   
                }


                Point[] directions = new Point[] {new Point(0, 1), new Point(0, -1), new Point(-1, 0), new Point(1, 0)};
                foreach (var dir in directions)
                {
                    Point next = new Point(point.X + dir.X, point.Y + dir.Y);

                    if (_isIdxValid(next) && !_isVisited(next) && !_isBlock(next))
                    {
                        bool anyTreasureFoundHere = _visit(next, ref numberOfTreasureFound);
                        if (numberOfTreasureFound == _numberOfTreasureAvail)
                        {
                            if (_tsp)
                            {
                                _solution.Add(point);
                            }
                            return true;
                        }

                        if (anyTreasureFoundHere)
                        {
                            anyTreasureFound = true;
                            _solution.Add(point);
                        }
                    }
                }

                
                if (!anyTreasureFound)
                {
                    _solution.RemoveAt(_solution.Count - 1);
                }

                return anyTreasureFound;
            }

            public string findPathDFS(bool tsp = false) 
            {
                if (_numberOfTreasureAvail == 0) 
                {
                    return "";
                }

                int numberOfTreasureFound = 0;

                _visit(_startPoint, ref numberOfTreasureFound);
                

                if (numberOfTreasureFound == _numberOfTreasureAvail)
                {
                    return _stringify(_solution);
                }
                else 
                {
                    return "";
                    throw new SolutionNotFoundExeption();    
                }
                

            }
            

            
        };

        public static string doDFS(List<List<string>> map, bool tsp = false)
        {
            DFSUtil pathfinder = new DFSUtil(map, tsp);
            return pathfinder.findPathDFS();
        }
    };
}
