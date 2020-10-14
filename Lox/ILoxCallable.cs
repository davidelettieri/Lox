using System;
using System.Collections.Generic;

namespace Lox
{
    public interface ILoxCallable
    {
        int Arity();
        object Call(Interpreter interpreter, List<object> arguments);
    }
}