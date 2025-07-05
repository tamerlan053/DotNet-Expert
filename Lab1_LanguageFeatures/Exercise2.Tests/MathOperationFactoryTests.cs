﻿using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H1", "Exercise02", @"Exercise2\MathOperationFactory.cs")]
    public class MathOperationFactoryTests
    {
        private MathOperationFactory _factory = null!;
        private string _factoryClassContent = null!;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _factoryClassContent = Solution.Current.GetFileContent(@"Exercise2\MathOperationFactory.cs");
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _factory = new MathOperationFactory();
        }

        [MonitoredTest]
        public void IMathOperationFactory_ShouldNotHaveBeenChanged()
        {
            string interfaceContentHash = Solution.Current.GetFileHash(@"Exercise2\IMathOperationFactory.cs");
            Assert.That(interfaceContentHash, Is.EqualTo("96-B0-27-CC-D6-29-6D-17-2B-0B-1B-C2-35-6E-B4-2B"));
        }

        [MonitoredTest]
        public void CreateCubicOperation_ShouldUseALambdaExpression()
        {
            SyntaxNode? body = GetMethodBody(nameof(MathOperationFactory.CreateCubicOperation));
            LambdaExpressionSyntax? lambdaSyntax = body?.DescendantNodes().OfType<LambdaExpressionSyntax>().FirstOrDefault();
            Assert.That(lambdaSyntax, Is.Not.Null);
        }

        [MonitoredTest]
        public void CreateCubicOperation_ShouldReturnTheCorrectFunction()
        {
            //Arrange
            int[] inputs = { 1, 2, 5, 10, 100, 1000 };
            long[] expectedOutputs = { 6, 34, 430, 3210, 3020100, 3002001000 };

            //Act
            var operation = _factory.CreateCubicOperation();

            //Assert
            for (int i = 0; i < inputs.Length; i++)
            {
                Calculation result = operation(inputs[i]);
                Assert.That(result.Input, Is.EqualTo(inputs[i]), "The input of the calculation should be the same as the input of the operation.");
                Assert.That(result.Result, Is.EqualTo(expectedOutputs[i]),
                    $"3x³ + 2x² + x should be {expectedOutputs[i]} when x is {inputs[i]}.");
            }
        }

        [MonitoredTest]
        public void CreateNthPrimeOperation_ShouldUseALambdaExpression()
        {
            SyntaxNode? body = GetMethodBody(nameof(MathOperationFactory.CreateNthPrimeOperation));
            LambdaExpressionSyntax? lambdaSyntax = body?.DescendantNodes().OfType<LambdaExpressionSyntax>().FirstOrDefault();
            Assert.That(lambdaSyntax, Is.Not.Null);
        }

        [MonitoredTest]
        public void CreateNthPrimeOperation_ShouldReturnTheCorrectFunction()
        {
            //Arrange
            int[] inputs = { 1, 2, 4, 5, 10, 100 };
            long[] expectedOutputs = { 2, 3, 7, 11, 29, 541 };

            //Act
            var operation = _factory.CreateNthPrimeOperation();

            //Assert
            for (int i = 0; i < inputs.Length; i++)
            {
                Calculation result = operation(inputs[i]);
                Assert.That(result.Input, Is.EqualTo(inputs[i]), "The input of the calculation should be the same as the input of the operation.");
                Assert.That(result.Result, Is.EqualTo(expectedOutputs[i]),
                    $"The {inputs[i]}th prime number should be {expectedOutputs[i]}.");
            }
        }

        private SyntaxNode? GetMethodBody(string methodName)
        {
            var syntaxTree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(_factoryClassContent);
            var root = syntaxTree.GetRoot();
            var method = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals(methodName));
            Assert.That(method, Is.Not.Null,
                () => $"Could not find the '{methodName}' method. You may have accidentally deleted or renamed it?");


            if (method!.Body != null) return method.Body;
            if (method.ExpressionBody != null) return method.ExpressionBody;
            Assert.Fail($"Could not find the body (or expression body) of the '{methodName}' method.");
            return null;
        }
    }
}