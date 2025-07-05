using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Layout", "Exercise2", @"Exercise2\DockPanelWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class DockPanelWindowTests
    {
        private DockPanelWindow _window = null!;
        private DockPanel? _dockPanel;
        private IList<Button> _buttons = [];
        private Image? _image;
        private Button? _topButton;
        private Button? _rightButton;
        private Button? _bottomButton;
        private Button? _leftButton;

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new DockPanelWindow();
            _window.Show();

            _dockPanel = _window.FindVisualChildren<DockPanel>().FirstOrDefault();
            _image = _dockPanel?.FindVisualChildren<Image>().FirstOrDefault();
            _buttons = _dockPanel?.FindVisualChildren<Button>().ToList() ?? [];
            _topButton = _buttons.FirstOrDefault(b => b.Content.ToString() == "Top");
            _rightButton = _buttons.FirstOrDefault(b => b.Content.ToString() == "Right");
            _bottomButton = _buttons.FirstOrDefault(b => b.Content.ToString() == "Bottom");
            _leftButton = _buttons.FirstOrDefault(b => b.Content.ToString() == "Left");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _window.Close();
        }

        [MonitoredTest]
        public void _01_TheWindowShouldContainADockPanel()
        {
            Assert.That(_dockPanel, Is.Not.Null, "Window should contain a DockPanel");
            Assert.That(_dockPanel!.Parent, Is.SameAs(_window), "The parent element of the DockPanel is the Window");
        }

        [MonitoredTest]
        public void _02_TheDockPanelShouldContain4ButtonsAndAnImage()
        {
            Assert.That(_buttons.Count, Is.EqualTo(4), "The DockPanel should contain 4 buttons");
            Assert.That(_topButton, Is.Not.Null, "Could not find a Button with Content 'Top'.");
            Assert.That(_rightButton, Is.Not.Null, "Could not find a Button with Content 'Right'.");
            Assert.That(_bottomButton, Is.Not.Null, "Could not find a Button with Content 'Bottom'.");
            Assert.That(_leftButton, Is.Not.Null, "Could not find a Button with Content 'Left'.");
            Assert.That(_image, Is.Not.Null, "The DockPanel should contain an Image");
            Assert.That(_image!.Parent, Is.SameAs(_dockPanel), "The parent element of the Image is the DockPanel");
        }

        [MonitoredTest]
        public void _03_TheButtonsShouldHaveCorrectMarginsAndDimensions()
        {
            Assert.That(_buttons.Count, Is.GreaterThan(0), "The DockPanel does not contain any buttons");
            Assert.That(_buttons.All(b => b.Height == 30), Is.True, "All buttons should have a Height of 30");
            Assert.That(_buttons.All(b => b.Width == 80), Is.True, "All buttons should have a Width of 80");
            Assert.That(_buttons.All(b => b.Margin.Left == 8), Is.True, "All buttons should have a Left Margin of 8");
            Assert.That(_buttons.All(b => b.Margin.Right == 8), Is.True, "All buttons should have a Right Margin of 8");
            Assert.That(_buttons.All(b => b.Margin.Top == 8), Is.True, "All buttons should have a Top Margin of 8");
            Assert.That(_buttons.All(b => b.Margin.Bottom == 8), Is.True, "All buttons should have a Bottom Margin of 8");
        }

        [MonitoredTest]
        public void _04_AllDockPanelChildrenShouldBeDockedCorrectly()
        {
            _02_TheDockPanelShouldContain4ButtonsAndAnImage();
            Assert.That(DockPanel.GetDock(_topButton!), Is.EqualTo(Dock.Top), "The Top Button should be docked on the top");
            Assert.That(DockPanel.GetDock(_rightButton!), Is.EqualTo(Dock.Right), "The Right Button should be docked on the right");
            Assert.That(DockPanel.GetDock(_bottomButton!), Is.EqualTo(Dock.Bottom), "The Bottom Button should be docked on the bottom");
            Assert.That(DockPanel.GetDock(_leftButton!), Is.EqualTo(Dock.Left), "The Left Button should be docked on the left");
        }
    }
}
