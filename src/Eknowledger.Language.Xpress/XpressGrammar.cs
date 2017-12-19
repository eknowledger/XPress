using Irony.Parsing;
using System;

namespace Eknowledger.Language.Xpress
{
    [Language("Xpress Language", "0.1", "GoFormz Simple Expression Language 2017")]
    public class XpressGrammar : Grammar
    {
        public XpressGrammar() : base(caseSensitive: false)
        {

            GrammarComments = string.Format("Built on {0}", DateTime.Now);
            Init();
        }

        protected void Init()
        {
            InitProductions();
            InitConfiguration();
        }

        internal virtual void InitProductions()
        {
            #region Definitions
            var statement = new NonTerminal(XpressConstants.Tokens.NonTerminals.Statement);
            var expression = new NonTerminal(XpressConstants.Tokens.NonTerminals.Expression);
            var parenExpression = new NonTerminal(XpressConstants.Tokens.NonTerminals.ParenExpression);
            var relationalExpression = new NonTerminal(XpressConstants.Tokens.NonTerminals.RelationalExpression);
            var binaryAdditiveExpression = new NonTerminal(XpressConstants.Tokens.NonTerminals.BinaryAdditiveExpression);
            var binaryMultiplicityExpression = new NonTerminal(XpressConstants.Tokens.NonTerminals.BinaryMultiplicityExpression);
            var unaryExpression = new NonTerminal(XpressConstants.Tokens.NonTerminals.UnaryExpression);
            var conditionalOperator = new NonTerminal(XpressConstants.Tokens.NonTerminals.ConditionalOperator);
            var relationalOperator = new NonTerminal(XpressConstants.Tokens.NonTerminals.RelationalOperator);
            var binaryAdditiveOperator = new NonTerminal(XpressConstants.Tokens.NonTerminals.BinaryAdditiveOperator);
            var binaryMultiplicityOperator = new NonTerminal(XpressConstants.Tokens.NonTerminals.BinaryMultiplicityOperator);
            var unaryOperator = new NonTerminal(XpressConstants.Tokens.NonTerminals.UnaryOperator);
            var primaryExpression = new NonTerminal(XpressConstants.Tokens.NonTerminals.PrimaryExpression);
            var nullLiteral = new NonTerminal(XpressConstants.Tokens.NonTerminals.NullLiteral);
            var booleanLiteral = new NonTerminal(XpressConstants.Tokens.NonTerminals.BooleanLiteral);
            var identifier = TerminalFactory.CreateCSharpIdentifier(XpressConstants.Tokens.NonTerminals.Identifier);
            var numberLiteral = TerminalFactory.CreateCSharpNumber(XpressConstants.Tokens.NonTerminals.NumberLiteral);
            var stringLiteral = TerminalFactory.CreateCSharpString(XpressConstants.Tokens.NonTerminals.StringLiteral);

            var expressionBegin = ToTerm(XpressConstants.Tokens.Terminals.ExpressionBeginSymbol);
            var expressionEnd = ToTerm(XpressConstants.Tokens.Terminals.ExpressionEndSymbol);
            var conditionalAndOperator = ToTerm(XpressConstants.Tokens.Terminals.ConditionalAndOperator);
            var conditionalOrOperator = ToTerm(XpressConstants.Tokens.Terminals.ConditionalOrOperator);
            var relationalGreatThanOperator = ToTerm(XpressConstants.Tokens.Terminals.RelationalGreatThanOperator);
            var relationalLessThanOperator = ToTerm(XpressConstants.Tokens.Terminals.RelationalLessThanOperator);
            var relationalGreatThanOrEqualOperator = ToTerm(XpressConstants.Tokens.Terminals.RelationalGreatThanOrEqualOperator);
            var relationalLessThanOrEqualOperator = ToTerm(XpressConstants.Tokens.Terminals.RelationalLessThanOrEqualOperator);
            var relationalEqualityOperator = ToTerm(XpressConstants.Tokens.Terminals.RelationalEqualityOperator);
            var relationalNonEqualityOperator = ToTerm(XpressConstants.Tokens.Terminals.RelationalNonEqualityOperator);
            var binaryAdditionOperator = ToTerm(XpressConstants.Tokens.Terminals.BinaryAdditionOperator);
            var binarySubtrationOperator = ToTerm(XpressConstants.Tokens.Terminals.BinarySubtrationOperator);
            var binaryMultiplicationOperator = ToTerm(XpressConstants.Tokens.Terminals.BinaryMultiplicationOperator);
            var binaryDivisionOperator = ToTerm(XpressConstants.Tokens.Terminals.BinaryDivisionOperator);
            var binaryModuloOperator = ToTerm(XpressConstants.Tokens.Terminals.BinaryModuloOperator);
            var unaryNegationOperator = ToTerm(XpressConstants.Tokens.Terminals.UnaryNegationOperator);
            var nullValue = ToTerm(XpressConstants.Tokens.Terminals.NullValue);
            var booleanTrueValue = ToTerm(XpressConstants.Tokens.Terminals.BooleanTrueValue);
            var booleanFalseValue = ToTerm(XpressConstants.Tokens.Terminals.BooleanFalseValue);
            stringLiteral.AddStartEnd(XpressConstants.Tokens.Terminals.StringStartEndSymbol, StringOptions.NoEscapes);
            #endregion

            // BNF

            Root = statement;
            statement.Rule = expression;

            parenExpression.Rule = expressionBegin + expression + expressionEnd;

            expression.Rule
                = expression + conditionalOperator + relationalExpression
                | relationalExpression;

            conditionalOperator.Rule = conditionalAndOperator | conditionalOrOperator;

            relationalExpression.Rule
                = relationalExpression + relationalOperator + binaryAdditiveExpression
                | binaryAdditiveExpression;

            relationalOperator.Rule = relationalGreatThanOperator
                                    | relationalGreatThanOrEqualOperator
                                    | relationalLessThanOperator
                                    | relationalLessThanOrEqualOperator
                                    | relationalEqualityOperator
                                    | relationalNonEqualityOperator;

            binaryAdditiveExpression.Rule
                = binaryAdditiveExpression + binaryAdditiveOperator + binaryMultiplicityExpression
                | binaryMultiplicityExpression;

            binaryAdditiveOperator.Rule = binaryAdditionOperator | binarySubtrationOperator;

            binaryMultiplicityExpression.Rule
                = binaryMultiplicityExpression + binaryMultiplicityOperator + unaryExpression
                | unaryExpression;

            binaryMultiplicityOperator.Rule = binaryMultiplicationOperator
                                  | binaryDivisionOperator
                                  | binaryModuloOperator;

            unaryExpression.Rule = unaryOperator + primaryExpression
                                  | primaryExpression;

            unaryOperator.Rule = unaryNegationOperator;

            primaryExpression.Rule = identifier
                      | numberLiteral
                      | stringLiteral
                      | booleanLiteral
                      | nullLiteral
                      | parenExpression;

            nullLiteral.Rule = nullValue;
            booleanLiteral.Rule = booleanTrueValue | booleanFalseValue;
        }

        internal virtual void InitConfiguration()
        {
            MarkReservedWords(XpressConstants.Tokens.Terminals.ConditionalAndOperator
                            , XpressConstants.Tokens.Terminals.ConditionalOrOperator
                            , XpressConstants.Tokens.Terminals.RelationalGreatThanOperator
                            , XpressConstants.Tokens.Terminals.RelationalGreatThanOrEqualOperator
                            , XpressConstants.Tokens.Terminals.RelationalLessThanOperator
                            , XpressConstants.Tokens.Terminals.RelationalLessThanOrEqualOperator
                            , XpressConstants.Tokens.Terminals.RelationalEqualityOperator
                            , XpressConstants.Tokens.Terminals.RelationalNonEqualityOperator
                            , XpressConstants.Tokens.Terminals.UnaryNegationOperator
                            , XpressConstants.Tokens.Terminals.NullValue
                            , XpressConstants.Tokens.Terminals.BooleanTrueValue
                            , XpressConstants.Tokens.Terminals.BooleanFalseValue
                          );

            RegisterBracePair(XpressConstants.Tokens.Terminals.ExpressionBeginSymbol
                            , XpressConstants.Tokens.Terminals.ExpressionEndSymbol);

            MarkPunctuation(XpressConstants.Tokens.Terminals.ExpressionBeginSymbol
                            , XpressConstants.Tokens.Terminals.ExpressionEndSymbol);
        }
    }
}
