using AdventOfCode2024.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days.Day05
{
    public class Day05 : IDay
    {
        public string PartOne()
        {
            var input = File.ReadLines(InputHelper.GetInputPath(5));
            var orderingRules = input
                .TakeWhile(line => line.Length > 0) // take only ordering rules
                .Aggregate(new Dictionary<int, ISet<int>>(), (acc, line) =>
                {
                    string[] substrs = line.Split('|');
                    int firstPageNumber = int.Parse(substrs[0]);
                    ISet<int>? secondPageNumers;
                    if (!acc.TryGetValue(firstPageNumber, out secondPageNumers))
                    {
                        secondPageNumers = new HashSet<int>();
                        acc.Add(firstPageNumber, secondPageNumers);
                    }
                    secondPageNumers.Add(int.Parse(substrs[1]));
                    return acc;
                });
            var updates = input
                .SkipWhile(line => line.Length > 0) // skip ordering rules
                .Skip(1)                            // skip empty line between ordering rules and updates
                .Select(line => line.Split(",").Select(pageNumber => int.Parse(pageNumber)));

            List<int> middlePages = [];
            ISet<int>? yPages;
            ISet<int> passedPages = new HashSet<int>();
            int count;
            foreach (var update in updates)
            {
                count = 0;
                passedPages.Clear();
                foreach (var page in update)
                {
                    if (passedPages.Count > 0)
                    {
                        if (orderingRules.TryGetValue(page, out yPages))
                        {
                            if (yPages.Intersect(passedPages).Count() > 0)
                            {
                                count = 0;
                                break;
                            }
                        }
                    }

                    passedPages.Add(page);
                    ++count;
                }

                if (count > 0)
                {
                    middlePages.Add(update.Skip(count / 2).First());
                }
            }

            return middlePages.Sum().ToString();
        }

        public string PartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
