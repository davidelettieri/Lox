using Humanizer;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Lox.Tests.Tool
{
    class Program
    {
        private static string _destinationPath;

        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            _destinationPath = args[0];

            var tests = Directory.GetFiles("./test");
            var directories = Directory.GetDirectories("./test");

            var sb = new StringBuilder();
            sb.AppendLine("using Xunit;");
            sb.AppendLine("using System.Diagnostics;");
            sb.AppendLine("namespace Lox.Tests {");
            sb.AppendLine("    public class Test");
            sb.AppendLine("    {");
            sb.AppendLine("        public string[] RunLoxWithArgument(string filePath)");
            sb.AppendLine("        {");
            sb.AppendLine("            Process p = new Process();");
            sb.AppendLine("            p.StartInfo.Arguments = filePath;");
            sb.AppendLine("            p.StartInfo.UseShellExecute = false;");
            sb.AppendLine("            p.StartInfo.RedirectStandardOutput = true;");
            sb.AppendLine("            p.StartInfo.FileName = \"./Lox/Lox.exe\";");
            sb.AppendLine("            p.Start();");
            sb.AppendLine("            string output = p.StandardOutput.ReadToEnd();");
            sb.AppendLine("            p.WaitForExit();");
            sb.AppendLine("            return output.Split('\\n');");
            sb.AppendLine("        }");

            foreach (var test in tests)
            {
                BuildTestFile("", test, sb);
            }

            foreach (var directory in directories)
            {
                foreach (var test in Directory.GetFiles(directory))
                {
                    BuildTestFile(directory, test, sb);
                }
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText($"{_destinationPath}/Tests.cs", sb.ToString());
        }

        static void BuildTestFile(string directoryName, string filePath, StringBuilder sb)
        {
            var fileInfo = new FileInfo(filePath);
            var lines = File.ReadAllLines(filePath);
            var testName = fileInfo.Name.Replace(".lox", "").Humanize().Dehumanize();

            if (directoryName.Length > 6)
            {
                testName = directoryName.Substring(7).Humanize().Dehumanize() + "_" + testName;
            }

            var displayName = testName;
            if (lines.Length > 0 && lines[0].StartsWith("//"))
                displayName = lines[0].Substring(2);

            sb.Append("        [Fact(DisplayName=\"");
            sb.Append(displayName);
            sb.AppendLine("\")]");
            sb.AppendLine($"        public void {testName}()");
            sb.AppendLine("        {");
            sb.AppendLine($"            var result = RunLoxWithArgument(\"{filePath.Replace('\\', '/')}\");");
            sb.AppendLine($"            var count = 0;");

            foreach (var item in lines)
            {
                var i = item.IndexOf("expect: ");

                if (i == -1)
                    continue;

                var assert = item.Substring(i + 8);
                sb.AppendLine($"            Assert.Equal(\"{assert}\\r\",result[count]);");
                sb.AppendLine($"            count++;");
            }


            sb.AppendLine("        }");

        }
    }
}
