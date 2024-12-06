using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day04
{
    public class Day04 : IDay
    {
        private static readonly string _FIRST_TOKEN = "XMAS";
        private static readonly string _SECOND_TOKEN = "MAS";

        public string PartOne()
        {
            var input = File.ReadAllLines(InputHelper.GetInputPath(4));
            uint result = 0;

            for (int y = 0; y < input.Length; ++y)
            {
                for (int x = 0; x < input[y].Length; ++x)
                {
                    if (input[y][x] == _FIRST_TOKEN[0] || input[y][x] == _FIRST_TOKEN[^1])
                    {
                        bool forward = input[y][x] == _FIRST_TOKEN[0];
                        if (LookDown(input, x, y, _FIRST_TOKEN, forward))
                            ++result;
                        if (LookDiagonalyDownBack(input, x, y, _FIRST_TOKEN, forward))
                            ++result;
                        if (LookDiagonalyDownForward(input, x, y, _FIRST_TOKEN, forward))
                            ++result;
                        if (LookRight(input, x, y, _FIRST_TOKEN, forward))
                        {
                            ++result;
                            x += _FIRST_TOKEN.Length - 2;
                        }
                    }
                }
            }

            return result.ToString();
        }

        public string PartTwo()
        {
            var input = File.ReadAllLines(InputHelper.GetInputPath(4));
            uint result = 0;

            for (int y = 0; y <= input.Length - _SECOND_TOKEN.Length; ++y)
            {
                for (int x = 0; x <= input[y].Length - _SECOND_TOKEN.Length; ++x)
                {
                    if (
                        (input[y][x] == _SECOND_TOKEN[0] || input[y][x] == _SECOND_TOKEN[^1])
                        && (input[y][x + 2] == _SECOND_TOKEN[0] || input[y][x + 2] == _SECOND_TOKEN[^1])
                    )
                    {
                        bool forward = input[y][x] == _SECOND_TOKEN[0];
                        if (!LookDiagonalyDownForward(input, x, y, _SECOND_TOKEN, forward))
                            continue;
                        forward = input[y][x + 2] == _SECOND_TOKEN[0];
                        if (LookDiagonalyDownBack(input, x + 2, y, _SECOND_TOKEN, forward))
                            ++result;
                    }
                }
            }

            return result.ToString();
        }

        private static bool LookRight(string[] input, int x, int y, string token, bool forward)
        {
            if (x + token.Length > input[y].Length)
                return false;

            return Lookup(input, x, y, token, forward, moveX: (x) => x + 1);
        }

        private static bool LookDown(string[] input, int x, int y, string token, bool forward)
        {
            if (y + token.Length > input.Length)
                return false;

            return Lookup(input, x, y, token, forward, moveY: (y) => y + 1);
        }

        private static bool LookDiagonalyDownBack(string[] input, int x, int y, string token, bool forward)
        {
            if (y + token.Length > input.Length || x < token.Length - 1)
                return false;

            return Lookup(input, x, y, token, forward, (x) => x - 1, (y) => y + 1);
        }

        private static bool LookDiagonalyDownForward(string[] input, int x, int y, string token, bool forward)
        {
            if (y + token.Length > input.Length || x + token.Length > input[y].Length)
                return false;

            return Lookup(input, x, y, token, forward, (x) => x + 1, (y) => y + 1);
        }

        private static bool Lookup(string[] input, int x, int y, string token, bool forward, Func<int, int> moveX = null, Func<int, int> moveY = null)
        {
            moveX ??= (it => it);
            moveY ??= (it => it);

            int i = forward ? 1 : token.Length - 2;
            int curX = moveX(x);
            int curY = moveY(y);
            while (input[curY][curX] == token[i])
            {
                curX = moveX(curX);
                curY = moveY(curY);

                i = forward ? i + 1 : i - 1;
                if ((forward && i >= token.Length) || (!forward && i < 0))
                    return true;
            }
            return false;
        }
    }
}
