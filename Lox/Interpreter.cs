using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Lox.TokenType;

namespace Lox
{
    public class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<Void>
    {
        private LoxEnvironment _environment = new LoxEnvironment();

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
        private void ExecuteBlock(List<Stmt> statements, LoxEnvironment environment)
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

        public Void VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.Statements, new LoxEnvironment(_environment));
            return null;
        }

        public Void VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        public Void VisitIfStmt(Stmt.If stmt)
        {
            if (IsTruthy(stmt.Condition))
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

        public object VisitAssignExpr(Expr.Assign expr)
        {
            var value = Evaluate(expr.Value);

            _environment.Assign(expr.Name, value);
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

        public object VisitGroupingExpr(Expr.Grouping expr)
            => Evaluate(expr.Expression);

        public object VisitLiteralExpr(Expr.Literal expr)
            => expr.Value;

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
            => _environment.Get(expr.Name);

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

            return a.Equals(b);
        }

        private string Stringify(object value)
        {
            if (value is null) return "nil";

            if (value is double d)
                return d.ToString();

            return value.ToString();
        }
    }
}
