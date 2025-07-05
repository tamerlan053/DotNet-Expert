using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Exercise3.OrderAggregate
{
    public class Order : INotifyPropertyChanged, IOrder
    {
        //Order implements INotifyPropertyChanged to notify the UI when the IsStarted and IsCompleted properties change.
        //The setter of the properties call OnPropertyChanged that invokes the PropertyChanged event to notify the UI of the change.
        //We will learn more about this in the next module.

        public OrderNumber Number { get; set; }

        public int NumberOfBurgers { get; }

        private bool _isStarted;
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;
                OnPropertyChanged();
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }

        public Order(int numberOfBurgers)
        {
            NumberOfBurgers = numberOfBurgers;
            Number = OrderNumber.CreateNext();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
