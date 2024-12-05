using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day04
{
    public class Day04 : IDay
    {
        private static readonly string _TOKEN = "XMAS";

        public string PartOne()
        {
            var input = File.ReadAllLines(InputHelper.GetInputPath(4));
            uint result = 0;

            for (int y = 0; y < input.Length; ++y)
            {
                for (int x = 0; x < input[y].Length; ++x)
                {
                    if (Match(input, x, y, 0) || Match(input, x, y, _TOKEN.Length - 1))
                    {
                        bool forward = Match(input, x, y, 0);
                        if (LookDown(input, x, y, forward))
                            ++result;
                        if (LookDiagonalyDownBack(input, x, y, forward))
                            ++result;
                        if (LookDiagonalyDownForward(input, x, y, forward))
                            ++result;
                        if (LookRight(input, x, y, forward))
                        {
                            ++result;
                            x += _TOKEN.Length - 2;
                        }
                    }
                }
            }

            return result.ToString();
        }

        private static bool LookRight(string[] input, int x, int y, bool forward)
        {
            if (x + _TOKEN.Length > input[y].Length)
                return false;

            return Lookup(input, x, y, forward, moveX: (x) => x + 1);
        }

        private static bool LookDown(string[] input, int x, int y, bool forward)
        {
            if (y + _TOKEN.Length > input.Length)
                return false;

            return Lookup(input, x, y, forward, moveY: (y) => y + 1);
        }

        private static bool LookDiagonalyDownBack(string[] input, int x, int y, bool forward)
        {
            if (y + _TOKEN.Length > input.Length || x < _TOKEN.Length - 1)
                return false;

            return Lookup(input, x, y, forward, (x) => x - 1, (y) => y + 1);
        }

        private static bool LookDiagonalyDownForward(string[] input, int x, int y, bool forward)
        {
            if (y + _TOKEN.Length > input.Length || x + _TOKEN.Length > input[y].Length)
                return false;

            return Lookup(input, x, y, forward, (x) => x + 1, (y) => y + 1);
        }

        private static bool Lookup(string[] input, int x, int y, bool forward, Func<int, int> moveX = null, Func<int, int> moveY = null)
        {
            moveX ??= (it => it);
            moveY ??= (it => it);

            int i = forward ? 1 : _TOKEN.Length - 2;
            int curX = moveX(x);
            int curY = moveY(y);
            while (Match(input, curX, curY, i))
            {
                curX = moveX(curX);
                curY = moveY(curY);

                i = forward ? i + 1 : i - 1;
                if ((forward && i >= _TOKEN.Length) || (!forward && i < 0))
                    return true;
            }
            return false;
        }

        private static bool Match(string[] input, int x, int y, int i) => input[y][x] == _TOKEN[i];

        public string PartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
