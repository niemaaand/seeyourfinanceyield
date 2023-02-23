using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace SYFY_Plugin_GUI_WPF.Converters
{
    [ValueConversion(typeof(long), typeof(String))]
    internal class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToString("yyy-MM-dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            DateTime resultDate;

            if (DateTime.TryParse(strValue, out resultDate))
            {
                return resultDate;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
