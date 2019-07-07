using System;
using Xunit;

namespace Eknowledger.Language.Xpress.Test
{
    public class Compiler_UnaryExpressions : TestBase
    {
        public Compiler_UnaryExpressions() : base() { }


        [Fact]
        public void Compile_NegationOfBooleanFalseLiteral_ShouldCompileAndEvalTrue()
        {
            var code = "not false";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_NegationOfBooleanFalseVariable_ShouldCompileAndEvalTrue()
        {
            var code = "not x";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "false" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_NegationOfNull_ShouldFailCompile()
        {
            var code = "not null";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_NegationOfString_ShouldFailCompile()
        {
            var code = "not 'true'";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_NegationOfNumber_ShouldFailCompile()
        {
            var code = "not 10";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_NegationOfNonBooleanVariable_ShouldCompileAndRuntimeError()
        {
            var code = "not x";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "1" } };
            var compilationResult = _compiler.Compile(code);

            Exception exception = null;
            try
            {
                var result = compilationResult.Code(runtimeCtx);
            }
            catch (Exception ex)
            {
                exception = ex;
            }


            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.NotNull(exception);
        }

        [Fact]
        public void Compile_NegationOfBinaryExpression_ShouldFailCompile()
        {
            var code = "not (1+2)";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_NegationOfRelationalExpression_ShouldCompileAndEvalTrue()
        {
            var code = "not (9 gt 10)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "false" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_NegationOfConditioanlExpression_ShouldCompileAndEvalTrue()
        {
            var code = "not ('a' eq 'b' and 9 gt 10)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "false" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_NegationOfBinaryConcatStrings_ShouldFailCompile()
        {
            var code = "not ('a' + 'b')";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }
    }
}
