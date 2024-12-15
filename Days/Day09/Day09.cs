using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days.Day09
{
    internal class Day09 : IDay
    {
        public string PartOne()
        {
            byte[] diskMap = File.ReadAllText(InputHelper.GetInputPath(9)).Select(c => (byte) (c - '0')).ToArray();

            ulong checkSum = 0;
            int positionInMap = 0;
            int positionOnDisk = 0;
            int positionInBlock = 0;
            int positionOfLastFile = diskMap.Length - 1;
            int positionInLastFile = 0;
            while (positionInMap < diskMap.Length && positionInMap <= positionOfLastFile)
            {
                if (positionInBlock < diskMap[positionInMap])
                {
                    if (positionInMap % 2 == 0 && positionInMap != positionOfLastFile) // file
                    {
                        checkSum += CalculatePositionValue(positionOnDisk, positionInMap);
                    }
                    else // empty space
                    {
                        if (positionInLastFile < diskMap[positionOfLastFile]) // just calculate check sum as if file moved
                        {
                            checkSum += CalculatePositionValue(positionOnDisk, positionOfLastFile);
                            ++positionInLastFile;
                        }
                        else // file moved completely, go to next rightmost file
                        {
                            positionInLastFile = 0;
                            positionOfLastFile -= 2;
                            continue;
                        }
                    }
                    ++positionOnDisk;
                    ++positionInBlock;
                }
                else
                {
                    positionInBlock = 0;
                    ++positionInMap;
                }
            }

            return checkSum.ToString();
        }

        private static ulong CalculatePositionValue(int positionOnDisk, int positionInMap) => (ulong)positionOnDisk * ((ulong)positionInMap / 2);

        public string PartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
