﻿using Swift.AST_Nodes;
using Swift.AST_Nodes.Types;
using Swift.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class SyntaxAnalyzer
    {
        /// <summary>
        /// Dictionary(operator, <associativity, precedence>)
        /// </summary>
        private Dictionary<string, Tuple<Global.Associativity, int>> operatorPrecedence = new Dictionary<string, Tuple<Global.Associativity, int>>()
        {
            { "<<", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 160) },
            { ">>", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 160) },
            { "*", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 150) },
            { "/", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 150) },
            { "%", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 150) },
            { "&*", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 150) },
            { "&/", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 150) },
            { "&%", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 150) },
            { "&", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 150) },
            { "+", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 140) },
            { "-", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 140) },
            { "&+", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 140) },
            { "&-", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 140) },
            { "|", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 140) },
            { "^", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 140) },
            { "..", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 135) },
            { "...", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 135) },
            { "is", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 132) },
            { "as", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 132) },
            { "as?", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 132) },
            { "as!", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 132) },
            { "??", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 131) },
            { "<", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { "<=", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { ">", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { ">=", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { "==", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { "!=", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { "===", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { "!==", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { "~=", new Tuple<Global.Associativity, int>(Global.Associativity.NONE, 130) },
            { "&&", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 120) },
            { "||", new Tuple<Global.Associativity, int>(Global.Associativity.LEFT, 110) },
            { "?", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 100) },
            { "=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "*=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "/=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "%=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "+=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "-=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "<<=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { ">>=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "&=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "|=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "^=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "&&=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
            { "||=", new Tuple<Global.Associativity, int>(Global.Associativity.RIGHT, 90) },
        };
        private List<Token> tokens;
        private List<LineContext> context;
        private List<Token> allTokens;
        private List<LineContext> allContext;
        private ASTNode node;
        private Base astBase;
        private Global.InstructionSets architecture;

        public Base CheckSyntax(List<Token> tokens, List<LineContext> context, Global.InstructionSets architecture)
        {
            allTokens = tokens;
            allContext = context;
            this.architecture = architecture;
            astBase = new Base(context[0]);
            EatStatements();
            return astBase;
        }

        private void EatStatements()
        {
            int i;
            while (true)
            {
                if (allTokens.Count == 0)
                    break;
                i = 0;
                while (allTokens[i].type != Global.DataType.ENDSTATEMENT)
                    i++;
                EatStatement(allTokens.GetRange(0, i), allContext.GetRange(0, i));
                allTokens.RemoveRange(0, i + 1);
                allContext.RemoveRange(0, i + 1);
            }
        }

        /// <summary>
        /// Parses a statement
        /// </summary>
        /// <param name="token"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private void EatStatement(List<Token> tokensIn, List<LineContext> contextIn)
        {
            if (tokensIn.Count == 0)
                return;
            tokens = tokensIn;
            context = contextIn;
            switch (tokens[0].type)
            {
                case Global.DataType.IDENTIFIER:
                    if (tokens[1].type == Global.DataType.OPEN_ROUND_BRACKET)
                        EatFunctionCall();
                    else
                        EatAssignment();
                    break;
                case Global.DataType.LET:
                    EatDeclaration();
                    break;
                case Global.DataType.VAR:
                    EatDeclaration();
                    break;
                default:
                    Swift.error("Syntax error: \"" + tokens[0].value + "\" at line " + context[0].GetLine().ToString() + ", colomn " + context[0].GetPos().ToString() + " could not be identified", 1);
                    break;
            }
        }

        private void EatFunctionCall()
        {
            FunctionCallExp node = new FunctionCallExp(context[0]);
            node.Name = new Identifier(tokens[0].value);
            CutData(1);
            while (tokens[0].type != Global.DataType.CLOSE_ROUND_BRACKET)
            {
                List<Token> arg = new List<Token>();
                List<LineContext> argContext = new List<LineContext>();
                /*while (token.type != Global.DataType.COMMA && token.type != Global.DataType.CLOSE_ROUND_BRACKET)
                {
                    arg.Add(token);
                    argContext.Add(context[i]);
                    token = tokens[++i];
                }*/
                CutData(1);
                node.Args.Add(EatFunctionArgument());
            }
            astBase.Children.Add(node);
        }

        private ParameterCall EatFunctionArgument()
        {
            ParameterCall node;
            if (tokens.Count > 1 && tokens[1].type == Global.DataType.COLON)
            {
                CutData(2);
                node = new ParameterCall(EatExpression(), tokens[0].value);
            }
            else
            {
                node = new ParameterCall(EatExpression());
            }
            return node;
        }

        private Exp EatExpression()
        {
            Exp primary = EatPrimary();
            return EatExpPart(0, primary);
        }

        private Exp EatExpPart(int precedence, Exp lhs)
        {
            if (tokens.Count <= 1 || tokens[1].type == Global.DataType.CLOSE_ROUND_BRACKET || tokens[1].type == Global.DataType.COMMA/* || (tokens[0].type != Global.DataType.OPERATOR && tokens[0].type != Global.DataType.INT && tokens[0].type != Global.DataType.IDENTIFIER && tokens[0].type != Global.DataType.STRING && tokens[0].type != Global.DataType.DOUBLE)*/)
                return lhs;
            if (tokens[0].type != Global.DataType.OPERATOR)
                Swift.error("Operator expected between the terms in the expression on line " + context[0].GetLine() + ", column" + context[0].GetPos(), 1);
            string firstOperator = tokens[0].value;
            int tokenPrecedence1 = operatorPrecedence[firstOperator].Item2;
            if (tokenPrecedence1 < precedence)
                return lhs;

            Token op = tokens[0];
            CutData(1);
            Exp rhs = EatPrimary();
            

            if (tokens.Count == 0)
                return BinaryExpression(op, lhs, rhs);
            int tokenPrecedence2 = operatorPrecedence[tokens[0].value].Item2;
            if (tokenPrecedence1 < tokenPrecedence2 || (tokenPrecedence1 == tokenPrecedence2 && operatorPrecedence[firstOperator].Item1 == Global.Associativity.RIGHT))
            {
                Exp newRhs = EatExpPart(tokenPrecedence1, rhs);
                return EatExpPart(tokenPrecedence1 + 1, BinaryExpression(op, lhs, newRhs));
            }
            return EatExpPart(precedence + 1, BinaryExpression(op, lhs, rhs));
        }

        private Exp BinaryExpression(Token op, Exp lhs, Exp rhs)
        {
            switch (op.value)
            {
                case "&&": return new AndExp(null, lhs, rhs);
                case "&": return new BitwiseAndExp(null, lhs, rhs);
                case "<<": return new BitwiseLeftShiftExp(null, lhs, rhs);
                case "~": return new BitwiseNotExp(null, lhs, rhs);
                case "|": return new BitwiseOrExp(null, lhs, rhs);
                case ">>": return new BitwiseRightShiftExp(null, lhs, rhs);
                case "^": return new BitwiseXorExp(null, lhs, rhs);
                case "/": return new DivisionExp(null, lhs, rhs);
                case "!": return new ExclamationExp(null, lhs, rhs);
                case "-": return new MinusExp(null, lhs, rhs);
                case "%": return new ModuloExp(null, lhs, rhs);
                case "*": return new MultiplicationExp(null, lhs, rhs);
                case "||": return new OrExp(null, lhs, rhs);
                case "&+": return new OverflowAddExp(null, lhs, rhs);
                case "&*": return new OverflowMultExp(null, lhs, rhs);
                case "&-": return new OverflowSubExp(null, lhs, rhs);
                case "+": return new PlusExp(null, lhs, rhs);
            }
            throw new UnknownOperatorException("The operator \"" + op.value + "\" is unknown");
        }

        private Exp EatPrimary()
        {
            switch (tokens[0].type)
            {
                case Global.DataType.IDENTIFIER:
                    return EatIdentifier();
                case Global.DataType.INT:
                    switch (architecture)
                    {
                        case Global.InstructionSets.X86: Exp result = new Int32Literal(context[0], tokens[0].value); CutData(1); return result;
                        case Global.InstructionSets.X86_64: result = new Int64Literal(context[0], tokens[0].value); CutData(1); return result;
                    }
                    break;
                case Global.DataType.DOUBLE:
                    switch (architecture)
                    {
                        case Global.InstructionSets.X86: Exp result = new FloatLiteral(context[0], tokens[0].value); CutData(1); return result;
                        case Global.InstructionSets.X86_64: result = new DoubleLiteral(context[0], tokens[0].value); CutData(1); return result;
                    }
                    break;
                case Global.DataType.STRING:
                    Exp literal = new StringLiteral(context[0], tokens[0].value);
                    CutData(1);
                    return literal;
                case Global.DataType.TRUE:
                    CutData(1);
                    return new BoolLiteral(context[0], true);
                case Global.DataType.FALSE:
                    CutData(1);
                    return new BoolLiteral(context[0], false);
            }
            return null;
        }

        private Exp EatIdentifier()
        {
            Exp literal = new IdentifierExp(context[0], new Identifier(tokens[0].value));
            CutData(1);
            return literal;
        }

        private void EatAssignment()
        {
            Assignment node = new Assignment(context[0]);
            Identifier lhs = new Identifier(tokens[0].value);
            CutData(2);
            Exp rhs = EatExpression();
            node.LHS = lhs;
            node.RHS = rhs;
            astBase.Children.Add(node);
        }

        /// <summary>
        /// Parses a declaration (eg, var a, let b)
        /// </summary>
        /// <param name="tokens">A list of tokens containing only the whole declaration</param>
        /// <param name="context">A list of LineContext corresponding to the tokens supplied</param>
        /// <returns></returns>
        private void EatDeclaration()
        {
            if (tokens[1].type != Global.DataType.IDENTIFIER)
            {
                Swift.error("Identifier expected at line " + context[0].GetLine().ToString() + ", colomn " + context[1].GetPos().ToString() + ".", 1);
            }
            if (tokens[0].type == Global.DataType.VAR)
            {
                if (tokens.Count >= 4)
                {
                    VarDeclaration node = new VarDeclaration(context[0]);
                    if (tokens[1].type != Global.DataType.IDENTIFIER)
                        Swift.error("Syntax error: identifier expected after keyword \"var\" on line " + context[1].GetLine() + ", column " + context[1].GetPos(), 1);
                    node.Name = new Identifier(tokens[1].value);
                    if (tokens[2].type == Global.DataType.OPERATOR && tokens[2].value == "=")
                    {
                        astBase.Children.Add(node);
                        CutData(1);
                        EatAssignment();
                        while (tokens.Count > 0)
                        {
                            if (tokens[0].type == Global.DataType.COMMA)
                            {
                                node = new VarDeclaration(context[0]);
                                node.Name = new Identifier(tokens[1].value);
                                astBase.Children.Add(node);
                                if (tokens[2].type == Global.DataType.OPERATOR && tokens[2].value == "=")
                                {
                                    CutData(1);
                                    EatAssignment();
                                }
                            }
                        }
                    }
                    else if (tokens[2].type == Global.DataType.COLON)
                    {
                        node = new VarDeclaration(context[1]);
                        node.Name = new Identifier(tokens[1].value);
                        if (tokens.Count <= 2)
                            Swift.error("Syntax error: type expected after the colon on line " + context[2].GetLine() + ", column " + context[2].GetPos(), 1);
                        node.Type = getASTTypeFromToken(tokens[3], context[3]);
                        astBase.Children.Add(node);
                    }
                    else
                    {
                        Swift.error("Assignment expected after a declaration without a type specification on line " + context[2].GetLine() + ", column " + context[2].GetPos(), -1);
                    }
                }
                else
                {
                    Swift.error("Assignment expected after a declaration without a type specification on line " + context[2].GetLine() + ", column " + context[2].GetPos(), -1);

                }
            }
            else if (tokens[0].type == Global.DataType.LET)
            {
                if (tokens.Count <= 2)
                    Swift.error("Constants must be initialized; error occurerred on line " + context[0].GetLine() + ", column " + context[0].GetPos() + ".", 1);
                if (tokens.Count >= 4)
                {
                    if (tokens[2].type == Global.DataType.OPERATOR && tokens[2].value == "=")
                    {
                        string tmpToken = tokens[1].value;
                        CutData(3);
                        ConstDeclaration node = new ConstDeclaration(context[0], EatExpression());
                        node.Name = new Identifier(tmpToken);
                        astBase.Children.Add(node);
                        while (tokens.Count > 0)
                        {
                            if (tokens[0].type == Global.DataType.COMMA)
                            {
                                tmpToken = tokens[1].value;
                                if (tokens[2].type == Global.DataType.OPERATOR && tokens[2].value == "=")
                                {
                                    CutData(3);
                                    node = new ConstDeclaration(context[0], EatExpression());
                                    node.Name = new Identifier(tmpToken);
                                    astBase.Children.Add(node);
                                }
                                else
                                    Swift.error("Syntax error on line " + context[0].GetLine() + ": Constants must be initialized", 1);
                            }
                            else
                                Swift.error("Unexpected code after constant declaration on line " + context[0].GetLine(), 1);
                        }
                    }
                    else
                    {
                        Swift.error("Constants must be initialized; error occurerred on line " + context[0].GetLine() + ", column " + context[0].GetPos() + ".", 1);
                    }
                }
                astBase.Children.Add(node);
            }
            else {
                Swift.error("Internal error parsing the variable declaration: " + tokens, 1);
                return;
            }
            //if (tokens.Count > 2)
            //    node.SetChildren();
            //return EatAssignment(tokens.GetRange(1, tokens.Count - 1), context.GetRange(1, context.Count - 1));
        }

        private void CutData(int amount)
        {
            int newLength = tokens.Count - amount;
            tokens = tokens.GetRange(amount, newLength);
            context = context.GetRange(amount, newLength);
        }

        private ASTType getASTTypeFromToken(Token token, LineContext context)
        {
            switch (token.type)
            {
                case Global.DataType.BOOLTYPE: return new BoolType();
                case Global.DataType.CHARACTERTYPE: return new CharType();
                case Global.DataType.DOUBLETYPE: return new DoubleType();
                case Global.DataType.FLOATTYPE: return new FloatType();
                case Global.DataType.INT16TYPE: return new Int16Type();
                case Global.DataType.INT32TYPE: return new Int32Type();
                case Global.DataType.INT64TYPE: return new Int64Type();
                case Global.DataType.INT8TYPE: return new Int8Type();
                case Global.DataType.STRINGTYPE: return new StringType();
                case Global.DataType.UINT16TYPE: return new UInt16Type();
                case Global.DataType.UINT32TYPE: return new UInt32Type();
                case Global.DataType.UINT64TYPE: return new UInt64Type();
                case Global.DataType.UINT8TYPE: return new UInt8Type();
                    
                case Global.DataType.OBOOLTYPE: return new BoolType(true);
                case Global.DataType.OCHARACTERTYPE: return new CharType(true);
                case Global.DataType.ODOUBLETYPE: return new DoubleType(true);
                case Global.DataType.OFLOATTYPE: return new FloatType(true);
                case Global.DataType.OINT16TYPE: return new Int16Type(true);
                case Global.DataType.OINT32TYPE: return new Int32Type(true);
                case Global.DataType.OINT64TYPE: return new Int64Type(true);
                case Global.DataType.OINT8TYPE: return new Int8Type(true);
                case Global.DataType.OSTRINGTYPE: return new StringType(true);
                case Global.DataType.OUINT16TYPE: return new UInt16Type(true);
                case Global.DataType.OUINT32TYPE: return new UInt32Type(true);
                case Global.DataType.OUINT64TYPE: return new UInt64Type(true);
                case Global.DataType.OUINT8TYPE: return new UInt8Type(true);

                default: Swift.error("Syntax error: unknown type found after the colon on line " + context.GetLine() + ", column " + context.GetPos(), 1);
                    return null;
            }
        }



        [Serializable()]
        private class UnknownTermException : System.Exception
        {
            public UnknownTermException() : base() { }
            public UnknownTermException(string message) : base(message) { }
            public UnknownTermException(string message, System.Exception inner) : base(message, inner) { }
            protected UnknownTermException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        }



        [Serializable()]
        private class UnknownOperatorException : System.Exception
        {
            public UnknownOperatorException() : base() { }
            public UnknownOperatorException(string message) : base(message) { }
            public UnknownOperatorException(string message, System.Exception inner) : base(message, inner) { }
            protected UnknownOperatorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        }
    }
}