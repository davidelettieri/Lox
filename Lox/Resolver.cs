using System;
using System.Collections.Generic;
using System.Linq;

namespace Lox;

public class Resolver(Interpreter interpreter) : IExprVisitor, IStmtVisitor 
{
    private readonly Stack<Dictionary<string, bool>> _scopes = new();
    private FunctionType _currentFunction = FunctionType.NONE;
    private ClassType _currentClass = ClassType.NONE;

    public void Resolve(List<IStmt> statements)
    {
        foreach (var statement in statements)
        {
            Resolve(statement);
        }
    }

    private void BeginScope()
        => _scopes.Push(new Dictionary<string, bool>());

    private void EndScope()
        => _scopes.Pop();

    private void Declare(Token name)
    {
        if (_scopes.Count == 0) return;

        var scope = _scopes.Peek();

        if (scope.ContainsKey(name.Lexeme))
        {
            Lox.Error(name, "Already a variable with this name in this scope.");
        }

        scope[name.Lexeme] = false;
    }

    private void Define(Token name)
    {
        if (_scopes.Count == 0) return;

        _scopes.Peek()[name.Lexeme] = true;
    }

    private void ResolveLocal(IExpr expr, Token name)
    {
        var scopeArray = _scopes.Reverse().ToArray();
        for (var i = _scopes.Count - 1; i >= 0; i--)
        {
            if (!scopeArray[i].ContainsKey(name.Lexeme)) continue;
            interpreter.Resolve(expr, _scopes.Count - 1 - i);
            return;
        }
    }

    public void Visit(Block stmt)
    {
        BeginScope();
        Resolve(stmt.Statements);
        EndScope();
    }

    public void Visit(Class stmt)
    {
        var enclosingClass = _currentClass;
        _currentClass = ClassType.CLASS;
        Declare(stmt.Name);
        Define(stmt.Name);

        if (stmt.Superclass != null &&
            stmt.Name.Lexeme.Equals(stmt.Superclass.Name.Lexeme, StringComparison.Ordinal))
        {
            Lox.Error(stmt.Superclass.Name, "A class can't inherit from itself.");
        }

        if (stmt.Superclass != null)
        {
            _currentClass = ClassType.SUBCLASS;
            Resolve(stmt.Superclass);
            BeginScope();
            _scopes.Peek()["super"] = true;
        }

        BeginScope();
        _scopes.Peek()["this"] = true;
        foreach (var method in stmt.Methods)
        {
            var declaration = FunctionType.METHOD;
            if (method.Name.Lexeme.Equals("init", StringComparison.Ordinal))
            {
                declaration = FunctionType.INITIALIZER;
            }

            ResolveFunction(method, declaration);
        }
        EndScope();

        if (stmt.Superclass != null) EndScope();

        _currentClass = enclosingClass;
    }

    public void Visit(Expression stmt)
    {
        Resolve(stmt.Expr);
    }

    public void Visit(Function stmt)
    {
        Declare(stmt.Name);
        Define(stmt.Name);
        ResolveFunction(stmt, FunctionType.FUNCTION);
    }

    public void Visit(If stmt)
    {
        Resolve(stmt.Condition);
        Resolve(stmt.ThenBranch);
        if (stmt.ElseBranch != null) Resolve(stmt.ElseBranch);
    }

    public void Visit(Print stmt)
    {
        Resolve(stmt.Expr);
    }

    public void Visit(ReturnStmt stmt)
    {
        if (_currentFunction == FunctionType.NONE)
        {
            Lox.Error(stmt.Keyword, "Can't return from top-level code.");
        }

        if (stmt.Value == null) return;
        if (_currentFunction == FunctionType.INITIALIZER)
        {
            Lox.Error(stmt.Keyword, "Can't return a value from an initializer.");
        }
        Resolve(stmt.Value);
    }

    public void Visit(While stmt)
    {
        Resolve(stmt.Condition);
        Resolve(stmt.Body);
    }

    public void Visit(Var stmt)
    {
        Declare(stmt.Name);
        if (stmt.Initializer != null)
        {
            Resolve(stmt.Initializer);
        }
        Define(stmt.Name);
    }

    public void Visit(Assign expr)
    {
        Resolve(expr.Value);
        ResolveLocal(expr, expr.Name);
    }

    public void Visit(Binary expr)
    {
        Resolve(expr.Left);
        Resolve(expr.Right);
    }

    public void Visit(Call expr)
    {
        Resolve(expr.Callee);

        foreach (var argument in expr.Arguments)
        {
            Resolve(argument);
        }
    }

    public void Visit(Get expr)
    {
        Resolve(expr.Obj);
    }

    public void Visit(Grouping expr)
    {
        Resolve(expr.Expression);
    }

    public void Visit(Literal expr) {}

    public void Visit(Logical expr)
    {
        Resolve(expr.Left);
        Resolve(expr.Right);
    }

    public void Visit(Set expr)
    {
        Resolve(expr.Value);
        Resolve(expr.Obj);
    }

    public void Visit(Super expr)
    {
        if (_currentClass == ClassType.NONE)
        {
            Lox.Error(expr.Keyword, "Can't use 'super' outside of a class.");
        }
        else if (_currentClass != ClassType.SUBCLASS)
        {
            Lox.Error(expr.Keyword, "Can't use 'super' in a class with no superclass.");
        }
        ResolveLocal(expr, expr.Keyword);
    }

    public void Visit(This expr)
    {
        if (_currentClass == ClassType.NONE)
        {
            Lox.Error(expr.Keyword, "Can't use 'this' outside of a class.");
            return;
        }
        ResolveLocal(expr, expr.Keyword);
    }

    public void Visit(Unary expr)
    {
        Resolve(expr.Right);
    }

    public void Visit(AnonymousFunction expr)
    {
        var enclosingFunction = _currentFunction;
        _currentFunction = FunctionType.FUNCTION;
        BeginScope();
        foreach (var param in expr.Parameters)
        {
            Declare(param);
            Define(param);
        }

        Resolve(expr.Body);
        EndScope();
        _currentFunction = enclosingFunction;
    }

    public void Visit(Variable expr)
    {
        if (_scopes.Count > 0 && _scopes.Peek().TryGetValue(expr.Name.Lexeme, out var defined) && !defined)
        {
            Lox.Error(expr.Name, "Can't read local variable in its own initializer.");
        }

        ResolveLocal(expr, expr.Name);
    }

    private void Resolve(IStmt stmt) => stmt.Accept(this);
    private void Resolve(IExpr expr) => expr.Accept(this);

    private void ResolveFunction(Function function, FunctionType type)
    {
        var enclosingFunction = _currentFunction;
        _currentFunction = type;
        BeginScope();
        foreach (var param in function.Parameters)
        {
            Declare(param);
            Define(param);
        }

        Resolve(function.Body);
        EndScope();
        _currentFunction = enclosingFunction;
    }
}