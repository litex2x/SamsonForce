using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodePound.SamsonForce.Core;

namespace CodePound.SamsonForce.CoreUnitTests
{
    [TestClass]
    public class BitcoinAddressUnitTests
    {
        [TestMethod]
        public void AddressPropertyTest()
        {
            byte[] expectedAddress = TestUtility.HexStringToByteArray("00010966776006953D5567439E5E39F86A0D273BEED61967F6");
            byte[] privateKey = TestUtility.HexStringToByteArray("18E14A7B6A307F426A94F8114701E7C8E774E7F9A47E2C2035DB29A206321725");
            BitcoinKeyPair pair = new BitcoinKeyPair(privateKey);
            BitcoinAddress address = new BitcoinAddress(pair.PublicKey);
            byte[] actualAddress = address.Address;

            for (int i = 0; i < expectedAddress.Length; i++)
            {
                Assert.AreEqual(expectedAddress[i], actualAddress[i]);
            }
        }
    }
}
