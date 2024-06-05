using BookLove.Context;
using BookLove.Model;
using BookLove.Views.Pages.BookPage;
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

namespace BookLove.Views.Pages.PublisherPage
{
    /// <summary>
    /// Interaction logic for PublisherAddPage.xaml
    /// </summary>
    public partial class PublisherAddPage : Page
    {
        public Publisher Publisher { get; set; }
        public PublisherAddPage(Publisher publisher)
        {
            InitializeComponent();
            Publisher = publisher;
            this.DataContext = this;
        }

        private void ButtonPublisherAdd_Click(object sender, RoutedEventArgs e)
        {
            if(Publisher.id == 0)
            {
                AppData.db.Publisher.Add(Publisher);
            }
            AppData.db.SaveChanges();
            MessageBox.Show("Новый издатель добавлен.", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new BookViewPage());
            GC.Collect();
        }
    }
}
