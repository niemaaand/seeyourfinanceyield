using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Documents;

namespace SYFY_Plugin_GUI_WPF.Converters
{
    [ValueConversion(typeof(long), typeof(String))]
    internal class AmountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long amount = (long)value;
            bool positiveSign = true;

            if(amount < 0)
            {
                positiveSign = false;
                amount = -amount;
            }

            long eur = amount / 100;
            long cent = amount % 100;
            return (positiveSign?"":"-") + eur.ToString() + "." + cent.ToString("00");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (string)value;
            decimal val = 0;

            if (Decimal.TryParse(strValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out val) )
            {
                return (long)(val * 100);
            }

            return DependencyProperty.UnsetValue;

            /*string strValue = value as string;
            strValue = strValue.Replace(" ", "");

            bool positiveSign = true;
            if (strValue.StartsWith("-"))
            {
                strValue = strValue.Remove(0, 1);
                positiveSign = false;
            }

            string[] eurCent = strValue.Split(".");

            if (eurCent.Length > 2) {
                //TODO adapt exception
                throw new ArrayTypeMismatchException("Wrong format for amount.");
            } else if (eurCent.Length == 0)
            {
                return 0;
            }

            if (eurCent[0].Length > 16)
            {
                //TODO
                throw new OverflowException("Value to high.");
            }

            long eur = 0;
            long cent = 0;

            if (!long.TryParse(eurCent[0], out eur) && !string.IsNullOrEmpty(eurCent[0]))
            {
                throw new ArrayTypeMismatchException("Wrong format for amount.");
            }

            if(eurCent.Length == 2 && !long.TryParse(eurCent[1], out cent) && !string.IsNullOrEmpty(eurCent[1])) {
                throw new ArrayTypeMismatchException("Wrong format for amount.");
            }

            if (cent >= 100)
            {
                throw new ArrayTypeMismatchException("Wrong format for amount.");
            }           

            return (positiveSign?1:-1) * (eur * 100 + cent);        */
        }
    }
}
