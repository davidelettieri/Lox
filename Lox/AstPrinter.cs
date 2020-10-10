using System;
using System.Collections.Generic;
using System.Text;

namespace Lox
{
    public class AstPrinter : Expr.IVisitor<string>
    {
        public string Print(Expr expr) => expr.Accept(this);

        public string VisitBinaryExpr(Expr.Binary expr)
            => Parenthesize(expr.)

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            throw new NotImplementedException();
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            throw new NotImplementedException();
        }
    }
}
