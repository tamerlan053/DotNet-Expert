using System.Windows;

namespace Exercise2
{
    public partial class MainWindow : Window
    {
        private readonly IMathOperationFactory _operationFactory;
        private readonly CalculationWorker _calculationWorker;

        public MainWindow(IMathOperationFactory operationFactory)
        {
            InitializeComponent();

            _operationFactory = operationFactory;

            //TODO: create a new CalculationWorker and subscribe to the CalculationCompleted event
            _calculationWorker = new CalculationWorker();
            _calculationWorker.CalculationCompleted += Calculator_CalculationCompleted;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            calculationProgressBar.Value = 0;
            outputTextBlock.Text = "";

            //TODO: create an array of integers from the input text
            int[] numbers = inputTextBox.Text
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            //TODO: create a math operation based on the selected radio button using the operation factory
            Func<int, Calculation> operation;

            if (cubicRadioButton.IsChecked == true)
            {
                operation = _operationFactory.CreateCubicOperation();
            }
            else
            {
                operation = _operationFactory.CreateNthPrimeOperation();
            }

            //TODO: start the calculation worker with the inputs and the math operation
            await _calculationWorker.DoWorkAsync(numbers, operation);

            //TIP: you don't need to wait for 'DoWorkAsync' Task to complete, the 'CalculationCompleted' event will be raised when a calculation is done

        }

        private void Calculator_CalculationCompleted(object sender, CalculationEventArgs args)
        {
            // Do not change this method. It is used to update the UI when a calculation is completed.
            // You can use this method to handle the CalculationCompleted event of the CalculationWorker.
            // The Dispatcher.InvokeAsync is used to make sure the UI is updated on the UI thread (and not on the calculation worker thread).
            Dispatcher.InvokeAsync(() =>
            {
                calculationProgressBar.Value = args.ProgressPercentage;
                outputTextBlock.Text = $"{outputTextBlock.Text} {args.Calculation.Input}->{args.Calculation.Result}".Trim();
            });
        }
    }
}
