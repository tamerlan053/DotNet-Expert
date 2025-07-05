using System;

namespace Exercise2
{
    public interface IMathOperationFactory
    {
        Func<int, Calculation> CreateCubicOperation();
        Func<int, Calculation> CreateNthPrimeOperation();
    }
}