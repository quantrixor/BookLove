using BookLove.Context;
using BookLove.Model;
using BookLove.Views.Pages.GuestPage;
using BookLove.Views.Pages.SystemPage;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookLove.Views.Pages.BookPage
{
    public partial class BasketPage : Page
    {
        private string sessionId;
        public BasketPage(string sessionId)
        {
            InitializeComponent();
            this.sessionId = sessionId;
            LoadBasket();
            UpdateTotalPrice();
            if(IsUserLoggedIn())
            {
                UserOrders.Visibility = Visibility.Visible;
            }
        }

        private void LoadBasket()
        {
            var basketItems = AppData.db.GuestBasket.Where(b => b.sessionId == sessionId).ToList();
            BasketListView.ItemsSource = basketItems;
        }

        private void UpdateTotalPrice()
        {
            string totalPrice;
            try
            {
                totalPrice = AppData.db.GuestBasket.Where(b => b.sessionId == sessionId).Sum(b => b.totalPrice).ToString();
            }
            catch
            {
                totalPrice = "0.00";
            }
            txtTotalPrice.Text = $"{totalPrice} руб.";
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int itemId = (int)button.Tag;
            var itemToRemove = AppData.db.GuestBasket.FirstOrDefault(b => b.id == itemId);
            if (itemToRemove != null)
            {
                AppData.db.GuestBasket.Remove(itemToRemove);
                AppData.db.SaveChanges();
                LoadBasket();
                UpdateTotalPrice();
            }
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUserLoggedIn())
            {
                MessageBox.Show("Пожалуйста, войдите или зарегистрируйтесь перед оформлением заказа.");
                NavigationService.Navigate(new LoginPage());
            }
            else
            {
                CompleteOrder();
            }
        }

        private bool IsUserLoggedIn()
        {
            // Проверка авторизации пользователя.
            return (Application.Current as App).CurrentUser != null;
        }
        public Login GetCurrentUser()
        {
            if (IsUserLoggedIn())
            {
                return (Application.Current as App).CurrentUser;
            }
            else
            {
                return null; // Или можно выбросить исключение, если предпочтительнее строгая проверка
            }
        }

        private void CompleteOrder()
        {
            var currentUser = (Application.Current as App).CurrentUser;
            var address = txtAddress.Text;
            var phoneNumber = txtPhoneNumber.Text;

            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Пожалуйста, введите адрес и номер телефона.");
                return;
            }
            var registration = AppData.db.Registration.FirstOrDefault(r => r.idLogin == currentUser.id);
            if (registration == null)
            {
                MessageBox.Show("Регистрация пользователя не найдена.");
                return;
            }
            var order = new Order
            {
                idUser = registration.id,
                orderDate = DateTime.Now,
                statusId = 1, // 'processing' статус
                address = address,
                totalPrice = CalculateTotalPrice()
            };

            AppData.db.Order.Add(order);
            AppData.db.SaveChanges();

            foreach (var item in AppData.db.GuestBasket.Where(b => b.sessionId == this.sessionId))
            {
                var book = AppData.db.Book.FirstOrDefault(b => b.id == item.idBook);
                if (book != null && book.count >= item.count)
                {
                    book.count -= item.count;
                    var orderDetail = new OrderDetail
                    {
                        idOrder = order.id,
                        idBook = book.id,
                        count = item.count,
                        unitPrice = item.totalPrice
                    };
                    AppData.db.OrderDetail.Add(orderDetail);
                    AppData.db.GuestBasket.Remove(item);
                }
                else
                {
                    MessageBox.Show($"Недостаточно книг '{book.title}' на складе.");
                    return;
                }
            }

            AppData.db.SaveChanges();
            MessageBox.Show("Ваш заказ успешно оформлен!");
            NavigationService.GoBack();
        }

        private double CalculateTotalPrice()
        {
            try
            {
                return AppData.db.GuestBasket.Where(b => b.sessionId == this.sessionId).Sum(b => b.totalPrice);
            }
            catch
            {
                return 0.00;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTotalPrice();
        }

        private void UserOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GuestOrderPage(GetCurrentUser()));
        }
    }
}
