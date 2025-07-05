using System;

namespace Exercise3.OrderAggregate
{
    public struct OrderNumber : IEquatable<OrderNumber>
    {
        private static int _sequence = 0;

        public int Sequence { get; }

        private OrderNumber(int sequence)
        {
            Sequence = sequence;
        }

        public static OrderNumber CreateNext()
        {
            _sequence = _sequence.CircularIncrement(1, 99);
            return new OrderNumber(_sequence);
        }

        public override string ToString()
        {
            return $"#{Sequence}";
        }

        public override bool Equals(object obj)
        {
            return obj is OrderNumber other && Equals(other);
        }

        public bool Equals(OrderNumber other)
        {
            return Sequence == other.Sequence;
        }

        public override int GetHashCode()
        {
            return Sequence.GetHashCode();
        }

        public static bool operator ==(OrderNumber left, OrderNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OrderNumber left, OrderNumber right)
        {
            return !(left == right);
        }
    }
}
