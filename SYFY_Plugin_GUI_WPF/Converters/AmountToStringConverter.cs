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
            long amount = (long)value;
            bool positiveSign = true;

            if (amount < 0)
            {
                positiveSign = false;
                amount = -amount;
            }

            long eur = amount / 100;
            long cent = amount % 100;
            return (positiveSign ? "" : "-") + eur.ToString() + "." + cent.ToString("00");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (string)value;
            decimal val = 0;

            if (Decimal.TryParse(strValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out val))
            {
                return (long)(val * 100);
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
