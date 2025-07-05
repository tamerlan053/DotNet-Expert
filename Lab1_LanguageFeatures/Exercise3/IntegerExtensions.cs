namespace Exercise3
{ 
    public static class IntegerExtensions
    {
        public static void Repeat(this int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        public static int CircularIncrement(this int value, int min, int max)
        {
            if (value < min || value > max)
            {
                return min;
            }

            if (value == max)
            {
                return min;
            }
            else
            {
                return value + 1;
            }
        }
    }
}