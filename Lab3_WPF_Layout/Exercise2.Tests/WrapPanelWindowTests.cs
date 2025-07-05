﻿using System.Windows.Controls;
using System.Windows.Shapes;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Layout", "Exercise2", @"Exercise2\WrapPanelWindow.xaml;Exercise2\WrapPanelWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class WrapPanelWindowTests
    {
        private WrapPanelWindow _window = null!;
        private Grid? _grid;
        private StackPanel? _stackPanel;
        private GroupBox? _groupBox;
        private IList<RadioButton> _radioButtons = [];
        private WrapPanel? _wrapPanel;
        private IList<Ellipse> _ellipses = [];

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new WrapPanelWindow();
            _window.Show();

            _grid = _window.FindVisualChildren<Grid>().FirstOrDefault();
            _groupBox = _grid?.Children.OfType<GroupBox>().FirstOrDefault();
            _stackPanel = _groupBox?.FindVisualChildren<StackPanel>().FirstOrDefault();
            _wrapPanel = _grid?.FindVisualChildren<WrapPanel>().FirstOrDefault();
            _radioButtons = _stackPanel?.FindVisualChildren<RadioButton>().ToList() ?? [];
            _ellipses = _wrapPanel?.Children.OfType<Ellipse>().ToList() ?? [];
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
            Assert.That(_grid.RowDefinitions[1].Height.IsStar, Is.True, "The second row of the outer grid should take all remaining space.");
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
            Assert.That(_groupBox, Is.Not.Null, "Grid should contain a GroupBox");
            Assert.That(_groupBox!.GetValue(Grid.RowProperty), Is.EqualTo(0), "Grid should contain a GroupBox in its first row");
            Assert.That(_groupBox.Header, Is.EqualTo("Orientation"), "The header of the GroupBox should be 'Orientation'");
        }

        [MonitoredTest]
        public void _03_GroupBoxShouldContainAStackPanelWith2RadioButtons()
        {
            Assert.That(_stackPanel, Is.Not.Null, "There has to be a stackPanel on the window");
            Assert.That(_stackPanel!.Parent, Is.EqualTo(_groupBox), "The StackPanel has to be within the GroupBox");
            Assert.That(_radioButtons.Count, Is.EqualTo(2), "The Groupbox has to contain 2 radioButtons");
            Assert.That(_radioButtons.All(r => r.Parent == _stackPanel), Is.True, "All radioButtons have to be inside the GroupBox");
        }

        [MonitoredTest]
        public void _04_HorizontalWrapPanelHasToBeInTheSecondRowOfTheGridAndContains8Ellipses()
        {
            Assert.That(_wrapPanel, Is.Not.Null, "There has to be a wrapPanel on the window");
            Assert.That(Grid.GetRow(_wrapPanel!), Is.EqualTo(1), "Grid should contain a WrapPanel in its second row");
            Assert.That(_wrapPanel!.Orientation, Is.EqualTo(Orientation.Horizontal), "The orientation of the WrapPanel has to be Horizontal");
            Assert.That(_radioButtons.Count, Is.EqualTo(2), "The GroupBox has to contain 2 radioButtons");
            Assert.That(_ellipses.Count, Is.EqualTo(8), "There have to be 8 ellipses within the WrapPanel");
        }

        [MonitoredTest]
        public void _05_TheOrientationOfTheWrapPanelHasToBecomeVerticalWhenClickingTheVerticalRadioButton()
        {
            RadioButton? verticalRadioButton = _radioButtons.FirstOrDefault(r => r.Content.ToString() == "Vertical");
            Assert.That(verticalRadioButton, Is.Not.Null, "Cannot find a 'RadioButton' with content 'Vertical'.");
            verticalRadioButton!.IsChecked = true;

            Assert.That(_wrapPanel, Is.Not.Null, "There has to be a wrapPanel on the window");
            Assert.That(_wrapPanel!.Orientation, Is.EqualTo(Orientation.Vertical),
                "The Orientation of the WrapPanel has to become Vertical when clicking the Vertical RadioButton");
        }

        [MonitoredTest]
        public void _06_TheOrientationOfTheWrapPanelHasToBecomeHorizontalWhenClickingTheHorizontalRadioButton()
        {
            RadioButton? horizontalRadioButton = _radioButtons.FirstOrDefault(r => r.Content.ToString() == "Horizontal");
            Assert.That(horizontalRadioButton, Is.Not.Null, "Cannot find a 'RadioButton' with content 'Horizontal'.");
            horizontalRadioButton!.IsChecked = true;

            Assert.That(_wrapPanel, Is.Not.Null, "There has to be a wrapPanel on the window");
            Assert.That(_wrapPanel!.Orientation, Is.EqualTo(Orientation.Horizontal),
                "The Orientation of the WrapPanel has to become Horizontal when clicking the Horizontal RadioButton");
        }
    }
}
