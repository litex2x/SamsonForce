using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics;
using Combinatorics.Collections;
using System.Threading;
using System.Collections.Concurrent;
using MathNet.Numerics;
using System.Text.RegularExpressions;

namespace CodePound.SamsonForce.Core
{
    public class BruteForceSpecial
    {
        private const string numerics = "0123456789";
        private ManualResetEvent[] manualResetEvents = null;

        private ConcurrentBag<string> CandidatesThreadSafe { get; set; }
        private string[] Candidates { get; set; }

        private string Address { get; set; }
        private string Paraphrase { get; set; }
        private string[] Dictionary { get; set; }

        public BruteForceSpecial(string address)
        {
            Address = address;
        }

        public string FindPrivateKey(string[] dictionary)
        {
            double combinations = SpecialFunctions.Factorial(dictionary.Length) / (SpecialFunctions.Factorial(3) * SpecialFunctions.Factorial(dictionary.Length - 3));
            int combinationIndex = 0;

            Dictionary = dictionary;
            CandidatesThreadSafe = new ConcurrentBag<string>();
            manualResetEvents = new ManualResetEvent[64];
            Console.WriteLine("Preparing candidates...");

            for (int first = 0; first < dictionary.Length; first++)
            {
                for (int second = first + 1; second < dictionary.Length; second++)
                {
                    for (int third = second + 1; third < dictionary.Length; third++)
                    {
                        manualResetEvents[combinationIndex % 64] = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(SeekCandidateThread, string.Format("{0},{1},{2},{3}", first, second, third, combinationIndex % 64));
                        WaitForThreads(combinationIndex + 1, manualResetEvents, combinations);
                        combinationIndex++;
                    }
                }
            }

            WaitHandle.WaitAll(manualResetEvents.Where(x => x != null).ToArray());
            Console.WriteLine("Running Samson Force algorithm...");
            manualResetEvents = new ManualResetEvent[64];
            Paraphrase = "-1";
            Candidates = CandidatesThreadSafe.ToArray();

            for (int i = 0; i < CandidatesThreadSafe.Count; i++)
            {
                manualResetEvents[i % 64] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(BruteForceThread, i % 64);
                WaitForThreads(i + 1, manualResetEvents, CandidatesThreadSafe.Count);
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

        private List<string> CreateCandidates(string[] values, string numerics)
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

            return candidates;
        }

        private void WaitForThreads(int runningTotal, ManualResetEvent[] workingThreads, double total = 0)
        {
            if (runningTotal % 64 == 0)
            {
                WaitHandle.WaitAll(workingThreads);

                if (total != 0)
                {
                    Console.WriteLine(string.Format("{0} of {1}", runningTotal, total));
                }
            }
        }

        private void SeekCandidateThread(Object threadContext)
        {
            List<string> newCandidates = null;
            int[] arguements = threadContext.ToString().Split(',').Select(x => Convert.ToInt32(x)).ToArray();
            string candidate = (Dictionary[arguements[0]] + Dictionary[arguements[1]] + Dictionary[arguements[2]]).ToLower();

            if (Regex.Match(candidate, "^[a-z]{17}$").Success)
            {
                newCandidates = CreateCandidates(new string[] 
                    { 
                        Dictionary[arguements[0]].ToLower(), 
                        Dictionary[arguements[1]].ToLower(), 
                        Dictionary[arguements[2]].ToLower() 
                    },
                    numerics);
                newCandidates.ForEach(x => CandidatesThreadSafe.Add(x));
            }

            manualResetEvents[arguements[3]].Set();
        }

        private void BruteForceThread(Object threadContext)
        {
            BitcoinKeyPair pair = null;
            BitcoinAddress walletAddress = null;
            Base58Check encoder = new Base58Check();
            int i = (int)threadContext;

            pair = new BitcoinKeyPair(Candidates[i]);
            walletAddress = new BitcoinAddress(pair.PublicKey);

            if (encoder.Encode(walletAddress.Address).ToLower() == Address.ToLower())
            {
                Paraphrase = Candidates[i];
            }

            manualResetEvents[i].Set();
        }
    }
}
