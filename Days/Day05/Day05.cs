using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day05
{
    public class Day05 : IDay
    {
        public string PartOne()
        {
            Dictionary<int, ISet<int>> orderingRules;
            List<List<int>> updates;
            ParseInput(out orderingRules, out updates);

            int result = 0;
            foreach (var update in updates)
            {
                Dictionary<WrongPage, ISet<int>> wrongUpdates = CheckUpdates(update, orderingRules, returnOnWrong: true);

                if (wrongUpdates.Count == 0)
                {
                    result += update[update.Count / 2];
                }
            }

            return result.ToString();
        }

        public string PartTwo()
        {
            Dictionary<int, ISet<int>> orderingRules;
            List<List<int>> updates;
            ParseInput(out orderingRules, out updates);

            int result = 0;
            foreach (var update in updates)
            {
                Dictionary<WrongPage, ISet<int>> wrongUpdates;
                wrongUpdates = CheckUpdates(update, orderingRules, returnOnWrong: false);

                if (wrongUpdates.Count == 0)
                    continue;

                ISet<int> rightPages;
                WrongPage wrongPage;
                while (wrongUpdates.Count > 0)
                {
                    wrongPage = wrongUpdates.First().Key;
                    rightPages = orderingRules[wrongPage.pageNumber].ToHashSet();
                    rightPages.UnionWith(wrongUpdates[wrongPage]);

                    for (int i = 0; i < update.Count; ++i)
                    {
                        if (rightPages.Contains(update[i]))
                        {
                            update.RemoveAt(wrongPage.Index);
                            update.Insert(i, wrongPage.pageNumber);
                            wrongUpdates.Remove(wrongPage);

                            Dictionary<WrongPage, ISet<int>> t = wrongUpdates;
                            wrongUpdates = [];
                            int wrongPageOldIndex = wrongPage.Index;
                            int wrongPageNewIndex = i;
                            foreach (var key in t.Keys)
                            {
                                wrongPage = wrongPageNewIndex <= key.Index && key.Index <= wrongPageOldIndex
                                    ? new WrongPage(key.pageNumber, key.Index + 1)
                                    : key;
                                wrongUpdates[wrongPage] = t[key];
                            }

                            break;
                        }
                    }
                }

                result += update[update.Count / 2];
            }

            return result.ToString();
        }

        private static void ParseInput(out Dictionary<int, ISet<int>> orderingRules, out List<List<int>> updates)
        {
            orderingRules = [];
            updates = [];
            bool hasOrderingRulesFinished = false;
            foreach (var line in File.ReadLines(InputHelper.GetInputPath(5)))
            {
                if (line.Length == 0)
                {
                    hasOrderingRulesFinished = true;
                    continue;
                }
                if (!hasOrderingRulesFinished)
                {
                    string[] substrs = line.Split('|');
                    int firstPageNumber = int.Parse(substrs[0]);
                    if (!orderingRules.TryGetValue(firstPageNumber, out ISet<int>? secondPageNumers))
                    {
                        secondPageNumers = new HashSet<int>();
                        orderingRules.Add(firstPageNumber, secondPageNumers);
                    }
                    secondPageNumers.Add(int.Parse(substrs[1]));
                }
                else
                {
                    updates.Add(line.Split(",").Aggregate(new List<int>(), (acc, pageNumber) => { acc.Add(int.Parse(pageNumber)); return acc; }));
                }

            }
        }

        private static Dictionary<WrongPage, ISet<int>> CheckUpdates(List<int> update, Dictionary<int, ISet<int>> orderingRules, bool returnOnWrong = true)
        {
            ISet<int> passedPages = new HashSet<int>();
            Dictionary<WrongPage, ISet<int>> wrongPages = [];
            for (int i = 0; i < update.Count; i++)
            {
                int page = update[i];
                if (passedPages.Count > 0)
                {
                    ISet<int>? yPages;
                    if (orderingRules.TryGetValue(page, out yPages))
                    {
                        IEnumerable<int> pagesShouldBeEarlier = yPages.Intersect(passedPages);
                        if (pagesShouldBeEarlier.Any())
                        {
                            wrongPages.Add(new WrongPage(page, i), pagesShouldBeEarlier.ToHashSet());
                            if (returnOnWrong)
                                break;
                        }
                    }
                }

                passedPages.Add(page);
            }
            return wrongPages;
        }

        private struct WrongPage(int pageNumber, int index)
        {
            public int pageNumber = pageNumber;
            public int Index = index;
        }
    }
}
