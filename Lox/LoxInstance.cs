using System.Collections.Generic;

namespace Lox;

public class LoxInstance(LoxClass lclass)
{
    private readonly Dictionary<string, object> _fields = new();

    public override string ToString()
    {
        return $"{lclass.Name} instance";
    }

    public object Get(Token name)
    {
        if (_fields.TryGetValue(name.Lexeme, out var value))
        {
            return value;
        }

        var method = lclass.FindMethod(name.Lexeme);

        if (method != null) return method.Bind(this);

        throw new RuntimeError(name, $"Undefined property '{name.Lexeme}'.");
    }

    public void Set(Token name, object value)
    {
        _fields[name.Lexeme] = value;
    }
}