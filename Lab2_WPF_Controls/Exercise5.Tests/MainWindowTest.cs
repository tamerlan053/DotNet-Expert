using System.Windows.Controls;
using System.Windows.Threading;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise5.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Controls", "Exercise5", @"Exercise5\MainWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window = null!;
        private ListView? _listView;
        private List<ListViewItem> _listViewItems = [];

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new MainWindow();
            _window.Show();

            _listView = _window.FindVisualChildren<ListView>().FirstOrDefault();
            if (_listView != null)
            {
                _listViewItems = _listView.Items.OfType<ListViewItem>().ToList();
            }
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
            var codeBehindFilePath = @"Exercise5\MainWindow.xaml.cs";
            var fileContent = Guts.Client.Core.TestTools.Solution.Current.GetFileContent(codeBehindFilePath);
            Assert.That(fileContent.Length, Is.LessThanOrEqualTo(200), () => $"The file '{codeBehindFilePath}' has changed. " +
                                                                                    "Undo your changes on the file to make this test pass. " + "This exercise can be completed by purely working with XAML."); 
        }

        [MonitoredTest]
        public void _2_ShouldHaveAListView()
        {
            AssertHasListView();
        }

        [MonitoredTest]
        public void _3_TheListShouldHave3Items()
        {
            AssertHasListView();
            AssertHasListViewItems();
        }

        [MonitoredTest]
        public void _4_EachListItemShouldBeCorrectlyFormatted()
        {
            AssertHasListView();
            AssertHasListViewItems();

            for (var index = 0; index < _listViewItems.Count; index++)
            {
                var listViewItem = _listViewItems[index];
                AssertListViewItem(listViewItem, index + 1);
            }
        }

        private void AssertHasListView()
        {
            Assert.That(_listView, Is.Not.Null, () => "No 'ListView' control is found.");
        }

        private void AssertHasListViewItems()
        {
            Assert.That(_listViewItems, Has.Count.EqualTo(3),
                () => "The 'ListView' should contain 3 instances of 'ListViewItem'.");
        }

        private void AssertListViewItem(ListViewItem item, int positionInTree)
        {
            var stackPanel = item.Content as StackPanel;

            Assert.That(stackPanel, Is.Not.Null,
                () =>
                    $"The 'ListViewItem' at position {positionInTree} should have a 'StackPanel' as 'Content'.");
            Assert.That(stackPanel!.Orientation, Is.EqualTo(Orientation.Horizontal),
                () =>
                    $"The 'ListViewItem' at position {positionInTree} should have a 'StackPanel' with a horizontal 'Orientation'.");
            Assert.That(stackPanel.Children, Has.Count.EqualTo(2),
                () =>
                    $"The 'ListViewItem' at position {positionInTree} should have a 'StackPanel' with 2 child controls.");

            var image = stackPanel.Children[0] as Image;
            Assert.That(image, Is.Not.Null,
                () =>
                    $"The 'StackPanel' of the 'ListViewItem' at position {positionInTree} should have an 'Image' control.");

            var textBlock = stackPanel.Children[1] as TextBlock;
            Assert.That(textBlock, Is.Not.Null,
                () =>
                    $"The 'StackPanel' of the 'ListViewItem' at position {positionInTree} should have an 'TextBlock' control.");
        }
    }
}