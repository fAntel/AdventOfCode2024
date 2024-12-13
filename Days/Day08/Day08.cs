using AdventOfCode2024.Common;
using System.Drawing;

namespace AdventOfCode2024.Days.Day08
{
    internal class Day08 : IDay
    {
        private static readonly ISet<char> s_possibleAntennaName =
            Enumerable.Range('a', 'z' - 'a' + 1)
                .Concat(Enumerable.Range('A', 'Z' - 'A' + 1))
                .Concat(Enumerable.Range('0', '9' - '0' + 1))
                .Select(i => (char) i)
                .ToHashSet();

        public string PartOne()
        {
            Dictionary<char, ISet<Point>> antennas = ParseMapIntoAntennas(out int height, out int width);
            ISet<Point> antennasCoords;
            int x, y;
            ISet<Point> antinodeCoords = new HashSet<Point>();
            foreach (char frequency in antennas.Keys)
            {
                antennasCoords = antennas[frequency];
                foreach (Point antenna in antennasCoords)
                {
                    foreach (Point anotherAntenna in antennasCoords)
                    {
                        if (antenna == anotherAntenna)
                            continue;

                        x = antenna.X + (antenna.X - anotherAntenna.X);
                        y = antenna.Y + (antenna.Y - anotherAntenna.Y);
                        if (0 <= x && x < height && 0<= y && y < width)
                        {
                            antinodeCoords.Add(new Point(x, y));
                        }

                        x = anotherAntenna.X + (anotherAntenna.X - antenna.X);
                        y = anotherAntenna.Y + (anotherAntenna.Y - antenna.Y);
                        if (0 <= x && x < height && 0 <= y && y < width)
                        {
                            antinodeCoords.Add(new Point(x, y));
                        }
                    }
                }
            }

            return antinodeCoords.Count.ToString();
        }

        public string PartTwo()
        {
            Dictionary<char, ISet<Point>> antennas = ParseMapIntoAntennas(out int height, out int width);
            ISet<Point> antennasCoords;
            int x, y;
            ISet<Point> antinodeCoords = new HashSet<Point>();
            foreach (char frequency in antennas.Keys)
            {
                antennasCoords = antennas[frequency];
                if (antennasCoords.Count <= 1)
                    continue;

                foreach (Point antenna in antennasCoords)
                {
                    foreach (Point anotherAntenna in antennasCoords)
                    {
                        if (antenna == anotherAntenna)
                            continue;

                        FindAllAntinodes(height, width, antinodeCoords, antenna, anotherAntenna);
                        FindAllAntinodes(height, width, antinodeCoords, antenna, anotherAntenna);
                    }
                }
            }

            return antinodeCoords.Count.ToString();
        }

        private static void FindAllAntinodes(int height, int width, ISet<Point> antinodeCoords, Point antenna, Point anotherAntenna)
        {
            int aX = antenna.X;
            int aY = antenna.Y;
            int bX = anotherAntenna.X;
            int bY = anotherAntenna.Y;
            int x = aX, y = aY;
            while (0 <= x && x < height && 0 <= y && y < width)
            {
                antinodeCoords.Add(new Point(x, y));
                x = aX + (aX - bX);
                y = aY + (aY - bY);
                bX = aX;
                bY = aY;
                aX = x;
                aY = y;
            }
        }

        private static Dictionary<char, ISet<Point>> ParseMapIntoAntennas(out int mapHeight, out int mapWidth)
        {
            string[] input = File.ReadAllLines(InputHelper.GetInputPath(8));
            mapHeight = input.Length;
            mapWidth = input[0].Length;

            Dictionary<char, ISet<Point>> antennas = [];
            string line;
            char c;
            ISet<Point>? points;
            for (int i = 0; i < input.Length; ++i)
            {
                line = input[i];
                for (int j = 0; j < line.Length; ++j)
                {
                    c = line[j];
                    if (s_possibleAntennaName.Contains(c))
                    {
                        antennas.TryGetValue(c, out points);
                        if (points == null)
                        {
                            points = new HashSet<Point>();
                            antennas[c] = points;
                        }
                        points.Add(new Point(j, i));
                    }
                }
            }

            return antennas;
        }
    }
}
