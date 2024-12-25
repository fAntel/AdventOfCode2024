using AdventOfCode2024.Common;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days.Day13
{
    internal class Day13 : IDay
    {
        private static readonly ulong s_buttonAPushCost = 3;
        private static readonly ulong s_buttonBPushCost = 1;
        private static readonly byte s_maxButtonPushes = 100;

        public string PartOne()
        {
            ulong result = 0;
            foreach (Machine machine in ReadInput())
            {
                result += CalculateMinCost(machine.PrizeCoordinates.X, machine.PrizeCoordinates.Y, machine.ButtonADeltas, machine.ButtonBDeltas);
            }

            return result.ToString();
        }

        public string PartTwo()
        {
            return "";
        }

        private static IEnumerable<Machine> ReadInput() =>
            File.ReadLines(InputHelper.GetInputPath(13))
                .Where(s => s.Length > 0)
                .Chunk(3)
                .Select(strigns => new Machine(strigns));

        private static ulong CalculateMinCost(long X, long Y, ButtonDeltas buttonADeltas, ButtonDeltas buttonBDeltas)
        {
            ulong minCost = 0, cost = 0, savedCost;
            byte AX = buttonADeltas.X, AY = buttonADeltas.Y;
            byte BX = buttonBDeltas.X, BY = buttonBDeltas.Y;
            byte buttonPushesCount = 0;
            long possibleBPushes;
            do
            {
                possibleBPushes = X / BX;
                if (possibleBPushes <= s_maxButtonPushes && X % BX == 0 && Y % BY == 0 && possibleBPushes == Y / BY)
                {
                    savedCost = cost;
                    cost += (ulong)possibleBPushes * s_buttonBPushCost;
                    if (minCost == 0 || minCost > cost)
                        minCost = cost;
                    cost = savedCost;
                }
                X -= AX; Y -= AY;
                cost += s_buttonAPushCost;
                ++buttonPushesCount;
                if (X == 0 && Y == 0)
                {
                    if (minCost == 0 || minCost > cost)
                        minCost = cost;
                    break;
                }
                if (X < 0 || Y < 0)
                    break;
            } while (buttonPushesCount <= s_maxButtonPushes);
            return minCost;
        }

        private readonly struct Machine
        {
            private static readonly string s_buttonInputPattern = "Button .{1}: X\\+(\\d+), Y\\+(\\d+)";
            private static readonly string s_prizeCoordinatesInputPattern = "Prize: X=(\\d+), Y=(\\d+)";

            public readonly ButtonDeltas ButtonADeltas;
            public readonly ButtonDeltas ButtonBDeltas;
            public readonly Point PrizeCoordinates;

            public Machine(string[] input)
            {
                var matchResult = Regex.Match(input[0], s_buttonInputPattern);
                ButtonADeltas = new ButtonDeltas(byte.Parse(matchResult.Groups[1].Value), byte.Parse(matchResult.Groups[2].Value));
                matchResult = Regex.Match(input[1], s_buttonInputPattern);
                ButtonBDeltas = new ButtonDeltas(byte.Parse(matchResult.Groups[1].Value), byte.Parse(matchResult.Groups[2].Value));
                matchResult = Regex.Match(input[2], s_prizeCoordinatesInputPattern);
                PrizeCoordinates = new Point(int.Parse(matchResult.Groups[1].Value), int.Parse(matchResult.Groups[2].Value));
            }
        }

        private readonly struct ButtonDeltas(byte X, byte Y)
        {
            public readonly byte X = X;
            public readonly byte Y = Y;

            public override readonly bool Equals(object? obj)
            {
                return obj is ButtonDeltas deltas &&
                       X == deltas.X &&
                       Y == deltas.Y;
            }

            public override readonly int GetHashCode() => HashCode.Combine(X, Y);
        }
    }
}