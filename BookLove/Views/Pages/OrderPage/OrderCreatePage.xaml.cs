using BookLove.Context;
using BookLove.Model;
using BookLove.Views.Pages.SystemPage;
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

namespace BookLove.Views.Pages.OrderPage
{
    /// <summary>
    /// Interaction logic for OrderCreatePage.xaml
    /// </summary>
    public partial class OrderCreatePage : Page
    {
        private int _idClient;
        private int _idProvider;
        private int _idBook;
        private int _balance = 1;
        private int _countProdcut { get; set; }
        private int _count = 1;
        private double _total { get; set; }
        private double _price { get; set; }
        public OrderCreatePage()
        {
            InitializeComponent();
            txbCount.Visibility = Visibility.Hidden;
            cmbBook.ItemsSource = AppData.db.Book.ToList();
            cmbBook.DisplayMemberPath = "GetFullTitle";
            cmbClient.ItemsSource = AppData.db.Client.ToList();
            cmbBook.DisplayMemberPath = "GetFullTitle";
            cmbProvider.ItemsSource = AppData.db.Provider.ToList();
            cmbProvider.DisplayMemberPath = "title";

        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            txbCount.Visibility = Visibility.Visible;
            if (_countProdcut != 0)
            {
                if (_countProdcut == _count)
                {
                    MessageBox.Show("Вы первысили количество товара, которое имеется в наличии склада.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    _count++;
                    _balance = _countProdcut - _count;
                    _total += _price;

                }
                txbTotalPrice.Text = "Итого: " + _total.ToString() + " руб.";
                txbCount.Text = _count.ToString();
            }
            else
            {
                MessageBox.Show("Вы первысили количество товара, которое имеется в наличии склада.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (_count != 0)
            {
                _count--;
                _total -= _price;
            }
            _balance = _countProdcut + _count;
            txbTotalPrice.Text = "Итого: " + _total.ToString() + " руб.";
            txbCount.Text = _count.ToString();
        }

        private void ComboBoxProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _idProvider = (cmbProvider.SelectedItem as Provider).id;

        }

        private void ComboBoxBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _idBook = (cmbBook.SelectedItem as Book).id;
            _total = (cmbBook.SelectedItem as Book).price;
            _countProdcut = (cmbBook.SelectedItem as Book).count;
            _price = _total;
            txbCount.Visibility = Visibility.Visible;
            if (_idBook != 0)
            {
                _count = 1;
                txbCount.Text = _count.ToString();
                txbTotalPrice.Text = "Итого: " + _total.ToString() + " руб.";
            }
            else
            {
                MessageBox.Show("Данного товара нет в наличии.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txbCount.Text = "0";
                txbTotalPrice.Text = "0";
            }
        }

        private void ComboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _idClient = (cmbClient.SelectedItem as Client).id;
        }

        private void RemoveSelectedBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = BasketList.SelectedItem as Basket;
                if (selectedItem != null)
                {
                    AppData.db.Basket.Remove(selectedItem);
                    AppData.db.SaveChanges();
                    BasketList.ItemsSource = AppData.db.Basket.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Supple supple = new Supple();
                Basket basket = new Basket();
                Book book = new Book();
                supple.idClient = _idClient;
                supple.idBook = _idBook;
                supple.dateSupple = DateTime.Now;
                supple.count = _count;
                supple.totalPrice = _total;
                supple.idProvider = _idProvider;
                supple.description = txbDescription.Text;
                AppData.db.Supple.Add(supple);
                var selectedCount = AppData.db.Book.FirstOrDefault(item => item.id == supple.idBook);
                selectedCount.count = _balance;
                basket.idClient = _idClient;
                basket.idBook = _idBook;
                basket.idProvider = _idProvider;
                basket.totalPrice = _total;
                basket.count = _count;
                AppData.db.Basket.Add(basket);
                AppData.db.SaveChanges();
                BasketList.ItemsSource = AppData.db.Basket.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonFinishOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var basket = AppData.db.Basket.ToList();
                AppData.db.Basket.RemoveRange(basket);
                AppData.db.SaveChanges();
                PrintDialog print = new PrintDialog();
                if (print.ShowDialog() == true)
                {
                    print.PrintVisual(BasketList, "Check");
                }
                MessageBox.Show("Оформление чека прошло успешно.", "Чек сохранен!", MessageBoxButton.OK, MessageBoxImage.Information);
                BasketList.ItemsSource = AppData.db.Basket.ToList();
                NavigationService.Navigate(new MenuPage());
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
