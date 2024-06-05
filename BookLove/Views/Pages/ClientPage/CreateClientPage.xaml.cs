using BookLove.Context;
using BookLove.Model;
using BookLove.Views.Pages.OrderPage;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookLove.Views.Pages.ClientPage
{
    /// <summary>
    /// Interaction logic for CreateClientPage.xaml
    /// </summary>
    public partial class CreateClientPage : Page
    {
        public Client Client { get; set; }
        public List<City> Cities{ get; set; }
        
        public CreateClientPage(Client client)
        {
            InitializeComponent();
            Cities = AppData.db.City.ToList();
            Client = client;
            this.DataContext = this;
        }

        private void SaveClient_Click(object sender, RoutedEventArgs e)
        {
            if(Client.id == 0)
            {
                AppData.db.Client.Add(Client);
            }
            AppData.db.SaveChanges();
            MessageBox.Show("Клиент успешно зарегистрирован!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new OrderCreatePage());
        }

        private void CreateSkipClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderCreatePage());
        }
    }
}
