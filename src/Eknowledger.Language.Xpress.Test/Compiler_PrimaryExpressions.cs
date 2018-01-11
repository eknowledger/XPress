using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eknowledger.Language.Xpress.Test
{
    [TestClass]
    public class Compiler_PrimaryExpressions
    {
        private static XpressCompiler _compiler;

        [ClassInitialize]
        public static void TestsInitialize(TestContext ctx)
        {
            _compiler = XpressCompiler.Default;
        }

        [TestMethod]
        public void Compile_BooleanTrueExpression_ShouldCompileAndEvalTrue()
        {
            var code = "true";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_BooleanFalseExpression_ShouldCompileAndEvalFalse()
        {
            var code = "false";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void Compile_NullExpression_ShouldFailCompile()
        {
            var code = "null";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
        }

        [TestMethod]
        public void Compile_StringTrueExpression_ShouldFailCompile()
        {
            var code = "'true'";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
        }

        [TestMethod]
        public void Compile_NumericalExpression_ShouldFailCompile()
        {
            var code = "1";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
        }

        [TestMethod]
        public void Compile_VariableDoesnotExists_ShouldCompileRuntimeErrorVariableNotFound()
        {
            var code = "x";
            var compilationResult = _compiler.Compile(code);

            Exception exception = null;
            var ctx = new XpressRuntimeContext();
            try
            {
                var result = compilationResult.Code(ctx);
            }
            catch (Exception ex)
            {
                exception = ex;
            }


            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception.Message, "Variable [x] was not found.");
        }

        [TestMethod]
        public void Compile_VariableInvalidType_ShouldCompileRuntimeErrorFormatException()
        {
            var code = "x";
            var compilationResult = _compiler.Compile(code);

            Exception exception = null;
            var ctx = new XpressRuntimeContext() { { "x", "4" } };

            try
            {
                var result = compilationResult.Code(ctx);
            }
            catch (Exception ex)
            {
                exception = ex;
            }


            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception.Message, "String was not recognized as a valid Boolean.");
        }

        [TestMethod]
        public void Compile_VariableTypeBoolean_ShouldCompileAndEvalTrue()
        {
            var code = "x";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "true" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_BinaryOperatorPrecedence_ShouldCompileAndEvalTrue()
        {
            var code = "r eq x+2*2-4/2"; // ((x+(2*2))-(4/2))
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "6" }, { "r", "8" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_BinaryOperatorPrecedenceInvalid_ShouldCompileAndEvalFalse()
        {
            var code = "r eq x+2*2-4/2"; // ((x+(2*2))-(4/2))
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "6" }, { "r", "2" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Compile_BinaryOperatorPrecedenceWithParanetcise_ShouldCompileAndEvalTrue()
        {
            var code = "r eq (((x+1)*2)-4)/5";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "6" }, { "r", "2" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

    }
}
