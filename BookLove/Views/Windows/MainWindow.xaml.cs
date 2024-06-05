using BookLove.Views.Pages.GuestPage;
using BookLove.Views.Pages.SystemPage;
using System;
using System.Windows;
using System.Windows.Threading;

namespace BookLove
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SessionId { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            SessionId = Guid.NewGuid().ToString();
            DispatcherTimer LiveTimer = new DispatcherTimer();
            LiveTimer.Interval = TimeSpan.FromSeconds(1);
            LiveTimer.Tick += LiveTimer_Tick;
            LiveTimer.Start();
            MainFrame.Navigate(new MarketBookPage());
        }

        private void LiveTimer_Tick(object sender, EventArgs e)
        {
            txbTime.Text = DateTime.Now.ToLongTimeString();
            txbDate.Text = DateTime.Now.ToLongDateString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
            GC.Collect();
        }

        private void MainFrame_ContentRendered(object sender, System.EventArgs e)
        {
            BackButton.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
