using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day07
{
    internal class Day07 : IDay
    {
        public string PartOne()
        {
            ISet<Operation> operations = new HashSet<Operation>() { Operation.Addition, Operation.Multiplication };
            return CalculateTotalCalibrationResult(operations).ToString();
        }

        public string PartTwo()
        {
            ISet<Operation> operations = new HashSet<Operation>() { Operation.Addition, Operation.Multiplication, Operation.Concatenation };
            return CalculateTotalCalibrationResult(operations).ToString();
        }

        private static ulong CalculateTotalCalibrationResult(ISet<Operation> operations)
        {
            IEnumerable<CalibrationEquation> input = File.ReadLines(InputHelper.GetInputPath(7))
                .Select(line => {
                    int colonIndex = line.IndexOf(':');
                    ulong textValue = ulong.Parse(line[..colonIndex]);
                    uint[] numbers = line[(colonIndex + 1)..]
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(str => uint.Parse(str))
                        .ToArray();
                    return new CalibrationEquation(textValue, numbers);
                });

            ulong sum = 0;
            foreach (CalibrationEquation calibrationEquation in input)
            {
                if (TestCalibrationEquation(calibrationEquation.textValue, calibrationEquation.numbers, calibrationEquation.numbers.Length - 1, operations))
                {
                    sum += calibrationEquation.textValue;
                }
            }

            return sum;
        }

        private static bool TestCalibrationEquation(ulong testValue, uint[] numbers, int lastIndex, ISet<Operation> operations)
        {
            if (lastIndex > 0)
            {
                ulong lastNumber = numbers[lastIndex];

                if (operations.Contains(Operation.Addition))
                {
                    if (TestCalibrationEquation(testValue - lastNumber, numbers, lastIndex - 1, operations))
                        return true;
                }

                if (operations.Contains(Operation.Multiplication))
                {
                    if (testValue % lastNumber == 0 && TestCalibrationEquation(testValue / lastNumber, numbers, lastIndex - 1, operations))
                        return true;
                }

                if (operations.Contains(Operation.Concatenation))
                {
                    ulong divider = CalculateDivider(lastNumber);
                    if (testValue % divider == lastNumber)
                        return TestCalibrationEquation(testValue / divider, numbers, lastIndex - 1, operations);
                }

                return false;
            }
            else
            {
                return testValue == numbers[lastIndex];
            }
        }

        private static ulong CalculateDivider(ulong number)
        {
            ulong result = 10;
            for (; number % result != number; result *= 10) ;
            return result;
        }

        private struct CalibrationEquation(ulong testValue, uint[] numbers)
        {
            public ulong textValue = testValue;
            public uint[] numbers = numbers;
        }

        private enum Operation { Addition, Multiplication, Concatenation }
    }
}
