using AdventOfCode2024.Common;

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
            string[] map = File.ReadAllLines(InputHelper.GetInputPath(12));
            int height = map.Length;
            int width = map[0].Length;

            List<ISet<(int, int)>> regions = FindRegions(map);

            ulong result = 0;
            foreach (ISet<(int, int)> region in regions)
            {
                result += (ulong)region.Count * CalculateRegionNumberOfSides(region, height, width);
            }

            return result.ToString();
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

        private static ulong CalculateRegionNumberOfSides(ISet<(int, int)> region, int height, int width)
        {
            if (region.Count < 3)
                return 4;

            ulong result = 0;
            Dictionary<(int, int), CheckedSides> points = new(region.Select(point => new KeyValuePair<(int, int), CheckedSides>(point, new CheckedSides())));

            CheckedSides sides;
            foreach ((int x, int y) in region)
            {
                sides = points[(x, y)];
                if (!sides.Left && TraverseVerticalSide(region, height, points, x, y, x - 1, checkedSides => checkedSides.Left = true))
                    result += 1;

                if (!sides.Top && TraverseHorizontalSide(region, width, points, x, y, y - 1, checkedSides => checkedSides.Top = true))
                    result += 1;

                if (!sides.Right && TraverseVerticalSide(region, height, points, x, y, x + 1, checkedSides => checkedSides.Right = true))
                    result += 1;

                if (!sides.Bottom && TraverseHorizontalSide(region, width, points, x, y, y + 1, checkedSides => checkedSides.Bottom = true))
                    result += 1;

                points[(x, y)].SetAllChecked();
            }

            return result;
        }

        private static bool TraverseVerticalSide(
            ISet<(int, int)> region, int height, Dictionary<(int, int), CheckedSides> points,
            int x, int y, int outsideX, Action<CheckedSides> sideUpdater
        )
        {
            if (!region.Contains((outsideX, y)))
            {
                for (int j = y - 1; j >= 0; --j)
                {
                    if (!region.Contains((x, j)))
                        break;

                    if (!region.Contains((outsideX, j))) sideUpdater(points[(x, j)]);
                    else break;
                }
                for (int j = y + 1; j < height; ++j)
                {
                    if (!region.Contains((x, j)))
                        break;

                    if (!region.Contains((outsideX, j))) sideUpdater(points[(x, j)]);
                    else break;
                }
                return true;
            }
            return false;
        }

        private static bool TraverseHorizontalSide(
            ISet<(int, int)> region, int width, Dictionary<(int, int), CheckedSides> points,
            int x, int y, int outsideY, Action<CheckedSides> sideUpdater
        )
        {
            if (!region.Contains((x, outsideY)))
            {
                for (int i = x - 1; i >= 0; --i)
                {
                    if (!region.Contains((i, y)))
                        break;

                    if (!region.Contains((i, outsideY))) sideUpdater(points[(i, y)]);
                    else break;
                }
                for (int i = x + 1; i < width; ++i)
                {
                    if (!region.Contains((i, y)))
                        break;

                    if (!region.Contains((i, outsideY))) sideUpdater(points[(i, y)]);
                    else break;
                }
                return true;
            }
            return false;
        }

        private class CheckedSides
        {
            public bool Left = false;
            public bool Top = false;
            public bool Right = false;
            public bool Bottom = false;

            public bool IsAllChecked() => Left && Top && Right && Bottom;

            public void SetAllChecked()
            {
                Left = true;
                Top = true;
                Right = true;
                Bottom = true;
            }
        }
    }
}