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

        public List<Stmt> Parse()
        {
            var statements = new List<Stmt>();

            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return statements;
        }

        private Expr Expression()
            => Assignment();

        private Stmt Declaration()
        {
            try
            {
                if (Check(FUN) && CheckNext(IDENTIFIER))
                {
                    Consume(FUN, null);
                    return Function("function");
                }
                if (Match(VAR)) return VarDeclaration();

                return Statement();
            }
            catch (ParseError error)
            {
                Syncronize();
                return null;
            }
        }

        private Stmt Statement()
        {
            if (Match(FOR)) return ForStatement();
            if (Match(IF)) return IfStatement();
            if (Match(PRINT)) return PrintStatement();
            if (Match(RETURN)) return ReturnStatement();
            if (Match(WHILE)) return WhileStatement();
            if (Match(LEFT_BRACE)) return new Stmt.Block(Block());
            return ExpressionStatement();
        }

        private Stmt ForStatement()
        {
            Consume(LEFT_PAREN, "Expect '(' after 'for'.");
            Stmt initializer;
            if (Match(SEMICOLON))
            {
                initializer = null;
            }
            else if (Match(VAR))
            {
                initializer = VarDeclaration();
            }
            else
            {
                initializer = ExpressionStatement();
            }

            Expr condition = null;

            if (!Check(SEMICOLON))
            {
                condition = Expression();
            }

            Consume(SEMICOLON, "Expect ';' after loop condition.");

            Expr increment = null;

            if (!Check(RIGHT_PAREN))
            {
                increment = Expression();
            }

            Consume(RIGHT_PAREN, "Expect ')' after for clauses.");

            var body = Statement();

            if (increment != null)
            {
                body = new Stmt.Block(new List<Stmt>() { body, new Stmt.Expression(increment) });
            }

            if (condition == null) condition = new Expr.Literal(true);
            body = new Stmt.While(condition, body);

            if (initializer != null)
            {
                body = new Stmt.Block(new List<Stmt>() { initializer, body });
            }

            return body;
        }

        private Stmt WhileStatement()
        {
            Consume(LEFT_PAREN, "Expect '(' after if");
            var expr = Expression();
            Consume(RIGHT_PAREN, "Expect ')' after expression");
            var stmt = Statement();

            return new Stmt.While(expr, stmt);
        }

        private Stmt IfStatement()
        {
            Consume(LEFT_PAREN, "Expect '(' after if");
            var expr = Expression();
            Consume(RIGHT_PAREN, "Expect ')' after expression");
            var thenBranch = Statement();
            Stmt elseBranch = null;

            if (Match(ELSE))
            {
                elseBranch = Statement();
            }

            return new Stmt.If(expr, thenBranch, elseBranch);
        }

        private Stmt PrintStatement()
        {
            var value = Expression();
            Consume(SEMICOLON, "Expect ';' after value.");
            return new Stmt.Print(value);
        }

        private Stmt ReturnStatement()
        {
            var keyword = Previous();
            Expr value = null;
            if (!Check(SEMICOLON))
            {
                value = Expression();
            }
            Consume(SEMICOLON, "Expect ';' after return value.");
            return new Stmt.Return(keyword, value);
        }

        private Stmt VarDeclaration()
        {
            Token name = Consume(IDENTIFIER, "Expect variable name.");

            Expr initializer = null;
            if (Match(EQUAL))
            {
                initializer = Expression();
            }

            Consume(SEMICOLON, "Expect ';' after variable declaration.");
            return new Stmt.Var(name, initializer);
        }

        private Stmt ExpressionStatement()
        {
            var value = Expression();
            Consume(SEMICOLON, "Expect ';' after value.");
            return new Stmt.Expression(value);
        }

        private Stmt.Function Function(string kind)
        {
            var name = Consume(IDENTIFIER, $"Expect {kind} name.");
            Consume(LEFT_PAREN, $"Expect '(' after {kind} name.");
            var parameters = new List<Token>();

            if (!Check(RIGHT_PAREN))
            {
                do
                {
                    if (parameters.Count >= 255)
                    {
                        Error(Peek(), "Can't have more than 255 parameters.");
                    }

                    parameters.Add(Consume(IDENTIFIER, "Expect parameter name."));
                } while (Match(COMMA));
            }

            Consume(RIGHT_PAREN, "Expect ')' after parameters");
            Consume(LEFT_BRACE, "Expect '{' after parameters");
            var body = Block();
            return new Stmt.Function(name, parameters, body);
        }

        private List<Stmt> Block()
        {
            var statements = new List<Stmt>();

            while (!Check(RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }

            Consume(RIGHT_BRACE, "Expect '}' after block.");
            return statements;
        }

        private Expr Assignment()
        {
            var expr = Or();

            if (Match(EQUAL))
            {
                var equals = Previous();
                var value = Assignment();

                if (expr is Expr.Variable v)
                {
                    return new Expr.Assign(v.Name, value);
                }

                Error(equals, "Invalid assignment target.");
            }

            return expr;
        }

        private Expr Or()
        {
            var expr = And();

            while (Match(OR))
            {
                var op = Previous();
                var and = And();

                expr = new Expr.Logical(expr, op, and);
            }

            return expr;
        }

        private Expr And()
        {
            var expr = Equality();

            while (Match(AND))
            {
                var op = Previous();
                var and = Equality();

                expr = new Expr.Logical(expr, op, and);
            }

            return expr;
        }

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

            return Call();
        }

        private Expr FinishCall(Expr callee)
        {
            var arguments = new List<Expr>();

            if (!Check(RIGHT_PAREN))
            {
                do
                {
                    if (arguments.Count >= 255)
                    {
                        Error(Peek(), "Can't have more than 255 arguments");
                    }

                    arguments.Add(Expression());
                } while (Match(COMMA));
            }

            var paren = Consume(RIGHT_PAREN, "Expect ')' after arguments");

            return new Expr.Call(callee, paren, arguments);
        }

        private Expr Call()
        {
            var expr = Primary();

            while (true)
            {
                if (Match(LEFT_PAREN))
                {
                    expr = FinishCall(expr);
                }
                else
                {
                    break;
                }
            }

            return expr;
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

            if (Match(IDENTIFIER))
            {
                return new Expr.Variable(Previous());
            }

            if (Match(LEFT_PAREN))
            {
                var expr = Expression();
                Consume(RIGHT_PAREN, "Expect ')' after expression");
                return new Expr.Grouping(expr);
            }

            if (Match(FUN))
            {
                return FunctionExpr();
            }

            throw Error(Peek(), "Expect expression.");
        }

        private Expr FunctionExpr()
        {
            Consume(LEFT_PAREN, $"Expect '(' after fun.");
            var parameters = new List<Token>();

            if (!Check(RIGHT_PAREN))
            {
                do
                {
                    if (parameters.Count >= 255)
                    {
                        Error(Peek(), "Can't have more than 255 parameters.");
                    }

                    parameters.Add(Consume(IDENTIFIER, "Expect parameter name."));
                } while (Match(COMMA));
            }

            Consume(RIGHT_PAREN, "Expect ')' after parameters");
            Consume(LEFT_BRACE, "Expect '{' after parameters");
            var body = Block();

            return new Expr.AnonymousFunction(parameters, body);
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

        private bool CheckNext(TokenType tokenType)
        {
            if (IsAtEnd()) return false;
            if (_tokens[_current + 1].Type == EOF) return false;
            return _tokens[_current + 1].Type == tokenType;
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
