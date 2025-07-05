using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Layout", "Exercise2", @"Exercise2\GridWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class GridWindowTests
    {
        private GridWindow _window = null!;
        private Grid? _grid, _innerGrid;
        private StackPanel? _stackPanel;
        private Button? _applyButton;
        private IList<TextBox> _textBoxes = [];

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new GridWindow();
            _window.Show();

            _grid = _window.FindVisualChildren<Grid>().FirstOrDefault();
            _innerGrid = _grid?.Children.OfType<Grid>().FirstOrDefault();
            _stackPanel = _window.FindVisualChildren<StackPanel>().FirstOrDefault();
            _applyButton = _stackPanel?.FindVisualChildren<Button>().FirstOrDefault();
            _textBoxes = _stackPanel?.FindVisualChildren<TextBox>().ToList() ?? [];
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _window.Close();
        }

        [MonitoredTest]
        public void _01_OuterGridShouldHave2Rows()
        {
            AssertHasOuterGridWith2Rows();
        }

        [MonitoredTest]
        public void _02_FirstRowOfOuterGridShouldContainAStackPanel()
        {
            AssertGridHasHorizontalStackPanelInHisFirstRow();
        }

        [MonitoredTest]
        public void _03_SecondRowOfOuterGridShouldContainAnInnerGrid()
        {
            Assert.That(_innerGrid, Is.Not.Null, "Outer Grid should contain an inner Grid");
            Assert.That(Grid.GetRow(_innerGrid!), Is.EqualTo(1), "The inner grid is not in the second row");
        }

        [MonitoredTest]
        public void _04_StackPanelShouldContain5Controls()
        {
            AssertGridHasHorizontalStackPanelInHisFirstRow();
            UIElementCollection stackPanelChildren = _stackPanel!.Children;
            Assert.That(stackPanelChildren.Count, Is.EqualTo(5), "The stackPanel within the first row of the Grid should contain 5 controls.");

            Assert.That(stackPanelChildren.OfType<TextBlock>().Count(), Is.EqualTo(2), "The StackPanel has to contain 2 TextBlocks.");
            Assert.That(_textBoxes.Count(), Is.EqualTo(2), "The StackPanel has to contain 2 TextBoxes.");
            Assert.That(_applyButton, Is.Not.Null, "The StackPanel has to contain 1 Button.");
        }

        [MonitoredTest]
        public void _05_InnerGridShouldHave4RowsAnd4Columns()
        {
            AssertInnerGridHas4RowsAnd4Columns();
        }

        [MonitoredTest]
        public void _06_InnerGridContainsAButton()
        {
            AssertHasInnerGrid();
            Button? greenButton = _innerGrid!.Children.OfType<Button>().FirstOrDefault();
            Assert.That(greenButton, Is.Not.Null, "Inner Grid should contain a Button");
            Assert.That((greenButton!.Background as SolidColorBrush)?.Color, Is.EqualTo(Colors.LightGreen),
                "The button should have a LightGreen background color");
        }

        [MonitoredTest]
        public void _07_ClickingTheApplyButtonShouldMoveGreenButtonToCell()
        {
            _04_StackPanelShouldContain5Controls();
            _06_InnerGridContainsAButton();

            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    AssertApplyButtonClick(row, column);
                }
            }
        }

        private void AssertApplyButtonClick(int row, int column)
        {
            _textBoxes[0].Text = row.ToString();
            _textBoxes[1].Text = column.ToString();
            _applyButton!.FireClickEvent();
            DispatcherUtil.DoEvents();

            Button greenButton = _innerGrid!.Children.OfType<Button>().First();
            int buttonRow = Grid.GetRow(greenButton);
            int buttonCol = Grid.GetColumn(greenButton);

            Assert.That(buttonRow, Is.EqualTo(row), $"The row of the green button is not correct. Row TextBox contains '{row.ToString()}'");
            Assert.That(buttonCol, Is.EqualTo(column), $"The column of the green button is not correct. Column TextBox contains '{column.ToString()}'");
            string? content = greenButton.Content as string;
            Assert.That(content, Is.Not.Null, "The content of the green button should be a string");
            Assert.That(content, Does.Contain(row.ToString()), "The content of the green button should contain the row index the button is moved to");
            Assert.That(content, Does.Contain(column.ToString()), "The content of the green button should contain the column index the button is moved to");
        }


        private void AssertGridHasHorizontalStackPanelInHisFirstRow()
        {
            Assert.That(_stackPanel, Is.Not.Null, "Outer Grid should contain a StackPanel");
            Assert.That(_stackPanel!.GetValue(Grid.RowProperty), Is.EqualTo(0), "Grid should contain a StackPanel in its first row");
            Assert.That(_stackPanel.Orientation, Is.EqualTo(Orientation.Horizontal),
                "The StackPanel inside the first row of the grid must have an horizontal orientation");
        }

        private void AssertHasOuterGridWith2Rows()
        {
            Assert.That(_grid, Is.Not.Null, "No 'Grid' could be found.");
            Assert.That(_grid!.Parent, Is.SameAs(_window),
                "The 'Grid' should be the child control of the 'Window'.");

            Assert.That(_grid.RowDefinitions, Has.Count.EqualTo(2), () => "The outer 'Grid' should have 2 rows defined.");

            Assert.That(_grid.RowDefinitions[0].Height.IsAuto, Is.True, "The first row of the outer grid should adjust to the height of its children.");
            Assert.That(_grid.RowDefinitions[1].Height.IsStar, Is.True, "The second row of the outer grid should take the remaining space");
            Assert.That(_grid.ColumnDefinitions, Has.Count.EqualTo(0), () => "The 'Grid' should have no columns defined.");
        }

        private void AssertHasInnerGrid()
        {
            Assert.That(_innerGrid, Is.Not.Null, "No inner 'Grid' could be found.");
            Assert.That(_innerGrid!.Parent, Is.SameAs(_grid),
                "The 'inner Grid' should be the child control of the outer 'Grid'.");
            Assert.That(_innerGrid.ShowGridLines, Is.True, "The inner 'Grid' should show grid lines. Tip: property 'ShowGridLines'.");
        }

        private void AssertInnerGridHas4RowsAnd4Columns()
        {
            AssertHasInnerGrid();

            Assert.That(_innerGrid!.RowDefinitions, Has.Count.EqualTo(4), () => "The 'Grid' should have 4 rows defined.");
            Assert.That(_innerGrid.ColumnDefinitions, Has.Count.EqualTo(4), () => "The 'Grid' should have 4 columns defined.");
        }
    }
}
