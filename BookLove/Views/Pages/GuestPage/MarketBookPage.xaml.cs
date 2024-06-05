using BookLove.Context;
using BookLove.Model;
using BookLove.Views.Pages.BookPage;
using BookLove.Views.Pages.SystemPage;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookLove.Views.Pages.GuestPage
{
    public partial class MarketBookPage : Page
    {
        private string sessionId;
        private bool isUserLoggedIn;
        private Login _user;
        public MarketBookPage(Login user = null)
        {
            InitializeComponent();
            sessionId = (Application.Current.MainWindow as MainWindow)?.SessionId;

            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString();
            }

            if (user != null)
            {
                isUserLoggedIn = true;
                btnSignIn.Visibility = Visibility.Collapsed;
                btnSignOut.Visibility = Visibility.Visible;
                _user = user;
            }
            else
            {
                isUserLoggedIn = false;
            }
        }

        private void btnBasket_Click(object sender, RoutedEventArgs e)
        {
            // Открыть окно корзины
            NavigationService.Navigate(new BasketPage(sessionId));
        }

        private void AddToBasket(Book book)
        {
            var existingEntry = AppData.db.GuestBasket.FirstOrDefault(b => b.sessionId == sessionId && b.idBook == book.id);

            if (existingEntry != null)
            {
                existingEntry.count++;
                existingEntry.totalPrice += book.price;
            }
            else
            {
                AppData.db.GuestBasket.Add(new GuestBasket
                {
                    sessionId = sessionId,
                    idBook = book.id,
                    count = 1,
                    totalPrice = book.price
                });
            }

            AppData.db.SaveChanges();
            MessageBox.Show($"Книга '{book.title}' добавлена в корзину.");
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void Update(string filter = "", string search = "")
        {
            var books = AppData.db.Book.ToList();
            if (!string.IsNullOrEmpty(filter))
            {
                if (filter == "Роман" || filter == "Саморазвитие" || filter == "Детектив" || filter == "Дорама")
                {
                    books = AppData.db.Book.Where(item => item.Genre.title == filter).ToList();
                }
            }
            if (!string.IsNullOrEmpty(search))
                books = books.Where(item => item.title.ToLower().Contains(txbSearch.Text.ToLower()) || item.Author.GetFullName.ToLower().Contains(search.ToLower())).ToList();
            BookListView.ItemsSource = books;
        }

        private void cmbFilterGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update((cmbFilterGender.SelectedItem as ComboBoxItem).Content.ToString(), txbSearch.Text);
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update(cmbFilterGender.Text, txbSearch.Text);
        }

        private void btnAddBasket_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookListView.SelectedItem as Book;
            if (selectedBook != null)
            {
                AddToBasket(selectedBook);
            }
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            _user = null;
            btnSignIn.Visibility = Visibility.Visible;
            btnSignOut.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
