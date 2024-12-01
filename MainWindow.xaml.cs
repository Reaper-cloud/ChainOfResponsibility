using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly RequestHandler _handlerChain;

        public MainWindow()
        {
            InitializeComponent();
            _handlerChain = CreateHandlerChain();
        }

        private RequestHandler CreateHandlerChain()
        {
            var loggingHandler = new LoggingHandler();
            var authenticationHandler = new AuthenticationHandler();
            var notificationHandler = new NotificationHandler();

            loggingHandler.SetNext(authenticationHandler);
            authenticationHandler.SetNext(notificationHandler);

            return loggingHandler;
        }

        private void SendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestTypeComboBox.SelectedItem is ComboBoxItem selectedItem &&
                !string.IsNullOrWhiteSpace(RequestContentTextBox.Text))
            {
                var requestType = selectedItem.Content.ToString() switch
                {
                    "Logging" => RequestType.Logging,
                    "Authentication" => RequestType.Authentication,
                    "Notification" => RequestType.Notification,
                    _ => throw new InvalidOperationException("Unknown request type")
                };

                var request = new Request(requestType, RequestContentTextBox.Text);
                OutputTextBlock.Text = string.Empty; // Clear previous output

                // Обработка запроса и получение результата
                var result = _handlerChain.HandleRequest(request);

                // Отображение результата
                if (result != null)
                {
                    OutputTextBlock.Text = result;
                }
                else
                {
                    OutputTextBlock.Text = "No handler for this request.";
                }
            }
            else
            {
                MessageBox.Show("Please select a request type and enter content.");
            }
        }
    }
}
