using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SimWordsGenApp.ViewModels
{
    public class MainWindowVMDummy : MainWindowViewModel
    {

        public MainWindowVMDummy() : base(new ObservableCollection<GeneratorProfile>()
        {
            new GeneratorProfile("Profile1"),
            new GeneratorProfile("Profile2"),
            new GeneratorProfile("Profile3")
        })
        {
            SelectedProfile = Profiles[0];
            SelectedProfile.GetProfile().Sources.Add(new GeneratorSource(@"D:\Projects\Desktop\SimilarWordsGenerator\source1.txt", "abcdefgh".Glue("абцдефжш", (o, r) => new Symbol(o, r))));
            SelectedProfile.GetProfile().Sources.Add(new GeneratorSource(@"D:\Projects\Desktop\SimilarWordsGenerator\source2.txt", "\na bc".Glue(" a bc", (o, r) => new Symbol(o, r))));
            SelectedProfile.GetProfile().Sources.Add(new GeneratorSource(@"D:\Projects\Desktop\SimilarWordsGenerator\source3.txt", "\na bc".Glue(" a bc", (o, r) => new Symbol(o, r))));
            GeneratorResult = "result1\nresult2";
        }
    }

    public class MainWindowViewModel : BindableBase
    {
        public WrappedObservableCollection<GeneratorProfileViewModel, GeneratorProfile> Profiles { get; protected set; }
        public GeneratorProfileViewModel SelectedProfile
        {
            get => _selectedProfile;
            set => SetProperty(ref _selectedProfile, value, () => Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested));
        }
        public string GeneratorResult
        {
            get => _generatorResult;
            protected set => SetProperty(ref _generatorResult, value);
        }

        public ICommand GenerateCommand { get; }

        private GeneratorProfileViewModel _selectedProfile;
        private string _generatorResult;

        private Generator _cachedGenerator;

        public MainWindowViewModel() : this(Settings.Instance.Main.Profiles)
        {
        }

        protected MainWindowViewModel(ObservableCollection<GeneratorProfile> profiles)
        {
            GenerateCommand = new DelegateCommand<GeneratorProfileViewModel>(Generate);
            Profiles = new WrappedObservableCollection<GeneratorProfileViewModel, GeneratorProfile>(profiles,
                p => new GeneratorProfileViewModel(p),
                (pvm, p) => pvm.HaveProfile(p));
        }

        private void Generate(GeneratorProfileViewModel profileVM)
        {
            var profile = profileVM?.GetProfile();
            if (!Generator.IsValidProfile(profile))
                return;
            if (_cachedGenerator == null || !_cachedGenerator.Profile.Equals(profile))
                _cachedGenerator = new Generator(profile);

            GeneratorResult = string.Join("\n", Enumerable.Repeat(7, 20).Select(v => _cachedGenerator.Generate(v)));
        }
    }
}
