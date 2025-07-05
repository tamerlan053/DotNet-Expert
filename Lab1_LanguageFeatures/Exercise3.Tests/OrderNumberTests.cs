﻿using Exercise3.OrderAggregate;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace Exercise3.Tests
{
    [ExerciseTestFixture("dotnetExp", "H1", "Exercise03", @"Exercise3\OrderAggregate\OrderNumber.cs")]
    public class OrderNumberTests
    {
        private Type _type = null!;
        private PropertyInfo? _sequenceProperty;
        private MethodInfo? _createNextMethod;
        private string _orderNumberClassContent = null!;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _orderNumberClassContent = Solution.Current.GetFileContent(@"Exercise3\OrderAggregate\OrderNumber.cs");
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _type = typeof(OrderNumber);
            _sequenceProperty = _type.GetRuntimeProperty("Sequence");
            _createNextMethod = _type.GetRuntimeMethod("CreateNext", new Type[] { });
        }

        [MonitoredTest]
        public void _01_ShouldBeAValueType()
        {
            Assert.That(_type.IsValueType, Is.True);
        }

        [MonitoredTest]
        public void _02_ShouldHaveAReadOnlySequenceProperty()
        {
            AssertHasSequenceProperty();
        }

        [MonitoredTest]
        public void _03_ShouldHaveAStaticCreateNextFactoryMethod()
        {
            AssertHasCreateNextMethod();
        }

        [MonitoredTest]
        public void _04_CreateNext_ShouldCreateANumberWithAnIncrementedSequence()
        {
            AssertHasSequenceProperty();
            AssertHasCreateNextMethod();

            Assert.That(CallsMemberMethod("CircularIncrement"), Is.True, "The 'CircularIncrement' extension method should be used.");

            OrderNumber number = CreateNext();
            int previousSequence = (int)_sequenceProperty!.GetValue(number)!;
            Assert.That(previousSequence, Is.GreaterThan(0).And.LessThan(100), $"The value of the 'Sequence' must always be between 1 and 99. " +
                                                                               "Error occurred at the first time the 'CreateNext' method was called.");

            for (int i = 1; i <= 100; i++)
            {
                number = CreateNext();
                int sequence = (int)_sequenceProperty.GetValue(number)!;

                Assert.That(sequence, Is.GreaterThan(0).And.LessThan(100), $"The value of the 'Sequence' must always be between 1 and 99. " +
                                                                           $"Error occurred at the {i + 1}th time the 'CreateNext' method was called.");

                if (sequence > previousSequence)
                {
                    Assert.That(sequence == previousSequence + 1, Is.True,
                        $"The value of the 'Sequence' ({sequence}) must be one more than the 'Sequence' of the previously created 'OrderNumber' ({previousSequence}). " +
                        "Tip: use a private static field. " +
                        $"Error occurred at the {i + 1}th time the 'CreateNext' method was called.");
                }
                else
                {
                    Assert.That(previousSequence == 99 && sequence == 1, Is.True,
                        "When the previously created 'OrderNumber' had a sequence of 99, the 'Sequence' should start at 1 again. " +
                        $"Error occurred at the {i + 1}th time the 'CreateNext' method was called. Sequence: {sequence}, Previous sequence: {previousSequence}");
                }
                previousSequence = sequence;
            }
        }

        [MonitoredTest]
        public void _05_ToString_ShouldPutAHashtagBeforeTheSequence()
        {
            AssertHasSequenceProperty();
            AssertHasCreateNextMethod();

            for (int i = 0; i < 3; i++)
            {
                OrderNumber number = CreateNext();
                int sequence = (int)_sequenceProperty!.GetValue(number)!;
                Assert.That(number.ToString(), Is.EqualTo($"#{sequence}"));
            }
        }

        [MonitoredTest]
        public void _06_ConstructorShouldBePrivate()
        {
            Assert.That(_type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length, Is.EqualTo(0),
                "A public constructor is found. There should only be one private constructor. " +
                "This forces other classes to use the static 'CreateNext' factory method.");
            Assert.That(_type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Length, Is.EqualTo(1),
                "No private constructor is found. There should be exactly one private constructor.");

        }

        private void AssertHasSequenceProperty()
        {
            Assert.That(_sequenceProperty, Is.Not.Null, "No (public) property with the name 'Sequence' can be found.");
            Assert.That(_sequenceProperty!.CanRead, Is.True, "The 'Sequence' property should have a getter.");
            Assert.That(_sequenceProperty.CanWrite, Is.False, "The 'Sequence' property should not have a setter.");
        }

        private void AssertHasCreateNextMethod()
        {
            Assert.That(_createNextMethod, Is.Not.Null, "No (public) method with the name 'CreateNext' can be found.");
            Assert.That(_createNextMethod!.IsStatic, Is.True, "The 'CreateNext' method should be static.");
            Assert.That(_createNextMethod.ReturnType, Is.EqualTo(_type),
                "The 'CreateNext' method should return an instance of 'OrderNumber'.");
            Assert.That(_createNextMethod.GetParameters().Length, Is.Zero,
                "The 'CreateNext' method should not have any parameters.");
        }

        private OrderNumber CreateNext()
        {
            return (OrderNumber)_createNextMethod!.Invoke(null, new object[] { })!;
        }

        private bool CallsMemberMethod(string methodName)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(_orderNumberClassContent);
            var root = syntaxTree.GetRoot();
            return root
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .Any(memberAccess => memberAccess.Name.ToString().ToLower() == methodName.ToLower());
        }
    }
}