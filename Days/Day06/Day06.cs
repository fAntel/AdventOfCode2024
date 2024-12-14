using AdventOfCode2024.Common;
using System.Drawing;

namespace AdventOfCode2024.Days.Day06
{
    internal class Day06 : IDay
    {
        private static readonly char s_obstacle = '#';

        public string PartOne()
        {
            char[][] map = PreprareMap();
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
            char[][] map = PreprareMap();
            FindStartingPosition(map, out int startingPositionX, out int startingPositionY, out Orientation startingOrientation);

            ISet<Point> points = new HashSet<Point>();
            int x, y;
            Orientation orientation;
            ISet<Position> positions = new HashSet<Position>();
            for (int i = 0; i < map.Length; ++i)
            {
                for (int j = 0; j < map[i].Length; ++j)
                {
                    if (map[i][j] == s_obstacle || (i == startingPositionY && j == startingPositionX))
                        continue;

                    map[i][j] = s_obstacle;
                    x = startingPositionX; y = startingPositionY; orientation = startingOrientation;
                    positions.Clear();
                    Position position = new(x, y, orientation);
                    do
                    {
                        positions.Add(position);
                        SimulateStep(map, x, y, orientation, out x, out y, out orientation);
                        position = new Position(x, y, orientation);
                    } while (IsWithinMap(map, x, y) && !positions.Contains(position));

                    if (IsWithinMap(map, x, y))
                    {
                        points.Add(new Point(j, i));
                    }
                    map[i][j] = '.';
                }
            }

            return points.Count.ToString();
        }

        private static bool IsWithinMap(char[][] map, int x, int y) => x >= 0 && y >= 0 && x < map[0].Length && y < map.Length;

        private static void SimulateStep(char[][] map, int x, int y, Orientation orientation, out int newX, out int newY, out Orientation newOrientation)
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

        private static char[][] PreprareMap() => File.ReadAllLines(InputHelper.GetInputPath(6)).Select(line => line.ToCharArray()).ToArray();

        private static void FindStartingPosition(char[][] map, out int x, out int y, out Orientation orientation)
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

        private readonly struct Position(int x, int y, Orientation orientation)
        {
            private readonly int x = x;
            private readonly int y = y;
            private readonly Orientation orientation = orientation;

            public override readonly bool Equals(object? obj)
            {
                return obj is Position position &&
                       x == position.x &&
                       y == position.y &&
                       orientation == position.orientation;
            }

            public override readonly int GetHashCode() => HashCode.Combine(x, y, orientation);
        }
    }
}
