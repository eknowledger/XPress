using Irony;
using Irony.Parsing;
using System;

namespace Eknowledger.Language.Xpress
{
    internal class XpressParser
    {
        private static string logParamName = "log";
        private static string codeParamName = "sourceCode";

        private XpressGrammar _grammar;
        private Parser _parser;

        public XpressParser(ILogWriter log)
        {
            if (log == null)
                throw new ArgumentNullException(logParamName);

            _grammar = new XpressGrammar();
            LanguageData flowLanguageData = new LanguageData(_grammar);
            _parser = new Irony.Parsing.Parser(flowLanguageData);
        }

        public IXpressParseTree Parse(string sourceCode, ILogWriter log)
        {

            log.Debug(XpressConstants.Messages.ParserDebugMessageStartParse,
                DateTime.Now.ToString(XpressConstants.Messages.FormatLongDateTime));
            try
            {
                if (string.IsNullOrEmpty(sourceCode))
                    throw new ArgumentNullException(codeParamName);


                ParseTree parseTree = _parser.Parse(sourceCode);
                foreach (var parserMessage in parseTree.ParserMessages)
                {
                    if (parserMessage.Level == ErrorLevel.Error)
                    {
                        log.Error("Parse error {0}, at location [{1}]", parserMessage.Message, parserMessage.Location.ToString());
                    }
                    else
                        log.Debug(parserMessage.Message);
                }
                if (parseTree.HasErrors())
                    _parser.RecoverFromError();

                return new XpressParseTree(sourceCode, parseTree);
            }
            catch (Exception ex)
            {
                log.Error(XpressConstants.Messages.ParserErrorMessage, ex.Message);
                log.Error(XpressConstants.Messages.ParserErrorMessageDetailed, ex);
                _parser.RecoverFromError();
            }

            log.Debug(XpressConstants.Messages.ParserDebugMessageEndParse, DateTime.Now.ToString(XpressConstants.Messages.FormatLongDateTime));
            return null;

        }

    }
}
