using System;
using System.Collections.Generic;

namespace Lox
{
    public abstract class Stmt
    {
        public interface IVisitor<R>
        {
            R VisitBlockStmt(Block stmt);
            R VisitClassStmt(Class stmt);
            R VisitExpressionStmt(Expression stmt);
            R VisitFunctionStmt(Function stmt);
            R VisitIfStmt(If stmt);
            R VisitPrintStmt(Print stmt);
            R VisitReturnStmt(Return stmt);
            R VisitVarStmt(Var stmt);
            R VisitWhileStmt(While stmt);
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
        public class Class : Stmt
        {
            public Token Name { get; }
            public Exprt.Variable Superclass { get; }
            public List<Stmt.Function> Methods { get; }
            public Class(Token name, Exprt.Variable superclass, List<Stmt.Function> methods)
            {
                Name = name;
                Superclass = superclass;
                Methods = methods;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitClassStmt(this);
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
        public class Function : Stmt
        {
            public Token Name { get; }
            public List<Token> Parameters { get; }
            public List<Stmt> Body { get; }
            public Function(Token name, List<Token> parameters, List<Stmt> body)
            {
                Name = name;
                Parameters = parameters;
                Body = body;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitFunctionStmt(this);
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
        public class Return : Stmt
        {
            public Token Keyword { get; }
            public Expr Value { get; }
            public Return(Token keyword, Expr value)
            {
                Keyword = keyword;
                Value = value;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitReturnStmt(this);
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
        public class While : Stmt
        {
            public Expr Condition { get; }
            public Stmt Body { get; }
            public While(Expr condition, Stmt body)
            {
                Condition = condition;
                Body = body;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitWhileStmt(this);
            }
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
