using SentimentML.Common;
using SentimentML.SentimentV3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml.Controls;

namespace SentimentML
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand AddDocumentCommand { get; }
        public RelayCommand ClearOutputCommand { get; }
        public RelayCommand CopyResponseContentCommand { get; }
        public RelayCommand<TextAnalyticsInput> RemoveDocumentCommand { get; }
        public RelayCommand ResetInputCommand { get; }
        public RelayCommand SaveResponseContentCommand { get; }
        public RelayCommand SubmitInputCommand { get; }
        public RelayCommand ToggleExpandedInputCommand { get; }
        public RelayCommand ToggleRequestPanelCommand { get; }
        public RelayCommand UploadDocumentCommand { get; }

        private ObservableCollection<TextAnalyticsInput> _documentInputs;
        public ObservableCollection<TextAnalyticsInput> DocumentInputs
        {
            get => _documentInputs;
            set => Set(ref _documentInputs, value);
        }

        private string _documentText;
        public string DocumentText
        {
            get => _documentText;
            set => Set(ref _documentText, value);
        }

        private bool _isInputEnabled;
        public bool IsInputEnabled
        {
            get => _isInputEnabled;
            set => Set(ref _isInputEnabled, value);
        }

        private bool _isInputExpanded;
        public bool IsInputExpanded
        {
            get => _isInputExpanded;
            set => Set(ref _isInputExpanded, value);
        }

        private bool _isRequestPanelExpanded;
        public bool IsRequestPanelExpanded
        {
            get => _isRequestPanelExpanded;
            set => Set(ref _isRequestPanelExpanded, value);
        }

        private string _languageCode;
        public string LanguageCode
        {
            get => _languageCode;
            set => Set(ref _languageCode, value);
        }

        private RequestStatistics _requestStatistics;
        public RequestStatistics RequestStatistics
        {
            get => _requestStatistics;
            set => Set(ref _requestStatistics, value);
        }

        private ObservableCollection<DocumentSentiment> _responseDocuments;
        public ObservableCollection<DocumentSentiment> ResponseDocuments
        {
            get => _responseDocuments;
            set => Set(ref _responseDocuments, value);
        }

        private ObservableCollection<ErrorRecord> _responseErrors;
        public ObservableCollection<ErrorRecord> ResponseErrors
        {
            get => _responseErrors;
            set => Set(ref _responseErrors, value);
        }

        private string _responseContent;
        public string ResponseContent
        {
            get => _responseContent;
            set => Set(ref _responseContent, value);
        }

        private DocumentSentiment _selectedDocument;
        public DocumentSentiment SelectedDocument
        {
            get => _selectedDocument;
            set => Set(ref _selectedDocument, value);
        }

        private SentenceSentiment _selectedSentence;
        public SentenceSentiment SelectedSentence
        {
            get => _selectedSentence;
            set => Set(ref _selectedSentence, value);
        }

        private string _textAnalyticsUrl;
        public string TextAnalyticsUrl
        {
            get => _textAnalyticsUrl;
            set => Set(ref _textAnalyticsUrl, value);
        }

        private string _textAnalyticsKey;
        public string TextAnalyticsKey
        {
            get => _textAnalyticsKey;
            set => Set(ref _textAnalyticsKey, value);
        }

        public MainViewModel()
        {
            AddDocumentCommand = new RelayCommand(AddDocument);
            ClearOutputCommand = new RelayCommand(ClearOutput);
            CopyResponseContentCommand = new RelayCommand(CopyResponseContent);
            RemoveDocumentCommand = new RelayCommand<TextAnalyticsInput>(RemoveDocument);
            ResetInputCommand = new RelayCommand(ResetInput);
            SaveResponseContentCommand = new RelayCommand(SaveResponseContent);
            SubmitInputCommand = new RelayCommand(SubmitInput);
            ToggleExpandedInputCommand = new RelayCommand(ToggleExpandedInput);
            ToggleRequestPanelCommand = new RelayCommand(ToggleRequestPanel);
            UploadDocumentCommand = new RelayCommand(UploadDocument);

            _documentInputs = new ObservableCollection<TextAnalyticsInput>();
            _documentText = string.Empty;
            _isInputEnabled = true;
            _isInputExpanded = false;
            _requestStatistics = null;
            _responseContent = string.Empty;
            _responseDocuments = new ObservableCollection<DocumentSentiment>();
            _responseErrors = new ObservableCollection<ErrorRecord>();
            _selectedDocument = null;
            _selectedSentence = null;

            var localSettings = ApplicationData.Current.LocalSettings;
            _languageCode = localSettings.Values[nameof(LanguageCode)] as string ?? "en";
            _textAnalyticsKey = localSettings.Values[nameof(TextAnalyticsKey)] as string ?? string.Empty;
            _textAnalyticsUrl = localSettings.Values[nameof(TextAnalyticsUrl)] as string ?? string.Empty;

            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(LanguageCode):
                    ApplicationData.Current.LocalSettings.Values[nameof(LanguageCode)] = _languageCode;
                    break;
                case nameof(TextAnalyticsKey):
                    ApplicationData.Current.LocalSettings.Values[nameof(TextAnalyticsKey)] = _textAnalyticsKey;
                    break;
                case nameof(TextAnalyticsUrl):
                    ApplicationData.Current.LocalSettings.Values[nameof(TextAnalyticsUrl)] = _textAnalyticsUrl;
                    break;
            }
        }

        private void AddDocument()
        {
            if (string.IsNullOrWhiteSpace(_documentText))
            {
                return;
            }

            var document = new TextAnalyticsInput()
            {
                Id = Guid.NewGuid().ToString(),
                Text = _documentText.Trim(),
                LanguageCode = _languageCode
            };
            DocumentInputs.Add(document);

            DocumentText = string.Empty;
        }

        private void ClearOutput()
        {
            SelectedDocument = null;
            SelectedSentence = null;
            RequestStatistics = null;
            ResponseContent = string.Empty;
            ResponseDocuments.Clear();
            ResponseErrors.Clear();
        }

        private void CopyResponseContent()
        {
            DataPackage dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(_responseContent);
            Clipboard.SetContent(dataPackage);
        }

        private void UploadDocument()
        {
            // TODO: Show FilePicker and extract contents as text.
        }

        private void ToggleExpandedInput()
        {
            IsInputExpanded = !_isInputExpanded;
        }

        private void ToggleRequestPanel()
        {
            IsRequestPanelExpanded = !_isRequestPanelExpanded;
        }

        private void RemoveDocument(TextAnalyticsInput document)
        {
            DocumentInputs.Remove(document);
        }

        private async void SaveResponseContent()
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
                SuggestedFileName = "sentiment",
            };
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".json" });
            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);// write to file

                await FileIO.WriteTextAsync(file, _responseContent);

                // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        private async void SubmitInput()
        {
            try
            {
                IsInputEnabled = false;

                var docsById = new Dictionary<string, TextAnalyticsInput>();
                foreach (var doc in _documentInputs)
                {
                    docsById.Add(doc.Id, doc);
                }

                var inputDocuments = new TextAnalyticsBatchInput()
                {
                    Documents = _documentInputs
                };

                var client = new TextAnalyticsSentimentV3Client(_textAnalyticsUrl, _textAnalyticsKey);
                var response = await client.SentimentV3PreviewPredictAsync(inputDocuments);

                ClearOutput();

                if (response.Errors.Count > 0)
                {
                    foreach (var error in response.Errors)
                    {
                        ResponseErrors.Add(error);
                    }
                    return;
                }

                RequestStatistics = response.Statistics;
                foreach (var document in response.Documents)
                {
                    var documentText = docsById[document.Id].Text;
                    foreach (var sentence in document.Sentences)
                    {
                        sentence.SetOriginalText(documentText);
                    }

                    ResponseDocuments.Add(document);
                }

                SelectedDocument = ResponseDocuments.First();
                SelectedSentence = SelectedDocument.Sentences.First();
                ResponseContent = response.ResponseContent.Replace(",", ",\n").Replace("{", "{\n").Replace("}", "\n}");
            }
            catch(Exception e)
            {
                var contentDialog = new ContentDialog()
                {
                    Title = "An error has occurred",
                    Content = e.Message,
                    CloseButtonText = "Ok"
                };

                await contentDialog.ShowAsync();
            }
            finally
            {
                IsInputEnabled = true;
            }
        }

        private void ResetInput()
        {
            DocumentText = string.Empty;
            DocumentInputs.Clear();
        }

        private void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
