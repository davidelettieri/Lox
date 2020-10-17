using System;
using System.IO;
using System.Collections.Generic;
using Humanizer;

namespace Lox.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }
            else
            {
                var outputDir = args[0];
                DefineAst(outputDir, "Expr", new List<string>() {
                    "Binary             : Expr left, Token @operator, Expr right",
                    "Call               : Expr callee, Token paren, List<Expr> arguments",
                    "Get                : Expr obj, Token name",
                    "Grouping           : Expr expression",
                    "Literal            : object value",
                    "Logical            : Expr left, Token @operator, Expr right",
                    "Unary              : Token @operator, Expr right",
                    "Set                : Expr obj, Token name, Expr value",
                    "Super              : Token keyword, Token method",
                    "This               : Token keyword",
                    "Variable           : Token name",
                    "Assign             : Token name, Expr value",
                    "AnonymousFunction  : List<Token> parameters, List<Stmt> body"
                });

                DefineAst(outputDir, "Stmt", new List<string>()
                {
                    "Block      : List<Stmt> statements",
                    "Class      : Token name, Expr.Variable superclass, List<Stmt.Function> methods",
                    "Expression : Expr expr",
                    "Function   : Token name, List<Token> parameters, List<Stmt> body",
                    "If         : Expr condition, Stmt thenBranch, Stmt elseBranch",
                    "Print      : Expr expr",
                    "Return     : Token keyword, Expr value",
                    "Var        : Token name, Expr initializer",
                    "While      : Expr condition, Stmt body"
                });
            }
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            var path = $"{outputDir}/{baseName}.cs";
            using var writer = new StreamWriter(path);

            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine("namespace Lox");
            writer.WriteLine("{");
            writer.WriteLine($"    public abstract class {baseName}");
            writer.WriteLine("    {");

            DefineVisitor(writer, baseName, types);

            foreach (var type in types)
            {
                var className = type.Split(":")[0].Trim();
                var fields = type.Split(":")[1].Trim();
                DefineType(writer, baseName, className, fields);
            }
            writer.WriteLine();
            writer.WriteLine("        public abstract R Accept<R>(IVisitor<R> visitor);");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, List<string> fieldList)
        {
            writer.WriteLine("        public interface IVisitor<R>");
            writer.WriteLine("        {");

            foreach (var type in fieldList)
            {
                var typeName = type.Split(':')[0].Trim();
                writer.WriteLine($"            R Visit{typeName}{baseName}({typeName} {baseName.ToLowerInvariant()});");
            }

            writer.WriteLine("        }");
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine($"        public class {className} : {baseName}");
            writer.WriteLine("        {");
            // Fields
            var fields = fieldList.Split(',');
            foreach (var field in fields)
            {
                var type = field.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
                var name = field.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
                writer.WriteLine($"            public {type.Trim()} {name.Trim().Trim('@').Titleize().Replace(" ", "")} {{ get; }}");
            }
            // Constructor.
            writer.WriteLine($"            public {className}({fieldList})");
            writer.WriteLine("            {");

            // Store parameters in fields.
            foreach (var field in fields)
            {
                var name = field.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                writer.WriteLine($"                {name[1].Trim().Trim('@').Titleize().Replace(" ","")} = {name[1]};");
            }
            writer.WriteLine("            }");
            writer.WriteLine("            public override R Accept<R>(IVisitor<R> visitor)");
            writer.WriteLine("            {");
            writer.WriteLine($"                return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
        }
    }
}
