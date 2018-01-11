using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eknowledger.Language.Xpress.Test
{
    [TestClass]
    public class Compiler_ConditionalExpressions
    {
        private static XpressCompiler _compiler;

        [ClassInitialize]
        public static void TestsInitialize(TestContext ctx)
        {
            _compiler = XpressCompiler.Default;
        }

        [TestMethod]
        public void Compile_ConditionalNullAndNumber_ShouldFailCompile()
        {
            var code = "null or 1";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
            // fail - Operator 'or' can only be applied to 'boolean' operands. Error at 'null' on position 0
        }

        [TestMethod]
        public void Compile_ConditionalBooleanAndNumber_ShouldFailCompile()
        {
            var code = "true or 1";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
            // fail - Operator 'or' can only be applied to 'boolean' operands. Error at '1' on position 8
        }

        [TestMethod]
        public void Compile_ConditionalStringAndNumber_ShouldFailCompile()
        {
            var code = "'s' or 1";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
            // fail - Operator 'or' can only be applied to 'boolean' operands. Error at ''s'' on position 0
        }

        [TestMethod]
        public void Compile_ConditionalStringAndNull_ShouldFailCompile()
        {
            var code = "'s' or null";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
            // fail - Operator 'or' can only be applied to 'boolean' operands. Error at ''s'' on position 0
        }

        [TestMethod]
        public void Compile_ConditionalBinaryNumericalExpressionAndBoolean_ShouldFailCompile()
        {
            var code = "(1+1) or true";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
            // fail - Operator 'or' can only be applied to 'boolean' operands. Error at '1' on position 0
        }

        [TestMethod]
        public void Compile_ConditionalFalseOrRelationalExpression_ShouldCompileAndEvalTrue()
        {
            var code = "false or 1 lt 2";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_ConditionalFalseOrRelationalExpressionWithVariable_ShouldCompileAndEvalFalse()
        {
            var code = "false or x lt 2";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "5" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Compile_ConditionalOfRelationals_ShouldCompileAndEvalTrue()
        {
            var code = "x gt 2 and y lt 5";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "5" }, { "y", "1" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_ComplexExpression_ShouldCompileAndEvalTrue()
        {
            var code = "(x ne null and x+1 gt 10) or (y ne null and (y*(5+1)-2) lt 5)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "5" }, { "y", "1" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }
    }
}
