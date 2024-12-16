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

        public string PartTwo()
        {
            byte[] diskMap = File.ReadAllText(InputHelper.GetInputPath(9)).Select(c => (byte)(c - '0')).ToArray();

            ulong checkSum = 0;
            int positionInMap = 0;
            int positionOnDisk = 0;
            int blockSize = diskMap[positionInMap];
            int positionOfLastFile = diskMap.Length - 1;
            int positionInFile;
            ISet<int> movedFileIds = new HashSet<int>();
            while (positionInMap < diskMap.Length)
            {
                if (positionInMap % 2 == 0) // file
                {
                    if (!movedFileIds.Contains(positionInMap))
                    {
                        for (positionInFile = 0; positionInFile < diskMap[positionInMap]; ++positionInFile, ++positionOnDisk)
                        {
                            checkSum += CalculatePositionValue(positionOnDisk, positionInMap);
                        }
                    }
                    else
                    {
                        positionOnDisk += diskMap[positionInMap];
                    }
                    ++positionInMap;
                    if (positionInMap < diskMap.Length)
                    {
                        blockSize = diskMap[positionInMap];
                    }
                }
                else if (positionOfLastFile > positionInMap && blockSize > 0) // empty space and there might be files to move
                {
                    while (positionOfLastFile > positionInMap)
                    {
                        if (movedFileIds.Contains(positionOfLastFile))
                        {
                            positionOfLastFile -= 2;
                            continue;
                        }
                        if (blockSize >= diskMap[positionOfLastFile])
                            break;
                        positionOfLastFile -= 2;
                    }
                    if (positionOfLastFile <= positionInMap)
                        continue;

                    for (positionInFile = 0; positionInFile < diskMap[positionOfLastFile]; ++positionInFile, ++positionOnDisk)
                    {
                        checkSum += CalculatePositionValue(positionOnDisk, positionOfLastFile);
                    }
                    movedFileIds.Add(positionOfLastFile);
                    blockSize -= diskMap[positionOfLastFile];
                    positionOfLastFile -= 2;
                }
                else // empty space and all files have been moved
                {
                    ++positionInMap;
                    if (positionInMap < diskMap.Length)
                    {
                        positionOnDisk += blockSize;
                        blockSize = diskMap[positionInMap];

                        positionOfLastFile = diskMap.Length - 1;
                        while (movedFileIds.Contains(positionOfLastFile) && positionOfLastFile > positionInMap)
                            positionOfLastFile -= 2;
                    }
                }
            }

            return checkSum.ToString();
        }

        private static ulong CalculatePositionValue(int positionOnDisk, int positionInMap) => (ulong)positionOnDisk * ((ulong)positionInMap / 2);
    }
}
