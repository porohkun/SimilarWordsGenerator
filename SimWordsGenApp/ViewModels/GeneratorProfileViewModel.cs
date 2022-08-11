using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace SimWordsGenApp.ViewModels
{
    public class GeneratorProfileViewModel : BindableBase
    {
        public string Name => _profile.Name;
        public WrappedObservableCollection<GeneratorSourceViewModel, GeneratorSource> Sources { get; protected set; }

        private GeneratorProfile _profile;

        public GeneratorProfileViewModel(GeneratorProfile profile)
        {
            _profile = profile;
            Sources = new WrappedObservableCollection<GeneratorSourceViewModel, GeneratorSource>(_profile.Sources,
                s => new GeneratorSourceViewModel(s),
                (svm, s) => svm.HaveSource(s));
        }

        public bool HaveProfile(GeneratorProfile profile)
        {
            return profile == _profile;
        }

        public GeneratorProfile GetProfile() => _profile;
    }
}
