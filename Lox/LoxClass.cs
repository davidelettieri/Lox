using System.Collections.Generic;

namespace Lox;

public class LoxClass(string name, LoxClass? superclass, Dictionary<string, LoxFunction> methods)
    : ILoxCallable
{
    public string Name { get; } = name;

    public LoxFunction? FindMethod(string name)
    {
        if (methods.TryGetValue(name, out var method))
        {
            return method;
        }

        if (superclass is not null)
        {
            return superclass.FindMethod(name);
        }

        return null;
    }

    public override string ToString()
        => Name;

    public int Arity()
    {
        var initializer = FindMethod("init");

        return initializer?.Arity() ?? 0;
    }

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        var instance = new LoxInstance(this);
        var initializer = FindMethod("init");
        if (initializer != null)
        {
            initializer.Bind(instance).Call(interpreter, arguments);
        }
        return instance;
    }
}