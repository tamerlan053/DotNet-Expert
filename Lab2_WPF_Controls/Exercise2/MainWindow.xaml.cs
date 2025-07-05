using System.Windows;
using System.Windows.Controls;

namespace Exercise2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void growButton_Click(object sender, RoutedEventArgs e)
        {
            double rectangleX = Canvas.GetLeft(myRectangle);
            double maxWidth = paperCanvas.ActualWidth - rectangleX;

            if (myRectangle.Width < maxWidth)
            {
                myRectangle.Height += 10;
                myRectangle.Width += 10;
            }
        }

        private void shrinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (myRectangle.Height > 10 && myRectangle.Width > 10)
            {
                myRectangle.Height -= 10;
                myRectangle.Width -= 10;
            }
        }
    }
}