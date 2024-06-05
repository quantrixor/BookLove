using BookLove.Model;
using BookLove.Views.Pages.BookPage;
using BookLove.Views.Pages.ClientPage;
using BookLove.Views.Pages.OrderPage;
using BookLove.Views.Pages.ProviderPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookLove.Views.Pages.SystemPage
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void BookActionPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BookViewPage());
        }

        private void CreateOrderPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateClientPage(new Client()));
        }

        private void HistoryPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HistoryOrderPage());
        }

        private void ProviderButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProviderViewPage());
        }

        private void OrderView_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersViewPage());
        }
    }
}
