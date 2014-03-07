using System.Linq;
using System.Text;
using Org.BouncyCastle.Math;


namespace CodePound.SamsonForce.Core
{
    public class Base58Check
    {
        private const string code = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        // https://en.bitcoin.it/wiki/Base58Check_encoding
        public string Encode(byte[] value)
        {
            StringBuilder result = new StringBuilder();
            BigInteger scalar = new BigInteger(value);
            BigInteger divider = BigInteger.ValueOf(58);

            while(scalar.CompareTo(BigInteger.Zero) > 0 )
            {
                result.Append(code[scalar.Mod(divider).IntValue]);
                scalar = scalar.Divide(divider);
            }

            for (int i = 0; i < value.Length && value[i] == 0; i++)
            {
                result.Append(code[0]);
            }

            return new string(result.ToString().ToArray<char>().Reverse().ToArray<char>());
        }
    }
}
