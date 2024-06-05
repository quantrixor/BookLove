using BookLove.Context;
using BookLove.Model;
using BookLove.Views.Pages.PublisherPage;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BookLove.Views.Pages.BookPage
{
    /// <summary>
    /// Interaction logic for BookActionPage.xaml
    /// </summary>
    public partial class BookActionPage : Page
    {
        public List<Publisher> Publishers { get; set; }
        public List<Genre> Genres { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
        public BookActionPage(Book book, Author author)
        {
            InitializeComponent();
            Publishers = AppData.db.Publisher.ToList();
            Genres = AppData.db.Genre.ToList();
            Author = author;
            Book = book;
            this.DataContext = this;
        }
        OpenFileDialog file = new OpenFileDialog();
        private void SelectedPicture_Click(object sender, RoutedEventArgs e)
        {
            file.Filter = "Image (*.jpg;*.jpeg;*png;)|*.jpg;*.jpeg;*png;";
            BookPicture.Source = file.ShowDialog() == true ? BookPicture.Source = new BitmapImage(new Uri(file.FileName)) : null;
        }

        private void BookSave_Click(object sender, RoutedEventArgs e)
        {
            if(Book.id == 0 && Author.id == 0)
            {
                AppData.db.Author.Add(Author);
                AppData.db.Book.Add(Book);
            }
            File.Copy(file.FileName, $"pics\\{System.IO.Path.GetFileName(file.FileName).Trim()}", true);
            Book.GetPicture = "\\pics\\" + System.IO.Path.GetFileName(file.FileName);
            AppData.db.SaveChanges();
            MessageBox.Show("Данные успешно сохранены в Базе Данных.", "Внимание, операция прошла успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            GC.Collect();
            NavigationService.GoBack();
        }

        private void ButtonPublisher_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PublisherAddPage(new Publisher()));
        }
    }
}
