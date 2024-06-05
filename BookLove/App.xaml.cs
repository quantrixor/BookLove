using BookLove.Context;
using BookLove.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookLove
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow mainWindow;
        public Login CurrentUser { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            mainWindow = new MainWindow();
            mainWindow.Closing += MainWindow_Closing;
            mainWindow.Show();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mainWindow != null && !string.IsNullOrEmpty(mainWindow.SessionId))
            {
                var basketItems = AppData.db.GuestBasket.Where(b => b.sessionId == mainWindow.SessionId).ToList();
                foreach (var item in basketItems)
                {
                    AppData.db.GuestBasket.Remove(item);
                }
                AppData.db.SaveChanges();
            }
        }
    }
}
