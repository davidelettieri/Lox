using System.Collections.Generic;

namespace Lox;

public class LoxClass(string name, LoxClass? superclass, IReadOnlyDictionary<string, LoxFunction> methods)
    : ILoxCallable
{
    public string Name { get; } = name;

    public LoxFunction? FindMethod(string methodName)
    {
        if (methods.TryGetValue(methodName, out var method))
        {
            return method;
        }

        return superclass?.FindMethod(methodName);
    }

    public override string ToString() => Name;

    public int Arity()
    {
        var initializer = FindMethod("init");

        return initializer?.Arity() ?? 0;
    }

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        var instance = new LoxInstance(this);
        var initializer = FindMethod("init");
        initializer?.Bind(instance).Call(interpreter, arguments);
        return instance;
    }
}