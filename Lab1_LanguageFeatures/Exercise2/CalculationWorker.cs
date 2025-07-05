namespace Exercise2
{
    public class CalculationWorker
    {
        public event CalculationCompleteHandler CalculationCompleted;

        public async Task DoWorkAsync(int[] inputs, Func<int, Calculation> mathOperation)
        {
            int total = inputs.Length;
            int completed = 0;

            var tasks = inputs.Select(input => Task.Run(() =>
            {
                Calculation result = mathOperation(input);
                return result;
            })).ToList();

            while (tasks.Any())
            {
                var finishedTask = await Task.WhenAny(tasks);
                tasks.Remove(finishedTask);

                Calculation result = await finishedTask;
                completed++;

                double progress = (double)completed / total;

                CalculationCompleted?.Invoke(this, new CalculationEventArgs(result, progress));
            }
        }
    }
}
