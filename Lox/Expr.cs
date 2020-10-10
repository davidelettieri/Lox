using System;
namespace Lox
{
    public abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitBinaryExpr(Binary expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitLiteralExpr(Literal expr);
            R VisitUnaryExpr(Unary expr);
        }
        public class Binary : Expr
        {
            public Expr Left { get; }
            public Token Operator { get; }
            public Expr Right { get; }
            Binary(expr left, token @operator, expr right)
            {
                _Left = Left;
                _Operator = @Operator;
                _Right = Right;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }
        public class Grouping : Expr
        {
            public Expr Expression { get; }
            Grouping(expr expression)
            {
                _Expression = Expression;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }
        }
        public class Literal : Expr
        {
            public Object Value { get; }
            Literal(object value)
            {
                _Value = Value;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }
        }
        public class Unary : Expr
        {
            public Token Operator { get; }
            public Expr Right { get; }
            Unary(token @operator, expr right)
            {
                _Operator = @Operator;
                _Right = Right;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
