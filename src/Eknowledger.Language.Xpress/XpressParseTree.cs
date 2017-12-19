using Irony.Parsing;
using System;

namespace Eknowledger.Language.Xpress
{
    internal class XpressParseTree : IXpressParseTree
    {
        private ParseTree _ironyParseTree;

        public XpressParseTree(string sourceCode, ParseTree parseTree)
        {
            Source = sourceCode;
            if (parseTree == null)
                throw new ArgumentNullException("Irony parse tree can't be null");

            _ironyParseTree = parseTree;
        }

        public string Source { get; private set; }
        public object Root { get { return _ironyParseTree; } }

        public ParseTree RootNode { get { return _ironyParseTree; } }

        public bool HasErrors
        {
            get
            {
                if (_ironyParseTree != null)
                    return _ironyParseTree.HasErrors();
                return false;
            }
        }
    }
}
