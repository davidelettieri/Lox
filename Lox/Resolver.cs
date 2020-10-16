using System.Collections.Generic;

namespace Lox
{
    public class Resolver : Expr.IVisitor<Void>, Stmt.IVisitor<Void>
    {
        private readonly Interpreter _interpreter;
        private readonly Stack<Dictionary<string, bool>> _scopes = new Stack<Dictionary<string, bool>>();

        public Resolver(Interpreter interpreter)
        {
            _interpreter = interpreter;
        }

        private void Resolve(List<Stmt> statements)
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
            scope[name.Lexeme] = false;
        }

        private void Define(Token name)
        {
            if (_scopes.Count == 0) return;

            _scopes.Peek()[name.Lexeme] = true;
        }

        private void ResolveLocal(Expr expr, Token name)
        {
            var scopeArray = _scopes.ToArray();
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

        public Void VisitFunctionStmt(Stmt.Function stmt)
        {
            Declare(stmt.Name);
            Define(stmt.Name);
            ResolveFunction(stmt);
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

        public Void VisitVariableExpr(Expr.Variable expr)
        {
            if (_scopes.Count > 0 && !_scopes.Peek().TryGetValue(expr.Name.Lexeme, out var defined) && !defined)
            {
                Lox.Error(expr.Name, "Can't read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.Name);
            return null;
        }

        private void Resolve(Stmt stmt) => stmt.Accept(this);
        private void Resolve(Expr expr) => expr.Accept(this);

        private void ResolveFunction(Stmt.Function function)
        {
            BeginScope();
            foreach (var param in function.Parameters)
            {
                Declare(param);
                Define(param);
            }

            Resolve(function.Body);
            EndScope();
        }
    }
}