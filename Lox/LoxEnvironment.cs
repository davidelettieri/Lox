﻿using System.Collections.Generic;

namespace Lox;

public class LoxEnvironment(LoxEnvironment? enclosing)
{
    private readonly Dictionary<string, object?> _values = new();
    public LoxEnvironment? Enclosing { get; } = enclosing;

    public LoxEnvironment() : this(null)
    {
    }

    public void Define(string name, object? value) => _values[name] = value;

    public object? Get(Token name)
    {
        if (_values.TryGetValue(name.Lexeme, out var value)) return value;
        if (Enclosing != null) return Enclosing.Get(name);

        throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
    }

    public void Assign(Token name, object? value)
    {
        if (_values.ContainsKey(name.Lexeme))
        {
            _values[name.Lexeme] = value;
            return;
        }

        if (Enclosing == null)
        {
            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }
        Enclosing.Assign(name, value);
    }

    public object? GetAt(int distance, string name)
        => Ancestor(distance)?._values[name];

    public void AssignAt(int distance, Token name, object? value)
    {
        var ancestor = Ancestor(distance);

        if (ancestor is null)
        {
            throw new RuntimeError(name, "Environment not found");
        }

        ancestor._values[name.Lexeme] = value;
    }

    private LoxEnvironment? Ancestor(int distance)
    {
        var environment = this;

        for (var i = 0; i < distance; i++)
        {
            environment = environment?.Enclosing;
        }

        return environment;
    }
}