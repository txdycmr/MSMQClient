using MSMQClient.Business;
using MSMQClient.Model;
using MSMQClient.ViewModel;
using System.Windows;

namespace MSMQClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageService = new MessageService();
                messageService.Insert(new MessageInfo { Id = 1, Content = "Hello, MSMQ" });
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Stop();
        }
    }
}
