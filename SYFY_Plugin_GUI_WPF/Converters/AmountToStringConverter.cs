using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace SYFY_Plugin_GUI_WPF.Converters
{
    [ValueConversion(typeof(long), typeof(String))]
    internal class AmountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertLongToString.AsCurrency((long)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long res;

            if (ConvertStringToLong.TryFromCurrency((string)value, out res))
            {
                return res;
            }

            return DependencyProperty.UnsetValue;
        }
    }

    internal static class ConvertLongToString
    {
        static internal string AsCurrency(long l)
        {
            int sign = Math.Sign(l);
            l = Math.Abs(l);

            long eur = l / 100;
            long cent = l % 100;

            return (sign >= 0 ? "" : "-") + eur.ToString() + "." + cent.ToString("00");
        }
    }

    internal static class ConvertStringToLong
    {
        static internal bool TryFromCurrency(string amount, out long value)
        {
            decimal val = 0;
            value = 0;

            if (Decimal.TryParse(amount, NumberStyles.Currency, CultureInfo.InvariantCulture, out val))
            {
                value = (long)(val * 100);
                return true;
            }

            return false;
        }
    }
}
