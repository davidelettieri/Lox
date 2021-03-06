﻿using System;
using System.Collections.Generic;
using System.Globalization;
using static Lox.TokenType;

namespace Lox
{
    public class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<Void>
    {
        public LoxEnvironment Globals { get; } = new LoxEnvironment();
        private LoxEnvironment _environment;
        private readonly Dictionary<Expr, int> _locals = new Dictionary<Expr, int>();

        public Interpreter()
        {
            Globals.Define("clock", new Clock());
            _environment = Globals;
        }

        public void Intepret(List<Stmt> statements)
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

        private object Evaluate(Expr expr) => expr.Accept(this);
        private object Execute(Stmt statement) => statement.Accept(this);
        public void ExecuteBlock(List<Stmt> statements, LoxEnvironment environment)
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

        internal void Resolve(Expr expr, int depth)
        {
            _locals[expr] = depth;
        }

        public Void VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.Statements, new LoxEnvironment(_environment));
            return null;
        }

        public Void VisitClassStmt(Stmt.Class stmt)
        {
            object superclass = null;
            if (stmt.Superclass != null)
            {
                superclass = Evaluate(stmt.Superclass);

                if (!(superclass is LoxClass))
                {
                    throw new RuntimeError(stmt.Superclass.Name, "Superclass must be a class");
                }
            }
            _environment.Define(stmt.Name.Lexeme, null);

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

            var lclass = new LoxClass(stmt.Name.Lexeme, superclass as LoxClass, methods);

            if (superclass != null)
            {
                _environment = _environment.Enclosing;
            }

            _environment.Assign(stmt.Name, lclass);
            return null;
        }

        public Void VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        public Void VisitFunctionStmt(Stmt.Function stmt)
        {
            var function = new LoxFunction(stmt, _environment, false);
            _environment.Define(stmt.Name.Lexeme, function);
            return null;
        }

        public Void VisitReturnStmt(Stmt.Return stmt)
        {
            object value = null;
            if (stmt.Value != null) value = Evaluate(stmt.Value);

            throw new Return(value);
        }

        public Void VisitIfStmt(Stmt.If stmt)
        {
            if (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.ThenBranch);
            }
            else if (stmt.ElseBranch != null)
            {
                Execute(stmt.ElseBranch);
            }

            return null;
        }

        public Void VisitPrintStmt(Stmt.Print stmt)
        {
            var value = Evaluate(stmt.Expr);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public Void VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;

            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            _environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        public Void VisitWhileStmt(Stmt.While stmt)
        {
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }

            return null;
        }

        public object VisitAssignExpr(Expr.Assign expr)
        {
            var value = Evaluate(expr.Value);

            if (_locals.TryGetValue(expr, out var distance))
            {
                _environment.AssignAt(distance, expr.Name, value);
            }
            else
            {
                Globals.Assign(expr.Name, value);
            }

            return value;
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            var left = Evaluate(expr.Left);
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case PLUS:
                    if (left is double ld && right is double rd)
                    {
                        return ld + rd;
                    }

                    if (left is string ls && right is string rs)
                    {
                        return ls + rs;
                    }

                    throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings");
                case SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
                case BANG_EQUAL: return !IsEqual(left, right);
                case EQUAL_EQUAL: return IsEqual(left, right);
            }

            // Unreachable
            return null;
        }

        public object VisitCallExpr(Expr.Call expr)
        {
            var callee = Evaluate(expr.Callee);
            var arguments = new List<object>();

            foreach (var argument in expr.Arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            if (callee is ILoxCallable function)
            {
                if (arguments.Count != function.Arity())
                {
                    throw new RuntimeError(expr.Paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
                }
                return function.Call(this, arguments);
            }

            throw new RuntimeError(expr.Paren, "Can only call functions and classes");
        }

        public object VisitGetExpr(Expr.Get expr)
        {
            var obj = Evaluate(expr.Obj);

            if (obj is LoxInstance li)
            {
                return li.Get(expr.Name);
            }

            throw new RuntimeError(expr.Name, "Only instances have properties");
        }

        public object VisitAnonymousFunctionExpr(Expr.AnonymousFunction expr)
            => new LoxAnonymousFunction(expr, _environment);

        public object VisitGroupingExpr(Expr.Grouping expr)
            => Evaluate(expr.Expression);

        public object VisitLiteralExpr(Expr.Literal expr)
            => expr.Value;

        public object VisitLogicalExpr(Expr.Logical expr)
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

        public object VisitSetExpr(Expr.Set expr)
        {
            var obj = Evaluate(expr.Obj);

            if (obj is LoxInstance li)
            {
                var value = Evaluate(expr.Value);
                li.Set(expr.Name, value);
                return value;
            }

            throw new RuntimeError(expr.Name, "Only instances have fields.");
        }

        public object VisitSuperExpr(Expr.Super expr)
        {
            var distance = _locals[expr];
            var superclass = _environment.GetAt(distance, "super") as LoxClass;
            var obj = _environment.GetAt(distance - 1, "this") as LoxInstance;
            var method = superclass.FindMethod(expr.Method.Lexeme);

            if (method == null)
            {
                throw new RuntimeError(expr.Method, $"Undefined property '{expr.Method.Lexeme}'.");
            }

            return method.Bind(obj);
        }

        public object VisitThisExpr(Expr.This expr)
            => LookUpVariable(expr.Keyword, expr);

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case BANG:
                    return !IsTruthy(right);
                case MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right;
            }

            // Unreachable
            return null;
        }

        public object VisitVariableExpr(Expr.Variable expr)
            => LookUpVariable(expr.Name, expr);

        private object LookUpVariable(Token name, Expr expr)
        {
            if (_locals.TryGetValue(expr, out var distance))
            {
                return _environment.GetAt(distance, name.Lexeme);
            }
            return Globals.Get(name);
        }

        private void CheckNumberOperand(Token op, object operand)
        {
            if (operand is double) return;
            throw new RuntimeError(op, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token op, object left, object right)
        {
            if (left is double && right is double) return;
            throw new RuntimeError(op, "Operand must be a number.");
        }

        private bool IsTruthy(object obj)
        {
            if (obj is null) return false;
            if (obj is bool b) return b;
            return true;
        }

        private bool IsEqual(object a, object b)
        {
            if (a is null && b is null) return true;
            if (a is null) return false;
            if (a is double d && double.IsNaN(d))
                return false;
            if (b is double e && double.IsNaN(e))
                return false;

            return a.Equals(b);
        }

        private string Stringify(object value)
        {
            if (value is null) return "nil";

            if (value is double d)
                return d.ToString(CultureInfo.InvariantCulture);

            if (value is bool b)
                return b.ToString().ToLower(CultureInfo.InvariantCulture);

            return value.ToString();
        }
    }
}
