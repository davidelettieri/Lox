using System.Collections.Generic;

namespace Lox
{
    public class LoxInstance
    {
        private readonly LoxClass _class;
        private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();
        public LoxInstance(LoxClass lclass)
        {
            _class = lclass;
        }

        public override string ToString()
        {
            return $"{_class.Name} instance";
        }

        public object Get(Token name)
        {
            if (_fields.TryGetValue(name.Lexeme, out var value))
            {
                return value;
            }

            var method = _class.FindMethod(name.Lexeme);

            if (method != null) return method.Bind(this);

            throw new RuntimeError(name, $"Undefined property '{name.Lexeme}'.");
        }

        public void Set(Token name, object value)
        {
            _fields[name.Lexeme] = value;
        }
    }
}
