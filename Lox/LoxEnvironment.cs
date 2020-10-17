using System;
using System.Collections.Generic;
using System.Text;

namespace Lox
{
    public class LoxEnvironment
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        public LoxEnvironment Enclosing { get; }

        public LoxEnvironment() => Enclosing = null;
        public LoxEnvironment(LoxEnvironment enclosing) => Enclosing = enclosing;

        public void Define(string name, object value) => _values[name] = value;

        public object Get(Token name)
        {
            if (_values.TryGetValue(name.Lexeme, out var value)) return value;
            if (Enclosing != null) return Enclosing.Get(name);

            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }

        public void Assign(Token name, object value)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                _values[name.Lexeme] = value;
                return;
            }

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }

        public object GetAt(int distance, string name)
            => Ancestor(distance)._values[name];
        public void AssignAt(int distance, Token name, object value)
            => Ancestor(distance)._values[name.Lexeme] = value;

        private LoxEnvironment Ancestor(int distance)
        {
            var environment = this;

            for (int i = 0; i < distance; i++)
            {
                environment = environment.Enclosing;
            }

            return environment;
        }
    }
}
