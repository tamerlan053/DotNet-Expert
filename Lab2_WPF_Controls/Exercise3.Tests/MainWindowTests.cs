using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;

namespace Exercise3.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Controls", "Exercise3", @"Exercise3\MainWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window = null!;
        private GroupBox? _ageGroupBox;
        private GroupBox? _genderGroupBox;
        private Canvas? _canvas;

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new MainWindow();
            _window.Show();

            _canvas = _window.FindVisualChildren<Canvas>().FirstOrDefault();
            var groupBoxes = _window.FindVisualChildren<GroupBox>().ToList();
            _ageGroupBox = groupBoxes.FirstOrDefault(box => (box.Header as string)?.ToLower() == "leeftijd");
            _genderGroupBox = groupBoxes.FirstOrDefault(box => (box.Header as string)?.ToLower() == "geslacht");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();
            _window.Close();
        }

        [MonitoredTest]
        public void _1_ShouldNotHaveChangedTheCodebehindFile()
        {
            var codeBehindFilePath = @"Exercise3\MainWindow.xaml.cs";
            var fileHash = Solution.Current.GetFileHash(codeBehindFilePath);
            Assert.That(fileHash, Is.EqualTo("FF-3A-83-89-31-B6-4D-0C-A0-5F-37-E9-F8-B1-9C-91"), () => $"The file '{codeBehindFilePath}' has changed. " +
                                                                             "Undo your changes on the file to make this test pass. " +
                                                                             "This exercise can be completed by purely working with XAML.");
        }

        [MonitoredTest]
        public void _2_ShouldHaveAnAgeGroupBox()
        {
            AssertHasAgeGroupBox();
            var stackPanel = _ageGroupBox!.Content as StackPanel;
            Assert.That(stackPanel, Is.Not.Null, () => "The 'Content' of the 'GroupBox' should be a 'StackPanel' that can contain multiple child controls.");
            var checkboxes = stackPanel!.Children.OfType<CheckBox>().ToList();
            Assert.That(checkboxes, Has.Count.EqualTo(3), () => "The 'StackPanel' in the 'GroupBox' should have 3 'CheckBox' controls.");
            Assert.That(checkboxes.First().IsChecked, Is.True, () => "The first 'CheckBox' should have its 'IsChecked' property set to true");
        }

        [MonitoredTest]
        public void _3_ShouldHaveAGenderGroupBox()
        {
            AssertHasGenderGroupBox();
            var stackPanel = _genderGroupBox!.Content as StackPanel;
            Assert.That(stackPanel, Is.Not.Null, () => "The 'Content' of the 'GroupBox' should be a 'StackPanel' that can contain multiple child controls.");
            var checkboxes = stackPanel!.Children.OfType<RadioButton>().ToList();
            Assert.That(checkboxes, Has.Count.EqualTo(2), () => "The 'StackPanel' in the 'GroupBox' should have 2 'RadioButton' controls.");
            Assert.That(checkboxes.First().IsChecked, Is.True, () => "The first 'RadioButton' should have its 'IsChecked' property set to true");
        }

        [MonitoredTest]
        public void _4_ShouldHaveAllControlsInACanvas()
        {
            AssertHasAgeGroupBox();
            AssertHasGenderGroupBox();
            Assert.That(_canvas, Is.Not.Null, () => "A 'Canvas' control could not be found");
            Assert.That(_ageGroupBox!.Parent, Is.EqualTo(_canvas), () => "The age 'GroupBox' is not in the 'Canvas'.");
            Assert.That(_genderGroupBox!.Parent, Is.EqualTo(_canvas), () => "The gender 'GroupBox' is not in the 'Canvas'.");

            AssertUsesCanvasPositioning(_ageGroupBox, "age GroupBox");
            AssertUsesCanvasPositioning(_genderGroupBox, "gender GroupBox");
        }

        private void AssertHasAgeGroupBox()
        {
            Assert.That(_ageGroupBox, Is.Not.Null, () => "A groupbox with header equal to 'Leeftijd' could not be found.");
        }

        private void AssertHasGenderGroupBox()
        {
            Assert.That(_genderGroupBox, Is.Not.Null, () => "A groupbox with header equal to 'Geslacht' could not be found.");
        }

        private void AssertUsesCanvasPositioning(Control control, string controlName)
        {
            Assert.That(Canvas.GetLeft(control), Is.GreaterThan(0),
                () => ShowCanvasPositioningMessage(controlName, "Canvas.Left"));
            Assert.That(Canvas.GetTop(control), Is.GreaterThan(0),
                () => ShowCanvasPositioningMessage(controlName, "Canvas.Top"));

            Assert.That(control.Margin.Left == 0 && control.Margin.Top == 0, Is.True,
                () =>
                    $"The '{controlName}' has a 'Margin' set, but that is not necessary when positioning controls in a 'Canvas'. " +
                    "Use the attached properties 'Canvas.Left' and 'Canvas.Top' instead.");
        }

        private string ShowCanvasPositioningMessage(string controlName, string attachedProperty)
        {
            return
                $"The '{controlName}' should use the attached property '{attachedProperty}' to position itself in the 'Canvas'.";
        }
    }
}