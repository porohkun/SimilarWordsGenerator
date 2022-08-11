using Prism.Mvvm;
using System.Text.RegularExpressions;

namespace SimWordsGenApp.ViewModels
{
    public class SymbolViewModel : BindableBase
    {
        public string Origin { get; private set; }

        public string Replace
        {
            get => Regex.Escape(_symbol.Replace.ToString());
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "\0";
                var replace = Regex.Unescape(value)[0];
                if (_symbol.Replace != replace)
                {
                    _symbol.Replace = replace;
                    RaisePropertyChanged();
                    Settings.Save();
                }
            }
        }


        private Symbol _symbol;

        public SymbolViewModel(Symbol symbol)
        {
            _symbol = symbol;
            Origin = Regex.Escape(_symbol.Origin.ToString());
        }

        public bool HaveSymbol(Symbol symbol)
        {
            return symbol == _symbol;
        }

    }
}
