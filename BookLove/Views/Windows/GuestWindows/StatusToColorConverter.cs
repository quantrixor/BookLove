using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BookLove.Views.Windows.GuestWindows
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            switch (status)
            {
                case "обработан":
                    return Brushes.Green;
                case "отправлен":
                    return Brushes.Blue;
                case "доставлен":
                    return Brushes.Gray;
                default:
                    return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
