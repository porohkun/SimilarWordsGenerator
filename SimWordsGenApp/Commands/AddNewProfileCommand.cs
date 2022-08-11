using Prism.Unity;
using SimWordsGenApp.Views;
using System.Linq;
using System.Windows;

namespace SimWordsGenApp.Commands
{
    [PrismResourceInjection]
    public class AddNewProfileCommand : InjectableCommand<AddNewProfileCommand, string>
    {
        protected override bool CanExecuteInternal(string defaultValue)
        {
            return true;
        }

        protected override void ExecuteInternal(string defaultValue)
        {
            var name = InputFieldWindow.Show("Creating new profile", "Type name for new profile", defaultValue)?.Trim();
            if (!string.IsNullOrWhiteSpace(name))
                if (Settings.Instance.Main.Profiles.Any(p => p.Name == name))
                {
                    MessageBox.Show($"Profile named '{name}' already exists.");
                    Execute(name);
                }
                else
                    Settings.Instance.Main.Profiles.Add(new GeneratorProfile(name));
        }
    }
}
