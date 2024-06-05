using BookLove.Context;
using BookLove.HelperObject;
using BookLove.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookLove.Views.Pages.SystemPage
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        public static string GenericPassword()
        {
            // Получаем количество слов и букв за слово.
            int num_letters = 8;
            int num_words = 1;

            // Создаем массив букв, которые мы будем использовать.
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdifghijklmnopqrstuvwxyz123456789".ToCharArray();

            // Создаем генератор случайных чисел.
            Random rand = new Random();

            // Сделайте слово.
            string word = "";
            // Делаем слова.
            for (int i = 1; i <= num_words; i++)
            {
                for (int j = 1; j <= num_letters; j++)
                {
                    // Выбор случайного числа от 0 до 25
                    // для выбора буквы из массива букв.
                    int letter_num = rand.Next(0, letters.Length - 1);

                    // Добавить письмо.
                    word += letters[letter_num];
                }
            }
            return word;
        }

        static string currentPassword = GenericPassword();


        public static async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.mail.ru", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("no-reply-autosystem@mail.ru", "jrHUtY5kV4BJb7Pu0G2X");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("no-reply-autosystem@mail.ru"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }
        //jrHUtY5kV4BJb7Pu0G2X
        private async void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Registration registration = new Registration();
            try
            {
                if (HelperClass.IsValidEmail(txbEmail.Text))
                {
                    if (AppData.db.Login.Count(item => item.username == txbEmail.Text) > 0)
                    {
                        MessageBox.Show("Учётная запись существует. Пожалуйста, если вы потеряли доступ, обратитесь к Администартору Системы.",
                       "В доступе отказано!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        login.username = txbEmail.Text;
                        login.password = txbPassword.Text;
                        registration.firstName = txbFirstName.Text;
                        registration.lastName = txbLastName.Text;
                        registration.secondName = txbSecondName.Text;
                        registration.idLogin = login.id;
                        registration.numberContract = "111110000";
                        registration.dateOfRegistration = DateTime.Now;
                        AppData.db.Login.Add(login);
                        AppData.db.Registration.Add(registration);
                        AppData.db.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Некорректный формат почты! Пожалуйста, внимательно прочтите, какой набор символов вы ввели и повторите попытку!",
                          "Некорректный формать логина!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GenerationPassword_Click(object sender, RoutedEventArgs e)
        {
            txbPassword.Text = GenericPassword();
        }
    }
}
