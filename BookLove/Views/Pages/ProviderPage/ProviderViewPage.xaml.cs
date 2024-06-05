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
using BookLove.Context;
using BookLove.Model;

namespace BookLove.Views.Pages.ProviderPage
{
    /// <summary>
    /// Interaction logic for ProviderViewPage.xaml
    /// </summary>
    public partial class ProviderViewPage : Page
    {
        
        public ProviderViewPage()
        {
            InitializeComponent();
        }

        private void ButtonAddProvider_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProviderActionPage(new Provider()));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ProviderList.ItemsSource = AppData.db.Provider.ToList();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ProviderList.SelectedItem as Provider;
            if (selectedItem != null)
            {
                NavigationService.Navigate(new ProviderActionPage(selectedItem));
            }
            else
                MessageBox.Show("Выберите поставщика, которого надо отредактировать.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void RemoveProvider_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ProviderList.SelectedItem as Provider;
            if (selectedItem != null)
            {
                if(MessageBox.Show("Вы действительно хотите удалить данные о поставщиках?", "Пожалуйста, подтвердите!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.db.Provider.Remove(selectedItem);
                    AppData.db.SaveChanges();
                    Page_Loaded(null, null);
                }
            }
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProviderList.ItemsSource = AppData.db.Provider.Where(item => item.title.Contains(txbSearch.Text) || item.city.Contains(txbSearch.Text)).ToList();
        }
    }
}
