using Xunit;

namespace Eknowledger.Language.Xpress.Test
{
    public class Compiler_RelationalExpressions : TestBase
    {
        public Compiler_RelationalExpressions() : base() { }

        [Fact]
        public void Compile_RelationalGreaterThan_ShouldCompileAndEvalTrue()
        {
            var code = "10 gt 9";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_RelationalLessThan_ShouldCompileAndEvalTrue()
        {
            var code = "9 lt 10";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_RelationalGreaterThanOrEqual_ShouldCompileAndEvalTrue()
        {
            var code = "10 ge 10";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_RelationalLessThanOrEqual_ShouldCompileAndEvalTrue()
        {
            var code = "9 le 10";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_RelationalVariableGreaterThanNumber_ShouldCompileAndEvalTrue()
        {
            var code = "x gt 9";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "10" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_RelationalNumberLessThanVariable_ShouldCompileAndEvalTrue()
        {
            var code = "9 lt x";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "10" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_RelationalNumberGreaterThanString_ShouldFailCompile()
        {
            var code = "10 gt '9'";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
            // fail - operator 'gt' can only be applied to boolean constants. Error at ''9'' position 6
        }

        [Fact]
        public void Compile_RelationalStringGreaterThanNumber_ShouldFailCompile()
        {
            var code = "'10' gt 9";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalVariableGreaterThanString_ShouldFailCompile()
        {
            var code = "x gt '9'";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalStringGreaterThanVariable_ShouldFailCompile()
        {
            var code = "'9' gt x";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }


        [Fact]
        public void Compile_RelationalNumberGreaterThanBoolean_ShouldFailCompile()
        {
            var code = "9 gt true";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalBooleanGreaterThanNumber_ShouldFailCompile()
        {
            var code = "false gt 10";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalNumberGreaterThanNull_ShouldFailCompile()
        {
            var code = "9 gt null";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalNullGreaterThanNumber_ShouldFailCompile()
        {
            var code = "null gt 9";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalVariableGreaterThanBoolean_ShouldFailCompile()
        {
            var code = "x gt true";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalVariableGreaterThanNull_ShouldFailCompile()
        {
            var code = "x gt null";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_RelationalVariableGreaterThanVariable_ShouldCompileAndEvalTrue()
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
