﻿using System;
using Xunit;

namespace Eknowledger.Language.Xpress.Test
{
    public class Compiler_BinaryExpressions : TestBase
    {
        public Compiler_BinaryExpressions() : base() { }

        [Fact]
        public void Compile_BinaryAdditionTwoNumbers_ShouldCompileAndEvalTrue()
        {
            var code = "x eq (1+2)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinaryAdditionVariableAndNumber_ShouldCompileAndEvalTrue()
        {
            var code = "x lt (x+2)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinaryAdditionConcatTwoStrings_ShouldCompileAndEvalTrue()
        {
            var code = "x eq ('ahmed '+'elmalt')";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "ahmed elmalt" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinarySubtractTwoStrings_ShouldFailCompile()
        {
            var code = "'hmed' eq ('ahmed' - 'a')";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryAdditionNumberAndString_ShouldCompileRuntimeError()
        {
            var code = "x eq (1 + '2')";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);

        }

        [Fact]
        public void Compile_BinarySubtractBooleanAndNumber_ShouldFailCompile()
        {
            var code = "x gt (true - 1)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryAddNullAndNumber_ShouldFailCompile()
        {
            var code = "x gt (null + 1)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryVariableSubtractNumber_ShouldCompileAndEvalTrue()
        {
            var code = "x gt (x - 1)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "10" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinaryVariableSubtractNull_ShouldFailCompile()
        {
            var code = "x gt (x - null)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryMultiplyTwoNumbers_ShouldCompileAndEvalTrue()
        {
            var code = "x gt (1*2)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "10" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinaryNumberMultiplyString_ShouldFailCompile()
        {
            var code = "x gt (1* '2')";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryBooleanMultiplyNumber_ShouldFailCompile()
        {
            var code = "x gt (true * 1)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryNullMultiplyNumber_ShouldFailCompile()
        {
            var code = "x gt (null * 1)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryVariableMultiplyNull_ShouldFailCompile()
        {
            var code = "x gt (x * null)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "3" } };
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryVariableMultiplyNumber_ShouldCompileAndEvalTrue()
        {
            var code = "x eq (x * 1)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "10" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinaryDividingNumbers_ShouldCompileAndEvalTrue()
        {
            var code = "x eq (8 / 2)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "4" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinaryModuloNumbers_ShouldCompileAndEvalTrue()
        {
            var code = "x eq (11 % 2)";
            XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "1" } };
            var compilationResult = _compiler.Compile(code);
            var result = compilationResult.Code(runtimeCtx);

            Assert.True(compilationResult.Compiled);
            Assert.False(compilationResult.Log.HasErrors);
            Assert.True(result);
        }

        [Fact]
        public void Compile_BinaryNumericalExpression_ShouldFailCompile()
        {
            var code = "1+6";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }

        [Fact]
        public void Compile_BinaryBinaryExpressionWithVariable_ShouldFailCompile()
        {
            var code = "x+6";
            var compilationResult = _compiler.Compile(code);

            Assert.False(compilationResult.Compiled);
            Assert.True(compilationResult.Log.HasErrors);
        }
    }
}
