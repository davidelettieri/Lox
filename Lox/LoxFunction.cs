using System;
using System.Collections.Generic;
using System.Linq;

namespace Lox
{
    public class LoxAnonymousFunction : ILoxCallable
    {
        private readonly Expr.AnonymousFunction _declaration;
        private readonly LoxEnvironment _closure;

        public LoxAnonymousFunction(Expr.AnonymousFunction declaration, LoxEnvironment closure)
        {
            _declaration = declaration;
            _closure = closure;
        }

        public int Arity() => _declaration.Parameters.Count;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            var environment = new LoxEnvironment(_closure);

            for (int i = 0; i < _declaration.Parameters.Count; i++)
            {
                environment.Define(_declaration.Parameters[i].Lexeme, arguments[i]);
            }
            try
            {
                interpreter.ExecuteBlock(_declaration.Body, environment);
            }
            catch (Return returnValue)
            {
                return returnValue.Value;
            }
            return null;
        }

        public override string ToString() => $"<anonymous fn>";
    }

    public class LoxFunction : ILoxCallable
    {
        private readonly Stmt.Function _declaration;
        private readonly LoxEnvironment _closure;
        private readonly bool _isInitializer;

        public LoxFunction(Stmt.Function declaration, LoxEnvironment closure, bool isInitializer)
        {
            _declaration = declaration;
            _closure = closure;
            _isInitializer = isInitializer;
        }

        public LoxFunction Bind(LoxInstance loxInstance)
        {
            var environment = new LoxEnvironment(_closure);
            environment.Define("this", loxInstance);
            return new LoxFunction(_declaration, environment, _isInitializer);
        }

        public int Arity() => _declaration.Parameters.Count;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            var environment = new LoxEnvironment(_closure);

            for (int i = 0; i < _declaration.Parameters.Count; i++)
            {
                environment.Define(_declaration.Parameters[i].Lexeme, arguments[i]);
            }
            try
            {
                interpreter.ExecuteBlock(_declaration.Body, environment);
            }
            catch (Return returnValue)
            {
                if (_isInitializer) return _closure.GetAt(0, "this");
                
                return returnValue.Value;
            }

            if (_isInitializer) return _closure.GetAt(0, "this");

            return null;
        }

        public override string ToString() => $"<fn {_declaration.Name.Lexeme}>";

    }
}
