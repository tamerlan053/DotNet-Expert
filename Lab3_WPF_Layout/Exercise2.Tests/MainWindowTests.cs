using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Layout", "Exercise2", @"Exercise2\MainWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window = null!;
        private Grid? _grid;
        private IList<Button> _allButtons = [];

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new MainWindow();
            _window.Show();

            _grid = _window.FindVisualChildren<Grid>().FirstOrDefault();
            _allButtons = _grid?.FindVisualChildren<Button>().ToList() ?? [];
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _window.Close();
        }

        [MonitoredTest]
        public void _01_ButtonsShouldHaveClickEventHandlers()
        {
            Assert.That(_allButtons.Count, Is.EqualTo(4), "There should be 4 buttons");
            Assert.That(_allButtons.All(HasClickEventHandler), Is.True, "All buttons should have a click event handler.");
        }

        [MonitoredTest]
        public void _02_ShouldHaveAGridWith4CellsContainingAButton()
        {
            AssertGridHas4Cells();
            AssertCellsContainButtonsWithContent();
        }

        [MonitoredTest]
        public void _03_TheGridShouldHaveTheCorrectElementInEachCell()
        {
            AssertGridHas4Cells();

            var allGridButtons = _grid!.Children.OfType<Button>().ToList();
            AssertCellHasControl<Button>(0, 0);
            AssertCellHasControl<Button>(0, 1);
            AssertCellHasControl<Button>(1, 0);
            AssertCellHasControl<Button>(1, 1);
        }

        private bool HasClickEventHandler(ButtonBase button)
        {
            var eventStore = button.GetType()
                .GetProperty("EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(button, null);

            if (eventStore == null) return false;
            MethodInfo? getEventHandlersMethod = eventStore
                .GetType()
                .GetMethod("GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (getEventHandlersMethod is null) return false;

            RoutedEventHandlerInfo[]? handlers =
                getEventHandlersMethod.Invoke(eventStore, new object[] { ButtonBase.ClickEvent }) as
                    RoutedEventHandlerInfo[];

            if (handlers == null || handlers.Length == 0) return false;

            return handlers.Any(handler => handler.Handler != null);
        }

        private void AssertCellsContainButtonsWithContent()
        {
            Assert.That(
                _allButtons.All(
                    button => button.Content is string contentString && !string.IsNullOrEmpty(contentString)), Is.True,
                "All buttons should have a string as Content");
        }

        private void AssertHasOuterGrid()
        {
            Assert.That(_grid, Is.Not.Null, "No 'Grid' could be found.");
            Assert.That(_grid!.Parent, Is.SameAs(_window),
                "The 'Grid' should be the child control of the 'Window'.");
        }

        private void AssertCellHasControl<T>(int row, int column) where T : FrameworkElement
        {
            var control = _grid!
                .FindVisualChildren<T>().FirstOrDefault(c => Grid.GetRow(c) == row && Grid.GetColumn(c) == column);

            var type = typeof(T);
            Assert.That(control, Is.Not.Null, $"No {type.Name} found in cell ({row},{column}).");
        }

        private void AssertGridHas4Cells()
        {
            AssertHasOuterGrid();

            Assert.That(_grid!.RowDefinitions, Has.Count.EqualTo(2), () => "The 'Grid' should have 2 rows defined.");
            Assert.That(_grid.ColumnDefinitions, Has.Count.EqualTo(2), () => "The 'Grid' should have 2 columns defined.");
        }
    }
}
