using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Windows.Threading;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Controls", "Exercise2", @"Exercise2\MainWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window = null!;
        private RepeatButton? _growButton;
        private RepeatButton? _shrinkButton;
        private Canvas? _canvas;
        private Rectangle? _rectangle;

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new MainWindow();
            _window.Show();

            List<ButtonBase> allButtons = _window.FindVisualChildren<ButtonBase>().ToList();
            _growButton = allButtons.FirstOrDefault(button => (button.Content as string)?.ToLower() == "grow") as RepeatButton;
            _shrinkButton = allButtons.FirstOrDefault(button => (button.Content as string)?.ToLower() == "shrink") as RepeatButton;

            _canvas = _window.FindVisualChildren<Canvas>().FirstOrDefault();
            if (_canvas != null)
            {
                _rectangle = _canvas.Children.OfType<Rectangle>().FirstOrDefault();
            }

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();
            _window.Close();
        }

        [MonitoredTest]
        public void _1_ShouldHaveTwoRepeatButtons()
        {
            AssertHasButtons();
        }

        [MonitoredTest]
        public void _2_ShouldHaveACanvasThatContainsARectangle()
        {
            AssertHasCanvasWithRectangle();
        }

        [MonitoredTest]
        public void _3_ShouldIncreaseTheRectangleWidthWhenHoldingTheGrowButton()
        {
            AssertHasButtons();
            AssertHasCanvasWithRectangle();

            AssertGrowsAfterClickEvent("The rectangle does not grow.");
            AssertGrowsAfterClickEvent("The rectangle does not grow further when holding the button.");
        }

        [MonitoredTest]
        public void _4_ShouldDecreaseTheRectangleWidthWhenHoldingTheShrinkButton()
        {
            AssertHasButtons();
            AssertHasCanvasWithRectangle();

            AssertShrinksAfterClickEvent("The rectangle does not shrink.");
            AssertShrinksAfterClickEvent("The rectangle does not shrink further when holding the button.");
        }

        [MonitoredTest]
        public void _5_ShouldNotDecreaseTheWithOfTheRectangleUnderZero()
        {
            AssertHasButtons();
            AssertHasCanvasWithRectangle();

            _rectangle!.Width = 0;
            _shrinkButton!.FireClickEvent();
            var newWidth = _rectangle.Width;

            Assert.That(newWidth, Is.Zero,
                () =>
                    "The width of the rectangle may not shrink to a value less than zero.");
        }


        [MonitoredTest]
        public void _6_ShouldNotIncreaseTheWidhtOfTheRectanglePassTheCanvasBorder()
        {
            AssertHasButtons();
            AssertHasCanvasWithRectangle();

            var maxRectangleWidth = _canvas!.Width - _rectangle!.Margin.Left;
            _rectangle.Width = maxRectangleWidth;
            _growButton!.FireClickEvent();
            var newWidth = _rectangle.Width;

            Assert.That(newWidth, Is.EqualTo(maxRectangleWidth),
                () =>
                    "The rectangle may not grow wider than the width of the canvas minus the x-position of the rectangle");
        }

        private void AssertHasButtons()
        {
            Assert.That(_growButton, Is.Not.Null, () => "A 'RepeatButton' with the text 'Grow' as 'Content' could not be found.");
            Assert.That(_shrinkButton, Is.Not.Null, () => "A 'RepeatButton' with the text 'Shrink' as 'Content' could not be found.");
        }

        private void AssertHasCanvasWithRectangle()
        {
            Assert.That(_canvas, Is.Not.Null, () => "No 'Canvas' layout control could be found.");
            Assert.That(_rectangle, Is.Not.Null, () => "The 'Canvas' does not contain a 'Rectangle'.");
        }

        private void AssertGrowsAfterClickEvent(string failureMessage)
        {
            var originalWidth = _rectangle!.Width;
            _growButton!.FireClickEvent();
            var newWidth = _rectangle.Width;
            Assert.That(newWidth, Is.GreaterThan(originalWidth), () => failureMessage);
        }

        private void AssertShrinksAfterClickEvent(string failureMessage)
        {
            var originalWidth = _rectangle!.Width;
            _shrinkButton!.FireClickEvent();
            var newWidth = _rectangle.Width;
            Assert.That(newWidth, Is.LessThan(originalWidth), () => failureMessage);
        }
    }
}