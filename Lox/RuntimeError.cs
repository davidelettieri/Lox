using System;

namespace Lox;

public class RuntimeError(Token token, string message) : Exception(message)
{
    public Token Token { get; } = token;
}