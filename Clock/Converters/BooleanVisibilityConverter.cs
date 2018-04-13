using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Clock.Converters
{
    public class BooleanVisibilityConverter : IValueConverter
    {
        public BooleanVisibilityConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() != typeof(bool))
            {
                throw new InvalidOperationException("The value must be a bool.");
            }
            switch((bool)value)
            {
                case true: return Visibility.Visible;
                default: return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}