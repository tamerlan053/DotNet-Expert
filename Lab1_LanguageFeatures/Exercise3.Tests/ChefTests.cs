﻿using Exercise3.ChefAggregate;
using Exercise3.FrontDeskAggregate;
using Exercise3.OrderAggregate;
using Guts.Client.Core;
using Moq;
using System.Reflection;

namespace Exercise3.Tests
{
    [ExerciseTestFixture("dotnetExp", "H1", "Exercise03", @"Exercise3\ChefAggregate\Chef.cs")]
    public class ChefTests
    {
        private FrontDesk _frontDesk = null!;
        private Mock<IChefActions> _chefActionsMock = null!;
        private Chef _chef = null!;
        private FieldInfo? _queueField;

        [SetUp]
        public void BeforeEachTest()
        {
            _queueField = typeof(Chef).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.FieldType.IsAssignableFrom(typeof(Queue<IOrder>)));

            _frontDesk = new FrontDesk();
            _chefActionsMock = new Mock<IChefActions>();
            _chef = new Chef(_frontDesk, _chefActionsMock.Object);
        }

        [MonitoredTest]
        public void ShouldListenForCreatedOrdersAndAddThemInAQueue()
        {
            Queue<IOrder> queue = GetAndAssertQueueFieldValue();

            _frontDesk.AddOrder(1);

            Assert.That(queue.Count, Is.EqualTo(1),
                "When the 'AddOrder' method of the 'Frontdesk' is invoked, the queue should contain exactly one order.");
        }

        [MonitoredTest]
        public void ProcessOrdersAsync_ShouldProcessOrdersInTheQueue()
        {
            Queue<IOrder> queue = GetAndAssertQueueFieldValue();

            int numberOfOrders = Random.Shared.Next(5, 11);
            int burgerTotal = 0;
            var orders = new List<IOrder>();
            for (int i = 0; i < numberOfOrders; i++)
            {
                int numberOfBurgers = Random.Shared.Next(1, 5);
                if (new Order(numberOfBurgers) is IOrder order)
                {
                    burgerTotal += numberOfBurgers;
                    orders.Add(order);
                    queue.Enqueue(order);
                }
            }

            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            _chef.ProcessOrdersAsync(cancellationToken);
            Thread.Sleep(150); //wait a little bit so that the queue is completely processed
            cancellationTokenSource.Cancel();

            Assert.That(queue.Count, Is.EqualTo(0),
                $"The queue (with {numberOfOrders} orders) should be empty after a while.");

            Assert.That(orders, Has.All.Matches((IOrder order) => order.IsStarted),
                "All orders should have 'IsStarted' true after processing the queue.");
            Assert.That(orders, Has.All.Matches((IOrder order) => order.IsCompleted),
                "All orders should have 'IsCompleted' true after processing the queue.");

            _chefActionsMock.Verify(a => a.CookBurger(), Times.Exactly(burgerTotal),
                "The 'CookBurger' action should have been called for each burger in each order.");

            _chefActionsMock.Verify(a => a.TakeABreather(), Times.AtLeast(numberOfOrders),
                "The 'TakeABreather' action should have been called for each order (at least).");
        }


        private Queue<IOrder> GetAndAssertQueueFieldValue()
        {
            Assert.That(_queueField, Is.Not.Null, "The class should have a private field of type 'Queue<IOrder>'.");
            var queue = (Queue<IOrder>?)_queueField!.GetValue(_chef);
            Assert.That(queue, Is.Not.Null, "The private queue is not instantiated (by the constructor).");
            return queue!;
        }
    }
}