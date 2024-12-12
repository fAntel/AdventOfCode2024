using AdventOfCode2024.Common;
using System.Drawing;

namespace AdventOfCode2024.Days.Day06
{
    internal class Day06 : IDay
    {
        private static readonly char s_obstacle = '#';

        public string PartOne()
        {
            string[] map = File.ReadAllLines(InputHelper.GetInputPath(6));
            FindStartingPosition(map, out int x, out int y, out Orientation orientation);

            ISet<Point> points = new HashSet<Point>();
            while (IsWithinMap(map, x, y))
            {
                points.Add(new Point(x, y));

                SimulateStep(map, x, y, orientation, out x, out y, out orientation);
            }

            return points.Count.ToString();
        }

        public string PartTwo()
        {
            throw new NotImplementedException();
        }

        private static bool IsWithinMap(string[] map, int x, int y) => x >= 0 && y >= 0 && x < map[0].Length && y < map.Length;

        private static void SimulateStep(string[] map, int x, int y, Orientation orientation, out int newX, out int newY, out Orientation newOrientation)
        {
            newOrientation = orientation;
            newX = x;
            newY = y;
            switch (orientation)
            {
                case Orientation.North:
                    if (y > 0 && map[y - 1][x] == s_obstacle)
                    {
                        newOrientation = Orientation.East;
                    }
                    else
                    {
                        --newY;
                    }
                    break;
                case Orientation.East:
                    if (x < map[0].Length - 1 && map[y][x + 1] == s_obstacle)
                    {
                        newOrientation = Orientation.South;
                    }
                    else
                    {
                        ++newX;
                    }
                    break;
                case Orientation.South:
                    if (y < map.Length - 1 && map[y + 1][x] == s_obstacle)
                    {
                        newOrientation = Orientation.West;
                    }
                    else
                    {
                        ++newY;
                    }
                    break;
                case Orientation.West:
                    if (x > 0 && map[y][x - 1] == s_obstacle)
                    {
                        newOrientation = Orientation.North;
                    }
                    else
                    {
                        --newX;
                    }
                    break;
            }
        }

        private static void FindStartingPosition(string[] map, out int x, out int y, out Orientation orientation)
        {
            x = 0;
            Orientation? t = null;
            for (y = 0; y < map.Length; ++y)
            {
                for (x = 0; x < map[y].Length; ++x)
                {
                    t = ParsePossibleOrientation(map[y][x]);
                    if (t != null)
                        break;
                }
                if (t != null)
                    break;
            }
            if (t == null)
                throw new Exception("Couldn't find the guard");
            else
                orientation = t.Value;
        }

        private static Orientation? ParsePossibleOrientation(char c) => c switch
        {
            '^' => Orientation.North,
            '>' => Orientation.East,
            'v' => Orientation.South,
            '<' => Orientation.West,
            _ => null,
        };

        private enum Orientation { North, East, South, West }
    }
}
