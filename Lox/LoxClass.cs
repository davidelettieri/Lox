using System.Collections.Generic;

namespace Lox
{
    public class LoxClass : ILoxCallable
    {
        public string Name { get; }
        private readonly Dictionary<string, LoxFunction> _methods;

        public LoxClass(string name, Dictionary<string, LoxFunction> methods)
        {
            Name = name;
            _methods = methods;
        }

        public LoxFunction FindMethod(string name)
        {
            if (_methods.TryGetValue(name, out var method))
            {
                return method;
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
}
