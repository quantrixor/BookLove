using BookLove.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookLove.Views.Pages.OrderPage
{
    /// <summary>
    /// Логика взаимодействия для OrdersViewPage.xaml
    /// </summary>
    public partial class OrdersViewPage : Page
    {
        public OrdersViewPage()
        {
            InitializeComponent();
            this.DataContext = new OrdersViewModel();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as OrdersViewModel;
            if (vm != null)
            {
                vm.SaveChanges();
            }
        }
    }
}
