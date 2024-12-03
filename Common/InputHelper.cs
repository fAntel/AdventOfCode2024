using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Common
{
    internal class InputHelper
    {
        internal static string GetInputPath(byte day) => Path.Combine(Environment.CurrentDirectory, "Days", $"Day{day:D2}", "input");
    }
}
