using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimWordsGenApp.ViewModels
{
    public class GeneratorSourceVMDummy : GeneratorSourceViewModel
    {
        public new string Path { get; private set; }
        public GeneratorSourceVMDummy(string path, string symbols, string replaceSymbols) : base(new GeneratorSource(path, Enumerable.Empty<Symbol>()))
        {
            Path = path;
            for (int i = 0; i < symbols.Length; i++)
                GetSource().Symbols.Add(new Symbol(symbols[i], replaceSymbols[i]));
        }
    }

    public class GeneratorSourceViewModel : BindableBase
    {
        public string Path => _source.Path;
        public WrappedObservableCollection<SymbolViewModel, Symbol> Symbols { get; protected set; }
        private GeneratorSource _source;

        public GeneratorSourceViewModel(GeneratorSource source)
        {
            _source = source;
            Symbols = new WrappedObservableCollection<SymbolViewModel, Symbol>(_source.Symbols,
               s => new SymbolViewModel(s),
               (svm, s) => svm.HaveSymbol(s));
        }

        public bool HaveSource(GeneratorSource source)
        {
            return source == _source;
        }

        public GeneratorSource GetSource() => _source;
    }
}
