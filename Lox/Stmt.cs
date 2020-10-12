using System;
using System.Collections.Generic;

namespace Lox
{
    public abstract class Stmt
    {
        public interface IVisitor<R>
        {
            R VisitBlockStmt(Block stmt);
            R VisitExpressionStmt(Expression stmt);
            R VisitIfStmt(If stmt);
            R VisitPrintStmt(Print stmt);
            R VisitVarStmt(Var stmt);
        }
        public class Block : Stmt
        {
            public List<Stmt> Statements { get; }
            public Block(List<Stmt> statements)
            {
                Statements = statements;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBlockStmt(this);
            }
        }
        public class Expression : Stmt
        {
            public Expr Expr { get; }
            public Expression(Expr expr)
            {
                Expr = expr;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitExpressionStmt(this);
            }
        }
        public class If : Stmt
        {
            public Expr Condition { get; }
            public Stmt ThenBranch { get; }
            public Stmt ElseBranch { get; }
            public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
            {
                Condition = condition;
                ThenBranch = thenBranch;
                ElseBranch = elseBranch;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitIfStmt(this);
            }
        }
        public class Print : Stmt
        {
            public Expr Expr { get; }
            public Print(Expr expr)
            {
                Expr = expr;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitPrintStmt(this);
            }
        }
        public class Var : Stmt
        {
            public Token Name { get; }
            public Expr Initializer { get; }
            public Var(Token name, Expr initializer)
            {
                Name = name;
                Initializer = initializer;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitVarStmt(this);
            }
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
