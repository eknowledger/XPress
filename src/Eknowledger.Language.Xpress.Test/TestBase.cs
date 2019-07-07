using System;

namespace Eknowledger.Language.Xpress.Test
{
    public abstract class TestBase : IDisposable
    {
        protected XpressCompiler _compiler;

        public TestBase()
        {
            _compiler = new XpressCompiler();
        }

        public void Dispose()
        {
            _compiler = null;
        }
    }
}
