using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Exercise2
{
    public partial class GridWindow : Window
    {
        public GridWindow()
        {
            InitializeComponent();
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(rowTextBox.Text, out int row) && int.TryParse(columnTextBox.Text, out int column)) {
                if (row >= 0 && row < 4 && column >= 0 && column < 4)
                {
                    Grid.SetRow(greenButton, row);
                    Grid.SetColumn(greenButton, column);
                    greenButton.Content = $"Row {row}, Column {column}";
                }
            }
            else
            {
                MessageBox.Show("Row and Column values must be between 0 and 3.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
