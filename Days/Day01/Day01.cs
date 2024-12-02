using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day01
{
    public class Day01 : IDay
    {
        public string PartOne()
        {
            IEnumerable<IOrderedEnumerable<int>> lists = ReadLists();

            var result = lists.First()
                .Zip(lists.Last(), (a, b) => Math.Abs(a - b))
                .Sum();

            return result.ToString();
        }

        public string PartTwo()
        {
            IEnumerable<IOrderedEnumerable<int>> lists = ReadLists();

            var firstList = lists.First();
            var secondList = lists.Last().ToArray();
            int i = 0;
            int counter = 0;
            int? prevItem = null;
            long result = 0;
            foreach (var item in firstList)
            {
                if (prevItem != null && prevItem == item)
                {
                    result += item * counter;
                    continue;
                }

                counter = 0;

                while (i < secondList.Length && secondList[i] < item)
                    ++i;
                if (i >= secondList.Length)
                    break;

                while (i < secondList.Length && item == secondList[i])
                {
                    ++i;
                    ++counter;
                }

                result += item * counter;

                if (i >= secondList.Length)
                    break;

                if (i > 0)
                    --i;

                prevItem = item;
            }

            return result.ToString();
        }

        private static IEnumerable<IOrderedEnumerable<int>> ReadLists()
        {
            return File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "Days", "Day01", "input"))
                .Select(line => line.Split(' '))
                .Select(line => line.Where(v => v.Length > 0))
                .Select(values => values.Select(s => int.Parse(s)))
                .Aggregate(new List<List<int>>(2) { new(), new() }, (acc, v) =>
                {
                    acc[0].Add(v.First());
                    acc[1].Add(v.Last());
                    return acc;
                })
                .Select(list => list.Order());
        }
    }
}
