using System.Windows.Controls;
using System.Windows.Threading;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;

namespace Exercise4.Tests
{
    [ExerciseTestFixture("dotnetExp", "H2-Controls", "Exercise4", @"Exercise4\MainWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window = null!;
        private TreeView? _treeView;
        private List<TreeViewItem> _continentItems = [];

        [OneTimeSetUp]
        public void Setup()
        {
            _window = new MainWindow();
            _window.Show();

            _treeView = _window.FindVisualChildren<TreeView>().FirstOrDefault();
            if (_treeView != null)
            {
                _continentItems = _treeView.Items.OfType<TreeViewItem>().ToList();
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
            var codeBehindFilePath = @"Exercise4\MainWindow.xaml.cs";
            var fileContent = Solution.Current.GetFileContent(codeBehindFilePath);
            Assert.That(fileContent.Length, Is.LessThanOrEqualTo(200), () => $"The file '{codeBehindFilePath}' has changed. " +
                                                                             "Undo your changes on the file to make this test pass. " +
                                                                             "This exercise can be completed by purely working with XAML.");
        }

        [MonitoredTest]
        public void _2_ShouldHaveATree()
        {
            AssertHasTree();
        }

        [MonitoredTest]
        public void _3_TheTreeShouldHave2ContinentItems()
        {
            AssertHasTree();
            AssertHasContinentItems();
            Assert.That(_continentItems, Has.All.Matches((TreeViewItem item) => (item.Header as string)?.Length > 0), () => "The 'Header' of the continent items should contain simple text.");
        }

        [MonitoredTest]
        public void _4_EachContinentItemInTheTreeShouldHave3CountryItems()
        {
            AssertHasTree();
            AssertHasContinentItems();
            Assert.That(_continentItems.First().Items.OfType<TreeViewItem>().ToList(), Has.Count.EqualTo(3), () => "There should be 3 instances of 'TreeViewItem' in the items collection of the 'North America' 'TreeViewItem'.");
            Assert.That(_continentItems.ElementAt(1).Items.OfType<TreeViewItem>().ToList(), Has.Count.EqualTo(3), () => "There should be 3 instances of 'TreeViewItem' in the items collection of the 'South America' 'TreeViewItem'.");
        }

        [MonitoredTest]
        public void _5_EachCountryItemInTheTreeShouldBeCorrectlyFormatted()
        {
            AssertHasTree();
            AssertHasContinentItems();

            var allCountryItems = _continentItems.SelectMany(continentItem => continentItem.Items.OfType<TreeViewItem>()).ToList();
            for (var index = 0; index < allCountryItems.Count; index++)
            {
                var countryItem = allCountryItems[index];
                AssertCountryItem(countryItem, index + 1);
            }
        }

        private void AssertHasTree()
        {
            Assert.That(_treeView, Is.Not.Null, () => "No 'TreeView' control is found.");
        }

        private void AssertHasContinentItems()
        {
            Assert.That(_continentItems, Has.Count.EqualTo(2),
                () => "The 'TreeView' should contain 2 instances of 'TreeViewItem'.");
        }

        private void AssertCountryItem(TreeViewItem countryItem, int positionInTree)
        {
            var stackPanel = countryItem.Header as StackPanel;

            Assert.That(stackPanel, Is.Not.Null,
                $"The country 'TreeViewItem' at position {positionInTree} should have a 'StackPanel' in its 'Header'.");
            Assert.That(stackPanel!.Orientation, Is.EqualTo(Orientation.Horizontal),
                $"The country 'TreeViewItem' at position {positionInTree} should have a 'StackPanel' with a horizontal 'Orientation'.");
            Assert.That(stackPanel.Children, Has.Count.EqualTo(2),
                $"The country 'TreeViewItem' at position {positionInTree} should have a 'StackPanel' with 2 child controls.");

            Assert.That(countryItem.Margin.Top > 0 || stackPanel.Margin.Top > 0, Is.True,
                "There should be some space between the country items. " +
                "You can achieve this by adding some 'Margin' on the top of the 'TreeViewItem' or the 'StackPanel'. " +
                $"No top margin could by found for the country at position {positionInTree}.");

            var textBlock = stackPanel.Children[0] as TextBlock;
            Assert.That(textBlock, Is.Not.Null,
                $"The 'StackPanel' of the country at position {positionInTree} should have a 'TextBlock' control as the first element.");

            var image = stackPanel.Children[1] as Image;
            Assert.That(image, Is.Not.Null,
                $"The 'StackPanel' of the country at position {positionInTree} should have an 'Image' control as the second element.");
            Assert.That(image!.Source, Is.Not.Null,
                $"The 'Source' of the 'Image' of the country at position {positionInTree} should be set.");
            Assert.That(image.Width > 0 || image.Height > 0, Is.True,
                $"The width or height of the image of the country at position {positionInTree} must be set. " +
                "Otherwise the image will be too big.");

            Assert.That(textBlock!.Margin.Right > 0 || image.Margin.Left > 0, Is.True,
                $"There should be some space (Margin) between the 'TextBlock' and the 'Image' of the country at position {positionInTree}.");
        }
    }
}
