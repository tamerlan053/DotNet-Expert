using System.Windows;

namespace Exercise2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void gridButton_Click(object sender, RoutedEventArgs e)
        {
            GridWindow gridWindow = new GridWindow();
            gridWindow.Show();
            this.Close();
        }

        private void stackPanelButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanelWindow stackPanelWindow = new StackPanelWindow();
            stackPanelWindow.Show();
            this.Close();
        }

        private void wrapPanelButton_Click(object sender, RoutedEventArgs e)
        {
            WrapPanelWindow wrapPanelWindow = new WrapPanelWindow();
            wrapPanelWindow.Show();
            this.Close();
        }

        private void dockPanelButton_Click(object sender, RoutedEventArgs e)
        {
            DockPanelWindow dockPanelWindow = new DockPanelWindow();
            dockPanelWindow.Show();
            this.Close();
        }

        //TODO: Add event handlers for the buttons
        //Tip: make a new instance of the window and call ShowDialog() method
    }
}