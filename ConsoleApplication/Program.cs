using System.IO;
using CodePound.SamsonForce.Core;
using System;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace CodePound.SamsonForce.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            BruteForceSpecial attacker = null;
            string[] dictionary = null;
            string paraphrase = null;
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);

            Logger.SetLogWriter(logWriterFactory.Create());

            if (args.Length != 2)
            {
                Console.WriteLine("sf <dictionary_path> <bitcoin_address>");
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("Dictionary path does not exist.");
            }
            else
            {
                try
                {
                    attacker = new BruteForceSpecial(args[1]);
                    dictionary = File.ReadAllLines(args[0]);
                    paraphrase = attacker.FindPrivateKey(dictionary);
                }
                catch (Exception ex)
                {
                    Logger.Write(ex);
                    Console.WriteLine("An unexpected exception was thrown. Please review trace.log for more details.");
                }
            }
        }
    }
}
