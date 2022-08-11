using Prism.Unity;

namespace SimWordsGenApp.Commands
{
    [PrismResourceInjection]
    public class TestCommand : InjectableCommand<TestCommand>
    {
        protected override bool CanExecuteInternal(object source)
        {
            return true;
        }

        protected override void ExecuteInternal(object source)
        {
            if (source == null)
                return;
            Settings.Save();
        }
    }
}
