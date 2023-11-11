using System;
using System.Collections.Generic;

namespace Lox;

public class Clock : ILoxCallable
{
    public int Arity() => 0;

    public object Call(Interpreter interpreter, List<object> arguments)
        => DateTime.UtcNow.Ticks / 10000.0;

    public override string ToString() => "<native fn>";
}