using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day02
{
    internal class Day02 : IDay
    {
        public string PartOne() => GetLevels().Where(IsSafe).Count().ToString();

        public string PartTwo() =>
            GetLevels()
            .Where(peport =>
            {
                List<byte> levels = peport.ToList();
                byte droppedLevel;
                for (int i = 0; i < levels.Count; ++i)
                {
                    droppedLevel = levels[i];
                    levels.RemoveAt(i);
                    if (IsSafe(levels))
                        return true;
                    levels.Insert(i, droppedLevel);
                }
                return false;
            })
            .Count()
            .ToString();

        private static IEnumerable<IEnumerable<byte>> GetLevels()
        {
            var lines = File.ReadLines(InputHelper.GetInputPath(2));
            var levels = lines.Select(lines => lines.Split(' ').Select(level => byte.Parse(level)));
            return levels;
        }

        private static bool IsSafe(IEnumerable<byte> levels)
        {
            byte prevLevel = 0;
            int sign = 0;
            int diff;
            int abs;
            foreach (byte level in levels)
            {
                if (prevLevel <= 0)
                {
                    prevLevel = level;
                    continue;
                }

                diff = level - prevLevel;
                abs = Math.Abs(diff);
                if (abs < 1 || abs > 3)
                    return false;

                prevLevel = level;

                if (sign == 0)
                {
                    sign = Math.Sign(diff);
                }
                else
                {
                    if (sign != Math.Sign(diff))
                        return false;
                }
            }
            return true;
        }
    }
}
