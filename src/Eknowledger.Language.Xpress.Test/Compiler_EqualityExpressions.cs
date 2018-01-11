using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eknowledger.Language.Xpress.Test
{
    [TestClass]
    public class Compiler_EqualityExpressions
    {
        private static XpressCompiler _compiler;

        [ClassInitialize]
        public static void TestsInitialize(TestContext ctx)
        {
            _compiler = XpressCompiler.Default;
        }

        [TestMethod]
        public void Compile_EqualityEqualTwoNumbers_ShouldCompileAndEvalFalse()
        {
            var code = "1 eq 4";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Compile_EqualityNotEqualTwoNumbers_ShouldCompileAndEvalTrue()
        {
            var code = "1 ne 4";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityNumberEqualString_ShouldFailCompile()
        {
            var code = "1 eq '1'";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);

            // fail - operator 'eq' cannot be applied to operands of type 'Int32' and 'String'. Error at '1' position 0
        }

        [TestMethod]
        public void Compile_EqualityStringEqualNumber_ShouldFailCompile()
        {
            var code = "'1' eq 1";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
        }

        [TestMethod]
        public void Compile_EqualityNullEqualNull_ShouldCompileAndEvalTrue()
        {
            var code = "null eq null";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityStringEqualString_ShouldCompileAndEvalTrue()
        {
            var code = "'1' eq '1'";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityStringEqualStringCaseInsensitive_ShouldCompileAndEvalTrue()
        {
            var code = "'ahmed' eq 'AHMED'";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityBooleanEqualBoolean_ShouldCompileAndEvalTrue()
        {
            var code = "true eq true";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext();
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityBooleanEqualString_ShouldFailCompile()
        {
            var code = "true eq 'true'";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);

            // fail - operator 'eq' cannot be applied to operands of type 'Boolean' and 'String'. Error at 'true' position 0
        }

        [TestMethod]
        public void Compile_EqualityStringEqualBoolean_ShouldFailCompile()
        {
            var code = "'true' eq true";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
        }

        [TestMethod]
        public void Compile_EqualityNumberEqualNull_ShouldFailCompile()
        {
            var code = "1 eq null";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
            // fail - operator 'eq' cannot be applied to operands of type 'Int32' and 'Object'. Error at '1' position 0
        }

        [TestMethod]
        public void Compile_EqualityBooleanEqualNull_ShouldFailCompile()
        {
            var code = "false eq null";
            var compilationResult = _compiler.Compile(code);

            Assert.IsFalse(compilationResult.Compiled);
            Assert.IsTrue(compilationResult.Log.HasErrors);
            // fail - operator 'eq' cannot be applied to operands of type 'Int32' and 'Object'. Error at '1' position 0
        }

        [TestMethod]
        public void Compile_EqualityEmptyStringEqualNull_ShouldCompileAndEvalTrue()
        {
            // this is odd case null is pretty much empty only works for strings since the language doesn't support objects
            var code = "'' eq null";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext();
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
            // fail - operator 'eq' cannot be applied to operands of type 'Int32' and 'Object'. Error at '1' position 0
        }

        [TestMethod]
        public void Compile_EqualityVariableEqualNull_ShouldCompileAndEvalTrue()
        {
            var code = "x eq null";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityNullEqualVariable_ShouldCompileAndEvalTrue()
        {
            var code = "null eq x";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityVariableEqualNumber_ShouldCompileAndEvalTrue()
        {
            var code = "x eq 1";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "1" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityVariableEqualBoolean_ShouldCompileAndEvalTrue()
        {
            var code = "x eq true";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "true" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityWithVariableNull_ShouldCompileRuntimeError()
        {
            var code = "10 ne x";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", null } };

            Exception exception = null;
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
        }

        [TestMethod]
        public void Compile_EqualityWithVariableEmptyString_ShouldCompileRuntimeError()
        {
            var code = "10 ne x";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "" } };

            Exception exception = null;
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
        }

        [TestMethod]
        public void Compile_EqualityEmptyVariableEqualNull_ShouldCompileAndEvalTrue()
        {
            // Check equality with null, will verify that variable exists first
            var code = "x eq null";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityVariableDoesnotEqualNull_ShouldCompileAndEvalTrue()
        {
            // Check equality with null, will verify that variable exists first
            var code = "xy eq null";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityVariableExistsAndNotEqualNull_ShouldCompileAndEvalFalse()
        {
            // Check equality with null, will verify that variable exists first
            var code = "x eq null";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "zzz" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Compile_EqualityVariableExistsAndNotEqualNull_ShouldCompileAndEvalTrue()
        {
            // Check equality with null, will verify that variable exists first
            var code = "x ne null";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "zzz" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Compile_EqualityVariableNotExistsAndNotEqualNull_ShouldCompileAndEvalFalse()
        {
            // Check equality with null, will verify that variable exists first
            var code = "xy ne null";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "zzz" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Compile_EqualityVariableNotExistsAndEqualEmptyString_ShouldCompileAndRuntimeError()
        {
            // Equality comparison with empty string will not check for existance first
            var code = "xy ne ''";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "" } };

            Exception exception = null;
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
        }

        [TestMethod]
        public void Compile_EqualityBinaryExprEqualBinaryExpr_ShouldCompileAndEvalTrue()
        {
            var code = "x+1 eq y+5";
            var compilationResult = _compiler.Compile(code);
            var ctx = new XpressRuntimeContext() { { "x", "5" }, { "y", "1" } };
            var result = compilationResult.Code(ctx);

            Assert.IsTrue(compilationResult.Compiled);
            Assert.IsFalse(compilationResult.Log.HasErrors);
            Assert.IsTrue(result);
        }
    }
}
