using System.Windows;
using System.Windows.Controls;

namespace Exercise2
{
    public partial class StackPanelWindow : Window
    {
        public StackPanelWindow()
        {
            InitializeComponent();
        }

        private void horizontalButton_Checked(object sender, RoutedEventArgs e)
        {
            if (mainStackPanel != null)
            {
                mainStackPanel.Orientation = Orientation.Horizontal;
            }
        }

        private void verticalButton_Checked(object sender, RoutedEventArgs e)
        {
            if (mainStackPanel != null)
            {
                mainStackPanel.Orientation = Orientation.Vertical;
            }
        }
    }
}
