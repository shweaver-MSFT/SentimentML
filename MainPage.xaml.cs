using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SentimentML
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel _vm => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            _vm.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(_vm.IsInputExpanded):

                    if (_vm.IsInputExpanded)
                    {
                        ExpandInputButton.RenderTransform = new RotateTransform() {
                            Angle = 180,
                            CenterX = ExpandInputButton.ActualWidth / 2,
                            CenterY = ExpandInputButton.ActualHeight / 2
                        };
                        DocumentInputTextBox.TextWrapping = TextWrapping.Wrap;
                        DocumentInputTextBox.MinHeight = 300;
                        ExpandedInput.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ExpandInputButton.RenderTransform = default;
                        DocumentInputTextBox.TextWrapping = TextWrapping.NoWrap;
                        DocumentInputTextBox.MinHeight = default;
                        ExpandedInput.Visibility = Visibility.Collapsed;
                    }

                    break;
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _vm.RemoveDocumentCommand.Execute(e.ClickedItem);
        }
    }
}
