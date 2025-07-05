using System;

namespace Exercise2
{
    public class CalculationEventArgs : EventArgs
    {
        public Calculation Calculation { get; }
        public double ProgressPercentage { get; set; }

        public CalculationEventArgs(Calculation calculation, double progress)
        {
            Calculation = calculation;
            ProgressPercentage = progress;
        }
    }
}