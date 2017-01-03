using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordsList = System.Collections.Generic.Dictionary<string, int>;

namespace SimWordsGenApp
{
    public class Parser
    {
        public static GenProfile Parse(IEnumerable<string> texts, string regex)
        {
            return GetProfile(GetWordsFrequencyList(texts, regex));
        }

        public static GenProfile Parse(string text, string regex)
        {
            return GetProfile(GetWordsFrequencyList(text, regex));
        }

        private static WordsList MergeWordsLists(IEnumerable<WordsList> lists)
        {
            IEnumerable<KeyValuePair<string, int>> pairs = Enumerable.Empty<KeyValuePair<string, int>>();
            foreach (var list in lists)
                pairs = pairs.Concat(list);
            return pairs.GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.Sum(v => v.Value));
        }

        private static WordsList GetWordsFrequencyList(IEnumerable<string> texts, string regex)
        {
            return MergeWordsLists(texts.Select(t => GetWordsFrequencyList(t, regex)));
        }

        static IEnumerable<string> GetWords(string input, string regex)
        {
            MatchCollection matches = Regex.Matches(input.ToLower(), regex);

            return from m in matches.Cast<Match>() select m.Value;
        }

        private static WordsList GetWordsFrequencyList(string text, string regex)
        {
            var wordFreq = new WordsList();
            //@"\b([a-zA-Z]+)\b"
            foreach (var word in GetWords(text, regex))
            {
                if (!wordFreq.ContainsKey(word))
                    wordFreq.Add(word, 1);
                else
                    wordFreq[word]++;
            }

            return wordFreq;
        }

        private static GenProfile GetProfile(WordsList list)
        {
            List<char> chars = new List<char>();

            int wordsCount = list.Count;

            foreach (var wordPair in list)
            {
                chars.AddRange(wordPair.Key);
            }
            chars = new List<char>(chars.Distinct());
            chars.Sort();

            Dictionary<char, int> charIds = new Dictionary<char, int>();
            chars.Insert(0, ' ');
            for (int i = 0; i < chars.Count; i++)
                charIds.Add(chars[i], i);

            int[,] matrix = new int[chars.Count, chars.Count];



            foreach (var wordPair in list)
            {
                int pind = 0;
                foreach (var ch in wordPair.Key)
                {
                    int ind = charIds[ch];
                    matrix[pind, ind] += wordPair.Value;
                    pind = ind;
                }
                matrix[pind, 0] += wordPair.Value;
            }
            return new GenProfile(chars.ToArray(), matrix);
        }
        
    }
}
