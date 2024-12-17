using AdventOfCode2024.Common;
using System.Drawing;

namespace AdventOfCode2024.Days.Day10
{
    internal class Day10 : IDay
    {
        private static readonly char s_tailhaed = '0';
        private static readonly char s_maxHeight = '9';

        public string PartOne()
        {
            string[] map = File.ReadAllLines(InputHelper.GetInputPath(10));

            ulong sum = 0;
            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[y].Length; ++x)
                {
                    if (map[y][x] == s_tailhaed)
                        sum += CalculateTailheadScore(map, x, y);
                }
            }

            return sum.ToString();
        }

        public string PartTwo()
        {
            string[] map = File.ReadAllLines(InputHelper.GetInputPath(10));

            ulong sum = 0;
            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[y].Length; ++x)
                {
                    if (map[y][x] == s_tailhaed)
                        sum += CalculateTailheadRating(map, x, y);
                }
            }

            return sum.ToString();
        }

        private static ulong CalculateTailheadScore(string[] map, int x, int y)
        {
            ISet<Point> currentPoints = new HashSet<Point>() { new Point(x, y) };
            ISet<Point> nextPoints = new HashSet<Point>();
            ISet<Point> t;

            char counter = s_tailhaed;
            do
            {
                ++counter;
                foreach (Point point in currentPoints)
                {
                    if (point.X > 0 && map[point.Y][point.X - 1] == counter)
                        nextPoints.Add(new Point(point.X - 1, point.Y));

                    if (point.Y > 0 && map[point.Y - 1][point.X] == counter)
                        nextPoints.Add(new Point(point.X, point.Y - 1));

                    if (point.X + 1 < map[0].Length && map[point.Y][point.X + 1] == counter)
                        nextPoints.Add(new Point(point.X + 1, point.Y));

                    if (point.Y + 1 < map.Length && map[point.Y + 1][point.X] == counter)
                        nextPoints.Add(new Point(point.X, point.Y + 1));
                }
                currentPoints.Clear();
                t = nextPoints;
                nextPoints = currentPoints;
                currentPoints = t;
            } while (counter < s_maxHeight && currentPoints.Any());

            return (ulong)currentPoints.Count;
        }

        private static ulong CalculateTailheadRating(string[] map, int x, int y)
        {
            ISet<List<Point>> trails = new HashSet<List<Point>>() { new(s_maxHeight - s_tailhaed + 1) { new(x, y) } };
            ISet<List<Point>> buffer = new HashSet<List<Point>>();
            ISet<List<Point>> t;

            Point point;
            List<Point> newTrail;
            char counter = s_tailhaed;
            do
            {
                ++counter;
                foreach (List<Point> trail in trails)
                {
                    point = trail.Last();
                    if (point.X > 0 && map[point.Y][point.X - 1] == counter)
                    {
                        newTrail = new List<Point>(trail)
                        {
                            new(point.X - 1, point.Y)
                        };
                        buffer.Add(newTrail);
                    }

                    if (point.Y > 0 && map[point.Y - 1][point.X] == counter)
                    {
                        newTrail = new List<Point>(trail)
                        {
                            new(point.X, point.Y - 1)
                        };
                        buffer.Add(newTrail);
                    }

                    if (point.X + 1 < map[0].Length && map[point.Y][point.X + 1] == counter)
                    {
                        newTrail = new List<Point>(trail)
                        {
                            new(point.X + 1, point.Y)
                        };
                        buffer.Add(newTrail);
                    }

                    if (point.Y + 1 < map.Length && map[point.Y + 1][point.X] == counter)
                    {
                        newTrail = new List<Point>(trail)
                        {
                            new(point.X, point.Y + 1)
                        };
                        buffer.Add(newTrail);
                    }
                }
                trails.Clear();
                t = buffer;
                buffer = trails;
                trails = t;
            } while (counter < s_maxHeight && trails.Any());

            return (ulong)trails.Count;
        }
    }
}
