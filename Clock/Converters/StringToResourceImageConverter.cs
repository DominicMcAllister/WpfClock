using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Clock.Converters
{
    public class StringToResourceImageConverter : IValueConverter
    {
        public StringToResourceImageConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.GetType() != typeof(string))
                {
                    throw new InvalidOperationException("The value must be a string");
                }

                var uri = new Uri((string)value, UriKind.Relative);
                return new BitmapImage(uri);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}