using System.Collections.Generic;
using System.Linq;

namespace Lox
{
    public class LoxFunction : ILoxCallable
    {
        private readonly Stmt.Function _declaration;

        public LoxFunction(Stmt.Function declaration)
        {
            _declaration = declaration;
        }

        public int Arity() => _declaration.Parameters.Count;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            var environment = new LoxEnvironment(interpreter.Globals);

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

        public override string ToString() => $"<fn {_declaration.Name.Lexeme}>";
    }
}
