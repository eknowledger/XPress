namespace Eknowledger.Language.Xpress
{
    internal static class XpressConstants
    {
        internal static class Tokens
        {
            internal static class Terminals
            {
                internal const string NullValue = "null";
                internal const string BooleanTrueValue = "true";
                internal const string BooleanFalseValue = "false";
                internal const string StringStartEndSymbol = "'";
                internal const string ExpressionBeginSymbol = "(";
                internal const string ExpressionEndSymbol = ")";
                internal const string ConditionalAndOperator = "and";
                internal const string ConditionalOrOperator = "or";
                internal const string RelationalGreatThanOperator = "gt";
                internal const string RelationalLessThanOperator = "lt";
                internal const string RelationalGreatThanOrEqualOperator = "ge";
                internal const string RelationalLessThanOrEqualOperator = "le";
                internal const string RelationalEqualityOperator = "eq";
                internal const string RelationalNonEqualityOperator = "ne";
                internal const string UnaryNegationOperator = "not";
                internal const string BinaryAdditionOperator = "+";
                internal const string BinarySubtrationOperator = "-";
                internal const string BinaryMultiplicationOperator = "*";
                internal const string BinaryDivisionOperator = "/";
                internal const string BinaryModuloOperator = "%";
            }

            internal static class NonTerminals
            {
                internal const string StringLiteral = "stringLiteral";
                internal const string NumberLiteral = "numberLiteral";
                internal const string Identifier = "identifier";
                internal const string ParenExpression = "parenExpression";
                internal const string Expression = "expression";
                internal const string ConditionalOperator = "conditionalOperator";
                internal const string BinaryAdditiveExpression = "binaryAdditiveExpression";
                internal const string RelationalExpression = "relationalExpression";
                internal const string RelationalOperator = "relationalOperator";
                internal const string BinaryMultiplicityExpression = "binaryMultiplicityExpression";
                internal const string BinaryAdditiveOperator = "binaryAdditiveOperator";
                internal const string UnaryExpression = "unaryExpression";
                internal const string BinaryMultiplicityOperator = "binaryMultiplicityOperator";
                internal const string UnaryOperator = "unaryOperator";
                internal const string PrimaryExpression = "primaryExpression";
                internal const string Statement = "statement";
                internal const string NullLiteral = "nullLiteral";
                internal const string BooleanLiteral = "booleanLiteral";
            }
        }

        internal static class Messages
        {
            internal const string FormatLongDateTime = "MM/dd/yyyy hh:mm:ss.fff tt";
            // Parser Constants
            internal const string ParserDebugMessageStartParse = "Parser: Start parsing\t{0}";
            internal const string ParserDebugMessageEndParse = "Parser: End parsing\t{0}";
            internal const string ParserErrorMessage = "Parser error: {0}";
            internal const string ParserErrorMessageDetailed = "Parser detailed error: {0}";
            internal const string ParserDebugMessageNoGrammar = "Warnning: no grammar was specified!";
        }
    }
}
