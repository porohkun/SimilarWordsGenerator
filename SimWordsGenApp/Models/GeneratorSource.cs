using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SimWordsGenApp
{
    public class GeneratorSource
    {
        [JsonProperty] public string Path { get; private set; }
        [JsonIgnore]
        public ObservableCollection<Symbol> Symbols
        {
            get
            {
                if (_symbols == null)
                    _symbols = new ObservableCollection<Symbol>();
                return _symbols;
            }
        }

        [JsonIgnore] public IReadOnlyDictionary<uint, IReadOnlyDictionary<char, int>> Data => _data;

        [JsonProperty(nameof(Symbols), ItemConverterType = typeof(SettingsConverters.SymbolObservableCollectionDataConverter))]
        private ObservableCollection<Symbol> _symbols = new ObservableCollection<Symbol>();

        [JsonProperty(nameof(Data))]
        private Dictionary<uint, IReadOnlyDictionary<char, int>> _data { get; set; }

        [JsonConstructor] private GeneratorSource() { }

        public GeneratorSource(string path, IEnumerable<Symbol> symbols)
        {
            Path = path;
            Symbols.AddRange(symbols);
        }

        public GeneratorSource(string path)
        {
            Path = path;
            Import(false);
        }

        private void Import(bool keepSymbols)
        {
            var symbols = new List<char>();
            _data = new Dictionary<uint, IReadOnlyDictionary<char, int>>();
            using (var reader = new StreamReader(File.OpenRead(Path)))
            {
                uint[] prev = new uint[2];
                int indexer = 0;
                while (!reader.EndOfStream)
                {
                    var c = char.ToLowerInvariant((char)reader.Read());
                    if (keepSymbols)
                        c = ReplaceSymbol(c);
                    if (c == 0 && prev[indexer] == 0)
                        continue;
                    symbols.Add(c);
                    uint addr = 0;
                    for (int i = 0; i < prev.Length; i++)
                    {
                        var pos = prev[(indexer + prev.Length - i) % prev.Length];
                        addr += pos << (i * 16);

                        IReadOnlyDictionary<char, int> data;
                        if (!_data.TryGetValue(addr, out data))
                        {
                            data = new Dictionary<char, int>();
                            _data[addr] = data;
                        }
                        (data as Dictionary<char, int>)[c] = data.TryGetValue(c, out var v) ? ++v : 1;
                    }
                    indexer = ++indexer % prev.Length;
                    if (c == 0)
                        prev.Clear();
                    else
                        prev[indexer] = c;
                }
            }
            if (!keepSymbols)
                _symbols.AddRange(symbols.Distinct().OrderBy(x => x).Select(s => new Symbol(s, s)));
        }

        private char ReplaceSymbol(char origin)
        {
            return _symbols.First(s => s.Origin == origin).Replace;
        }

        public void Reimport()
        {
            Import(true);
        }

    }
}
