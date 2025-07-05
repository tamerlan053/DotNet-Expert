using Exercise3.OrderAggregate;
using System.Collections.ObjectModel;

namespace Exercise3.FrontDeskAggregate
{
    public class FrontDesk
    {
        private readonly ObservableCollection<IOrder> _orders;
        public ReadOnlyObservableCollection<IOrder> Orders { get; }


        public event OrderCreatedHandler? OrderCreated;

        public FrontDesk()
        {
            _orders = new ObservableCollection<IOrder>();
            Orders = new ReadOnlyObservableCollection<IOrder>(_orders);
        }

        public void AddOrder(int numberOfBurgers)
        {
            var order = new Order(numberOfBurgers);
            _orders.Add(order);
            OrderCreated?.Invoke(this, new OrderEventArgs(order));
        }

        public void RemoveCompletedOrders()
        {
            for (int i = Orders.Count - 1; i >= 0; i--)
            {
                if (Orders[i].IsCompleted)
                {
                    _orders.RemoveAt(i);
                }
            }
        }
    }
}