using System.Linq;
using System.Security.Cryptography;

namespace CodePound.SamsonForce.Core
{
    public class BitcoinAddress
    {
        private const byte Version = 0x00;
        
        public byte[] Address { get; private set; }

        // https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
        public BitcoinAddress(byte[] publicKey)
        {
            byte[] address = null;
            byte[] checksum = null;

            using (SHA256 sha256Hasher = SHA256Managed.Create())
            {
                using (RIPEMD160 ripemd160Hasher = RIPEMD160Managed.Create())
                {
                    address = new byte[] { Version }.Concat<byte>(ripemd160Hasher.ComputeHash(sha256Hasher.ComputeHash(publicKey))).ToArray<byte>();
                    checksum = sha256Hasher.ComputeHash(sha256Hasher.ComputeHash(address)).Take<byte>(4).ToArray<byte>();
                    Address = address.Concat<byte>(checksum).ToArray<byte>();
                }
            }
        }
    }
}
