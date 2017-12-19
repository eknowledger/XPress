using System;

namespace Eknowledger.Language.Xpress
{
    public class XpressCompilationResult
    {
        public bool Compiled;
        public ILogReader Log;
        public Func<XpressRuntimeContext, bool> Code;
    }
}
