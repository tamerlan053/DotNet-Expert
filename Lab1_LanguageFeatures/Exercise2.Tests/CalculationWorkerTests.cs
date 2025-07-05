using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H1", "Exercise02", @"Exercise2\CalculationWorker.cs")]
    public class CalculationWorkerTests
    {
        private CalculationWorker _worker = null!;
        private List<CalculationEventArgs> _receivedEventArgs = [];
        private readonly string _calculationWorkerClassContent;

        public CalculationWorkerTests()
        {
            _calculationWorkerClassContent = Solution.Current.GetFileContent(@"Exercise2\CalculationWorker.cs");
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _worker = new CalculationWorker();
            _receivedEventArgs = [];
        }

        [MonitoredTest]
        public void ShouldHaveACalculationCompletedEvent()
        {
            EventHelper.AssertAndRetrieveEventInfo();
        }

        [MonitoredTest]
        public void CalculationCompleteHandler_CalculationEventArgs_ShouldNotHaveBeenChanged()
        {
            string calculationCompleteHandlerContentHash = Solution.Current.GetFileHash(@"Exercise2\CalculationCompleteHandler.cs");
            Assert.That(calculationCompleteHandlerContentHash, Is.EqualTo("32-69-9A-7B-CD-E5-FF-81-1B-7C-68-97-48-0E-AE-0F"));

            string calculationEventArgsContentHash = Solution.Current.GetFileHash(@"Exercise2\CalculationEventArgs.cs");
            Assert.That(calculationEventArgsContentHash, Is.EqualTo("AB-6D-8E-73-E0-6C-B8-B4-57-97-76-61-98-A2-35-37"));
        }

        [MonitoredTest]
        public void DoWorkAsync_ShouldUse_Await_Run_WhenAny()
        {
            string methodBody = GetMethodBodyWithoutComments(nameof(CalculationWorker.DoWorkAsync));
            Assert.That(methodBody, Contains.Substring("Task.Run("),
                "A task should be created for each input using 'Task.Run'");
            Assert.That(methodBody, Contains.Substring("Task.WhenAny("),
                "The 'Task.WhenAny' method should be used to wait for any task to complete.");
            Assert.That(methodBody, Contains.Substring("await "),
                "The 'await' keyword should be used when waiting for a task to complete.");
            Assert.That(methodBody, Contains.Substring("while ("),
                "A while loop should be used to wait for all tasks to complete.");
        }

        [MonitoredTest]
        public void DoWorkAsync_ShouldInvokeTheCalculationCompletedEventAfterEachCalculation()
        {
            //Arrange
            EventInfo eventInfo = EventHelper.AssertAndRetrieveEventInfo();
            eventInfo.AddEventHandler(_worker, new CalculationCompleteHandler(CalculationCompleted));

            var inputs = new List<int>();
            var expectedOutputs = new List<Calculation>();
            var expectedProgresses = new List<double>();
            int numberOfInputs = Random.Shared.Next(2, 11);

            double progressStep = 1.0 / numberOfInputs;
            double progress = 0.0;
            for (int i = 0; i < numberOfInputs; i++)
            {
                int input = Random.Shared.Next(2, 1001);
                inputs.Add(input);
                expectedOutputs.Add(new Calculation(input, input + 1));
                progress += progressStep;
                expectedProgresses.Add(progress);
            }

            //Act
            _worker.DoWorkAsync(inputs.ToArray(), n => new Calculation(n, n + 1)).Wait();

            //Assert
            Assert.That(_receivedEventArgs.Count, Is.EqualTo(inputs.Count), "The event was not triggered as many times as expected.");

            int progressIndex = 0;
            foreach (CalculationEventArgs args in _receivedEventArgs)
            {
                Calculation? matchingExpectedOutput = expectedOutputs.FirstOrDefault(o => o.Input == args.Calculation.Input);
                Assert.That(matchingExpectedOutput, Is.Not.Null, $"A calculation with input '{args.Calculation.Input}' is unexpected.");
                Assert.That(args.Calculation.Result, Is.EqualTo(matchingExpectedOutput!.Result), $"The result for input '{args.Calculation.Input}' is unexpected.");

                double actualProgress = args.ProgressPercentage;
                double expectedProgress = expectedProgresses[progressIndex];
                Assert.That(actualProgress, Is.EqualTo(expectedProgress).Within(0.001), $"Unexpected progress percentage for the {progressIndex + 1}th completed calculation");
                progressIndex++;
            }
        }

        [MonitoredTest]
        public void DoWorkAsync_NoSubscribersOnTheCalculationCompletedEvent_ShouldNotInvokeTheEvent()
        {
            EventHelper.AssertAndRetrieveEventInfo();

            //Act
            _worker.DoWorkAsync(new[] { 1, 2, 3 }, n => new Calculation(n, n * 2)).Wait();

            //Assert
            Assert.That(_receivedEventArgs.Count, Is.Zero);
        }

        private void CalculationCompleted(object sender, CalculationEventArgs args)
        {
            Assert.That(sender, Is.SameAs(_worker), "The sender should be the instance that invoked the event.");
            _receivedEventArgs.Add(args);
        }

        private string GetMethodBodyWithoutComments(string methodName)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(_calculationWorkerClassContent);
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