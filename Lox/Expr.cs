using System;
using System.Collections.Generic;

namespace Lox
{
    public abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitBinaryExpr(Binary expr);
            R VisitCallExpr(Call expr);
            R VisitGetExpr(Get expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitLiteralExpr(Literal expr);
            R VisitLogicalExpr(Logical expr);
            R VisitUnaryExpr(Unary expr);
            R VisitSetExpr(Set expr);
            R VisitSuperExpr(Super expr);
            R VisitThisExpr(This expr);
            R VisitVariableExpr(Variable expr);
            R VisitAssignExpr(Assign expr);
            R VisitAnonymousFunctionExpr(AnonymousFunction expr);
        }
        public class Binary : Expr
        {
            public Expr Left { get; }
            public Token Operator { get; }
            public Expr Right { get; }
            public Binary(Expr left, Token @operator, Expr right)
            {
                Left = left;
                Operator = @operator;
                Right = right;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }
        public class Call : Expr
        {
            public Expr Callee { get; }
            public Token Paren { get; }
            public List<Expr> Arguments { get; }
            public Call(Expr callee, Token paren, List<Expr> arguments)
            {
                Callee = callee;
                Paren = paren;
                Arguments = arguments;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitCallExpr(this);
            }
        }
        public class Get : Expr
        {
            public Expr Obj { get; }
            public Token Name { get; }
            public Get(Expr obj, Token name)
            {
                Obj = obj;
                Name = name;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGetExpr(this);
            }
        }
        public class Grouping : Expr
        {
            public Expr Expression { get; }
            public Grouping(Expr expression)
            {
                Expression = expression;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }
        }
        public class Literal : Expr
        {
            public object Value { get; }
            public Literal(object value)
            {
                Value = value;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }
        }
        public class Logical : Expr
        {
            public Expr Left { get; }
            public Token Operator { get; }
            public Expr Right { get; }
            public Logical(Expr left, Token @operator, Expr right)
            {
                Left = left;
                Operator = @operator;
                Right = right;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLogicalExpr(this);
            }
        }
        public class Unary : Expr
        {
            public Token Operator { get; }
            public Expr Right { get; }
            public Unary(Token @operator, Expr right)
            {
                Operator = @operator;
                Right = right;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }
        }
        public class Set : Expr
        {
            public Expr Obj { get; }
            public Token Name { get; }
            public Expr Value { get; }
            public Set(Expr obj, Token name, Expr value)
            {
                Obj = obj;
                Name = name;
                Value = value;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitSetExpr(this);
            }
        }
        public class Super : Expr
        {
            public Token Keyword { get; }
            public Token Method { get; }
            public Super(Token keyword, Token method)
            {
                Keyword = keyword;
                Method = method;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitSuperExpr(this);
            }
        }
        public class This : Expr
        {
            public Token Keyword { get; }
            public This(Token keyword)
            {
                Keyword = keyword;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitThisExpr(this);
            }
        }
        public class Variable : Expr
        {
            public Token Name { get; }
            public Variable(Token name)
            {
                Name = name;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitVariableExpr(this);
            }
        }
        public class Assign : Expr
        {
            public Token Name { get; }
            public Expr Value { get; }
            public Assign(Token name, Expr value)
            {
                Name = name;
                Value = value;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitAssignExpr(this);
            }
        }
        public class AnonymousFunction : Expr
        {
            public List<Token> Parameters { get; }
            public List<Stmt> Body { get; }
            public AnonymousFunction(List<Token> parameters, List<Stmt> body)
            {
                Parameters = parameters;
                Body = body;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitAnonymousFunctionExpr(this);
            }
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
