using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Layout", "Exercise2", @"Exercise2\StackPanelWindow.xaml;Exercise2\StackPanelWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class StackPanelWindowTests
    {
        private StackPanelWindow _window = null!;
        private Grid? _grid;
        private GroupBox? _groupBox;
        private StackPanel? _orientationStackPanel, _stackPanel;
        private IList<RadioButton> _radioButtons = new List<RadioButton>();
        private IList<Button> _buttons = new List<Button>();

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new StackPanelWindow();
            _window.Show();

            _grid = _window.FindVisualChildren<Grid>().FirstOrDefault();
            _groupBox = _grid?.Children.OfType<GroupBox>().FirstOrDefault();
            _orientationStackPanel = _window.FindVisualChildren<StackPanel>().FirstOrDefault(s => s.Parent == _groupBox);
            _radioButtons = _window.FindVisualChildren<RadioButton>().ToList();
            _buttons = _window.FindVisualChildren<Button>().ToList();
            _stackPanel = _window.FindVisualChildren<StackPanel>().FirstOrDefault(s => s.Parent == _grid);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _window.Close();
        }

        [MonitoredTest]
        public void _01_WindowShouldContainAGridWith2Rows()
        {
            AssertGridHas2Cells();
        }

        private void AssertGridHas2Cells()
        {
            AssertHasOuterGrid();

            Assert.That(_grid!.RowDefinitions, Has.Count.EqualTo(2), () => "The 'Grid' should have 2 rows defined.");
            Assert.That(_grid.RowDefinitions[0].Height.IsAuto, Is.True, "The first row of the outer grid should adjust to the height of its children.");
            Assert.That(_grid.ColumnDefinitions, Has.Count.EqualTo(0), () => "The 'Grid' should have no columns defined.");
        }

        private void AssertHasOuterGrid()
        {
            Assert.That(_grid, Is.Not.Null, "No 'Grid' could be found.");
            Assert.That(_grid!.Parent, Is.SameAs(_window),
                "The 'Grid' should be the child control of the 'Window'.");
        }

        [MonitoredTest]
        public void _02_FirstRowOfGridShouldContainAGroupBox()
        {
            AssertGridHasGroupBoxInHisFirstRow();
        }

        private void AssertGridHasGroupBoxInHisFirstRow()
        {
            Assert.That(_groupBox, Is.Not.Null, "Grid should contain a GroupBox");
            Assert.That(_groupBox!.GetValue(Grid.RowProperty), Is.EqualTo(0), "Grid should contain a GroupBox in its first row");
            Assert.That(_groupBox.Header, Is.EqualTo("Orientation"), "The header of the groupBox should be 'Orientation'");
        }

        [MonitoredTest]
        public void _03_GroupBoxShouldContainAStackPanelWith2RadioButtons()
        {
            Assert.That(_orientationStackPanel, Is.Not.Null, "There has to be a stackPanel on the window");
            Assert.That(_orientationStackPanel!.Parent, Is.EqualTo(_groupBox), "The StackPanel has to be within the GroupBox");
            Assert.That(_radioButtons.Count, Is.EqualTo(2), "The StackPanel in the GroupBox has to contain 2 radioButtons");
            Assert.That(_radioButtons.All(r => r.Parent == _orientationStackPanel), Is.True, "All radioButtons have to be inside the StackPanel of the GroupBox");
        }

        [MonitoredTest]
        public void _04_TheStackPanelShouldBeInTheSecondRowOfTheGrid()
        {
            Assert.That(_stackPanel, Is.Not.Null, "There should be a stackPanel within the Grid");
            Assert.That(_stackPanel!.GetValue(Grid.RowProperty), Is.EqualTo(1), "Grid should contain a StackPanel in its second row");
            Assert.That(_buttons.Count, Is.EqualTo(2), "There should be 2 buttons inside the StackPanel");
            Assert.That(_buttons.All(b => b.Parent == _stackPanel), Is.True, "The 2 Buttons should be inside the stackPanel");
        }


        [MonitoredTest]
        public void _05_TheOrientationOfTheStackPanelHasToBecomeVerticalWhenClickingTheVerticalRadioButton()
        {
            RadioButton? verticalRadioButton = _radioButtons.FirstOrDefault(r => r.Content.ToString() == "Vertical");
            Assert.That(verticalRadioButton, Is.Not.Null, "Cannot find a 'RadioButton' with content 'Vertical'.");
            verticalRadioButton!.IsChecked = true;
            Assert.That(_stackPanel, Is.Not.Null, "There should be a stackPanel within the Grid");
            Assert.That(_stackPanel!.Orientation, Is.EqualTo(Orientation.Vertical), "The Orientation of the StackPanel is not Vertical.");
        }

        [MonitoredTest]
        public void _06_TheOrientationOfTheWrapPanelHasToBecomeHorizontalWhenClickingTheHorizontalRadioButton()
        {
            RadioButton? horizontalRadioButton = _radioButtons.FirstOrDefault(r => r.Content.ToString() == "Horizontal");
            Assert.That(horizontalRadioButton, Is.Not.Null, "Cannot find a 'RadioButton' with content 'Horizontal'.");
            horizontalRadioButton!.IsChecked = true;
            Assert.That(_stackPanel, Is.Not.Null, "There should be a stackPanel within the Grid");
            Assert.That(_stackPanel!.Orientation, Is.EqualTo(Orientation.Horizontal), "The Orientation of the StackPanel is not Horizontal");
        }
    }
}