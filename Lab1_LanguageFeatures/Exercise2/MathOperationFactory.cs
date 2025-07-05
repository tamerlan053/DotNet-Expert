namespace Exercise2
{
    public class MathOperationFactory : IMathOperationFactory
    {
        public Func<int, Calculation> CreateCubicOperation() //3*x³ + 2*x² + x
        {
            //TODO: create a function that calculates 3*x³ + 2*x² + x and return the function (not the result)
            return x =>
            {
                long result = 3L * x * x * x + 2L * x * x + x;
                return new Calculation(x, result);
            };
        }

        public Func<int, Calculation> CreateNthPrimeOperation() //for input n, return the nth prime number
        {
            //TODO: create a function that calculates the nth prime number and return the function (not the result)
            //Tip: Check each number starting from 2. If a number is prime, increment a counter. When the counter reaches n, return the number.
            //Tip: you can use the IsPrime method to check if a number is prime.
            return n =>
            {
                int count = 0;
                int number = 1;

                while (count < n)
                {
                    number++;
                    if (IsPrime(number))
                    {
                        count++;
                    }
                }

                return new Calculation(n, number);
            };
        }

        private bool IsPrime(long number)
        {
            for (long i = 2; i < number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}