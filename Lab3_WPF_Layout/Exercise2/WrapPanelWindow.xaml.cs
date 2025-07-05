using System.Windows;
using System.Windows.Controls;

namespace Exercise2
{
    public partial class WrapPanelWindow : Window
    {
        public WrapPanelWindow()
        {
            InitializeComponent();
        }

        private void horizontalButton_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWrapPanel != null)
            {
                mainWrapPanel.Orientation = Orientation.Horizontal;
            }
        }

        private void verticalButton_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWrapPanel != null)
            {
                mainWrapPanel.Orientation = Orientation.Vertical;
            }
        }
    }
}
