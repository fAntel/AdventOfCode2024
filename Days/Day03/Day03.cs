using AdventOfCode2024.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days.Day03
{
    public class Day03 : IDay
    {
        public string PartOne()
        {
            var input = File.ReadAllText(InputHelper.GetInputPath(3));

            ICollection<IOperation> operations = new LinkedList<IOperation>();
            Parser parser = new();
            int i = 0;
            while (i >= 0 && i < input.Length)
            {
                i = parser.Parse(false, i, input, operations);
            }

            int result = operations.Select(operation => operation.Run()).Sum();

            return result.ToString();
        }

        public string PartTwo()
        {
            var input = File.ReadAllText(InputHelper.GetInputPath(3));

            ICollection<IOperation> operations = new LinkedList<IOperation>();
            Parser parser = new();
            bool isParsingDisabled = false;
            int i = 0;
            while (i >= 0 && i < input.Length)
            {
                i = parser.Parse(isParsingDisabled, i, input, operations);
                if (operations.Count > 0)
                {
                    isParsingDisabled = operations.Last() is DontOperation;
                }
            }

            int result = operations.Select(operation => operation.Run()).Sum();

            return result.ToString();
        }
    }

    file class Parser
    {
        private char _lookahead;
        private int _index;

        internal int Parse(bool isParsingDisabled, int i, string input, ICollection<IOperation> operations)
        {
            if (!PrepareState(i, input))
                return -1;

            if (isParsingDisabled)
            {
                return ParseDo(i, input, operations);
            }
            else
            {
                if (_lookahead == 'm')
                {
                    return ParseMul(i, input, operations);
                }
                else if (_lookahead == 'd')
                {
                    return ParseDont(i, input, operations);
                }
                else
                {
                    return i + 1;
                }
            }
        }

        private int ParseDo(int i, string input, ICollection<IOperation> operations)
        {
            if (Do(input) && OpeningBracket(input) && ClosingBracket(input))
            {
                operations.Add(new DoOperation());
            }
            return _index;
        }

        private int ParseDont(int i, string input, ICollection<IOperation> operations)
        {
            if (Dont(input) && OpeningBracket(input) && ClosingBracket(input))
            {
                operations.Add(new DontOperation());
            }
            return _index;
        }

        private int ParseMul(int i, string input, ICollection<IOperation> operations)
        {
            if (!PrepareState(i, input))
                return -1;

            if (!Mul(input))
                return _index;

            if (!OpeningBracket(input))
                return _index;

            ushort firstFactor = 0;
            if (Char.IsAsciiDigit(_lookahead))
            {
                firstFactor = Number(input);
            } else
            {
                return _index;
            }

            if (!Comma(input))
                return _index;

            ushort secondFactor = 0;
            if (Char.IsAsciiDigit(_lookahead))
            {
                secondFactor = Number(input);
            }
            else
            {
                return _index;
            }

            if (!ClosingBracket(input))
                return _index;

            operations.Add(new MulOperation(firstFactor, secondFactor));

            return _index;
        }

        private bool PrepareState(int i, string input)
        {
            if (i >= input.Length)
                return false;

            _lookahead = input[i];
            _index = i;

            return true;
        }

        private bool Dont(string input)
        {
            bool hasMatch = Match('d', input);
            if (!hasMatch)
            {
                ++_index;
                return false;
            }
            return Match('o', input) && Match('n', input) && Match('\'', input) && Match('t', input);
        }

        private bool Do(string input)
        {
            bool hasMatch = Match('d', input);
            if (!hasMatch)
            {
                ++_index;
                return false;
            }
            return Match('o', input);
        }

        private bool Mul(string input)
        {
            bool hasMatch = Match('m', input);
            if (!hasMatch)
            {
                ++_index;
                return false;
            }
            return Match('u', input) && Match('l', input);
        }

        private ushort Number(string input)
        {
            ushort value = 0;
            do
            {
                value = (ushort)(value * 10 + (_lookahead - '0'));
                Match(_lookahead, input);
            } while (Char.IsAsciiDigit(_lookahead));
            return value;
        }

        private bool OpeningBracket(string input) => Match('(', input);

        private bool Comma(string input) => Match(',', input);

        private bool ClosingBracket(string input) => Match(')', input);

        private bool Match(char c, string input)
        {
            if (_lookahead == c)
            {
                ++_index;
                _lookahead = _index >= input.Length ? Convert.ToChar(0) : input[_index];
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    file interface IOperation
    {
        internal int Run();
    }

    file class MulOperation : IOperation
    {
        private readonly ushort _firstFactor;
        private readonly ushort _secondFactor;

        internal MulOperation(ushort firstFactor, ushort secondFactor)
        {
            _firstFactor = firstFactor;
            _secondFactor = secondFactor;
        }

        int IOperation.Run() => _firstFactor * _secondFactor;
    }

    file class DoOperation : IOperation
    {
        int IOperation.Run() => 0;
    }

    file class DontOperation : IOperation
    {
        int IOperation.Run() => 0;
    }
}
