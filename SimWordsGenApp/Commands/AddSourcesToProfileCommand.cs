using Microsoft.Win32;
using Prism.Unity;
using SimWordsGenApp.ViewModels;

namespace SimWordsGenApp.Commands
{
    [PrismResourceInjection]
    public class AddSourcesToProfileCommand : InjectableCommand<AddSourcesToProfileCommand, GeneratorProfileViewModel>
    {
        protected override bool CanExecuteInternal(GeneratorProfileViewModel profile)
        {
            return true;
        }

        protected override void ExecuteInternal(GeneratorProfileViewModel profile)
        {
            if (profile == null)
                return;
            var ofd = new OpenFileDialog()
            {
                Multiselect = true,
                Title = "Select text sources"
            };
            if (ofd.ShowDialog() == true)
            {
                foreach (var file in ofd.FileNames)
                {
                    profile.GetProfile().AddSource(System.IO.Path.GetFullPath(file));
                }
                Settings.Save();
            }
        }
    }
}
