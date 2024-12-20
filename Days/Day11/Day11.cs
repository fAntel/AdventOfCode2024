using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day11
{
    internal class Day11 : IDay
    {
        public string PartOne() => SimulateBlinking(25).ToString();

        public string PartTwo() => SimulateBlinking(75).ToString();

        private static ulong SimulateBlinking(byte blinkingCount)
        {
            Dictionary<ulong, ulong> stones = File.ReadAllText(InputHelper.GetInputPath(11))
                            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                            .Select(ulong.Parse)
                            .Aggregate(new Dictionary<ulong, ulong>(), (acc, l) =>
                            {
                                acc[l] = acc.TryGetValue(l, out ulong value) ? value + 1 : 1;
                                return acc;
                            })
                            .ToDictionary();

            Dictionary<ulong, ulong> buffer = [];
            Dictionary<ulong, Tuple<ulong, ulong>?> dict = [];
            ulong t, newStone;
            for (byte i = 0; i < blinkingCount; ++i)
            {
                buffer.Clear();
                foreach (var (stone, count) in stones)
                {
                    if (stone == 0)
                    {
                        buffer[1] = buffer.TryGetValue(1, out t) ? t + count : count;
                        continue;
                    }

                    dict.TryGetValue(stone, out Tuple<ulong, ulong>? splitStones);
                    if (splitStones == null)
                    {
                        splitStones = DivideIfEvenTens(stone);
                        if (splitStones != null)
                            dict[stone] = splitStones;
                    }
                    if (splitStones != null)
                    {
                        buffer[splitStones.Item1] = buffer.TryGetValue(splitStones.Item1, out t) ? t + count : count;
                        buffer[splitStones.Item2] = buffer.TryGetValue(splitStones.Item2, out t) ? t + count : count;
                    }
                    else
                    {
                        newStone = stone * 2024;
                        buffer[newStone] = buffer.TryGetValue(newStone, out t) ? t + count : count;
                    }
                }

                (buffer, stones) = (stones, buffer);
            }

            return stones.Values.Aggregate((ulong)0, (acc, l) => acc + l);
        }

        private static Tuple<ulong, ulong>? DivideIfEvenTens(ulong stone)
        {
            byte i = 1;
            ulong divider = 10;
            for (; stone % divider != stone; ++i, divider *= 10) ;
            if (i % 2 != 0)
                return null;

            divider = 1;
            for (i /= 2; i > 0; --i, divider *= 10) ;
            return new Tuple<ulong, ulong>(stone % divider, stone / divider);
        }
    }
}
