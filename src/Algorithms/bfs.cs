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
        class BFSUtil : PathFinder
        {   
            public BFSUtil(List<List<string>> map, bool tsp) : base (map, tsp)
            {
            }

            private void enqueueNeighbour(List<Point> currentPath, ref Queue<List<Point>> queue)
            {   
                Point point = currentPath[^1];
                            
                Point[] directions = new Point[] {new Point(0, 1), new Point(0, -1), new Point(-1, 0), new Point(1, 0)};
                foreach (var dir in directions)
                {
                    Point next = new Point(point.X + dir.X, point.Y + dir.Y);
                    if (_isIdxValid(next) && !_isVisited(next) && !_isBlock(next))
                    {   
                        List<Point> temp = new List<Point>(currentPath);
                        temp.Add(next);
                        queue.Enqueue(temp);
                        _remember(point);
                    }  
                }
            }

            public string findPathBFS(bool tsp = false) 
            {
                bool doneTsp = false;

                if (_numberOfTreasureAvail == 0) {
                    return ""; // end
                }

                Queue<List<Point>> queue = new Queue<List<Point>>();
                int numberOfTreasureFound = 0;

                _remember(_startPoint);
                queue.Enqueue(new List<Point> {_startPoint});
                while (queue.Count != 0) 
                {
                    List<Point> currentPath = queue.Dequeue();
                    Point currentPoint = currentPath[^1];
                    if (_isTreasure(currentPoint))
                    {
                        numberOfTreasureFound++;
                        if (numberOfTreasureFound >= _numberOfTreasureAvail)
                        {
                            if (!tsp || doneTsp)
                            {
                                return (_stringify(currentPath));
                
                            }
                            else 
                            {
                                _map[_startPoint.Y][_startPoint.X] = TREASURE;
                                doneTsp = true;
                            }
                        }
                        /* set current treasure as reg. path, redo bfs for n-1 treasure */
                        _map[currentPoint.Y][currentPoint.X] = PATH;
                        _forgetAll();
                        queue.Clear();
                        _remember(currentPoint);
                    }
                    enqueueNeighbour(currentPath, ref queue);
                }
                throw new SolutionNotFoundExeption();
            }
        };

        public static string doBFS(List<List<string>> map, bool tsp = false)
        {
            BFSUtil pathfinder = new BFSUtil(map, tsp);
            return pathfinder.findPathBFS(tsp);
        }
    };
}

