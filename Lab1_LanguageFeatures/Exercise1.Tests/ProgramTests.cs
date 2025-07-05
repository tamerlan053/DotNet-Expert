
using Guts.Client.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Solution = Guts.Client.Core.TestTools.Solution;

namespace Exercise1.Tests
{
    [ExerciseTestFixture("dotnetExp", "H1", "Exercise01", @"Exercise1\Program.cs")]
    public class ProgramTests
    {
        private string _programClassContent = null!;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _programClassContent = Solution.Current.GetFileContent(@"Exercise1\Program.cs");
        }

        [SetUp]
        public void BeforeEachTest()
        {
            var consoleReader = new StringReader(" ");
            Console.SetIn(consoleReader);
        }

        [MonitoredTest]
        public void _01_LengthOfTheCode_ShouldBeShort()
        {
            int maximumCharacterCount = 700;
            Assert.That(_programClassContent.Length, Is.LessThanOrEqualTo(maximumCharacterCount),
                $"The main method is too long. It could be done in {maximumCharacterCount} or less.");
        }

        [MonitoredTest]
        public void _02_Main_ShouldRunBalloonProgram()
        {
            string mainBody = GetMethodBodyWithoutComments(nameof(Program.Main));
            Assert.That(mainBody, Contains.Substring("new BalloonProgram"), "An instance of BalloonProgram should be created.");
            Assert.That(mainBody, Contains.Substring(".Run();"), "The 'Run' method of the balloon program should be called.");
        }

        [MonitoredTest]
        public void _03_Main_ShouldWriteToDebugAndConsole()
        {
            //Listen to console
            var consoleReader = new StringReader(" ");
            var consoleWriter = new StringWriter();
            Console.SetOut(consoleWriter);
            Console.SetIn(consoleReader);

            //Listen to debug
            var debugWriter = new StringWriter();
            var debugListener = new TextWriterTraceListener(debugWriter);
            Trace.Listeners.Add(debugListener);

            //execute the program
            Program.Main([]);

            string consoleOutput = consoleWriter.ToString();
            string debugOutput = debugWriter.ToString();

            Assert.That(consoleOutput, Is.Not.Empty, "Nothing has been written to the console.");
            Assert.That(debugOutput, Is.Not.Empty, "Nothing has been written to the debug window.");

            string[] consoleLines = consoleOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Assert.That(consoleLines.Count(line => line.ToLower().Contains("balloon")), Is.GreaterThanOrEqualTo(5),
                "At least 5 lines, written to the console, should contain the text 'balloon'.");

            Assert.That(consoleOutput.ToUpper().Contains(debugOutput), Is.True,
                "The debug output should be the same as the console output with all text upper case.");
        }

        private string GetMethodBodyWithoutComments(string methodName)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(_programClassContent);
            SyntaxNode root = syntaxTree.GetRoot();
            MethodDeclarationSyntax? method = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals(methodName));
            Assert.That(method, Is.Not.Null,
                $"Could not find the '{methodName}' method. You may have accidentally deleted or renamed it?");
            Assert.That(method!.Body, Is.Not.Null,
                $"Could not find a body for the '{methodName}' method.");

            //filter out comments
            IEnumerable<StatementSyntax> realStatements = method!.Body!.Statements.Where(s =>
                s.Kind() != SyntaxKind.SingleLineCommentTrivia && s.Kind() != SyntaxKind.MultiLineCommentTrivia);

            var builder = new StringBuilder();
            foreach (StatementSyntax statement in realStatements)
            {
                builder.AppendLine(statement.ToString());
            }
            return builder.ToString();
        }

    }
}