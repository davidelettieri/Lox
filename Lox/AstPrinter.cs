using System;
using System.Collections.Generic;
using System.Text;

namespace Lox
{
    public class AstPrinter : Expr.IVisitor<string>
    {
        public string Print(Expr expr) => expr.Accept(this);

        public string VisitBinaryExpr(Expr.Binary expr)
            => Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);

        public string VisitGroupingExpr(Expr.Grouping expr)
            => Parenthesize("group", expr.Expression);

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            if (expr.Value is null) return "nil";
            return expr.Value.ToString();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
            => Parenthesize(expr.Operator.Lexeme, expr.Right);

        private string Parenthesize(string name, params Expr[] exprs)
        {
            var builder = new StringBuilder();

            builder.Append("(").Append(name);

            foreach (var expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
