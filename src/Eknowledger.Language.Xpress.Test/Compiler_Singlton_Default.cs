using System;
using Xunit;

namespace Eknowledger.Language.Xpress.Test
{
    public class Compiler_Singlton_Default : IDisposable
    {
        private XpressCompiler _compiler;

        public Compiler_Singlton_Default()
        {
            _compiler = new XpressCompiler();
        }

        public void Dispose()
        {
            _compiler = null;
        }

        [Fact]
        public void Compile_Default_ComplexExpression_ShouldCompileAndEvalTrue()
        {
            var code = "(x ne null and x+1 gt 10) or (y ne null and (y*(5+1)-2) lt 5)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "5" }, { "y", "1" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_Default_RelationalVariableGreaterThanVariable_ShouldCompileAndEvalTrue()
        {
            var code = "x gt y";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "10" }, { "y", "9" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }
    }
}
