using System;
using System.Collections.Generic;
using System.Text;

namespace Lox
{
    public class LoxEnvironment
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        private readonly LoxEnvironment _enclosing;

        public LoxEnvironment() => _enclosing = null;
        public LoxEnvironment(LoxEnvironment enclosing) => _enclosing = enclosing;

        public void Define(string name, object value) => _values[name] = value;

        public object Get(Token name)
        {
            if (_values.TryGetValue(name.Lexeme, out var value)) return value;
            if (_enclosing != null) return _enclosing.Get(name);

            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }

        internal void Assign(Token name, object value)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                _values[name.Lexeme] = value;
                return;
            }

            if (_enclosing != null)
            {
                _enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }
    }
}
