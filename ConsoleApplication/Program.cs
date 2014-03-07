using System.IO;
using CodePound.SamsonForce.Core;
using System;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodePound.SamsonForce.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            BruteForceSpecial attacker = new BruteForceSpecial(args[1]);
            string[] dictionary = File.ReadAllLines(args[0]);
            string paraphrase = attacker.FindPrivateKey(dictionary);
        }
    }
}
