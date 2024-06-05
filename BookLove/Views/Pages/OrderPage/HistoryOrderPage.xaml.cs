using BookLove.Context;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;

namespace BookLove.Views.Pages.OrderPage
{
    /// <summary>
    /// Interaction logic for HistoryOrderPage.xaml
    /// </summary>
    public partial class HistoryOrderPage : Page
    {
        public HistoryOrderPage()
        {
            InitializeComponent();
            HistoryOrderList.ItemsSource = AppData.db.Supple.ToList();
        }

        private void dtpDateHistory_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            HistoryOrderList.ItemsSource = AppData.db.Supple.Where(item => item.dateSupple == dtpDateHistory.SelectedDate).ToList();
        }

        private void btnFilterOff_Click(object sender, RoutedEventArgs e)
        {
            dtpDateHistory.SelectedDate = null;
            HistoryOrderList.ItemsSource = AppData.db.Supple.ToList();
        }

        private void btnPDFExport_Click(object sender, RoutedEventArgs e)
        {
            var word = new Word.Application();
            try
            {
                var document = word.Documents.Add();
                var paragraph = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragraph.Range;
                var supples = AppData.db.Supple.ToList();
                var table = document.Tables.Add(tableRange, supples.Count, 7);
                table.Borders.Enable = 1;
                table.Cell(1, 0).Range.Text = "Клиент";
                table.Cell(1, 1).Range.Text = "Книга";
                table.Cell(1, 2).Range.Text = "Поставщик";
                table.Cell(1, 3).Range.Text = "Дата";
                table.Cell(1, 4).Range.Text = "Цена";
                table.Cell(1, 5).Range.Text = "Количество";
                table.Cell(1, 6).Range.Text = "Описание";
                int i = 2;
                foreach (var item in supples)
                {
                    table.Cell(i, 0).Range.Text = item.Client.GetFullTitle;
                    table.Cell(i, 1).Range.Text = item.Book.GetFullTitle;
                    table.Cell(i, 2).Range.Text = item.Provider.title;
                    table.Cell(i, 3).Range.Text = item.dateSupple.ToString();
                    table.Cell(i, 4).Range.Text = item.totalPrice.ToString();
                    table.Cell(i, 5).Range.Text = item.count.ToString();
                    table.Cell(i, 6).Range.Text = item.description;
                    i++;
                }
                document.SaveAs2(Environment.CurrentDirectory + @"history.pdf", Word.WdSaveFormat.wdFormatPDF);
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
                Process.Start(Environment.CurrentDirectory + @"history.pdf");
            }
        }

        private void btnCSV_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream stream = new FileStream(Environment.CurrentDirectory + @"history.csv", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    var supples = AppData.db.Supple.ToList();
                    writer.WriteLine("Клиент;Книга;Поставщик;Дата;Цена;Количество;Описание");
                    foreach (var item in supples)
                    {
                        writer.WriteLine($"{item.Client.GetFullTitle};{item.Book.GetFullTitle};{item.dateSupple};{item.totalPrice};{item.count};{item.description}");
                    }
                }
            }
            MessageBox.Show("Сохранение прошло успешно!", "Сохранено!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
