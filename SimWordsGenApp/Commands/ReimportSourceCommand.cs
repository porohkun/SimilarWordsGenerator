using Prism.Unity;
using SimWordsGenApp.ViewModels;

namespace SimWordsGenApp.Commands
{
    [PrismResourceInjection]
    public class ReimportSourceCommand : InjectableCommand<ReimportSourceCommand, GeneratorSourceViewModel>
    {
        protected override bool CanExecuteInternal(GeneratorSourceViewModel source)
        {
            return true;
        }

        protected override void ExecuteInternal(GeneratorSourceViewModel source)
        {
            if (source == null)
                return;
            source.GetSource().Reimport();
            Settings.Save();
        }
    }
}
