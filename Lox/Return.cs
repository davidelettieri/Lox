using System;

namespace Lox;

public class ReturnException(object? value) : Exception
{
    public object? Value { get; }= value;
}