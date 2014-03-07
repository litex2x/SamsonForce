using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using CodePound.SamsonForce.Core;
using System.Linq;

namespace CodePound.SamsonForce.CoreUnitTests
{
    [TestClass]
    public class BitCoinKeyPairUnitTest
    {
        [TestMethod]
        public void PublicKeyPropertyTest()
        {
            byte[] expectedPublicKey = TestUtility.HexStringToByteArray("0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6");
            byte[] privateKey = TestUtility.HexStringToByteArray("18E14A7B6A307F426A94F8114701E7C8E774E7F9A47E2C2035DB29A206321725");
            BitcoinKeyPair pair = new BitcoinKeyPair(privateKey);
            byte[] actualPublicKey = pair.PublicKey;

            for (int i = 0; i < expectedPublicKey.Length; i++)
            {
                Assert.AreEqual(expectedPublicKey[i], actualPublicKey[i]);
            }
        }

        [TestMethod]
        public void PublicKeyPropertyWithParaphraseTest()
        {
            byte[] expectedPublicKey = TestUtility.HexStringToByteArray("0458a7fc70b8bbb675d19451f7d7234c4d79879d860406ec8e56412aa55d8026225a4c9cdc3c1018754c7be7e0088ce0910c8f609ee57f421ee8183d711ee65c21");
            BitcoinKeyPair pair = new BitcoinKeyPair("Man made it to the moon,, and decided it stinked like yellow cheeeese.");
            byte[] actualPublicKey = pair.PublicKey;

            for (int i = 0; i < expectedPublicKey.Length; i++)
            {
                Assert.AreEqual(expectedPublicKey[i], actualPublicKey[i]);
            }
        }
    }
}
