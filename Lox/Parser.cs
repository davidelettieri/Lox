using System;
using System.Collections.Generic;
using static Lox.TokenType;

namespace Lox;

public class Parser(List<Token> tokens)
{
    private class ParseError : Exception
    {

    }

    private int _current;

    public List<IStmt> Parse()
    {
        var statements = new List<IStmt>();

        while (!IsAtEnd())
        {
            var decl = Declaration();
            if (decl is not null)
            {
                statements.Add(decl);
            }
        }

        return statements;
    }

    private IExpr Expression()
        => Assignment();

    private IStmt? Declaration()
    {
        try
        {
            if (Match(CLASS)) return ClassDeclaration();
            if (Check(FUN) && CheckNext(IDENTIFIER))
            {
                Consume(FUN, null);
                return Function("function");
            }
            if (Match(VAR)) return VarDeclaration();

            return Statement();
        }
        catch (ParseError)
        {
            Syncronize();
            return null;
        }
    }

    private IStmt ClassDeclaration()
    {
        var name = Consume(IDENTIFIER, "Expect class name");

        Variable? superclass = null;

        if (Match(LESS))
        {
            Consume(IDENTIFIER, "Expect superclass name.");
            superclass = new Variable(Previous());
        }

        Consume(LEFT_BRACE, "Expect '{' before class body.");
        var methods = new List<Function>();
        while (!Check(RIGHT_BRACE) && !IsAtEnd())
        {
            methods.Add(Function("method"));
        }

        Consume(RIGHT_BRACE, "Expect '}' after class body.");

        return new Class(name, superclass, methods);
    }

    private IStmt Statement()
    {
        if (Match(FOR)) return ForStatement();
        if (Match(IF)) return IfStatement();
        if (Match(PRINT)) return PrintStatement();
        if (Match(RETURN)) return ReturnStatement();
        if (Match(WHILE)) return WhileStatement();
        if (Match(LEFT_BRACE)) return new Block(Block());
        return ExpressionStatement();
    }

    private IStmt ForStatement()
    {
        Consume(LEFT_PAREN, "Expect '(' after 'for'.");
        IStmt? initializer;
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

        IExpr? condition = null;

        if (!Check(SEMICOLON))
        {
            condition = Expression();
        }

        Consume(SEMICOLON, "Expect ';' after loop condition.");

        IExpr? increment = null;

        if (!Check(RIGHT_PAREN))
        {
            increment = Expression();
        }

        Consume(RIGHT_PAREN, "Expect ')' after for clauses.");

        var body = Statement();

        if (increment != null)
        {
            body = new Block(new List<IStmt>() { body, new Expression(increment) });
        }

        condition ??= new Literal(true);
        body = new While(condition, body);

        if (initializer != null)
        {
            body = new Block(new List<IStmt>() { initializer, body });
        }

        return body;
    }

    private IStmt WhileStatement()
    {
        Consume(LEFT_PAREN, "Expect '(' after if");
        var expr = Expression();
        Consume(RIGHT_PAREN, "Expect ')' after condition");
        var stmt = Statement();

        return new While(expr, stmt);
    }

    private IStmt IfStatement()
    {
        Consume(LEFT_PAREN, "Expect '(' after if");
        var expr = Expression();
        Consume(RIGHT_PAREN, "Expect ')' after condition");
        var thenBranch = Statement();
        IStmt? elseBranch = null;

        if (Match(ELSE))
        {
            elseBranch = Statement();
        }

        return new If(expr, thenBranch, elseBranch);
    }

    private IStmt PrintStatement()
    {
        var value = Expression();
        Consume(SEMICOLON, "Expect ';' after value.");
        return new Print(value);
    }

    private IStmt ReturnStatement()
    {
        var keyword = Previous();
        IExpr? value = null;
        if (!Check(SEMICOLON))
        {
            value = Expression();
        }
        Consume(SEMICOLON, "Expect ';' after return value.");
        return new ReturnStmt(keyword, value);
    }

    private IStmt VarDeclaration()
    {
        Token name = Consume(IDENTIFIER, "Expect variable name.");

        IExpr? initializer = null;
        if (Match(EQUAL))
        {
            initializer = Expression();
        }

        Consume(SEMICOLON, "Expect ';' after variable declaration.");
        return new Var(name, initializer);
    }

    private IStmt ExpressionStatement()
    {
        var value = Expression();
        Consume(SEMICOLON, "Expect ';' after expression.");
        return new Expression(value);
    }

    private Function Function(string kind)
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

        Consume(RIGHT_PAREN, "Expect ')' after parameters.");
        Consume(LEFT_BRACE, $"Expect '{{' before {kind} body.");
        var body = Block();
        return new Function(name, parameters, body);
    }

    private List<IStmt> Block()
    {
        var statements = new List<IStmt>();

        while (!Check(RIGHT_BRACE) && !IsAtEnd())
        {
            var decl = Declaration();

            if (decl is not null)
            {
                statements.Add(decl);
            }
        }

        Consume(RIGHT_BRACE, "Expect '}' after block.");
        return statements;
    }

    private IExpr Assignment()
    {
        var expr = Or();

        if (Match(EQUAL))
        {
            var equals = Previous();
            var value = Assignment();

            switch (expr)
            {
                case Variable v:
                    return new Assign(v.Name, value);
                case Get g:
                    return new Set(g.Obj, g.Name, value);
                default:
                    Error(equals, "Invalid assignment target.");
                    break;
            }
        }

        return expr;
    }

    private IExpr Or()
    {
        var expr = And();

        while (Match(OR))
        {
            var op = Previous();
            var and = And();

            expr = new Logical(expr, op, and);
        }

        return expr;
    }

    private IExpr And()
    {
        var expr = Equality();

        while (Match(AND))
        {
            var op = Previous();
            var and = Equality();

            expr = new Logical(expr, op, and);
        }

        return expr;
    }

    private IExpr Equality()
    {
        var expr = Comparison();

        while (Match(BANG_EQUAL, EQUAL_EQUAL))
        {
            var op = Previous();
            var right = Comparison();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private IExpr Comparison()
    {
        var expr = Term();

        while (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
        {
            var op = Previous();
            var right = Term();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private IExpr Term()
    {
        var expr = Factor();

        while (Match(MINUS, PLUS))
        {
            var op = Previous();
            var right = Factor();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private IExpr Factor()
    {
        var expr = Unary();

        while (Match(SLASH, STAR))
        {
            var op = Previous();
            var right = Unary();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private IExpr Unary()
    {
        if (Match(BANG, MINUS))
        {
            var op = Previous();
            var right = Unary();
            return new Unary(op, right);
        }

        return Call();
    }

    private IExpr FinishCall(IExpr callee)
    {
        var arguments = new List<IExpr>();

        if (!Check(RIGHT_PAREN))
        {
            do
            {
                if (arguments.Count >= 255)
                {
                    Error(Peek(), "Can't have more than 255 arguments.");
                }

                arguments.Add(Expression());
            } while (Match(COMMA));
        }

        var paren = Consume(RIGHT_PAREN, "Expect ')' after arguments.");

        return new Call(callee, paren, arguments);
    }

    private IExpr Call()
    {
        var expr = Primary();

        while (true)
        {
            if (Match(LEFT_PAREN))
            {
                expr = FinishCall(expr);
            }
            else if (Match(DOT))
            {
                var name = Consume(IDENTIFIER, "Expect property name after '.'.");
                expr = new Get(expr, name);
            }
            else
            {
                break;
            }
        }

        return expr;
    }

    private IExpr Primary()
    {
        if (Match(FALSE)) return new Literal(false);
        if (Match(TRUE)) return new Literal(true);
        if (Match(NIL)) return new Literal(null);

        if (Match(NUMBER, STRING))
        {
            return new Literal(Previous().Literal);
        }

        if (Match(SUPER))
        {
            var keyword = Previous();
            Consume(DOT, "Expect '.' after 'super'.");
            var method = Consume(IDENTIFIER, "Expect superclass method name.");
            return new Super(keyword, method);
        }

        if (Match(THIS)) return new This(Previous());

        if (Match(IDENTIFIER))
        {
            return new Variable(Previous());
        }

        if (Match(LEFT_PAREN))
        {
            var expr = Expression();
            Consume(RIGHT_PAREN, "Expect ')' after expression");
            return new Grouping(expr);
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

    private Token Consume(TokenType type, string? message)
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
        if (tokens[_current + 1].Type == EOF) return false;
        return tokens[_current + 1].Type == tokenType;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) _current++;

        return Previous();
    }

    private bool IsAtEnd() => Peek().Type == EOF;
    private Token Peek() => tokens[_current];
    private Token Previous() => tokens[_current - 1];

    private ParseError Error(Token token, string? message)
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