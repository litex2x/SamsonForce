using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodePound.SamsonForce.Core;
using System.IO;

namespace CodePound.SamsonForce.CoreUnitTests
{
    [TestClass]
    public class BruteForceSpecialUnitTest
    {
        [TestMethod]
        public void FindPrivateKeyMethodTest()
        {
            BruteForceSpecial attacker = new BruteForceSpecial();
            string[] dictionary = { "asdfg", "sdfsasw", "sdfsa" };
            string password = attacker.FindPrivateKey(dictionary, "16XQFuU6bYcsz1RxdTotHScK8kU9eDaX5V");

            Assert.AreEqual("asdfgsdfsaswsdfsa0", password);
        }
    }
}
