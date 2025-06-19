using System;
using System.Globalization;
using System.Windows.Data;

namespace KinopoiskWpfApp.Converters
{
    public class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is double rating && rating == 0))
            {
                return "—";
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:F1}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}