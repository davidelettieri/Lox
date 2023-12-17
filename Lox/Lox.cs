using System;
using System.IO;

namespace Lox;

internal static class Lox
{
    private static readonly Interpreter Interpreter = new();
    private static bool _hadError;
    private static bool _hadRuntimeError;
    private static void Main(string[] args)
    {
        var oldInputEncoding = Console.InputEncoding;
        var oldOutputEncoding = Console.OutputEncoding;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        switch (args.Length)
        {
            case > 1:
                Console.WriteLine("Usage: Lox [script]");
                Environment.Exit(64);
                break;
            case 1:
                RunFile(args[0]);
                break;
            default:
                RunPrompt();
                break;
        }

        Console.InputEncoding = oldInputEncoding;
        Console.OutputEncoding = oldOutputEncoding;
    }

    private static void RunFile(string path)
    {
        var source = File.ReadAllText(path);
        Run(source);
        if (_hadError) Environment.Exit(65);
        if (_hadRuntimeError) Environment.Exit(70);
    }

    private static void RunPrompt()
    {
        for (; ; )
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (line is null)
            {
                break;
            }

            Run(line);
            _hadError = false;
        }
    }

    private static void Run(string source)
    {
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new Parser(tokens);
        var statements = parser.Parse();

        if (_hadError) return;

        var resolver = new Resolver(Interpreter);
        resolver.Resolve(statements);

        if (_hadError) return;

        Interpreter.Interpret(statements);
    }

    public static void RuntimeError(RuntimeError error)
    {
        Console.Error.WriteLine($"{error.Message}\n[line {error.Token.Line}]");
        _hadRuntimeError = true;
    }


    public static void Error(Token token, string? message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, " at '" + token.Lexeme + "'", message);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string? message)
    {
        Console.Error.WriteLine(message is not null
            ? $"[line {line}] Error{where}: {message}"
            : $"[line {line}] Error{where}");
        _hadError = true;
    }
}