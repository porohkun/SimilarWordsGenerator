namespace SimWordsGenApp
{
    public class Symbol
    {
        public char Origin { get; private set; }
        public char Replace { get; set; }
        public Symbol(char symbol, char replaceSymbol)
        {
            Origin = symbol;
            Replace = replaceSymbol;
        }
    }
}
