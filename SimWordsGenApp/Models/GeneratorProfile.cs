using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimWordsGenApp
{
    public class GeneratorProfile
    {
        [JsonProperty]
        public string Name { get; private set; }

        [JsonIgnore]
        public ObservableCollection<GeneratorSource> Sources
        {
            get
            {
                if (_sources == null)
                    _sources = new ObservableCollection<GeneratorSource>();
                return _sources;
            }
        }

        [JsonProperty(nameof(Sources))]
        private ObservableCollection<GeneratorSource> _sources = new ObservableCollection<GeneratorSource>();

        public GeneratorProfile()
        {

        }

        public GeneratorProfile(string name) : this()
        {
            Name = name;
        }

        public bool AddSource(string path)
        {
            path = path.Replace('\\', '/');
            if (_sources.Any(s => s.Path == path))
                return false;

            _sources.Add(new GeneratorSource(path));
            return true;
        }

        public bool RemoveSource(string source)
        {
            var result = _sources.Remove(_sources.FirstOrDefault(s => s.Path == source));
            return result;
        }
    }
}
