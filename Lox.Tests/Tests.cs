using Xunit;
using System.Diagnostics;
namespace Lox.Tests {
    public class Test
    {
        public string[] RunLoxWithArgument(string filePath)
        {
            Process p = new Process();
            p.StartInfo.Arguments = filePath;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "./Lox/Lox.exe";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output.Split('\n');
        }
        [Fact(DisplayName="EmptyFile")]
        public void EmptyFile()
        {
            var result = RunLoxWithArgument("./test/empty_file.lox");
            var count = 0;
        }
        [Fact(DisplayName=" * has higher precedence than +.")]
        public void Precedence()
        {
            var result = RunLoxWithArgument("./test/precedence.lox");
            var count = 0;
            Assert.Equal("14\r",result[count]);
            count++;
            Assert.Equal("8\r",result[count]);
            count++;
            Assert.Equal("4\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("4\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 3] Error: Unexpected character.")]
        public void UnexpectedCharacter()
        {
            var result = RunLoxWithArgument("./test/unexpected_character.lox");
            var count = 0;
        }
        [Fact(DisplayName="Assignment_Associativity")]
        public void Assignment_Associativity()
        {
            var result = RunLoxWithArgument("./test/assignment/associativity.lox");
            var count = 0;
            Assert.Equal("c\r",result[count]);
            count++;
            Assert.Equal("c\r",result[count]);
            count++;
            Assert.Equal("c\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Assignment_Global")]
        public void Assignment_Global()
        {
            var result = RunLoxWithArgument("./test/assignment/global.lox");
            var count = 0;
            Assert.Equal("before\r",result[count]);
            count++;
            Assert.Equal("after\r",result[count]);
            count++;
            Assert.Equal("arg\r",result[count]);
            count++;
            Assert.Equal("arg\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Assignment_Grouping")]
        public void Assignment_Grouping()
        {
            var result = RunLoxWithArgument("./test/assignment/grouping.lox");
            var count = 0;
        }
        [Fact(DisplayName="Assignment_InfixOperator")]
        public void Assignment_InfixOperator()
        {
            var result = RunLoxWithArgument("./test/assignment/infix_operator.lox");
            var count = 0;
        }
        [Fact(DisplayName="Assignment_Local")]
        public void Assignment_Local()
        {
            var result = RunLoxWithArgument("./test/assignment/local.lox");
            var count = 0;
            Assert.Equal("before\r",result[count]);
            count++;
            Assert.Equal("after\r",result[count]);
            count++;
            Assert.Equal("arg\r",result[count]);
            count++;
            Assert.Equal("arg\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Assignment_PrefixOperator")]
        public void Assignment_PrefixOperator()
        {
            var result = RunLoxWithArgument("./test/assignment/prefix_operator.lox");
            var count = 0;
        }
        [Fact(DisplayName=" Assignment on RHS of variable.")]
        public void Assignment_Syntax()
        {
            var result = RunLoxWithArgument("./test/assignment/syntax.lox");
            var count = 0;
            Assert.Equal("var\r",result[count]);
            count++;
            Assert.Equal("var\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Assignment_ToThis")]
        public void Assignment_ToThis()
        {
            var result = RunLoxWithArgument("./test/assignment/to_this.lox");
            var count = 0;
        }
        [Fact(DisplayName="Assignment_Undefined")]
        public void Assignment_Undefined()
        {
            var result = RunLoxWithArgument("./test/assignment/undefined.lox");
            var count = 0;
        }
        [Fact(DisplayName="Benchmark_BinaryTrees")]
        public void Benchmark_BinaryTrees()
        {
            var result = RunLoxWithArgument("./test/benchmark/binary_trees.lox");
            var count = 0;
        }
        [Fact(DisplayName="Benchmark_Equality")]
        public void Benchmark_Equality()
        {
            var result = RunLoxWithArgument("./test/benchmark/equality.lox");
            var count = 0;
        }
        [Fact(DisplayName="Benchmark_Fib")]
        public void Benchmark_Fib()
        {
            var result = RunLoxWithArgument("./test/benchmark/fib.lox");
            var count = 0;
        }
        [Fact(DisplayName=" This benchmark stresses instance creation and initializer calling.")]
        public void Benchmark_Instantiation()
        {
            var result = RunLoxWithArgument("./test/benchmark/instantiation.lox");
            var count = 0;
        }
        [Fact(DisplayName=" This benchmark stresses just method invocation.")]
        public void Benchmark_Invocation()
        {
            var result = RunLoxWithArgument("./test/benchmark/invocation.lox");
            var count = 0;
        }
        [Fact(DisplayName="Benchmark_MethodCall")]
        public void Benchmark_MethodCall()
        {
            var result = RunLoxWithArgument("./test/benchmark/method_call.lox");
            var count = 0;
        }
        [Fact(DisplayName=" This benchmark stresses both field and method lookup.")]
        public void Benchmark_Properties()
        {
            var result = RunLoxWithArgument("./test/benchmark/properties.lox");
            var count = 0;
        }
        [Fact(DisplayName="Benchmark_StringEquality")]
        public void Benchmark_StringEquality()
        {
            var result = RunLoxWithArgument("./test/benchmark/string_equality.lox");
            var count = 0;
        }
        [Fact(DisplayName="Benchmark_Trees")]
        public void Benchmark_Trees()
        {
            var result = RunLoxWithArgument("./test/benchmark/trees.lox");
            var count = 0;
        }
        [Fact(DisplayName="Benchmark_Zoo")]
        public void Benchmark_Zoo()
        {
            var result = RunLoxWithArgument("./test/benchmark/zoo.lox");
            var count = 0;
        }
        [Fact(DisplayName="Block_Empty")]
        public void Block_Empty()
        {
            var result = RunLoxWithArgument("./test/block/empty.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Block_Scope")]
        public void Block_Scope()
        {
            var result = RunLoxWithArgument("./test/block/scope.lox");
            var count = 0;
            Assert.Equal("inner\r",result[count]);
            count++;
            Assert.Equal("outer\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Bool_Equality")]
        public void Bool_Equality()
        {
            var result = RunLoxWithArgument("./test/bool/equality.lox");
            var count = 0;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Bool_Not")]
        public void Bool_Not()
        {
            var result = RunLoxWithArgument("./test/bool/not.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Call_Bool")]
        public void Call_Bool()
        {
            var result = RunLoxWithArgument("./test/call/bool.lox");
            var count = 0;
        }
        [Fact(DisplayName="Call_Nil")]
        public void Call_Nil()
        {
            var result = RunLoxWithArgument("./test/call/nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Call_Num")]
        public void Call_Num()
        {
            var result = RunLoxWithArgument("./test/call/num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Call_Object")]
        public void Call_Object()
        {
            var result = RunLoxWithArgument("./test/call/object.lox");
            var count = 0;
        }
        [Fact(DisplayName="Call_String")]
        public void Call_String()
        {
            var result = RunLoxWithArgument("./test/call/string.lox");
            var count = 0;
        }
        [Fact(DisplayName="Class_Empty")]
        public void Class_Empty()
        {
            var result = RunLoxWithArgument("./test/class/empty.lox");
            var count = 0;
            Assert.Equal("Foo\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Class_InheritedMethod")]
        public void Class_InheritedMethod()
        {
            var result = RunLoxWithArgument("./test/class/inherited_method.lox");
            var count = 0;
            Assert.Equal("in foo\r",result[count]);
            count++;
            Assert.Equal("in bar\r",result[count]);
            count++;
            Assert.Equal("in baz\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Class_InheritSelf")]
        public void Class_InheritSelf()
        {
            var result = RunLoxWithArgument("./test/class/inherit_self.lox");
            var count = 0;
        }
        [Fact(DisplayName="Class_LocalInheritOther")]
        public void Class_LocalInheritOther()
        {
            var result = RunLoxWithArgument("./test/class/local_inherit_other.lox");
            var count = 0;
            Assert.Equal("B\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Class_LocalInheritSelf")]
        public void Class_LocalInheritSelf()
        {
            var result = RunLoxWithArgument("./test/class/local_inherit_self.lox");
            var count = 0;
        }
        [Fact(DisplayName="Class_LocalReferenceSelf")]
        public void Class_LocalReferenceSelf()
        {
            var result = RunLoxWithArgument("./test/class/local_reference_self.lox");
            var count = 0;
            Assert.Equal("Foo\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Class_ReferenceSelf")]
        public void Class_ReferenceSelf()
        {
            var result = RunLoxWithArgument("./test/class/reference_self.lox");
            var count = 0;
            Assert.Equal("Foo\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_AssignToClosure")]
        public void Closure_AssignToClosure()
        {
            var result = RunLoxWithArgument("./test/closure/assign_to_closure.lox");
            var count = 0;
            Assert.Equal("local\r",result[count]);
            count++;
            Assert.Equal("after f\r",result[count]);
            count++;
            Assert.Equal("after f\r",result[count]);
            count++;
            Assert.Equal("after g\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_AssignToShadowedLater")]
        public void Closure_AssignToShadowedLater()
        {
            var result = RunLoxWithArgument("./test/closure/assign_to_shadowed_later.lox");
            var count = 0;
            Assert.Equal("inner\r",result[count]);
            count++;
            Assert.Equal("assigned\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_ClosedClosureInFunction")]
        public void Closure_ClosedClosureInFunction()
        {
            var result = RunLoxWithArgument("./test/closure/closed_closure_in_function.lox");
            var count = 0;
            Assert.Equal("local\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_CloseOverFunctionParameter")]
        public void Closure_CloseOverFunctionParameter()
        {
            var result = RunLoxWithArgument("./test/closure/close_over_function_parameter.lox");
            var count = 0;
            Assert.Equal("param\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" This is a regression test. There was a bug where if an upvalue for an")]
        public void Closure_CloseOverLaterVariable()
        {
            var result = RunLoxWithArgument("./test/closure/close_over_later_variable.lox");
            var count = 0;
            Assert.Equal("b\r",result[count]);
            count++;
            Assert.Equal("a\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_CloseOverMethodParameter")]
        public void Closure_CloseOverMethodParameter()
        {
            var result = RunLoxWithArgument("./test/closure/close_over_method_parameter.lox");
            var count = 0;
            Assert.Equal("param\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_NestedClosure")]
        public void Closure_NestedClosure()
        {
            var result = RunLoxWithArgument("./test/closure/nested_closure.lox");
            var count = 0;
            Assert.Equal("a\r",result[count]);
            count++;
            Assert.Equal("b\r",result[count]);
            count++;
            Assert.Equal("c\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_OpenClosureInFunction")]
        public void Closure_OpenClosureInFunction()
        {
            var result = RunLoxWithArgument("./test/closure/open_closure_in_function.lox");
            var count = 0;
            Assert.Equal("local\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_ReferenceClosureMultipleTimes")]
        public void Closure_ReferenceClosureMultipleTimes()
        {
            var result = RunLoxWithArgument("./test/closure/reference_closure_multiple_times.lox");
            var count = 0;
            Assert.Equal("a\r",result[count]);
            count++;
            Assert.Equal("a\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_ReuseClosureSlot")]
        public void Closure_ReuseClosureSlot()
        {
            var result = RunLoxWithArgument("./test/closure/reuse_closure_slot.lox");
            var count = 0;
            Assert.Equal("a\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Closure_ShadowClosureWithLocal")]
        public void Closure_ShadowClosureWithLocal()
        {
            var result = RunLoxWithArgument("./test/closure/shadow_closure_with_local.lox");
            var count = 0;
            Assert.Equal("closure\r",result[count]);
            count++;
            Assert.Equal("shadow\r",result[count]);
            count++;
            Assert.Equal("closure\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" This is a regression test. There was a bug where the VM would try to close")]
        public void Closure_UnusedClosure()
        {
            var result = RunLoxWithArgument("./test/closure/unused_closure.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" This is a regression test. When closing upvalues for discarded locals, it")]
        public void Closure_UnusedLaterClosure()
        {
            var result = RunLoxWithArgument("./test/closure/unused_later_closure.lox");
            var count = 0;
            Assert.Equal("a\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Comments_LineAtEof")]
        public void Comments_LineAtEof()
        {
            var result = RunLoxWithArgument("./test/comments/line_at_eof.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" comment")]
        public void Comments_OnlyLineComment()
        {
            var result = RunLoxWithArgument("./test/comments/only_line_comment.lox");
            var count = 0;
        }
        [Fact(DisplayName=" comment")]
        public void Comments_OnlyLineCommentAndLine()
        {
            var result = RunLoxWithArgument("./test/comments/only_line_comment_and_line.lox");
            var count = 0;
        }
        [Fact(DisplayName=" Unicode characters are allowed in comments.")]
        public void Comments_Unicode()
        {
            var result = RunLoxWithArgument("./test/comments/unicode.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_Arguments")]
        public void Constructor_Arguments()
        {
            var result = RunLoxWithArgument("./test/constructor/arguments.lox");
            var count = 0;
            Assert.Equal("init\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_CallInitEarlyReturn")]
        public void Constructor_CallInitEarlyReturn()
        {
            var result = RunLoxWithArgument("./test/constructor/call_init_early_return.lox");
            var count = 0;
            Assert.Equal("init\r",result[count]);
            count++;
            Assert.Equal("init\r",result[count]);
            count++;
            Assert.Equal("Foo instance\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_CallInitExplicitly")]
        public void Constructor_CallInitExplicitly()
        {
            var result = RunLoxWithArgument("./test/constructor/call_init_explicitly.lox");
            var count = 0;
            Assert.Equal("Foo.init(one)\r",result[count]);
            count++;
            Assert.Equal("Foo.init(two)\r",result[count]);
            count++;
            Assert.Equal("Foo instance\r",result[count]);
            count++;
            Assert.Equal("init\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_Default")]
        public void Constructor_Default()
        {
            var result = RunLoxWithArgument("./test/constructor/default.lox");
            var count = 0;
            Assert.Equal("Foo instance\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_DefaultArguments")]
        public void Constructor_DefaultArguments()
        {
            var result = RunLoxWithArgument("./test/constructor/default_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Constructor_EarlyReturn")]
        public void Constructor_EarlyReturn()
        {
            var result = RunLoxWithArgument("./test/constructor/early_return.lox");
            var count = 0;
            Assert.Equal("init\r",result[count]);
            count++;
            Assert.Equal("Foo instance\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_ExtraArguments")]
        public void Constructor_ExtraArguments()
        {
            var result = RunLoxWithArgument("./test/constructor/extra_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Constructor_InitNotMethod")]
        public void Constructor_InitNotMethod()
        {
            var result = RunLoxWithArgument("./test/constructor/init_not_method.lox");
            var count = 0;
            Assert.Equal("not initializer\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_MissingArguments")]
        public void Constructor_MissingArguments()
        {
            var result = RunLoxWithArgument("./test/constructor/missing_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Constructor_ReturnInNestedFunction")]
        public void Constructor_ReturnInNestedFunction()
        {
            var result = RunLoxWithArgument("./test/constructor/return_in_nested_function.lox");
            var count = 0;
            Assert.Equal("bar\r",result[count]);
            count++;
            Assert.Equal("Foo instance\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Constructor_ReturnValue")]
        public void Constructor_ReturnValue()
        {
            var result = RunLoxWithArgument("./test/constructor/return_value.lox");
            var count = 0;
        }
        [Fact(DisplayName=" Note: This is just for the expression evaluating chapter which evaluates an")]
        public void Expressions_Evaluate()
        {
            var result = RunLoxWithArgument("./test/expressions/evaluate.lox");
            var count = 0;
            Assert.Equal("2\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" Note: This is just for the expression parsing chapter which prints the AST.")]
        public void Expressions_Parse()
        {
            var result = RunLoxWithArgument("./test/expressions/parse.lox");
            var count = 0;
            Assert.Equal("(+ (group (- 5.0 (group (- 3.0 1.0)))) (- 1.0))\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Field_CallFunctionField")]
        public void Field_CallFunctionField()
        {
            var result = RunLoxWithArgument("./test/field/call_function_field.lox");
            var count = 0;
            Assert.Equal("bar\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Field_CallNonfunctionField")]
        public void Field_CallNonfunctionField()
        {
            var result = RunLoxWithArgument("./test/field/call_nonfunction_field.lox");
            var count = 0;
        }
        [Fact(DisplayName=" Bound methods have identity equality.")]
        public void Field_GetAndSetMethod()
        {
            var result = RunLoxWithArgument("./test/field/get_and_set_method.lox");
            var count = 0;
            Assert.Equal("other\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("method\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Field_GetOnBool")]
        public void Field_GetOnBool()
        {
            var result = RunLoxWithArgument("./test/field/get_on_bool.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_GetOnClass")]
        public void Field_GetOnClass()
        {
            var result = RunLoxWithArgument("./test/field/get_on_class.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_GetOnFunction")]
        public void Field_GetOnFunction()
        {
            var result = RunLoxWithArgument("./test/field/get_on_function.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_GetOnNil")]
        public void Field_GetOnNil()
        {
            var result = RunLoxWithArgument("./test/field/get_on_nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_GetOnNum")]
        public void Field_GetOnNum()
        {
            var result = RunLoxWithArgument("./test/field/get_on_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_GetOnString")]
        public void Field_GetOnString()
        {
            var result = RunLoxWithArgument("./test/field/get_on_string.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_Many")]
        public void Field_Many()
        {
            var result = RunLoxWithArgument("./test/field/many.lox");
            var count = 0;
            Assert.Equal("apple\r",result[count]);
            count++;
            Assert.Equal("apricot\r",result[count]);
            count++;
            Assert.Equal("avocado\r",result[count]);
            count++;
            Assert.Equal("banana\r",result[count]);
            count++;
            Assert.Equal("bilberry\r",result[count]);
            count++;
            Assert.Equal("blackberry\r",result[count]);
            count++;
            Assert.Equal("blackcurrant\r",result[count]);
            count++;
            Assert.Equal("blueberry\r",result[count]);
            count++;
            Assert.Equal("boysenberry\r",result[count]);
            count++;
            Assert.Equal("cantaloupe\r",result[count]);
            count++;
            Assert.Equal("cherimoya\r",result[count]);
            count++;
            Assert.Equal("cherry\r",result[count]);
            count++;
            Assert.Equal("clementine\r",result[count]);
            count++;
            Assert.Equal("cloudberry\r",result[count]);
            count++;
            Assert.Equal("coconut\r",result[count]);
            count++;
            Assert.Equal("cranberry\r",result[count]);
            count++;
            Assert.Equal("currant\r",result[count]);
            count++;
            Assert.Equal("damson\r",result[count]);
            count++;
            Assert.Equal("date\r",result[count]);
            count++;
            Assert.Equal("dragonfruit\r",result[count]);
            count++;
            Assert.Equal("durian\r",result[count]);
            count++;
            Assert.Equal("elderberry\r",result[count]);
            count++;
            Assert.Equal("feijoa\r",result[count]);
            count++;
            Assert.Equal("fig\r",result[count]);
            count++;
            Assert.Equal("gooseberry\r",result[count]);
            count++;
            Assert.Equal("grape\r",result[count]);
            count++;
            Assert.Equal("grapefruit\r",result[count]);
            count++;
            Assert.Equal("guava\r",result[count]);
            count++;
            Assert.Equal("honeydew\r",result[count]);
            count++;
            Assert.Equal("huckleberry\r",result[count]);
            count++;
            Assert.Equal("jabuticaba\r",result[count]);
            count++;
            Assert.Equal("jackfruit\r",result[count]);
            count++;
            Assert.Equal("jambul\r",result[count]);
            count++;
            Assert.Equal("jujube\r",result[count]);
            count++;
            Assert.Equal("juniper\r",result[count]);
            count++;
            Assert.Equal("kiwifruit\r",result[count]);
            count++;
            Assert.Equal("kumquat\r",result[count]);
            count++;
            Assert.Equal("lemon\r",result[count]);
            count++;
            Assert.Equal("lime\r",result[count]);
            count++;
            Assert.Equal("longan\r",result[count]);
            count++;
            Assert.Equal("loquat\r",result[count]);
            count++;
            Assert.Equal("lychee\r",result[count]);
            count++;
            Assert.Equal("mandarine\r",result[count]);
            count++;
            Assert.Equal("mango\r",result[count]);
            count++;
            Assert.Equal("marionberry\r",result[count]);
            count++;
            Assert.Equal("melon\r",result[count]);
            count++;
            Assert.Equal("miracle\r",result[count]);
            count++;
            Assert.Equal("mulberry\r",result[count]);
            count++;
            Assert.Equal("nance\r",result[count]);
            count++;
            Assert.Equal("nectarine\r",result[count]);
            count++;
            Assert.Equal("olive\r",result[count]);
            count++;
            Assert.Equal("orange\r",result[count]);
            count++;
            Assert.Equal("papaya\r",result[count]);
            count++;
            Assert.Equal("passionfruit\r",result[count]);
            count++;
            Assert.Equal("peach\r",result[count]);
            count++;
            Assert.Equal("pear\r",result[count]);
            count++;
            Assert.Equal("persimmon\r",result[count]);
            count++;
            Assert.Equal("physalis\r",result[count]);
            count++;
            Assert.Equal("pineapple\r",result[count]);
            count++;
            Assert.Equal("plantain\r",result[count]);
            count++;
            Assert.Equal("plum\r",result[count]);
            count++;
            Assert.Equal("plumcot\r",result[count]);
            count++;
            Assert.Equal("pomegranate\r",result[count]);
            count++;
            Assert.Equal("pomelo\r",result[count]);
            count++;
            Assert.Equal("quince\r",result[count]);
            count++;
            Assert.Equal("raisin\r",result[count]);
            count++;
            Assert.Equal("rambutan\r",result[count]);
            count++;
            Assert.Equal("raspberry\r",result[count]);
            count++;
            Assert.Equal("redcurrant\r",result[count]);
            count++;
            Assert.Equal("salak\r",result[count]);
            count++;
            Assert.Equal("salmonberry\r",result[count]);
            count++;
            Assert.Equal("satsuma\r",result[count]);
            count++;
            Assert.Equal("strawberry\r",result[count]);
            count++;
            Assert.Equal("tamarillo\r",result[count]);
            count++;
            Assert.Equal("tamarind\r",result[count]);
            count++;
            Assert.Equal("tangerine\r",result[count]);
            count++;
            Assert.Equal("tomato\r",result[count]);
            count++;
            Assert.Equal("watermelon\r",result[count]);
            count++;
            Assert.Equal("yuzu\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Field_Method")]
        public void Field_Method()
        {
            var result = RunLoxWithArgument("./test/field/method.lox");
            var count = 0;
            Assert.Equal("got method\r",result[count]);
            count++;
            Assert.Equal("arg\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Field_MethodBindsThis")]
        public void Field_MethodBindsThis()
        {
            var result = RunLoxWithArgument("./test/field/method_binds_this.lox");
            var count = 0;
            Assert.Equal("foo1\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Field_OnInstance")]
        public void Field_OnInstance()
        {
            var result = RunLoxWithArgument("./test/field/on_instance.lox");
            var count = 0;
            Assert.Equal("bar value\r",result[count]);
            count++;
            Assert.Equal("baz value\r",result[count]);
            count++;
            Assert.Equal("bar value\r",result[count]);
            count++;
            Assert.Equal("baz value\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Field_SetEvaluationOrder")]
        public void Field_SetEvaluationOrder()
        {
            var result = RunLoxWithArgument("./test/field/set_evaluation_order.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_SetOnBool")]
        public void Field_SetOnBool()
        {
            var result = RunLoxWithArgument("./test/field/set_on_bool.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_SetOnClass")]
        public void Field_SetOnClass()
        {
            var result = RunLoxWithArgument("./test/field/set_on_class.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_SetOnFunction")]
        public void Field_SetOnFunction()
        {
            var result = RunLoxWithArgument("./test/field/set_on_function.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_SetOnNil")]
        public void Field_SetOnNil()
        {
            var result = RunLoxWithArgument("./test/field/set_on_nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_SetOnNum")]
        public void Field_SetOnNum()
        {
            var result = RunLoxWithArgument("./test/field/set_on_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_SetOnString")]
        public void Field_SetOnString()
        {
            var result = RunLoxWithArgument("./test/field/set_on_string.lox");
            var count = 0;
        }
        [Fact(DisplayName="Field_Undefined")]
        public void Field_Undefined()
        {
            var result = RunLoxWithArgument("./test/field/undefined.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'class': Expect expression.")]
        public void For_ClassInBody()
        {
            var result = RunLoxWithArgument("./test/for/class_in_body.lox");
            var count = 0;
        }
        [Fact(DisplayName="For_ClosureInBody")]
        public void For_ClosureInBody()
        {
            var result = RunLoxWithArgument("./test/for/closure_in_body.lox");
            var count = 0;
            Assert.Equal("4\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("4\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
            Assert.Equal("4\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at 'fun': Expect expression.")]
        public void For_FunInBody()
        {
            var result = RunLoxWithArgument("./test/for/fun_in_body.lox");
            var count = 0;
        }
        [Fact(DisplayName="For_ReturnClosure")]
        public void For_ReturnClosure()
        {
            var result = RunLoxWithArgument("./test/for/return_closure.lox");
            var count = 0;
            Assert.Equal("i\r",result[count]);
            count++;
        }
        [Fact(DisplayName="For_ReturnInside")]
        public void For_ReturnInside()
        {
            var result = RunLoxWithArgument("./test/for/return_inside.lox");
            var count = 0;
            Assert.Equal("i\r",result[count]);
            count++;
        }
        [Fact(DisplayName="For_Scope")]
        public void For_Scope()
        {
            var result = RunLoxWithArgument("./test/for/scope.lox");
            var count = 0;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("-1\r",result[count]);
            count++;
            Assert.Equal("after\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 3] Error at '{': Expect expression.")]
        public void For_StatementCondition()
        {
            var result = RunLoxWithArgument("./test/for/statement_condition.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at '{': Expect expression.")]
        public void For_StatementIncrement()
        {
            var result = RunLoxWithArgument("./test/for/statement_increment.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 3] Error at '{': Expect expression.")]
        public void For_StatementInitializer()
        {
            var result = RunLoxWithArgument("./test/for/statement_initializer.lox");
            var count = 0;
        }
        [Fact(DisplayName=" Single-expression body.")]
        public void For_Syntax()
        {
            var result = RunLoxWithArgument("./test/for/syntax.lox");
            var count = 0;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
            Assert.Equal("done\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at 'var': Expect expression.")]
        public void For_VarInBody()
        {
            var result = RunLoxWithArgument("./test/for/var_in_body.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 3] Error at '123': Expect '{' before function body.")]
        public void Function_BodyMustBeBlock()
        {
            var result = RunLoxWithArgument("./test/function/body_must_be_block.lox");
            var count = 0;
        }
        [Fact(DisplayName="Function_EmptyBody")]
        public void Function_EmptyBody()
        {
            var result = RunLoxWithArgument("./test/function/empty_body.lox");
            var count = 0;
            Assert.Equal("nil\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Function_ExtraArguments")]
        public void Function_ExtraArguments()
        {
            var result = RunLoxWithArgument("./test/function/extra_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Function_LocalMutualRecursion")]
        public void Function_LocalMutualRecursion()
        {
            var result = RunLoxWithArgument("./test/function/local_mutual_recursion.lox");
            var count = 0;
        }
        [Fact(DisplayName="Function_LocalRecursion")]
        public void Function_LocalRecursion()
        {
            var result = RunLoxWithArgument("./test/function/local_recursion.lox");
            var count = 0;
            Assert.Equal("21\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Function_MissingArguments")]
        public void Function_MissingArguments()
        {
            var result = RunLoxWithArgument("./test/function/missing_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 3] Error at 'c': Expect ')' after parameters.")]
        public void Function_MissingCommaInParameters()
        {
            var result = RunLoxWithArgument("./test/function/missing_comma_in_parameters.lox");
            var count = 0;
        }
        [Fact(DisplayName="Function_MutualRecursion")]
        public void Function_MutualRecursion()
        {
            var result = RunLoxWithArgument("./test/function/mutual_recursion.lox");
            var count = 0;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Function_Parameters")]
        public void Function_Parameters()
        {
            var result = RunLoxWithArgument("./test/function/parameters.lox");
            var count = 0;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
            Assert.Equal("6\r",result[count]);
            count++;
            Assert.Equal("10\r",result[count]);
            count++;
            Assert.Equal("15\r",result[count]);
            count++;
            Assert.Equal("21\r",result[count]);
            count++;
            Assert.Equal("28\r",result[count]);
            count++;
            Assert.Equal("36\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Function_Print")]
        public void Function_Print()
        {
            var result = RunLoxWithArgument("./test/function/print.lox");
            var count = 0;
            Assert.Equal("<fn foo>\r",result[count]);
            count++;
            Assert.Equal("<native fn>\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Function_Recursion")]
        public void Function_Recursion()
        {
            var result = RunLoxWithArgument("./test/function/recursion.lox");
            var count = 0;
            Assert.Equal("21\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Function_TooManyArguments")]
        public void Function_TooManyArguments()
        {
            var result = RunLoxWithArgument("./test/function/too_many_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName=" 256 parameters.")]
        public void Function_TooManyParameters()
        {
            var result = RunLoxWithArgument("./test/function/too_many_parameters.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'class': Expect expression.")]
        public void If_ClassInElse()
        {
            var result = RunLoxWithArgument("./test/if/class_in_else.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'class': Expect expression.")]
        public void If_ClassInThen()
        {
            var result = RunLoxWithArgument("./test/if/class_in_then.lox");
            var count = 0;
        }
        [Fact(DisplayName=" A dangling else binds to the right-most if.")]
        public void If_DanglingElse()
        {
            var result = RunLoxWithArgument("./test/if/dangling_else.lox");
            var count = 0;
            Assert.Equal("good\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" Evaluate the 'else' expression if the condition is false.")]
        public void If_Else()
        {
            var result = RunLoxWithArgument("./test/if/else.lox");
            var count = 0;
            Assert.Equal("good\r",result[count]);
            count++;
            Assert.Equal("good\r",result[count]);
            count++;
            Assert.Equal("block\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at 'fun': Expect expression.")]
        public void If_FunInElse()
        {
            var result = RunLoxWithArgument("./test/if/fun_in_else.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'fun': Expect expression.")]
        public void If_FunInThen()
        {
            var result = RunLoxWithArgument("./test/if/fun_in_then.lox");
            var count = 0;
        }
        [Fact(DisplayName=" Evaluate the 'then' expression if the condition is true.")]
        public void If_If()
        {
            var result = RunLoxWithArgument("./test/if/if.lox");
            var count = 0;
            Assert.Equal("good\r",result[count]);
            count++;
            Assert.Equal("block\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" False and nil are false.")]
        public void If_Truth()
        {
            var result = RunLoxWithArgument("./test/if/truth.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("nil\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("empty\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at 'var': Expect expression.")]
        public void If_VarInElse()
        {
            var result = RunLoxWithArgument("./test/if/var_in_else.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'var': Expect expression.")]
        public void If_VarInThen()
        {
            var result = RunLoxWithArgument("./test/if/var_in_then.lox");
            var count = 0;
        }
        [Fact(DisplayName="Inheritance_Constructor")]
        public void Inheritance_Constructor()
        {
            var result = RunLoxWithArgument("./test/inheritance/constructor.lox");
            var count = 0;
            Assert.Equal("value\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Inheritance_InheritFromFunction")]
        public void Inheritance_InheritFromFunction()
        {
            var result = RunLoxWithArgument("./test/inheritance/inherit_from_function.lox");
            var count = 0;
        }
        [Fact(DisplayName="Inheritance_InheritFromNil")]
        public void Inheritance_InheritFromNil()
        {
            var result = RunLoxWithArgument("./test/inheritance/inherit_from_nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Inheritance_InheritFromNumber")]
        public void Inheritance_InheritFromNumber()
        {
            var result = RunLoxWithArgument("./test/inheritance/inherit_from_number.lox");
            var count = 0;
        }
        [Fact(DisplayName="Inheritance_InheritMethods")]
        public void Inheritance_InheritMethods()
        {
            var result = RunLoxWithArgument("./test/inheritance/inherit_methods.lox");
            var count = 0;
            Assert.Equal("foo\r",result[count]);
            count++;
            Assert.Equal("bar\r",result[count]);
            count++;
            Assert.Equal("bar\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Inheritance_ParenthesizedSuperclass")]
        public void Inheritance_ParenthesizedSuperclass()
        {
            var result = RunLoxWithArgument("./test/inheritance/parenthesized_superclass.lox");
            var count = 0;
        }
        [Fact(DisplayName="Inheritance_SetFieldsFromBaseClass")]
        public void Inheritance_SetFieldsFromBaseClass()
        {
            var result = RunLoxWithArgument("./test/inheritance/set_fields_from_base_class.lox");
            var count = 0;
            Assert.Equal("foo 1\r",result[count]);
            count++;
            Assert.Equal("foo 2\r",result[count]);
            count++;
            Assert.Equal("bar 1\r",result[count]);
            count++;
            Assert.Equal("bar 2\r",result[count]);
            count++;
            Assert.Equal("bar 1\r",result[count]);
            count++;
            Assert.Equal("bar 2\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Limit_LoopTooLarge")]
        public void Limit_LoopTooLarge()
        {
            var result = RunLoxWithArgument("./test/limit/loop_too_large.lox");
            var count = 0;
        }
        [Fact(DisplayName="Limit_NoReuseConstants")]
        public void Limit_NoReuseConstants()
        {
            var result = RunLoxWithArgument("./test/limit/no_reuse_constants.lox");
            var count = 0;
        }
        [Fact(DisplayName="Limit_StackOverflow")]
        public void Limit_StackOverflow()
        {
            var result = RunLoxWithArgument("./test/limit/stack_overflow.lox");
            var count = 0;
        }
        [Fact(DisplayName="Limit_TooManyConstants")]
        public void Limit_TooManyConstants()
        {
            var result = RunLoxWithArgument("./test/limit/too_many_constants.lox");
            var count = 0;
        }
        [Fact(DisplayName="Limit_TooManyLocals")]
        public void Limit_TooManyLocals()
        {
            var result = RunLoxWithArgument("./test/limit/too_many_locals.lox");
            var count = 0;
        }
        [Fact(DisplayName="Limit_TooManyUpvalues")]
        public void Limit_TooManyUpvalues()
        {
            var result = RunLoxWithArgument("./test/limit/too_many_upvalues.lox");
            var count = 0;
        }
        [Fact(DisplayName=" Note: These tests implicitly depend on ints being truthy.")]
        public void LogicalOperator_And()
        {
            var result = RunLoxWithArgument("./test/logical_operator/and.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" False and nil are false.")]
        public void LogicalOperator_AndTruth()
        {
            var result = RunLoxWithArgument("./test/logical_operator/and_truth.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("nil\r",result[count]);
            count++;
            Assert.Equal("ok\r",result[count]);
            count++;
            Assert.Equal("ok\r",result[count]);
            count++;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" Note: These tests implicitly depend on ints being truthy.")]
        public void LogicalOperator_Or()
        {
            var result = RunLoxWithArgument("./test/logical_operator/or.lox");
            var count = 0;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" False and nil are false.")]
        public void LogicalOperator_OrTruth()
        {
            var result = RunLoxWithArgument("./test/logical_operator/or_truth.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
            Assert.Equal("ok\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("s\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Method_Arity")]
        public void Method_Arity()
        {
            var result = RunLoxWithArgument("./test/method/arity.lox");
            var count = 0;
            Assert.Equal("no args\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
            Assert.Equal("6\r",result[count]);
            count++;
            Assert.Equal("10\r",result[count]);
            count++;
            Assert.Equal("15\r",result[count]);
            count++;
            Assert.Equal("21\r",result[count]);
            count++;
            Assert.Equal("28\r",result[count]);
            count++;
            Assert.Equal("36\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Method_EmptyBlock")]
        public void Method_EmptyBlock()
        {
            var result = RunLoxWithArgument("./test/method/empty_block.lox");
            var count = 0;
            Assert.Equal("nil\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Method_ExtraArguments")]
        public void Method_ExtraArguments()
        {
            var result = RunLoxWithArgument("./test/method/extra_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Method_MissingArguments")]
        public void Method_MissingArguments()
        {
            var result = RunLoxWithArgument("./test/method/missing_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Method_NotFound")]
        public void Method_NotFound()
        {
            var result = RunLoxWithArgument("./test/method/not_found.lox");
            var count = 0;
        }
        [Fact(DisplayName="Method_PrintBoundMethod")]
        public void Method_PrintBoundMethod()
        {
            var result = RunLoxWithArgument("./test/method/print_bound_method.lox");
            var count = 0;
            Assert.Equal("<fn method>\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Method_ReferToName")]
        public void Method_ReferToName()
        {
            var result = RunLoxWithArgument("./test/method/refer_to_name.lox");
            var count = 0;
        }
        [Fact(DisplayName="Method_TooManyArguments")]
        public void Method_TooManyArguments()
        {
            var result = RunLoxWithArgument("./test/method/too_many_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Method_TooManyParameters")]
        public void Method_TooManyParameters()
        {
            var result = RunLoxWithArgument("./test/method/too_many_parameters.lox");
            var count = 0;
        }
        [Fact(DisplayName="Nil_Literal")]
        public void Nil_Literal()
        {
            var result = RunLoxWithArgument("./test/nil/literal.lox");
            var count = 0;
            Assert.Equal("nil\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at end: Expect property name after '.'.")]
        public void Number_DecimalPointAtEof()
        {
            var result = RunLoxWithArgument("./test/number/decimal_point_at_eof.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at '.': Expect expression.")]
        public void Number_LeadingDot()
        {
            var result = RunLoxWithArgument("./test/number/leading_dot.lox");
            var count = 0;
        }
        [Fact(DisplayName="Number_Literals")]
        public void Number_Literals()
        {
            var result = RunLoxWithArgument("./test/number/literals.lox");
            var count = 0;
            Assert.Equal("123\r",result[count]);
            count++;
            Assert.Equal("987654\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("-0\r",result[count]);
            count++;
            Assert.Equal("123.456\r",result[count]);
            count++;
            Assert.Equal("-0.001\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Number_NanEquality")]
        public void Number_NanEquality()
        {
            var result = RunLoxWithArgument("./test/number/nan_equality.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at ';': Expect property name after '.'.")]
        public void Number_TrailingDot()
        {
            var result = RunLoxWithArgument("./test/number/trailing_dot.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_Add")]
        public void Operator_Add()
        {
            var result = RunLoxWithArgument("./test/operator/add.lox");
            var count = 0;
            Assert.Equal("579\r",result[count]);
            count++;
            Assert.Equal("string\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_AddBoolNil")]
        public void Operator_AddBoolNil()
        {
            var result = RunLoxWithArgument("./test/operator/add_bool_nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_AddBoolNum")]
        public void Operator_AddBoolNum()
        {
            var result = RunLoxWithArgument("./test/operator/add_bool_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_AddBoolString")]
        public void Operator_AddBoolString()
        {
            var result = RunLoxWithArgument("./test/operator/add_bool_string.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_AddNilNil")]
        public void Operator_AddNilNil()
        {
            var result = RunLoxWithArgument("./test/operator/add_nil_nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_AddNumNil")]
        public void Operator_AddNumNil()
        {
            var result = RunLoxWithArgument("./test/operator/add_num_nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_AddStringNil")]
        public void Operator_AddStringNil()
        {
            var result = RunLoxWithArgument("./test/operator/add_string_nil.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_Comparison")]
        public void Operator_Comparison()
        {
            var result = RunLoxWithArgument("./test/operator/comparison.lox");
            var count = 0;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_Divide")]
        public void Operator_Divide()
        {
            var result = RunLoxWithArgument("./test/operator/divide.lox");
            var count = 0;
            Assert.Equal("4\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_DivideNonnumNum")]
        public void Operator_DivideNonnumNum()
        {
            var result = RunLoxWithArgument("./test/operator/divide_nonnum_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_DivideNumNonnum")]
        public void Operator_DivideNumNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/divide_num_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_Equals")]
        public void Operator_Equals()
        {
            var result = RunLoxWithArgument("./test/operator/equals.lox");
            var count = 0;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" Bound methods have identity equality.")]
        public void Operator_EqualsClass()
        {
            var result = RunLoxWithArgument("./test/operator/equals_class.lox");
            var count = 0;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" Bound methods have identity equality.")]
        public void Operator_EqualsMethod()
        {
            var result = RunLoxWithArgument("./test/operator/equals_method.lox");
            var count = 0;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_GreaterNonnumNum")]
        public void Operator_GreaterNonnumNum()
        {
            var result = RunLoxWithArgument("./test/operator/greater_nonnum_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_GreaterNumNonnum")]
        public void Operator_GreaterNumNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/greater_num_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_GreaterOrEqualNonnumNum")]
        public void Operator_GreaterOrEqualNonnumNum()
        {
            var result = RunLoxWithArgument("./test/operator/greater_or_equal_nonnum_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_GreaterOrEqualNumNonnum")]
        public void Operator_GreaterOrEqualNumNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/greater_or_equal_num_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_LessNonnumNum")]
        public void Operator_LessNonnumNum()
        {
            var result = RunLoxWithArgument("./test/operator/less_nonnum_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_LessNumNonnum")]
        public void Operator_LessNumNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/less_num_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_LessOrEqualNonnumNum")]
        public void Operator_LessOrEqualNonnumNum()
        {
            var result = RunLoxWithArgument("./test/operator/less_or_equal_nonnum_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_LessOrEqualNumNonnum")]
        public void Operator_LessOrEqualNumNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/less_or_equal_num_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_Multiply")]
        public void Operator_Multiply()
        {
            var result = RunLoxWithArgument("./test/operator/multiply.lox");
            var count = 0;
            Assert.Equal("15\r",result[count]);
            count++;
            Assert.Equal("3.702\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_MultiplyNonnumNum")]
        public void Operator_MultiplyNonnumNum()
        {
            var result = RunLoxWithArgument("./test/operator/multiply_nonnum_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_MultiplyNumNonnum")]
        public void Operator_MultiplyNumNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/multiply_num_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_Negate")]
        public void Operator_Negate()
        {
            var result = RunLoxWithArgument("./test/operator/negate.lox");
            var count = 0;
            Assert.Equal("-3\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
            Assert.Equal("-3\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_NegateNonnum")]
        public void Operator_NegateNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/negate_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_Not")]
        public void Operator_Not()
        {
            var result = RunLoxWithArgument("./test/operator/not.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_NotClass")]
        public void Operator_NotClass()
        {
            var result = RunLoxWithArgument("./test/operator/not_class.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_NotEquals")]
        public void Operator_NotEquals()
        {
            var result = RunLoxWithArgument("./test/operator/not_equals.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("false\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
            Assert.Equal("true\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_Subtract")]
        public void Operator_Subtract()
        {
            var result = RunLoxWithArgument("./test/operator/subtract.lox");
            var count = 0;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Operator_SubtractNonnumNum")]
        public void Operator_SubtractNonnumNum()
        {
            var result = RunLoxWithArgument("./test/operator/subtract_nonnum_num.lox");
            var count = 0;
        }
        [Fact(DisplayName="Operator_SubtractNumNonnum")]
        public void Operator_SubtractNumNonnum()
        {
            var result = RunLoxWithArgument("./test/operator/subtract_num_nonnum.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at ';': Expect expression.")]
        public void Print_MissingArgument()
        {
            var result = RunLoxWithArgument("./test/print/missing_argument.lox");
            var count = 0;
        }
        [Fact(DisplayName="Regression_394")]
        public void Regression_394()
        {
            var result = RunLoxWithArgument("./test/regression/394.lox");
            var count = 0;
            Assert.Equal("B\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Regression_40")]
        public void Regression_40()
        {
            var result = RunLoxWithArgument("./test/regression/40.lox");
            var count = 0;
            Assert.Equal("false\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Return_AfterElse")]
        public void Return_AfterElse()
        {
            var result = RunLoxWithArgument("./test/return/after_else.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Return_AfterIf")]
        public void Return_AfterIf()
        {
            var result = RunLoxWithArgument("./test/return/after_if.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Return_AfterWhile")]
        public void Return_AfterWhile()
        {
            var result = RunLoxWithArgument("./test/return/after_while.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Return_AtTopLevel")]
        public void Return_AtTopLevel()
        {
            var result = RunLoxWithArgument("./test/return/at_top_level.lox");
            var count = 0;
        }
        [Fact(DisplayName="Return_InFunction")]
        public void Return_InFunction()
        {
            var result = RunLoxWithArgument("./test/return/in_function.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Return_InMethod")]
        public void Return_InMethod()
        {
            var result = RunLoxWithArgument("./test/return/in_method.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Return_ReturnNilIfNoValue")]
        public void Return_ReturnNilIfNoValue()
        {
            var result = RunLoxWithArgument("./test/return/return_nil_if_no_value.lox");
            var count = 0;
            Assert.Equal("nil\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Scanning_Identifiers")]
        public void Scanning_Identifiers()
        {
            var result = RunLoxWithArgument("./test/scanning/identifiers.lox");
            var count = 0;
            Assert.Equal("IDENTIFIER andy null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER formless null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER fo null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER _ null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER _123 null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER _abc null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER ab123 null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_ null\r",result[count]);
            count++;
            Assert.Equal("EOF  null\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Scanning_Keywords")]
        public void Scanning_Keywords()
        {
            var result = RunLoxWithArgument("./test/scanning/keywords.lox");
            var count = 0;
            Assert.Equal("AND and null\r",result[count]);
            count++;
            Assert.Equal("CLASS class null\r",result[count]);
            count++;
            Assert.Equal("ELSE else null\r",result[count]);
            count++;
            Assert.Equal("FALSE false null\r",result[count]);
            count++;
            Assert.Equal("FOR for null\r",result[count]);
            count++;
            Assert.Equal("FUN fun null\r",result[count]);
            count++;
            Assert.Equal("IF if null\r",result[count]);
            count++;
            Assert.Equal("NIL nil null\r",result[count]);
            count++;
            Assert.Equal("OR or null\r",result[count]);
            count++;
            Assert.Equal("RETURN return null\r",result[count]);
            count++;
            Assert.Equal("SUPER super null\r",result[count]);
            count++;
            Assert.Equal("THIS this null\r",result[count]);
            count++;
            Assert.Equal("TRUE true null\r",result[count]);
            count++;
            Assert.Equal("VAR var null\r",result[count]);
            count++;
            Assert.Equal("WHILE while null\r",result[count]);
            count++;
            Assert.Equal("EOF  null\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Scanning_Numbers")]
        public void Scanning_Numbers()
        {
            var result = RunLoxWithArgument("./test/scanning/numbers.lox");
            var count = 0;
            Assert.Equal("NUMBER 123 123.0\r",result[count]);
            count++;
            Assert.Equal("NUMBER 123.456 123.456\r",result[count]);
            count++;
            Assert.Equal("DOT . null\r",result[count]);
            count++;
            Assert.Equal("NUMBER 456 456.0\r",result[count]);
            count++;
            Assert.Equal("NUMBER 123 123.0\r",result[count]);
            count++;
            Assert.Equal("DOT . null\r",result[count]);
            count++;
            Assert.Equal("EOF  null\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Scanning_Punctuators")]
        public void Scanning_Punctuators()
        {
            var result = RunLoxWithArgument("./test/scanning/punctuators.lox");
            var count = 0;
            Assert.Equal("LEFT_PAREN ( null\r",result[count]);
            count++;
            Assert.Equal("RIGHT_PAREN ) null\r",result[count]);
            count++;
            Assert.Equal("LEFT_BRACE { null\r",result[count]);
            count++;
            Assert.Equal("RIGHT_BRACE } null\r",result[count]);
            count++;
            Assert.Equal("SEMICOLON ; null\r",result[count]);
            count++;
            Assert.Equal("COMMA , null\r",result[count]);
            count++;
            Assert.Equal("PLUS + null\r",result[count]);
            count++;
            Assert.Equal("MINUS - null\r",result[count]);
            count++;
            Assert.Equal("STAR * null\r",result[count]);
            count++;
            Assert.Equal("BANG_EQUAL != null\r",result[count]);
            count++;
            Assert.Equal("EQUAL_EQUAL == null\r",result[count]);
            count++;
            Assert.Equal("LESS_EQUAL <= null\r",result[count]);
            count++;
            Assert.Equal("GREATER_EQUAL >= null\r",result[count]);
            count++;
            Assert.Equal("BANG_EQUAL != null\r",result[count]);
            count++;
            Assert.Equal("LESS < null\r",result[count]);
            count++;
            Assert.Equal("GREATER > null\r",result[count]);
            count++;
            Assert.Equal("SLASH / null\r",result[count]);
            count++;
            Assert.Equal("DOT . null\r",result[count]);
            count++;
            Assert.Equal("EOF  null\r",result[count]);
            count++;
        }
        //[Fact(DisplayName="Scanning_Strings")]
        //public void Scanning_Strings()
        //{
        //    var result = RunLoxWithArgument("./test/scanning/strings.lox");
        //    var count = 0;
        //    Assert.Equal("STRING "" \r",result[count]);
        //    count++;
        //    Assert.Equal("STRING "string" string\r",result[count]);
        //    count++;
        //    Assert.Equal("EOF  null\r",result[count]);
        //    count++;
        //}
        [Fact(DisplayName="Scanning_Whitespace")]
        public void Scanning_Whitespace()
        {
            var result = RunLoxWithArgument("./test/scanning/whitespace.lox");
            var count = 0;
            Assert.Equal("IDENTIFIER space null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER tabs null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER newlines null\r",result[count]);
            count++;
            Assert.Equal("IDENTIFIER end null\r",result[count]);
            count++;
            Assert.Equal("EOF  null\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" Tests that we correctly track the line info across multiline strings.")]
        public void String_ErrorAfterMultiline()
        {
            var result = RunLoxWithArgument("./test/string/error_after_multiline.lox");
            var count = 0;
        }
        [Fact(DisplayName="String_Literals")]
        public void String_Literals()
        {
            var result = RunLoxWithArgument("./test/string/literals.lox");
            var count = 0;
            Assert.Equal("()\r",result[count]);
            count++;
            Assert.Equal("a string\r",result[count]);
            count++;
            Assert.Equal("A~¶Þॐஃ\r",result[count]);
            count++;
        }
        [Fact(DisplayName="String_Multiline")]
        public void String_Multiline()
        {
            var result = RunLoxWithArgument("./test/string/multiline.lox");
            var count = 0;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error: Unterminated string.")]
        public void String_Unterminated()
        {
            var result = RunLoxWithArgument("./test/string/unterminated.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_BoundMethod")]
        public void Super_BoundMethod()
        {
            var result = RunLoxWithArgument("./test/super/bound_method.lox");
            var count = 0;
            Assert.Equal("A.method(arg)\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_CallOtherMethod")]
        public void Super_CallOtherMethod()
        {
            var result = RunLoxWithArgument("./test/super/call_other_method.lox");
            var count = 0;
            Assert.Equal("Derived.bar()\r",result[count]);
            count++;
            Assert.Equal("Base.foo()\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_CallSameMethod")]
        public void Super_CallSameMethod()
        {
            var result = RunLoxWithArgument("./test/super/call_same_method.lox");
            var count = 0;
            Assert.Equal("Derived.foo()\r",result[count]);
            count++;
            Assert.Equal("Base.foo()\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_Closure")]
        public void Super_Closure()
        {
            var result = RunLoxWithArgument("./test/super/closure.lox");
            var count = 0;
            Assert.Equal("Base\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_Constructor")]
        public void Super_Constructor()
        {
            var result = RunLoxWithArgument("./test/super/constructor.lox");
            var count = 0;
            Assert.Equal("Derived.init()\r",result[count]);
            count++;
            Assert.Equal("Base.init(a, b)\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_ExtraArguments")]
        public void Super_ExtraArguments()
        {
            var result = RunLoxWithArgument("./test/super/extra_arguments.lox");
            var count = 0;
            Assert.Equal("Derived.foo()\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_IndirectlyInherited")]
        public void Super_IndirectlyInherited()
        {
            var result = RunLoxWithArgument("./test/super/indirectly_inherited.lox");
            var count = 0;
            Assert.Equal("C.foo()\r",result[count]);
            count++;
            Assert.Equal("A.foo()\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_MissingArguments")]
        public void Super_MissingArguments()
        {
            var result = RunLoxWithArgument("./test/super/missing_arguments.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_NoSuperclassBind")]
        public void Super_NoSuperclassBind()
        {
            var result = RunLoxWithArgument("./test/super/no_superclass_bind.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_NoSuperclassCall")]
        public void Super_NoSuperclassCall()
        {
            var result = RunLoxWithArgument("./test/super/no_superclass_call.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_NoSuperclassMethod")]
        public void Super_NoSuperclassMethod()
        {
            var result = RunLoxWithArgument("./test/super/no_superclass_method.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_Parenthesized")]
        public void Super_Parenthesized()
        {
            var result = RunLoxWithArgument("./test/super/parenthesized.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_ReassignSuperclass")]
        public void Super_ReassignSuperclass()
        {
            var result = RunLoxWithArgument("./test/super/reassign_superclass.lox");
            var count = 0;
            Assert.Equal("Base.method()\r",result[count]);
            count++;
            Assert.Equal("Base.method()\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_SuperAtTopLevel")]
        public void Super_SuperAtTopLevel()
        {
            var result = RunLoxWithArgument("./test/super/super_at_top_level.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_SuperInClosureInInheritedMethod")]
        public void Super_SuperInClosureInInheritedMethod()
        {
            var result = RunLoxWithArgument("./test/super/super_in_closure_in_inherited_method.lox");
            var count = 0;
            Assert.Equal("A\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_SuperInInheritedMethod")]
        public void Super_SuperInInheritedMethod()
        {
            var result = RunLoxWithArgument("./test/super/super_in_inherited_method.lox");
            var count = 0;
            Assert.Equal("A\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Super_SuperInTopLevelFunction")]
        public void Super_SuperInTopLevelFunction()
        {
            var result = RunLoxWithArgument("./test/super/super_in_top_level_function.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_SuperWithoutDot")]
        public void Super_SuperWithoutDot()
        {
            var result = RunLoxWithArgument("./test/super/super_without_dot.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_SuperWithoutName")]
        public void Super_SuperWithoutName()
        {
            var result = RunLoxWithArgument("./test/super/super_without_name.lox");
            var count = 0;
        }
        [Fact(DisplayName="Super_ThisInSuperclassMethod")]
        public void Super_ThisInSuperclassMethod()
        {
            var result = RunLoxWithArgument("./test/super/this_in_superclass_method.lox");
            var count = 0;
            Assert.Equal("a\r",result[count]);
            count++;
            Assert.Equal("b\r",result[count]);
            count++;
        }
        [Fact(DisplayName="This_Closure")]
        public void This_Closure()
        {
            var result = RunLoxWithArgument("./test/this/closure.lox");
            var count = 0;
            Assert.Equal("Foo\r",result[count]);
            count++;
        }
        [Fact(DisplayName="This_NestedClass")]
        public void This_NestedClass()
        {
            var result = RunLoxWithArgument("./test/this/nested_class.lox");
            var count = 0;
            Assert.Equal("Outer instance\r",result[count]);
            count++;
            Assert.Equal("Outer instance\r",result[count]);
            count++;
            Assert.Equal("Inner instance\r",result[count]);
            count++;
        }
        [Fact(DisplayName="This_NestedClosure")]
        public void This_NestedClosure()
        {
            var result = RunLoxWithArgument("./test/this/nested_closure.lox");
            var count = 0;
            Assert.Equal("Foo\r",result[count]);
            count++;
        }
        [Fact(DisplayName="This_ThisAtTopLevel")]
        public void This_ThisAtTopLevel()
        {
            var result = RunLoxWithArgument("./test/this/this_at_top_level.lox");
            var count = 0;
        }
        [Fact(DisplayName="This_ThisInMethod")]
        public void This_ThisInMethod()
        {
            var result = RunLoxWithArgument("./test/this/this_in_method.lox");
            var count = 0;
            Assert.Equal("baz\r",result[count]);
            count++;
        }
        [Fact(DisplayName="This_ThisInTopLevelFunction")]
        public void This_ThisInTopLevelFunction()
        {
            var result = RunLoxWithArgument("./test/this/this_in_top_level_function.lox");
            var count = 0;
        }
        [Fact(DisplayName="Variable_CollideWithParameter")]
        public void Variable_CollideWithParameter()
        {
            var result = RunLoxWithArgument("./test/variable/collide_with_parameter.lox");
            var count = 0;
        }
        [Fact(DisplayName="Variable_DuplicateLocal")]
        public void Variable_DuplicateLocal()
        {
            var result = RunLoxWithArgument("./test/variable/duplicate_local.lox");
            var count = 0;
        }
        [Fact(DisplayName="Variable_DuplicateParameter")]
        public void Variable_DuplicateParameter()
        {
            var result = RunLoxWithArgument("./test/variable/duplicate_parameter.lox");
            var count = 0;
        }
        [Fact(DisplayName="Variable_EarlyBound")]
        public void Variable_EarlyBound()
        {
            var result = RunLoxWithArgument("./test/variable/early_bound.lox");
            var count = 0;
            Assert.Equal("outer\r",result[count]);
            count++;
            Assert.Equal("outer\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_InMiddleOfBlock")]
        public void Variable_InMiddleOfBlock()
        {
            var result = RunLoxWithArgument("./test/variable/in_middle_of_block.lox");
            var count = 0;
            Assert.Equal("a\r",result[count]);
            count++;
            Assert.Equal("a b\r",result[count]);
            count++;
            Assert.Equal("a c\r",result[count]);
            count++;
            Assert.Equal("a b d\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_InNestedBlock")]
        public void Variable_InNestedBlock()
        {
            var result = RunLoxWithArgument("./test/variable/in_nested_block.lox");
            var count = 0;
            Assert.Equal("outer\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_LocalFromMethod")]
        public void Variable_LocalFromMethod()
        {
            var result = RunLoxWithArgument("./test/variable/local_from_method.lox");
            var count = 0;
            Assert.Equal("variable\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_RedeclareGlobal")]
        public void Variable_RedeclareGlobal()
        {
            var result = RunLoxWithArgument("./test/variable/redeclare_global.lox");
            var count = 0;
            Assert.Equal("nil\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_RedefineGlobal")]
        public void Variable_RedefineGlobal()
        {
            var result = RunLoxWithArgument("./test/variable/redefine_global.lox");
            var count = 0;
            Assert.Equal("2\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_ScopeReuseInDifferentBlocks")]
        public void Variable_ScopeReuseInDifferentBlocks()
        {
            var result = RunLoxWithArgument("./test/variable/scope_reuse_in_different_blocks.lox");
            var count = 0;
            Assert.Equal("first\r",result[count]);
            count++;
            Assert.Equal("second\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_ShadowAndLocal")]
        public void Variable_ShadowAndLocal()
        {
            var result = RunLoxWithArgument("./test/variable/shadow_and_local.lox");
            var count = 0;
            Assert.Equal("outer\r",result[count]);
            count++;
            Assert.Equal("inner\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_ShadowGlobal")]
        public void Variable_ShadowGlobal()
        {
            var result = RunLoxWithArgument("./test/variable/shadow_global.lox");
            var count = 0;
            Assert.Equal("shadow\r",result[count]);
            count++;
            Assert.Equal("global\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_ShadowLocal")]
        public void Variable_ShadowLocal()
        {
            var result = RunLoxWithArgument("./test/variable/shadow_local.lox");
            var count = 0;
            Assert.Equal("shadow\r",result[count]);
            count++;
            Assert.Equal("local\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_UndefinedGlobal")]
        public void Variable_UndefinedGlobal()
        {
            var result = RunLoxWithArgument("./test/variable/undefined_global.lox");
            var count = 0;
        }
        [Fact(DisplayName="Variable_UndefinedLocal")]
        public void Variable_UndefinedLocal()
        {
            var result = RunLoxWithArgument("./test/variable/undefined_local.lox");
            var count = 0;
        }
        [Fact(DisplayName="Variable_Uninitialized")]
        public void Variable_Uninitialized()
        {
            var result = RunLoxWithArgument("./test/variable/uninitialized.lox");
            var count = 0;
            Assert.Equal("nil\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_UnreachedUndefined")]
        public void Variable_UnreachedUndefined()
        {
            var result = RunLoxWithArgument("./test/variable/unreached_undefined.lox");
            var count = 0;
            Assert.Equal("ok\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at 'false': Expect variable name.")]
        public void Variable_UseFalseAsVar()
        {
            var result = RunLoxWithArgument("./test/variable/use_false_as_var.lox");
            var count = 0;
        }
        [Fact(DisplayName="Variable_UseGlobalInInitializer")]
        public void Variable_UseGlobalInInitializer()
        {
            var result = RunLoxWithArgument("./test/variable/use_global_in_initializer.lox");
            var count = 0;
            Assert.Equal("value\r",result[count]);
            count++;
        }
        [Fact(DisplayName="Variable_UseLocalInInitializer")]
        public void Variable_UseLocalInInitializer()
        {
            var result = RunLoxWithArgument("./test/variable/use_local_in_initializer.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'nil': Expect variable name.")]
        public void Variable_UseNilAsVar()
        {
            var result = RunLoxWithArgument("./test/variable/use_nil_as_var.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'this': Expect variable name.")]
        public void Variable_UseThisAsVar()
        {
            var result = RunLoxWithArgument("./test/variable/use_this_as_var.lox");
            var count = 0;
        }
        [Fact(DisplayName=" [line 2] Error at 'class': Expect expression.")]
        public void While_ClassInBody()
        {
            var result = RunLoxWithArgument("./test/while/class_in_body.lox");
            var count = 0;
        }
        [Fact(DisplayName="While_ClosureInBody")]
        public void While_ClosureInBody()
        {
            var result = RunLoxWithArgument("./test/while/closure_in_body.lox");
            var count = 0;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at 'fun': Expect expression.")]
        public void While_FunInBody()
        {
            var result = RunLoxWithArgument("./test/while/fun_in_body.lox");
            var count = 0;
        }
        [Fact(DisplayName="While_ReturnClosure")]
        public void While_ReturnClosure()
        {
            var result = RunLoxWithArgument("./test/while/return_closure.lox");
            var count = 0;
            Assert.Equal("i\r",result[count]);
            count++;
        }
        [Fact(DisplayName="While_ReturnInside")]
        public void While_ReturnInside()
        {
            var result = RunLoxWithArgument("./test/while/return_inside.lox");
            var count = 0;
            Assert.Equal("i\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" Single-expression body.")]
        public void While_Syntax()
        {
            var result = RunLoxWithArgument("./test/while/syntax.lox");
            var count = 0;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
            Assert.Equal("3\r",result[count]);
            count++;
            Assert.Equal("0\r",result[count]);
            count++;
            Assert.Equal("1\r",result[count]);
            count++;
            Assert.Equal("2\r",result[count]);
            count++;
        }
        [Fact(DisplayName=" [line 2] Error at 'var': Expect expression.")]
        public void While_VarInBody()
        {
            var result = RunLoxWithArgument("./test/while/var_in_body.lox");
            var count = 0;
        }
    }
}
