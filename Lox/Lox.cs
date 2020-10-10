using System;
using System.IO;

namespace Lox
{
    class Lox
    {
        private static bool _hadError = false;
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: Lox [script]");
                Environment.Exit(64);
            }

            if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        private static void RunFile(string path)
        {
            var source = File.ReadAllText(path);
            Run(source);
            if (_hadError)
            {
                Environment.Exit(65);
            }
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

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line} ] Error {where}: {message}");
            _hadError = true;
        }
    }
}
