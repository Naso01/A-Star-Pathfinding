using System;
using System.Collections.Generic;
using System.Linq;

namespace AStarPathfinding
{
    public class Location
    {
        public int X;
        public int Y;
        public double F;
        public double G;
        public double H;
        public Location Parent;
    }

    class Program
    {
        static string[] map;
        static Location current = null;
        static readonly Location start = new Location { X = 1, Y = 2 };
        static readonly Location target = new Location { X = 2, Y = 5 };
        static readonly List<Location> openList = new List<Location>();
        static readonly List<Location> closedList = new List<Location>();

        static void CreateMap()
        {
            map = new string[]
            {
                "+------+",
                "|      |",
                "|A X   |",
                "|XXX   |",
                "|  XX  |",
                "| B    |",
                "|      |",
                "+------+"
            };

            foreach (var line in map)
            {
                Console.WriteLine(line);
            }
        }

        static List<Location> GetWalkableAdjacentSquares(int _x, int _y)
        {
            var proposedLocations = new List<Location>()
            {
                new Location { X = _x,      Y = _y - 1}, // Top
                new Location { X = _x,      Y = _y + 1 }, // Bottom
                new Location { X = _x - 1,  Y = _y }, // Left
                new Location { X = _x + 1,  Y = _y }, // Right
                new Location { X = _x - 1,  Y = _y - 1 }, //Top Left
                new Location { X = _x - 1,  Y = _y + 1 }, //Bottom Left
                new Location { X = _x + 1,  Y = _y - 1 }, //Top Right
                new Location { X = _x + 1,  Y = _y + 1 } //Bottom Right
            };

            return proposedLocations.Where(l => map[l.Y][l.X] == ' ' || map[l.Y][l.X] == 'B').ToList();
        }

        static double EuclidianDistance(int _x1, int _y1, int _x2, int _y2)
        {
            int dX = _x1 - _x2;
            int dY = _y1 - _y2;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        static bool ProcessAdjacentSquares()
        {
            //Get the square with the lowest F score
            var lowest = openList.Min(l => l.F);
            current = openList.First(l => l.F == lowest);

            //Add the current square to the closed list
            closedList.Add(current);

            //Show current square on the map
            Console.SetCursorPosition(current.X, current.Y);
            Console.Write('.');
            Console.SetCursorPosition(current.X, current.Y);
            System.Threading.Thread.Sleep(500);

            // Remove it from the open list
            openList.Remove(current);

            //If we added the destination to the closed list, we've found a path
            if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                return true;

            var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y);
            foreach (var adjacentSquare in adjacentSquares)
            {
                ProcessAdjacentSquare(adjacentSquare);
            }
            return false;
        }

        static void ProcessAdjacentSquare(Location _square)
        {
            //If the adjacent square is already in the closed list, ignore it
            if (closedList.FirstOrDefault(l => l.X == _square.X &&
                                            l.Y == _square.Y) != null)
                return;

            // If its not in the open list...
            double g = EuclidianDistance(_square.X, _square.Y, start.X, start.Y);
            if (openList.FirstOrDefault(l => l.X == _square.X &&
                                             l.Y == _square.Y) == null)
            {
                // ...add it to the open list. Make the current square the parent.
                _square.G = g;
                _square.H = EuclidianDistance(_square.X, _square.Y, target.X, target.Y);
                _square.F = _square.G + _square.H;
                _square.Parent = current;

                //And add it to the open list
                openList.Insert(0, _square);
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "A* Pathfinding";
            CreateMap();
            openList.Add(start);

            while (openList.Count > 0)
            {
                if (ProcessAdjacentSquares()) break;
            }

            //show the path
            while (current != null)
            {
                Console.SetCursorPosition(current.X, current.Y);
                Console.Write('_');
                Console.SetCursorPosition(current.X, current.Y);
                current = current.Parent;
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}