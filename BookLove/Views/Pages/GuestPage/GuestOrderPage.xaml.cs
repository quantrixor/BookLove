using BookLove.Context;
using BookLove.Model;
using BookLove.Views.Windows.GuestWindows;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookLove.Views.Pages.GuestPage
{
    public partial class GuestOrderPage : Page
    {
        private Login currentUser;

        public GuestOrderPage(Login user)
        {
            InitializeComponent();
            if (user == null)
            {
                MessageBox.Show("Ошибка: Данные пользователя не переданы.");
            }
            else
            {
                this.currentUser = user;
                LoadOrders();
            }
        }


        private void LoadOrders()
        {
            var registration = AppData.db.Registration.FirstOrDefault(r => r.idLogin == currentUser.id);
            if (registration == null)
            {
                MessageBox.Show("Регистрация пользователя не найдена.");
                return;
            }

            var orders = AppData.db.Order
                .Where(o => o.idUser == registration.id)
                .ToList();

            OrderListView.ItemsSource = orders;
        }

        public Order GetOrderWithDetails(int orderId)
        {
            var order = AppData.db.Order
                .Include("OrderStatus")
                .Include("OrderDetail") 
                .Include("OrderDetail.Book.Genre")
                .Include("OrderDetail.Book")
                .FirstOrDefault(o => o.id == orderId);

            return order;
        }
    }
}
