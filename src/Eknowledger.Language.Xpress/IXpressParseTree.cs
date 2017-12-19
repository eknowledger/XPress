namespace Eknowledger.Language.Xpress
{
    internal interface IXpressParseTree
    {
        string Source { get; }
        object Root { get; }
        bool HasErrors { get; }
    }
}
