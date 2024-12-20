using AdventOfCode2024.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days.Day12
{
    internal class Day12 : IDay
    {
        public string PartOne()
        {
            string[] map = File.ReadAllLines(InputHelper.GetInputPath(12));

            List<ISet<(int, int)>> regions = FindRegions(map);

            ulong result = 0;
            foreach (ISet<(int, int)> region in regions)
            {
                result += (ulong)region.Count * CalculateRegionPerimeter(region);
            }

            return result.ToString();
        }

        public string PartTwo()
        {
            return "";
        }

        private static List<ISet<(int, int)>> FindRegions(string[] map)
        {
            List<ISet<(int, int)>> result = [];
            ISet<(int, int)> passedPoints = new HashSet<(int, int)>();
            ISet<(int, int)> region;
            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[y].Length; ++x)
                {
                    if (passedPoints.Contains((x, y)))
                        continue;

                    region = new HashSet<(int, int)>();
                    TraverseRegion(map[y][x], x, y, region, map);
                    passedPoints.UnionWith(region);
                    result.Add(region);
                }
            }

            return result;
        }

        private static void TraverseRegion(char name, int x, int y, ISet<(int, int)> region, string[] map)
        {
            if (region.Contains((x, y)))
                return;

            if (y < 0 || y >= map.Length || x < 0 || x >= map[y].Length)
                return;

            if (map[y][x] != name)
                return;

            region.Add((x, y));

            TraverseRegion(name, x - 1, y, region, map);
            TraverseRegion(name, x, y - 1, region, map);
            TraverseRegion(name, x + 1, y, region, map);
            TraverseRegion(name, x, y + 1, region, map);
        }

        private static ulong CalculateRegionPerimeter(ISet<(int, int)> region)
        {
            ulong result = 0;

            foreach ((int x, int y) in region)
            {
                if (!region.Contains((x - 1, y)))
                    result += 1;

                if (!region.Contains((x, y - 1)))
                    result += 1;

                if (!region.Contains((x + 1, y)))
                    result += 1;

                if (!region.Contains((x, y + 1)))
                    result += 1;
            }

            return result;
        }
    }
}