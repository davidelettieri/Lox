using System;
using System.Collections.Generic;
using static Lox.TokenType;

namespace Lox
{
    public class Parser
    {
        private class ParseError : Exception
        {

        }

        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseError error)
            {
                return null;
            }
        }

        private Expr Expression()
            => Equality();

        private Expr Equality()
        {
            var expr = Comparison();

            while (Match(BANG_EQUAL, EQUAL_EQUAL))
            {
                var op = Previous();
                var right = Comparison();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            var expr = Term();

            while (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
            {
                var op = Previous();
                var right = Term();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Term()
        {
            var expr = Factor();

            while (Match(MINUS, PLUS))
            {
                var op = Previous();
                var right = Factor();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Factor()
        {
            var expr = Unary();

            while (Match(SLASH, STAR))
            {
                var op = Previous();
                var right = Unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(BANG, MINUS))
            {
                var op = Previous();
                var right = Unary();
                return new Expr.Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(FALSE)) return new Expr.Literal(false);
            if (Match(TRUE)) return new Expr.Literal(true);
            if (Match(NIL)) return new Expr.Literal(null);

            if (Match(NUMBER, STRING))
            {
                return new Expr.Literal(Previous().Literal);
            }

            if (Match(LEFT_PAREN))
            {
                var expr = Expression();
                Consume(RIGHT_PAREN, "Expect ')' after expression");
                return new Expr.Grouping(expr);
            }

            throw Error(Peek(), "Expect expression.");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();

            throw Error(Peek(), message);
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;

            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;

            return Previous();
        }

        private bool IsAtEnd() => Peek().Type == EOF;
        private Token Peek() => _tokens[_current];
        private Token Previous() => _tokens[_current - 1];

        private ParseError Error(Token token, string message)
        {
            Lox.Error(token, message);
            return new ParseError();
        }

        private void Syncronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == SEMICOLON) return;

                switch (Peek().Type)
                {
                    case CLASS:
                    case FUN:
                    case VAR:
                    case FOR:
                    case IF:
                    case WHILE:
                    case PRINT:
                    case RETURN:
                        return;
                }

                Advance();
            }
        }
    }
}
