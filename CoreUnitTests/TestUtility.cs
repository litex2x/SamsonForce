using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePound.SamsonForce.CoreUnitTests
{
    class TestUtility
    {
        public static byte[] HexStringToByteArray(string hexValue)
        {
            return Enumerable.Range(0, hexValue.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hexValue.Substring(x, 2), 16))
                     .ToArray();
        }
    }
}
