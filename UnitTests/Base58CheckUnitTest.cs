using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodePound.SamsonForce.Core;

namespace CodePound.SamsonForce.CoreUnitTests
{
    [TestClass]
    public class Base58CheckUnitTest
    {
        [TestMethod]
        public void EncodeMethodTest()
        {
            byte[] privateKey = TestUtility.HexStringToByteArray("18E14A7B6A307F426A94F8114701E7C8E774E7F9A47E2C2035DB29A206321725");
            BitcoinKeyPair pair = new BitcoinKeyPair(privateKey);
            BitcoinAddress address = new BitcoinAddress(pair.PublicKey);
            Base58Check encoder = new Base58Check();
            string expected = "16UwLL9Risc3QfPqBUvKofHmBQ7wMtjvM";
            
            Assert.AreEqual(expected, encoder.Encode(address.Address));
        }
    }
}
