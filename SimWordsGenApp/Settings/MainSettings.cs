using Newtonsoft.Json;

namespace SimWordsGenApp
{
    public class MainSettings : SettingsPartWithNotifier
    {
        [JsonProperty(nameof(Profiles))]
        private bool _profiles = true;



        [JsonIgnore]
        public bool Profiles
        {
            get => _profiles;
            set => SetProperty(ref _profiles, value);
        }
    }
}
