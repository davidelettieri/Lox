using System.Collections.Generic;

namespace Lox;

public class LoxFunction(Function declaration, LoxEnvironment? closure, bool isInitializer)
    : ILoxCallable
{
    public LoxFunction Bind(LoxInstance loxInstance)
    {
        var environment = new LoxEnvironment(closure);
        environment.Define("this", loxInstance);
        return new LoxFunction(declaration, environment, isInitializer);
    }

    public int Arity() => declaration.Parameters.Count;

    public object? Call(Interpreter interpreter, List<object> arguments)
    {
        var environment = new LoxEnvironment(closure);

        for (var i = 0; i < declaration.Parameters.Count; i++)
        {
            environment.Define(declaration.Parameters[i].Lexeme, arguments[i]);
        }
        try
        {
            interpreter.ExecuteBlock(declaration.Body, environment);
        }
        catch (ReturnException returnValue)
        {
            return isInitializer ? closure?.GetAt(0, "this") : returnValue.Value;
        }

        return isInitializer ? closure?.GetAt(0, "this") : null;
    }

    public override string ToString() => $"<fn {declaration.Name.Lexeme}>";

}