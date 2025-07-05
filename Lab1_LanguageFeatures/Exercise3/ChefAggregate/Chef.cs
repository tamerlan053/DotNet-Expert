using Exercise3.FrontDeskAggregate;
using Exercise3.OrderAggregate;
using System.Collections.Concurrent;

namespace Exercise3.ChefAggregate
{
    public class Chef
    {
        private readonly IChefActions _chefActions;
        private readonly Queue<IOrder> _queue = new();
        private readonly object _lock = new();

        public Chef(FrontDesk frontDesk, IChefActions chefActions)
        {
            _chefActions = chefActions;
            frontDesk.OrderCreated += OnOrderCreated;
        }

        private void OnOrderCreated(object sender, OrderEventArgs args)
        {
            lock (_lock)
            {
                _queue.Enqueue(args.Order);
            }
        }

        public Task ProcessOrdersAsync(CancellationToken cancellationToken)
        {
            //Task.Run invokes an action in a new thread so that the calculation does not block the UI thread (otherwise the UI would hang)
            //The cancellationToken is used to stop the task when the user closes the application
            return Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    IOrder? order = null;

                    lock (_lock)
                    {
                        if (_queue.Count > 0)
                        {
                            order = _queue.Dequeue();
                        }
                    }

                    if (order != null)
                    {
                        order.IsStarted = true;

                        order.NumberOfBurgers.Repeat(() =>
                        {
                            _chefActions.CookBurger();
                        });

                        order.IsCompleted = true;

                        _chefActions.TakeABreather();
                    }

                    Thread.Sleep(10); // avoid tight loop
                }
            }, cancellationToken);
        }
    }
}