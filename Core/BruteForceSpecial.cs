using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics;
using Combinatorics.Collections;
using System.Threading;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace CodePound.SamsonForce.Core
{
    public class BruteForceSpecial
    {
        private const string numerics = "0123456789";
        private ManualResetEvent[] manualResetEvents = null;
        private bool isMatchFound = false;

        private string Address { get; set; }
        private string Paraphrase { get; set; }
        private string[] Dictionary { get; set; }

        public BruteForceSpecial(string address)
        {
            Address = address;
        }

        public string FindPrivateKey(string[] dictionary)
        {
            int combinationIndex = 0;

            Dictionary = dictionary;
            manualResetEvents = new ManualResetEvent[64];
            Paraphrase = "-1";
            Console.WriteLine("Running Samson Force algorithm...");

            for (int first = 0; first < dictionary.Length && !isMatchFound; first++)
            {
                for (int second = first + 1; second < dictionary.Length && !isMatchFound; second++)
                {
                    for (int third = second + 1; third < dictionary.Length && !isMatchFound; third++)
                    {
                        manualResetEvents[combinationIndex % 64] = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(ProcessCombination, string.Format("{0},{1},{2},{3}", first, second, third, combinationIndex % 64));
                        WaitForThreads(combinationIndex + 1, manualResetEvents);
                        combinationIndex++;
                    }
                }
            }

            WaitHandle.WaitAll(manualResetEvents.Where(x => x != null).ToArray());

            if (Paraphrase == "-1")
            {
                Console.WriteLine("No paraphrases from the dictionary matched the target address.");
            }
            else
            {
                Console.WriteLine(string.Format("{0} matches the target address.", Paraphrase));
            }

            return Paraphrase;
        }

        private string[] CreateCandidates(string[] values, string numerics)
        {
            List<string> candidates = new List<string>();
            Permutations<string> permutations = new Permutations<string>(values, GenerateOption.WithoutRepetition);

            foreach (List<string> permutation in permutations)
            {
                foreach (char numeric in numerics)
                {
                    candidates.Add(string.Format("{0}{1}{2}{3}", permutation[0], permutation[1], permutation[2], numeric));
                }
            }

            return candidates.ToArray();
        }

        private void WaitForThreads(int runningTotal, ManualResetEvent[] workingThreads)
        {
            if (runningTotal % 64 == 0)
            {
                WaitHandle.WaitAll(workingThreads);
            }
        }

        private void ProcessCombination(Object threadContext)
        {
            string[] candidates = null;
            int[] arguements = null;
            string possibleCandidate = null;

            if (isMatchFound)
            {
                return;
            }

            arguements = threadContext.ToString().Split(',').Select(x => Convert.ToInt32(x)).ToArray();
            possibleCandidate = (Dictionary[arguements[0]] + Dictionary[arguements[1]] + Dictionary[arguements[2]]).ToLower();

            if (Regex.Match(possibleCandidate, "^[a-z]{17}$").Success)
            {
                candidates = CreateCandidates(new string[] 
                    { 
                        Dictionary[arguements[0]].ToLower(), 
                        Dictionary[arguements[1]].ToLower(), 
                        Dictionary[arguements[2]].ToLower() 
                    },
                    numerics);
                foreach(string candidate in candidates)
                {
                    isMatchFound = IsMatch(candidate);
                }
            }

            manualResetEvents[arguements[3]].Set();
        }

        private bool IsMatch(string value)
        {
            BitcoinKeyPair pair = null;
            BitcoinAddress walletAddress = null;
            Base58Check encoder = new Base58Check();

            pair = new BitcoinKeyPair(value);
            walletAddress = new BitcoinAddress(pair.PublicKey);
            Console.WriteLine(string.Format("Testing: {0}", value));

            if (encoder.Encode(walletAddress.Address).ToLower() == Address.ToLower())
            {
                Paraphrase = value;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
