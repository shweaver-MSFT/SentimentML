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
                        ExpandedInput.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ExpandInputButton.RenderTransform = default;
                        ExpandedInput.Visibility = Visibility.Collapsed;
                    }
                    break;

                case nameof(_vm.IsRequestPanelExpanded):

                    if (_vm.IsRequestPanelExpanded)
                    {
                        InputPanel.MaxWidth = 72;
                        HiddenPanel.Visibility = Visibility.Visible;
                        ToggleRequestPanelButton.RenderTransform = new RotateTransform()
                        {
                            Angle = 180,
                            CenterX = ToggleRequestPanelButton.ActualWidth / 2,
                            CenterY = ToggleRequestPanelButton.ActualHeight / 2
                        };
                    }
                    else
                    {
                        InputPanel.MaxWidth = 400;
                        HiddenPanel.Visibility = Visibility.Collapsed;
                        ToggleRequestPanelButton.RenderTransform = default;
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
