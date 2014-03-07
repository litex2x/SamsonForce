using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics;
using Combinatorics.Collections;

namespace CodePound.SamsonForce.Core
{
    public class BruteForceSpecial
    {
        private const int max = 18;
        private const string numerics = "0123456789";

        public string FindPrivateKey(string[] dictionary, string address)
        {
            List<string> candidates = new List<string>();
            BitcoinKeyPair pair = null;
            BitcoinAddress walletAddress = null;
            Base58Check encoder = new Base58Check();

            for (int first = 0; first < dictionary.Length; first++)
            {
                for (int second = first + 1; second < dictionary.Length; second++)
                {
                    for (int third = second + 1; third < dictionary.Length; third++)
                    {
                        if ((dictionary[first] + dictionary[second] + dictionary[third]).Length == (max - 1))
                        {
                            candidates.AddRange(
                                CreateCandidates(
                                new string[] 
                                { 
                                    dictionary[first], 
                                    dictionary[second], 
                                    dictionary[third] 
                                }
                                , numerics));  
                        }
                    }
                }
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                pair = new BitcoinKeyPair(candidates[i]);
                walletAddress = new BitcoinAddress(pair.PublicKey);

                if (encoder.Encode(walletAddress.Address).ToLower() == address.ToLower())
                {
                    Console.WriteLine(string.Format("{0} passed! ({1} of {2})", candidates[i], i, candidates.Count));

                    return candidates[i];
                }
                else
                {
                    Console.WriteLine(string.Format("{0} failed! ({1} of {2})", candidates[i], i, candidates.Count));
                }
            }

            return "-1";
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
    }
}
