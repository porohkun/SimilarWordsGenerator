using System;
using System.Collections.Generic;
using System.Linq;

namespace SimWordsGenApp
{
    public class Generator
    {
        public GeneratorProfile Profile { get; }

        private Dictionary<uint, CharVariants> _data;
        private Random _rnd;

        public Generator(GeneratorProfile profile)
        {
            Profile = profile;
            _rnd = new Random();
        }

        public static bool IsValidProfile(GeneratorProfile profile)
        {
            return profile != null && profile.Sources.Count() > 0;
        }

        public string Generate(int length)
        {
            if (_data == null)
                MergeSources();

            var word = new char[length];
            for (var i = 0; i < length; i++)
                word[i] = GetSymbol(word, i, 2);
            return new string(word);
        }

        private char GetSymbol(char[] word, int index, int depth)
        {
            if (index == 0)
                return _data[0].GetRandomCharacter(_rnd, true);
            uint key = 0;
            for (int i = 0; i <= depth; i++)
                if (index >= i)
                    key += ((uint)word[index - i]) << i * 16;
            if (_data.TryGetValue(key, out var variants))
                return variants.GetRandomCharacter(_rnd, true);
            else if (depth > 0)
                return GetSymbol(word, index, depth - 1);
            else
                throw new NotImplementedException();
        }

        private void MergeSources()
        {
            var temp = new Dictionary<uint, Dictionary<char, int>>();

            foreach (var source in Profile.Sources)
                foreach (var dataPair in source.Data)
                {
                    Dictionary<char, int> dataDict;
                    if (!temp.TryGetValue(dataPair.Key, out dataDict))
                    {
                        dataDict = new Dictionary<char, int>();
                        temp[dataPair.Key] = dataDict;
                    }
                    dataDict.Merge(dataPair.Value, (l, r) => l + r);
                }

            _data = temp.ToDictionary(t => t.Key, t => new CharVariants(t.Value));
        }

        private class CharVariants
        {
            public readonly long SumWeight;
            public readonly IReadOnlyList<CharPair> CharWeights;
            public CharVariants(Dictionary<char, int> data)
            {
                var weights = new List<CharPair>();
                var haveZero = false;
                foreach (var pair in data.OrderBy(e => e.Key))
                {
                    if (!haveZero && pair.Key != 0)
                    {
                        weights.Add(new CharPair('\0', 0));
                        haveZero = true;
                    }
                    SumWeight += pair.Value;
                    weights.Add(new CharPair(pair.Key, SumWeight));
                }
                CharWeights = weights;
            }

            public char GetRandomCharacter(Random rnd, bool exceptZero = false)
            {
                long min = exceptZero ? CharWeights[0].Weight : 0;
                long max = SumWeight;
                var multiplier = Contract(ref min, ref max);
                var roll = rnd.Next((int)min, (int)max) * multiplier + 1;

                char result = '\0';
                foreach (var pair in CharWeights)
                {
                    if (pair.Weight > roll)
                        break;
                    result = pair.Char;
                }
                return result;
            }

            private int Contract(ref long min, ref long max)
            {
                var multiplier = max / int.MaxValue + 1;
                min = min / multiplier;
                max = max / multiplier;
                return (int)multiplier;
            }
        }

        private class CharPair
        {
            public readonly char Char;
            public readonly long Weight;

            public CharPair(char character, long weight)
            {
                Char = character;
                Weight = weight;
            }
        }
    }
}
