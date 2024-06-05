using BookLove.Context;
using BookLove.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Word = Microsoft.Office.Interop.Word;

namespace BookLove.Views.Pages.BookPage
{
    /// <summary>
    /// Interaction logic for BookViewPage.xaml
    /// </summary>
    public partial class BookViewPage : Page
    {
        public BookViewPage()
        {
            InitializeComponent();
        }

        private void CreateBookPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BookActionPage(new Book(), new Author()));
        }

        private void EditBookPage_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookListView.SelectedItem as Book;
            if (selectedBook != null)
            {
                var selectedAuthor = selectedBook.Author;

                NavigationService.Navigate(new BookActionPage(selectedBook, selectedAuthor));
            }
            else
                MessageBox.Show("Чтобы редактировать запись, вам необходимо его выбрать.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BookListView.ItemsSource = AppData.db.Book.ToList();
        }

        private void RemoveSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookListView.SelectedItem as Book;
            if (selectedBook != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить данные? Данные будут удалены навсегда.", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.db.Book.Remove(selectedBook);
                    AppData.db.SaveChanges();
                    Page_Loaded(null, null);
                    GC.Collect();
                }
                else
                    MessageBox.Show("Чтобы удалить запись, вам необходимо его выбрать.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnPDFExport_Click(object sender, RoutedEventArgs e)
        {
            var word = new Word.Application();
            try
            {
                var document = word.Documents.Add();
                var paragraph = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragraph.Range;
                var books = AppData.db.Book.ToList();
                var table = document.Tables.Add(tableRange, books.Count, 6);
                table.Borders.Enable = 1;
                table.Cell(1, 0).Range.Text = "Наименование";
                table.Cell(1, 1).Range.Text = "Автор";
                table.Cell(1, 2).Range.Text = "Жанр";
                table.Cell(1, 3).Range.Text = "Издатель";
                table.Cell(1, 4).Range.Text = "Количество";
                table.Cell(1, 5).Range.Text = "Цена";
                int i = 2;
                foreach (var item in books)
                {
                    table.Cell(i, 0).Range.Text = item.title;
                    table.Cell(i, 1).Range.Text = item.Author.GetFullName;
                    table.Cell(i, 2).Range.Text = item.Genre.title;
                    table.Cell(i, 3).Range.Text = item.Publisher.titile;
                    table.Cell(i, 4).Range.Text = item.count.ToString();
                    table.Cell(i, 5).Range.Text = item.price.ToString();
                    i++;
                }
                document.SaveAs2(Environment.CurrentDirectory + @"books.pdf", Word.WdSaveFormat.wdFormatPDF);
                document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                MessageBox.Show("Сохранение прошло успешно!", "Сохранено!", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source + " выдал исключение!", MessageBoxButton.OK, MessageBoxImage.Error);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
            }

            if (MessageBox.Show("Документ PDF успешно сформирован. Хотите открыть его?", "Операция прошла успешно!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Process.Start(Environment.CurrentDirectory + @"books.pdf");
            }
        }

        private void btnCSVExport_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream stream = new FileStream(Environment.CurrentDirectory + @"books.csv", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    var books = AppData.db.Book.ToList();
                    writer.WriteLine("Наименование;Автор;Жанр;Издатель;Цена;Количество");
                    foreach (var item in books)
                    {
                        writer.WriteLine($"{item.title};{item.Author.GetFullName};{item.Genre.title};{item.Publisher.titile};{item.count}");
                    }
                }
            }
            MessageBox.Show($"Сохранение прошло успешно! Откройте документ тут: {Environment.CurrentDirectory}", "Сохранено!", MessageBoxButton.OK, MessageBoxImage.Information); ;
        }
        private void Update(string filter = "", string search = "")
        {
            var books = AppData.db.Book.ToList();
            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filter))
            {
                if (filter == "Роман" || filter == "Саморазвитие" || filter == "Детектив" || filter == "Дорама")
                {
                    books = AppData.db.Book.Where(item => item.Genre.title == filter).ToList();
                }
            }
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(search))
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
    }
}
