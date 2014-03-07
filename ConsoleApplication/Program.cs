using System.IO;
using CodePound.SamsonForce.Core;

namespace CodePound.SamsonForce.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            BruteForceSpecial attacker = new BruteForceSpecial();
            string[] dictionary = File.ReadAllLines(args[0]);

            attacker.FindPrivateKey(dictionary, args[1]);
        }
    }
}
