using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CodePound.SamsonForce.Core
{
    public class BitcoinKeyPair
    {
        private readonly X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");

        public byte[] PrivateKey { get; private set; }
        public byte[] PublicKey { get; private set; }

        public BitcoinKeyPair(string paraphrase)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                PrivateKey = hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(paraphrase));
            }

            PublicKey = GetPublicKey(PrivateKey);
        }

        public BitcoinKeyPair(byte[] privateKey)
        {
            PrivateKey = privateKey;
            PublicKey = GetPublicKey(PrivateKey);
        }

        // http://bitcoin.stackexchange.com/questions/20450/public-key-from-private-key-generation-problem
        private byte[] GetPublicKey(byte[] privateKey)
        {
            BigInteger secretComponent = new BigInteger(1, privateKey);
            ECPoint point = curve.G.Multiply(secretComponent);
            BigInteger x = point.X.ToBigInteger();
            BigInteger y = point.Y.ToBigInteger();

            return new byte[] { 0x04 }.Concat(x.ToByteArrayUnsigned()).Concat(y.ToByteArrayUnsigned()).ToArray();
        }
    }
}
