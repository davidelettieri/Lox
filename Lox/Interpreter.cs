using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static Lox.TokenType;

namespace Lox;

public class Interpreter : IExprVisitor<object?>, IStmtVisitor
{
    private LoxEnvironment Globals { get; } = new();
    private LoxEnvironment? _environment;
    private readonly Dictionary<IExpr, int> _locals = new();

    public Interpreter()
    {
        Globals.Define("clock", new Clock());
        _environment = Globals;
    }

    public void Interpret(List<IStmt> statements)
    {
        try
        {
            foreach (var statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError error)
        {
            Lox.RuntimeError(error);
        }
    }

    private object? Evaluate(IExpr expr) => expr.Accept(this);
    private void Execute(IStmt statement) => statement.Accept(this);

    public void ExecuteBlock(List<IStmt> statements, LoxEnvironment environment)
    {
        var previous = _environment;
        try
        {
            _environment = environment;

            foreach (var statement in statements)
            {
                Execute(statement);
            }
        }
        finally
        {
            _environment = previous;
        }
    }

    internal void Resolve(IExpr expr, int depth)
    {
        _locals[expr] = depth;
    }

    public void Visit(Block stmt)
    {
        ExecuteBlock(stmt.Statements, new LoxEnvironment(_environment));
    }

    public void Visit(Class stmt)
    {
        object? superclass = null;
        if (stmt.Superclass != null)
        {
            superclass = Evaluate(stmt.Superclass);

            if (!(superclass is LoxClass))
            {
                throw new RuntimeError(stmt.Superclass.Name, "Superclass must be a class.");
            }
        }

        _environment?.Define(stmt.Name.Lexeme, null);

        if (stmt.Superclass != null)
        {
            _environment = new LoxEnvironment(_environment);
            _environment.Define("super", superclass);
        }

        var methods = new Dictionary<string, LoxFunction>();
        foreach (var method in stmt.Methods)
        {
            var isInitializer = method.Name.Lexeme.Equals("init", StringComparison.Ordinal);
            var function = new LoxFunction(method, _environment, isInitializer);
            methods[method.Name.Lexeme] = function;
        }

        var loxClass = new LoxClass(stmt.Name.Lexeme, superclass as LoxClass, methods);

        if (superclass != null)
        {
            _environment = _environment?.Enclosing;
        }

        _environment?.Assign(stmt.Name, loxClass);
    }

    public void Visit(Expression stmt)
    {
        Evaluate(stmt.Expr);
    }

    public void Visit(Function stmt)
    {
        var function = new LoxFunction(stmt, _environment, false);
        _environment?.Define(stmt.Name.Lexeme, function);
    }

    public void Visit(ReturnStmt stmt)
    {
        object? value = null;
        if (stmt.Value != null) value = Evaluate(stmt.Value);

        throw new ReturnException(value);
    }

    public void Visit(If stmt)
    {
        if (IsTruthy(Evaluate(stmt.Condition)))
        {
            Execute(stmt.ThenBranch);
        }
        else if (stmt.ElseBranch != null)
        {
            Execute(stmt.ElseBranch);
        }
    }

    public void Visit(Print stmt)
    {
        var value = Evaluate(stmt.Expr);
        Console.WriteLine(Stringify(value));
    }

    public void Visit(Var stmt)
    {
        object? value = null;

        if (stmt.Initializer != null)
        {
            value = Evaluate(stmt.Initializer);
        }

        _environment?.Define(stmt.Name.Lexeme, value);
    }

    public void Visit(While stmt)
    {
        while (IsTruthy(Evaluate(stmt.Condition)))
        {
            Execute(stmt.Body);
        }
    }

    public object? Visit(Assign expr)
    {
        var value = Evaluate(expr.Value);

        if (_locals.TryGetValue(expr, out var distance))
        {
            _environment?.AssignAt(distance, expr.Name, value);
        }
        else
        {
            Globals.Assign(expr.Name, value);
        }

        return value;
    }

    public object Visit(Binary expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        switch (expr.Operator.Type)
        {
            case GREATER:
                var (lg, rg) = CheckNumberOperands(expr.Operator, left, right);
                return lg > rg;
            case GREATER_EQUAL:
                var (lge, rge) = CheckNumberOperands(expr.Operator, left, right);
                return lge >= rge;
            case LESS:
                var (ll, rl) = CheckNumberOperands(expr.Operator, left, right);
                return ll < rl;
            case LESS_EQUAL:
                var (lle, rle) = CheckNumberOperands(expr.Operator, left, right);
                return lle <= rle;
            case MINUS:
                var (lm, rm) = CheckNumberOperands(expr.Operator, left, right);
                return lm - rm;
            case PLUS:
                if (left is double ld && right is double rd)
                {
                    return ld + rd;
                }

                if (left is string ls && right is string rs)
                {
                    return ls + rs;
                }

                throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings.");
            case SLASH:
                var (lsl, rsl) = CheckNumberOperands(expr.Operator, left, right);
                return lsl / rsl;
            case STAR:
                var (lst, rst) = CheckNumberOperands(expr.Operator, left, right);
                return lst * rst;
            case BANG_EQUAL: return !IsEqual(left, right);
            case EQUAL_EQUAL: return IsEqual(left, right);
        }

        // Unreachable
        throw new InvalidOperationException("Should not arrive here");
    }

    public object? Visit(Call expr)
    {
        var callee = Evaluate(expr.Callee);
        var arguments = new List<object>();

        foreach (var argument in expr.Arguments)
        {
            var a = Evaluate(argument);
            if (a is not null)
            {
                arguments.Add(a);
            }
        }

        if (callee is ILoxCallable function)
        {
            if (arguments.Count != function.Arity())
            {
                throw new RuntimeError(expr.Paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
            }

            return function.Call(this, arguments);
        }

        throw new RuntimeError(expr.Paren, "Can only call functions and classes.");
    }

    public object Visit(Get expr)
    {
        var obj = Evaluate(expr.Obj);

        if (obj is LoxInstance li)
        {
            return li.Get(expr.Name);
        }

        throw new RuntimeError(expr.Name, "Only instances have properties.");
    }

    public object Visit(AnonymousFunction expr)
    {
        if (_environment is null)
        {
            throw new Exception("Should not be null");
        }

        return new LoxAnonymousFunction(expr, _environment);
    }

    public object? Visit(Grouping expr)
        => Evaluate(expr.Expression);

    public object? Visit(Literal expr)
        => expr.Value;

    public object? Visit(Logical expr)
    {
        var left = Evaluate(expr.Left);

        if (expr.Operator.Type == OR)
        {
            if (IsTruthy(left)) return left;
        }
        else
        {
            if (!IsTruthy(left)) return left;
        }

        return Evaluate(expr.Right);
    }

    public object Visit(Set expr)
    {
        var obj = Evaluate(expr.Obj);

        if (obj is LoxInstance li)
        {
            var value = Evaluate(expr.Value);

            if (value is null)
            {
                throw new Exception("Shouldn't be null");
            }
            
            li.Set(expr.Name, value);
            return value;
        }

        throw new RuntimeError(expr.Name, "Only instances have fields.");
    }

    public object Visit(Super expr)
    {
        var distance = _locals[expr];
        var superclass = _environment?.GetAt(distance, "super") as LoxClass;
        var obj = _environment?.GetAt(distance - 1, "this") as LoxInstance;
        var method = superclass?.FindMethod(expr.Method.Lexeme);

        if (method == null)
        {
            throw new RuntimeError(expr.Method, $"Undefined property '{expr.Method.Lexeme}'.");
        }

        if (obj is null)
        {
            throw new RuntimeError(expr.Method, "Undefined object");
        }

        return method.Bind(obj);
    }

    public object? Visit(This expr)
        => LookUpVariable(expr.Keyword, expr);

    public object Visit(Unary expr)
    {
        var right = Evaluate(expr.Right);

        switch (expr.Operator.Type)
        {
            case BANG:
                return !IsTruthy(right);
            case MINUS:
                var d = CheckNumberOperand(expr.Operator, right);
                return -d;
        }

        // Unreachable
        throw new InvalidOperationException("Should not arrive here");
    }

    public object? Visit(Variable expr)
        => LookUpVariable(expr.Name, expr);

    private object? LookUpVariable(Token name, IExpr expr)
    {
        if (_locals.TryGetValue(expr, out var distance))
        {
            return _environment?.GetAt(distance, name.Lexeme);
        }

        return Globals.Get(name);
    }

    private double CheckNumberOperand(Token op, object? operand)
    {
        if (operand is double d) return d;
        throw new RuntimeError(op, "Operand must be a number.");
    }

    private (double left, double right) CheckNumberOperands(Token op, object? left, object? right)
    {
        if (left is double l && right is double r) return (l, r);
        throw new RuntimeError(op, "Operands must be numbers.");
    }

    private bool IsTruthy(object? obj)
    {
        if (obj is null) return false;
        if (obj is bool b) return b;
        return true;
    }

    private bool IsEqual(object? a, object? b)
    {
        if (a is null && b is null) return true;
        if (a is null) return false;
        if (a is Double.NaN)
            return false;
        if (b is Double.NaN)
            return false;

        return a.Equals(b);
    }

    private string Stringify(object? value)
    {
        return value switch
        {
            null => "nil",
            double d => d.ToString(CultureInfo.InvariantCulture),
            bool b => b.ToString().ToLower(CultureInfo.InvariantCulture),
            _ => value.ToString() ?? ""
        };
    }
}