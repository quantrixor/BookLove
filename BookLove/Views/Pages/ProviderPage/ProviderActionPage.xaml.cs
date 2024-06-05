using BookLove.Context;
using BookLove.Model;
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

namespace BookLove.Views.Pages.ProviderPage
{
    /// <summary>
    /// Interaction logic for ProviderActionPage.xaml
    /// </summary>
    public partial class ProviderActionPage : Page
    {
        public Provider Provider { get; set; }
        public ProviderActionPage(Provider provider)
        {
            InitializeComponent();
            Provider = provider;
            this.DataContext = this;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if(Provider.id == 0)
            {
                AppData.db.Provider.Add(Provider);
            }
            AppData.db.SaveChanges();
            MessageBox.Show("Добавление прошло успешно.", "Данные успешно сохранены!", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
            GC.Collect();
        }
    }
}
