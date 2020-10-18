using System;
using System.Collections.Generic;
using System.Linq;

namespace Lox
{
    public class Resolver : Expr.IVisitor<Void>, Stmt.IVisitor<Void>
    {
        private readonly Interpreter _interpreter;
        private readonly Stack<Dictionary<string, bool>> _scopes = new Stack<Dictionary<string, bool>>();
        private FunctionType _currentFunction = FunctionType.NONE;
        private ClassType _currentClass = ClassType.NONE;
        public Resolver(Interpreter interpreter)
        {
            _interpreter = interpreter;
        }

        public void Resolve(List<Stmt> statements)
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
                Lox.Error(name, "Already variable with this name in this scope.");
            }

            scope[name.Lexeme] = false;
        }

        private void Define(Token name)
        {
            if (_scopes.Count == 0) return;

            _scopes.Peek()[name.Lexeme] = true;
        }

        private void ResolveLocal(Expr expr, Token name)
        {
            var t = _scopes.ToArray();
            var scopeArray = _scopes.Reverse().ToArray();
            for (int i = _scopes.Count - 1; i >= 0; i--)
            {
                if (scopeArray[i].ContainsKey(name.Lexeme))
                {
                    _interpreter.Resolve(expr, _scopes.Count - 1 - i);
                    return;
                }
            }
        }

        public Void VisitBlockStmt(Stmt.Block stmt)
        {
            BeginScope();
            Resolve(stmt.Statements);
            EndScope();
            return null;
        }

        public Void VisitClassStmt(Stmt.Class stmt)
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
            return null;
        }

        public Void VisitExpressionStmt(Stmt.Expression stmt)
        {
            Resolve(stmt.Expr);
            return null;
        }

        public Void VisitFunctionStmt(Stmt.Function stmt)
        {
            Declare(stmt.Name);
            Define(stmt.Name);
            ResolveFunction(stmt, FunctionType.FUNCTION);
            return null;
        }

        public Void VisitIfStmt(Stmt.If stmt)
        {
            Resolve(stmt.Condition);
            Resolve(stmt.ThenBranch);
            if (stmt.ElseBranch != null) Resolve(stmt.ElseBranch);
            return null;
        }

        public Void VisitPrintStmt(Stmt.Print stmt)
        {
            Resolve(stmt.Expr);
            return null;
        }

        public Void VisitReturnStmt(Stmt.Return stmt)
        {
            if (_currentFunction == FunctionType.NONE)
            {
                Lox.Error(stmt.Keyword, "Can't return from top-level code.");
            }
            if (stmt.Value != null)
            {
                if (_currentFunction == FunctionType.INITIALIZER)
                {
                    Lox.Error(stmt.Keyword, "Can't return a value from an initializer");
                }
                Resolve(stmt.Value);
            }

            return null;
        }

        public Void VisitWhileStmt(Stmt.While stmt)
        {
            Resolve(stmt.Condition);
            Resolve(stmt.Body);
            return null;
        }

        public Void VisitVarStmt(Stmt.Var stmt)
        {
            Declare(stmt.Name);
            if (stmt.Initializer != null)
            {
                Resolve(stmt.Initializer);
            }
            Define(stmt.Name);
            return null;
        }

        public Void VisitAssignExpr(Expr.Assign expr)
        {
            Resolve(expr.Value);
            ResolveLocal(expr, expr.Name);
            return null;
        }

        public Void VisitBinaryExpr(Expr.Binary expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }

        public Void VisitCallExpr(Expr.Call expr)
        {
            Resolve(expr.Callee);

            foreach (var argument in expr.Arguments)
            {
                Resolve(argument);
            }

            return null;
        }

        public Void VisitGetExpr(Expr.Get expr)
        {
            Resolve(expr.Obj);
            return null;
        }

        public Void VisitGroupingExpr(Expr.Grouping expr)
        {
            Resolve(expr.Expression);
            return null;
        }

        public Void VisitLiteralExpr(Expr.Literal expr) => null;

        public Void VisitLogicalExpr(Expr.Logical expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }

        public Void VisitSetExpr(Expr.Set expr)
        {
            Resolve(expr.Value);
            Resolve(expr.Obj);
            return null;
        }

        public Void VisitSuperExpr(Expr.Super expr)
        {
            if (_currentClass == ClassType.NONE)
            {
                Lox.Error(expr.Keyword, "Can't use 'super' outside of a class");
            }
            else if (_currentClass != ClassType.SUBCLASS)
            {
                Lox.Error(expr.Keyword, "Can't use 'super' in a class with no superclass.");
            }
            ResolveLocal(expr, expr.Keyword);
            return null;
        }

        public Void VisitThisExpr(Expr.This expr)
        {
            if (_currentClass == ClassType.NONE)
            {
                Lox.Error(expr.Keyword, "Can't use 'this' outside of a class.");
                return null;
            }
            ResolveLocal(expr, expr.Keyword);
            return null;
        }

        public Void VisitUnaryExpr(Expr.Unary expr)
        {
            Resolve(expr.Right);
            return null;
        }

        public Void VisitAnonymousFunctionExpr(Expr.AnonymousFunction expr)
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
            return null;
        }

        public Void VisitVariableExpr(Expr.Variable expr)
        {
            if (_scopes.Count > 0 && _scopes.Peek().TryGetValue(expr.Name.Lexeme, out var defined) && !defined)
            {
                Lox.Error(expr.Name, "Can't read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.Name);
            return null;
        }

        private void Resolve(Stmt stmt) => stmt.Accept(this);
        private void Resolve(Expr expr) => expr.Accept(this);

        private void ResolveFunction(Stmt.Function function, FunctionType type)
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
}