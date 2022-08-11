using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SimWordsGenApp
{
    public class MainSettings : SettingsPartWithNotifier
    {
        [JsonProperty(nameof(Profiles))]
        private ObservableCollection<GeneratorProfile> _profiles = new ObservableCollection<GeneratorProfile>();

        [JsonIgnore]
        public ObservableCollection<GeneratorProfile> Profiles
        {
            get
            {
                if (_profiles == null)
                    _profiles = new ObservableCollection<GeneratorProfile>();
                return _profiles;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Profiles.CollectionChanged += Profiles_CollectionChanged;
        }

        private void Profiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Profiles)));
        }
    }
}
