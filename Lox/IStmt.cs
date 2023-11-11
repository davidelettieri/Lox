using System.Collections.Generic;
using VisitorGenerator;

namespace Lox;

[VisitorNode]
public partial interface IStmt
{
}

public partial class Block(List<IStmt> statements) : IStmt
{
    public List<IStmt> Statements { get; } = statements;
}

public partial class Class(Token name, Variable? superclass, List<Function> methods)
    : IStmt
{
    public Token Name { get; } = name;
    public Variable? Superclass { get; } = superclass;
    public List<Function> Methods { get; } = methods;
}

public partial class Expression(IExpr expr) : IStmt
{
    public IExpr Expr { get; } = expr;
}

public partial class Function(Token name, List<Token> parameters, List<IStmt> body) : IStmt
{
    public Token Name { get; } = name;
    public List<Token> Parameters { get; } = parameters;
    public List<IStmt> Body { get; } = body;
}

public partial class If(IExpr condition, IStmt thenBranch, IStmt? elseBranch) : IStmt
{
    public IExpr Condition { get; } = condition;
    public IStmt ThenBranch { get; } = thenBranch;
    public IStmt? ElseBranch { get; } = elseBranch;
}

public partial class Print(IExpr expr) : IStmt
{
    public IExpr Expr { get; } = expr;
}

public partial class ReturnStmt(Token keyword, IExpr? value) : IStmt
{
    public Token Keyword { get; } = keyword;
    public IExpr? Value { get; } = value;
}

public partial class Var(Token name, IExpr? initializer) : IStmt
{
    public Token Name { get; } = name;
    public IExpr? Initializer { get; } = initializer;
}

public partial class While(IExpr condition, IStmt body) : IStmt
{
    public IExpr Condition { get; } = condition;
    public IStmt Body { get; } = body;
}