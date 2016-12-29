using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SimWordsGenApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker _worker = new BackgroundWorker();
        private Random _rnd = new Random();
        private char[] _chars;
        private int[,] _matrix;

        public MainWindow()
        {
            InitializeComponent();
            int[,] gg = new int[2, 3] 
            {
                { 0, 0, 0 },
                { 0, 0, 0 }
            };
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, InitialDirectory = AppDomain.CurrentDomain.BaseDirectory };
            if (ofd.ShowDialog().Value)
            {
                _worker = new BackgroundWorker();
                _worker.WorkerReportsProgress = true;
                _worker.DoWork += _worker_DoWork;
                _worker.ProgressChanged += _worker_ProgressChanged;
                _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
                _worker.RunWorkerAsync(ofd.FileNames);
            }
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var files = (string[])e.Argument;

            Dictionary<string, int> wordFreq = new Dictionary<string, int>();
            List<char> chars = new List<char>();


            _worker.ReportProgress(0, new int[2] { 0, files.Length });

            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                _worker.ReportProgress(0, new int[2] { i, files.Length });

                foreach (var word in GetWords(System.IO.File.ReadAllText(file)))
                {
                    if (!wordFreq.ContainsKey(word))
                        wordFreq.Add(word, 1);
                    else
                        wordFreq[word]++;
                }

                _worker.ReportProgress(0, new int[2] { i + 1, files.Length });
            }

            int ii = 0;
            int wordsCount = wordFreq.Count;
            _worker.ReportProgress(1, new int[2] { ii, wordsCount });
            foreach (var wordPair in wordFreq)
            {
                chars.AddRange(wordPair.Key);
                ii++;
                _worker.ReportProgress(1, new int[2] { ii, wordsCount });
            }
            chars = new List<char>(chars.Distinct());
            chars.Sort();

            Dictionary<char, int> charIds = new Dictionary<char, int>();
            chars.Insert(0, ' ');
            for (int i = 0; i < chars.Count; i++)
                charIds.Add(chars[i], i);

            int[,] matrix = new int[chars.Count, chars.Count];

            ii = 0;
            _worker.ReportProgress(1, new int[2] { ii, wordsCount });
            foreach (var wordPair in wordFreq)
            {
                int pind = 0;
                foreach (var ch in wordPair.Key)
                {
                    int ind = charIds[ch];
                    matrix[pind, ind] += wordPair.Value;
                    pind = ind;
                }
                matrix[pind, 0] += wordPair.Value;
                ii++;
                _worker.ReportProgress(1, new int[2] { ii, wordsCount });
            }
            e.Result = new object[2] { chars.ToArray(), matrix };
        }

        static IEnumerable<string> GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input.ToLower(), @"\b([a-zA-Z]+)\b");

            return from m in matches.Cast<Match>() select m.Value;
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var prog = (int[])e.UserState;
            switch (e.ProgressPercentage)
            {
                case 0:
                    filesProgBar.Maximum = prog[1];
                    filesProgBar.Value = prog[0];
                    break;
                case 1:
                    fileProgBar.Maximum = prog[1];
                    fileProgBar.Value = prog[0];
                    break;
            }
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _chars = (char[])((object[])e.Result)[0];
            _matrix = (int[,])((object[])e.Result)[1];

            string result = "char[] _chars = new char["+_chars.Length+"]{";
            for (int i = 0; i < _chars.Length; i++)
            {
                result += "'" + _chars[i] + "'";
                if (i != _chars.Length - 1) result += ",";
            }
            result += "};\r\nint[,] _matrix=new int[" + _matrix.GetLength(0) + "," + _matrix.GetLength(1) + "]\r\n{\r\n";
            for (int y = 0; y < _matrix.GetLength(0); y++)
            {
                result += "{";
                for (int x = 0; x < _matrix.GetLength(1); x++)
                {
                    result += _matrix[y, x];
                    if (x != _matrix.GetLength(1) - 1) result += ",";
                }
                result += "}";
                if (y != _matrix.GetLength(0) - 1) result += ",";
                result += "\r\n";
            }
            result += "};";
            logBox.Text = result;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Generate(6));

            namesBox.Text = "";

            for (int i = 4; i < 15; i++)
            {
                namesBox.Text += "==== " + i + " letters names ====\r\n";
                for (int j = 0; j < 10; j++)
                    namesBox.Text += Generate(i) + "\r\n";
            }
        }

        public string Generate(int length)
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
            return GetLetter(pi);
        }
    }
}
