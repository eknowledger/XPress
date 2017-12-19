using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Irony.Parsing;

namespace Eknowledger.Language.Xpress
{
    public class XpressCompiler
    {
        private XpressParser _parser;
        private ILog _defaultLog;
        private static XpressCompiler _default;
        private static object _lockObj = new object();

        public static XpressCompiler Default
        {
            get
            {
                if (_default == null)
                {
                    lock (_lockObj)
                        if (_default == null)
                            _default = new XpressCompiler();
                }

                return _default;
            }
        }

        public XpressCompiler()
        {
            _defaultLog = new InMemoryLogger();
            _parser = new XpressParser(_defaultLog);
        }

        public XpressCompilationResult Compile(string code)
        {
            ILog log = new InMemoryLogger();
            XpressCompilationResult result = new XpressCompilationResult();
            try
            {
                var parseTree = Parse(code, log);
                if (parseTree != null)
                {
                    var func = Translate(parseTree, log);
                    result.Code = func;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Compilation error: {ex}");
            }

            result.Compiled = !log.HasErrors;
            result.Log = log;
            return result;
        }

        internal IXpressParseTree Parse(string code, ILogWriter log)
        {
            var parseTree = _parser.Parse(code, log);
            return parseTree;
        }

        internal Func<XpressRuntimeContext, bool> Translate(IXpressParseTree parseTree, ILogWriter log)
        {
            try
            {
                var ironyParseTree = (ParseTree)parseTree.Root;
                var parseNodes = ironyParseTree.Root.ChildNodes;

                var requestInfoParam = Expression.Parameter(typeof(XpressRuntimeContext), "context");
                var ctx = new XpressCompilationContext { RuntimeContextParameter = requestInfoParam };

                Expression condition = null;

                foreach (var parseNode in parseNodes)
                {
                    if (parseNode.Term.Name == XpressConstants.Tokens.NonTerminals.Expression)
                    {
                        condition = TranslateExpression(ctx, parseNode);

                        if (!IsRuntimeContextGetExpression(condition) && condition.Type != typeof(bool))
                            throw new Exception($"Expression must evluate to a 'boolean' value. Error at '{parseNode.FindTokenAndGetText()}' on position {parseNode.Span.Location.Position}");
                        condition = ConvertToBoolean(condition);

                        if (condition != null)
                            log.Debug($"{condition.ToString()}");
                    }
                }


                // Define a return statement label and expression
                var returnTarget = Expression.Label(typeof(bool));
                var returnExpression = Expression.Return(returnTarget, condition);

                // Define func body statements
                List<Expression> bodyStatements = new List<Expression>();
                bodyStatements.Add(returnExpression);
                bodyStatements.Add(Expression.Label(returnTarget, condition));

                var bodyExpression = Expression.Block(bodyStatements);
                var lambdaExpression = Expression.Lambda<Func<XpressRuntimeContext, bool>>(bodyExpression, requestInfoParam);

                return lambdaExpression.Compile();

            }
            catch (Exception ex)
            {
                log.Error($"Error interpreting parse tree: {ex.Message}");
            }

            return null;

        }

        private Expression TranslateExpression(XpressCompilationContext ctx, ParseTreeNode expressoinNode)
        {
            //  expression -> expression  booleanOperator  relationalExpression
            //              | relationalExpression

            var nodes = expressoinNode.ChildNodes;

            if (nodes.Count == 3) // expression  booleanOperator  relationalExpression
            {
                var conditionalOp = nodes[1].FindTokenAndGetText().ToLowerInvariant();
                var leftExpr = TranslateExpression(ctx, nodes[0]);
                var rightExpr = TranslateRelationalExpression(ctx, nodes[2]);

                if (!IsRuntimeContextGetExpression(leftExpr) && leftExpr.Type != typeof(bool))
                    throw new Exception($"Operator '{conditionalOp}' can only be applied to 'boolean' operands. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");

                if (!IsRuntimeContextGetExpression(rightExpr) && rightExpr.Type != typeof(bool))
                    throw new Exception($"Operator '{conditionalOp}' can only be applied to 'boolean' operands. Error at '{nodes[2].FindTokenAndGetText()}' on position {nodes[2].Span.Location.Position}");


                leftExpr = ConvertToBoolean(leftExpr);
                rightExpr = ConvertToBoolean(rightExpr);

                if (conditionalOp == XpressConstants.Tokens.Terminals.ConditionalAndOperator)
                    return Expression.AndAlso(leftExpr, rightExpr);
                else
                    return Expression.OrElse(leftExpr, rightExpr);

            }
            else if (nodes.Count == 1) // relationalExpression
            {
                return TranslateRelationalExpression(ctx, nodes[0]);
            }

            return null;
        }

        private Expression TranslateRelationalExpression(XpressCompilationContext ctx, ParseTreeNode relationalNode)
        {
            // relationalExpression -> relationalExpression  relationalOperator  binaryAdditiveExpression
            //                       | binaryAdditiveExpression

            var nodes = relationalNode.ChildNodes;

            // validation
            if (nodes.Count == 3) // relationalExpression  relationalOperator  binaryAdditiveExpression
            {
                var relationalOp = nodes[1].FindTokenAndGetText().ToLowerInvariant();
                var leftExpr = TranslateRelationalExpression(ctx, nodes[0]);
                var rightExpr = TranslateBinaryAdditiveExpression(ctx, nodes[2]);

                var relationalOps = new[] { XpressConstants.Tokens.Terminals.RelationalGreatThanOperator
                                          , XpressConstants.Tokens.Terminals.RelationalLessThanOperator
                                          , XpressConstants.Tokens.Terminals.RelationalGreatThanOrEqualOperator
                                          , XpressConstants.Tokens.Terminals.RelationalLessThanOrEqualOperator };

                if (relationalOps.Contains(relationalOp)) // comparison ops
                {
                    if (leftExpr.NodeType == ExpressionType.Constant && leftExpr.Type != typeof(int))
                        throw new Exception($"Operator '{relationalOp}' can only be applied to 'Int32' operands. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");

                    if (rightExpr.NodeType == ExpressionType.Constant && rightExpr.Type != typeof(int))
                        throw new Exception($"Operator '{relationalOp}' can only be applied to 'Int32' operands. Error at '{nodes[2].FindTokenAndGetText()}' on position {nodes[2].Span.Location.Position}");

                    leftExpr = ConvertToInteger(leftExpr);
                    rightExpr = ConvertToInteger(rightExpr);

                    if (relationalOp == XpressConstants.Tokens.Terminals.RelationalGreatThanOperator)
                        return Expression.GreaterThan(leftExpr, rightExpr);
                    else if (relationalOp == XpressConstants.Tokens.Terminals.RelationalLessThanOperator)
                        return Expression.LessThan(leftExpr, rightExpr);
                    else if (relationalOp == XpressConstants.Tokens.Terminals.RelationalGreatThanOrEqualOperator)
                        return Expression.GreaterThanOrEqual(leftExpr, rightExpr);
                    else if (relationalOp == XpressConstants.Tokens.Terminals.RelationalLessThanOrEqualOperator)
                        return Expression.LessThanOrEqual(leftExpr, rightExpr);
                }
                else // equality ops
                {
                    return TranslateEqualityExpression(ctx, relationalOp, leftExpr, rightExpr, nodes);
                }
            }
            else if (nodes.Count == 1) // binaryAdditiveExpression
            {
                return TranslateBinaryAdditiveExpression(ctx, nodes[0]);
            }

            return null;

        }

        private Expression TranslateEqualityExpression(XpressCompilationContext ctx, string op, Expression leftExpr, Expression rightExpr, ParseTreeNodeList nodes)
        {
            // implicit casting for variables to match the compared operand type
            if (IsRuntimeContextGetExpression(leftExpr))
            {
                if (rightExpr.Type == typeof(bool))
                    leftExpr = ConvertToBoolean(leftExpr);
                else if (rightExpr.Type == typeof(int))
                    leftExpr = ConvertToInteger(leftExpr);
            }
            else if (IsRuntimeContextGetExpression(rightExpr))
            {
                if (leftExpr.Type == typeof(bool))
                    rightExpr = ConvertToBoolean(rightExpr);
                else if (leftExpr.Type == typeof(int))
                    rightExpr = ConvertToInteger(rightExpr);
            }


            if (leftExpr.Type != rightExpr.Type)
            {
                if ((leftExpr.Type == typeof(string) && IsNullExpression(rightExpr)) ||
                         (rightExpr.Type == typeof(string) && IsNullExpression(leftExpr)))
                {
                    // string.IsNullOrEmpty()
                    Expression operand = leftExpr;
                    // handle null comparison
                    if (IsNullExpression(leftExpr))
                        operand = rightExpr;
                    else if (IsNullExpression(rightExpr))
                        operand = leftExpr;

                    Expression nullCheckExpr = null;
                    if (IsRuntimeContextGetExpression(operand))
                    {
                        var keyExpr = ((MethodCallExpression)operand).Arguments[0];

                        if (op == XpressConstants.Tokens.Terminals.RelationalEqualityOperator) // ! requestInfo.Exists(key) || IsNotNullorEmpty(requestInfo.Get(key))
                            nullCheckExpr = Expression.OrElse(
                                Expression.Not(Expression.Call(ctx.RuntimeContextParameter, XpressCompilationContext.RuntimeContextExists, keyExpr)),
                                Expression.Call(XpressCompilationContext.IsNullOrEmptyMethod, operand));
                        else if (op == XpressConstants.Tokens.Terminals.RelationalNonEqualityOperator) // requestInfo.Exists(key) && ! IsNotNullorEmpty(requestInfo.Get(key))
                            nullCheckExpr = Expression.AndAlso(
                                Expression.Call(ctx.RuntimeContextParameter, XpressCompilationContext.RuntimeContextExists, keyExpr),
                                Expression.Not(Expression.Call(XpressCompilationContext.IsNullOrEmptyMethod, operand)));
                    }
                    else
                    {
                        nullCheckExpr = Expression.Call(XpressCompilationContext.IsNullOrEmptyMethod, operand);
                        if (op == XpressConstants.Tokens.Terminals.RelationalNonEqualityOperator)
                            nullCheckExpr = Expression.Not(nullCheckExpr);
                    }

                    return nullCheckExpr;
                }
                else // any other type missmatch .. error out .. we can't implicitly cast
                    throw new Exception($"Operator '{op}' cannot be applied to operands of type '{leftExpr.Type.Name}' and '{rightExpr.Type.Name}'. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");
            }

            // string comparison
            if (leftExpr.Type == typeof(string) || rightExpr.Type == typeof(string))
            {
                //string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase)
                Expression equalityExpr = Expression.Call(XpressCompilationContext.StringEquals, leftExpr, rightExpr, Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
                if (op == XpressConstants.Tokens.Terminals.RelationalNonEqualityOperator)
                    equalityExpr = Expression.Not(equalityExpr);

                return equalityExpr;
            }
            else
            {
                if (op == XpressConstants.Tokens.Terminals.RelationalEqualityOperator)
                    return Expression.Equal(leftExpr, rightExpr);
                else if (op == XpressConstants.Tokens.Terminals.RelationalNonEqualityOperator)
                    return Expression.NotEqual(leftExpr, rightExpr);
            }

            return null;
        }

        private Expression TranslateBinaryAdditiveExpression(XpressCompilationContext ctx, ParseTreeNode addExpressionNode)
        {
            //   binaryAdditiveExpression -> binaryAdditiveExpression  binaryAdditiveOperator  binaryMultiplicityExpression
            //                             | binaryMultiplicityExpression

            var nodes = addExpressionNode.ChildNodes;

            if (nodes.Count == 3) // binaryAdditiveExpression  binaryAdditiveOperator  binaryMultiplicityExpression
            {
                var op = nodes[1].FindTokenAndGetText().ToLowerInvariant();

                if (op == XpressConstants.Tokens.Terminals.BinaryAdditionOperator)
                    return TranslateAdditionExpression(ctx, nodes);
                else if (op == XpressConstants.Tokens.Terminals.BinarySubtrationOperator)
                    return TranslateSubtractionExpression(ctx, nodes);
            }
            else if (nodes.Count == 1) // binaryMultiplicityExpression
            {
                return TranslateBinaryMultiplicityExpression(ctx, nodes[0]);
            }

            return null;
        }

        private Expression TranslateAdditionExpression(XpressCompilationContext ctx, ParseTreeNodeList nodes)
        {
            var leftExpr = TranslateBinaryAdditiveExpression(ctx, nodes[0]);
            var rightExpr = TranslateBinaryMultiplicityExpression(ctx, nodes[2]);

            var supporttedTypes = new[] { typeof(int), typeof(string) };

            // Handle 1+null , 1+ true , true +true, null+null, true+null, true +'a', null+'a'
            if (leftExpr.NodeType == ExpressionType.Constant && !supporttedTypes.Contains(leftExpr.Type))
                throw new Exception($"Operator '{XpressConstants.Tokens.Terminals.BinaryAdditionOperator}' cannot be applied to operands of type {leftExpr.Type.Name}. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");

            if (rightExpr.NodeType == ExpressionType.Constant && !supporttedTypes.Contains(rightExpr.Type))
                throw new Exception($"Operator '{XpressConstants.Tokens.Terminals.BinaryAdditionOperator}' cannot be applied to operands of type {leftExpr.Type.Name} and {rightExpr.Type.Name}. Error at '{nodes[2].FindTokenAndGetText()}' on position {nodes[2].Span.Location.Position}");

            // Handle 1 + '2' or '1' + 2 cases
            if (leftExpr.Type == typeof(int) && rightExpr.Type == typeof(string)
                && rightExpr.NodeType == ExpressionType.Constant)
                throw new Exception($"Operator '{XpressConstants.Tokens.Terminals.BinaryAdditionOperator}' cannot be applied to operands of type {leftExpr.Type.Name} and {rightExpr.Type.Name}. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");

            if (rightExpr.Type == typeof(int) && leftExpr.Type == typeof(string) && leftExpr.NodeType == ExpressionType.Constant)
                throw new Exception($"Operator '{XpressConstants.Tokens.Terminals.BinaryAdditionOperator}' cannot be applied to operands of type {leftExpr.Type.Name}. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");



            if (leftExpr.Type == typeof(int) || rightExpr.Type == typeof(int))
            {
                // add integers
                leftExpr = ConvertToInteger(leftExpr);
                rightExpr = ConvertToInteger(rightExpr);

                return Expression.Add(leftExpr, rightExpr);
            }
            else
            {
                // string.concat()
                leftExpr = ConvertToString(leftExpr);
                rightExpr = ConvertToString(rightExpr);

                var stringConcatMethod = typeof(string).GetMethod("Concat",
                        (BindingFlags.Public | BindingFlags.Static), null, CallingConventions.Any,
                        new Type[] { typeof(string), typeof(string) }, null);
                return Expression.Call(stringConcatMethod, leftExpr, rightExpr);
            }
        }

        private Expression TranslateSubtractionExpression(XpressCompilationContext ctx, ParseTreeNodeList nodes)
        {
            var leftExpr = TranslateBinaryAdditiveExpression(ctx, nodes[0]);
            var rightExpr = TranslateBinaryMultiplicityExpression(ctx, nodes[2]);

            if (leftExpr.NodeType == ExpressionType.Constant && leftExpr.Type != typeof(int))
                throw new Exception($"Operator '{XpressConstants.Tokens.Terminals.BinarySubtrationOperator}' cannot be applied to operands of type {leftExpr.Type.Name}. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");

            if (rightExpr.NodeType == ExpressionType.Constant && rightExpr.Type != typeof(int))
                throw new Exception($"Operator '{XpressConstants.Tokens.Terminals.BinarySubtrationOperator}' cannot be applied to operands of type {rightExpr.Type.Name}. Error at '{nodes[2].FindTokenAndGetText()}' on position {nodes[2].Span.Location.Position}");


            // add integers
            leftExpr = ConvertToInteger(leftExpr);
            rightExpr = ConvertToInteger(rightExpr);

            return Expression.Subtract(leftExpr, rightExpr);
        }

        private Expression TranslateBinaryMultiplicityExpression(XpressCompilationContext ctx, ParseTreeNode multiplyExpressionNode)
        {
            //   binaryMultiplicityExpression -> binaryMultiplicityExpression  binaryMultiplicityOperator  unaryExpression
            //                                 | unaryExpression

            var nodes = multiplyExpressionNode.ChildNodes;

            if (nodes.Count == 3) // binaryMultiplicityExpression  binaryMultiplicityOperator  unaryExpression
            {
                var op = nodes[1].FindTokenAndGetText().ToLowerInvariant();
                var leftExpr = TranslateBinaryMultiplicityExpression(ctx, nodes[0]);
                var rightExpr = TranslateUnaryExpression(ctx, nodes[2]);

                if (leftExpr.NodeType == ExpressionType.Constant && leftExpr.Type != typeof(int))
                    throw new Exception($"Operator '{op}' can only be applied to 'Int32' constants. Error at '{nodes[0].FindTokenAndGetText()}' on position {nodes[0].Span.Location.Position}");

                if (rightExpr.NodeType == ExpressionType.Constant && rightExpr.Type != typeof(int))
                    throw new Exception($"Operator '{op}' can only be applied to 'Int32' constants. Error at '{nodes[2].FindTokenAndGetText()}' on position {nodes[2].Span.Location.Position}");

                leftExpr = ConvertToInteger(leftExpr);
                rightExpr = ConvertToInteger(rightExpr);

                if (op == XpressConstants.Tokens.Terminals.BinaryMultiplicationOperator)
                    return Expression.Multiply(leftExpr, rightExpr);
                else if (op == XpressConstants.Tokens.Terminals.BinaryDivisionOperator)
                    return Expression.Divide(leftExpr, rightExpr);
                else if (op == XpressConstants.Tokens.Terminals.BinaryModuloOperator)
                    return Expression.Modulo(leftExpr, rightExpr);
            }
            else if (nodes.Count == 1) // prefixExpression
            {
                return TranslateUnaryExpression(ctx, nodes[0]);
            }

            return null;
        }

        private Expression TranslateUnaryExpression(XpressCompilationContext ctx, ParseTreeNode prefixExpressionNode)
        {
            //  unaryExpression -> unaryOperator  term
            //                   | primaryExpression

            var nodes = prefixExpressionNode.ChildNodes;

            if (nodes.Count == 2) // unaryOperator primaryExpression
            {
                var rightExpr = TranslatePrimaryExpression(ctx, nodes[1]);
                if (!IsRuntimeContextGetExpression(rightExpr) && rightExpr.Type != typeof(bool))
                    throw new Exception($"Operator '{XpressConstants.Tokens.Terminals.UnaryNegationOperator}' can only be applied to 'boolean' operands. Error at '{nodes[1].FindTokenAndGetText()}' on position {nodes[1].Span.Location.Position}");

                var op = nodes[0].FindTokenAndGetText().ToLowerInvariant();

                if (op == XpressConstants.Tokens.Terminals.UnaryNegationOperator)
                    return Expression.Not(ConvertToBoolean(rightExpr));
            }
            else if (nodes.Count == 1) // term
            {
                return TranslatePrimaryExpression(ctx, nodes[0]);
            }

            return null;
        }

        private Expression TranslatePrimaryExpression(XpressCompilationContext ctx, ParseTreeNode termExpressionNode)
        {
            //  primaryExpression -> Identifier | NumberLiteral | StringLiteral | booleanLiteral | nullLiteral | parenExpression

            // nullLiteral -> ToTerm("null")
            // booleanLiteral -> ToTerm("true") | "false"

            var nodes = termExpressionNode.ChildNodes;

            if (nodes[0].Term.Name == XpressConstants.Tokens.NonTerminals.Identifier)
            {
                var identifierKey = nodes[0].Token.ValueString;
                var identifierKeyExpression = Expression.Constant(identifierKey, typeof(string));

                var requestInfoGetMethodInfo = XpressCompilationContext.RuntimeContextGet;

                var requestInfoGetIdentifier = Expression.Call(ctx.RuntimeContextParameter, requestInfoGetMethodInfo, identifierKeyExpression);

                return requestInfoGetIdentifier;

            }
            else if (nodes[0].Term.Name == XpressConstants.Tokens.NonTerminals.NumberLiteral)
            {
                var number = (int)nodes[0].Token.Value;
                return Expression.Constant(number, typeof(int));
            }

            else if (nodes[0].Term.Name == XpressConstants.Tokens.NonTerminals.StringLiteral)
            {
                var str = (string)nodes[0].Token.Value;
                return Expression.Constant(str, typeof(string));
            }
            else if (nodes[0].Term.Name == XpressConstants.Tokens.NonTerminals.BooleanLiteral)
            {
                var op = nodes[0].FindTokenAndGetText().ToLowerInvariant();

                if (op == XpressConstants.Tokens.Terminals.BooleanTrueValue)
                    return Expression.Constant(true, typeof(bool));

                if (op == XpressConstants.Tokens.Terminals.BooleanFalseValue)
                    return Expression.Constant(false, typeof(bool));
            }
            else if (nodes[0].Term.Name == XpressConstants.Tokens.NonTerminals.NullLiteral)
            {
                return Expression.Constant(null);
            }
            else if (nodes[0].Term.Name == XpressConstants.Tokens.NonTerminals.ParenExpression)
            {
                return TranslateExpression(ctx, nodes[0].ChildNodes[0]);
            }
            return null;
        }

        private bool IsNullExpression(Expression expression)
        {
            return expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value == null;
        }

        private bool IsRuntimeContextGetExpression(Expression expression)
        {
            return expression.NodeType == ExpressionType.Call && ((MethodCallExpression)expression).Method == XpressCompilationContext.RuntimeContextGet;
        }

        private static Expression ConvertToBoolean(Expression sourceExpression)
        {
            var sourceType = sourceExpression.Type;

            if (sourceType == typeof(bool)) return sourceExpression;

            if (sourceType != typeof(string))
            {
                var toStringMethod = sourceType.GetMethod("ToString",
                    (BindingFlags.Public | BindingFlags.Instance), null, CallingConventions.Any, new Type[] { }, null);
                sourceExpression = Expression.Call(sourceExpression, toStringMethod);
            }

            var boolParseMethod = typeof(bool).GetMethod("Parse",
                        (BindingFlags.Public | BindingFlags.Static), null, CallingConventions.Any,
                        new Type[] { typeof(string) }, null);

            return Expression.Call(boolParseMethod, sourceExpression);
        }

        private static Expression ConvertToInteger(Expression sourceExpression)
        {
            var sourceType = sourceExpression.Type;

            if (sourceType == typeof(int)) return sourceExpression;

            if (sourceType != typeof(string))
            {
                var toStringMethod = sourceType.GetMethod("ToString",
                    (BindingFlags.Public | BindingFlags.Instance), null, CallingConventions.Any, new Type[] { }, null);
                sourceExpression = Expression.Call(sourceExpression, toStringMethod);
            }


            var boolParseMethod = typeof(int).GetMethod("Parse",
                        (BindingFlags.Public | BindingFlags.Static), null, CallingConventions.Any,
                        new Type[] { typeof(string) }, null);

            return Expression.Call(boolParseMethod, sourceExpression);
        }

        private static Expression ConvertToString(Expression sourceExpression)
        {
            var sourceType = sourceExpression.Type;

            if (sourceType == typeof(string)) return sourceExpression;

            var toStringMethod = sourceType.GetMethod("ToString",
                (BindingFlags.Public | BindingFlags.Instance), null, CallingConventions.Any, new Type[] { }, null);
            return Expression.Call(sourceExpression, toStringMethod);
        }
    }
}
