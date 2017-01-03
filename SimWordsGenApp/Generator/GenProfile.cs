using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimWordsGenApp
{
    public class GenProfile
    {
        private Random _rnd = new Random();

        private char[] _chars;
        private int[,] _matrix;

        public int Length { get { return _chars.Length; } }

        public GenProfile(char[] chars, int[,] matrix)
        {
            _chars = chars;
            _matrix = matrix;
        }

        public string GetRandomWord(int length)
        {
            string name = "";
            int pi = 0;
            for (int i = 0; i < length - 1; i++)
            {
                int chi = GetLetter(pi);
                pi = chi;
                name += _chars[chi];
            }
            name += _chars[GetLastLetter(pi)];

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        }

        private int GetLetter(int pi)
        {
            int sum = 0;
            for (int i = 1; i < _chars.Length; i++)
                sum += _matrix[pi, i];
            int roll = _rnd.Next(0, sum);
            sum = 0;
            for (int i = 1; i < _chars.Length; i++)
            {
                sum += _matrix[pi, i];
                if (roll < sum)
                    return i;
            }
            return -1;
        }

        private int GetLastLetter(int pi)
        {
            return GetLetter(pi);//TODO make last letter valuable
        }

        public void SaveToFile(string filename)
        {
            SaveToStream(new FileStream(filename, FileMode.Create));
        }

        public void SaveToStream(Stream stream)
        {
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(Length);
                bw.Write(_chars);
                for (int i = 0; i < Length; i++)
                    for (int j = 0; j < Length; j++)
                        bw.Write(_matrix[i, j]);
            }
        }

        public static GenProfile LoadFromFile(string filename)
        {
            return LoadFromStream(new FileStream(filename, FileMode.Open));
        }

        public static GenProfile LoadFromStream(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                int length = br.ReadInt32();
                char[] chars = br.ReadChars(length);
                int[,] matrix = new int[length, length];
                for (int i = 0; i < length; i++)
                    for (int j = 0; j < length; j++)
                        matrix[i, j] = br.ReadInt32();
                return new GenProfile(chars, matrix);
            }
        }

        internal int GetWeight(int i, int j)
        {
            return _matrix[i, j];
        }

        internal string GetChar(int i)
        {
            return (i >= 0 && i < Length) ? _chars[i].ToString() : string.Empty;
        }
    }
}
