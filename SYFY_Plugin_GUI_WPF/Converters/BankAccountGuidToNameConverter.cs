using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using SYFY_Application.DatabaseAccess;
using SYFY_Application.BusinessLogic;

namespace SYFY_Plugin_GUI_WPF.Converters
{
    [ValueConversion(typeof(Guid), typeof(String))]

    internal class BankAccountGuidToNameConverter : IValueConverter
    {

        private DataManagement dataManager;
               

        public BankAccountGuidToNameConverter(DataManagement dbConnector)
        {
            dataManager= dbConnector;
        }
        
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Guid id = (Guid)value;
            return dataManager.GetBankAccountByID(id).Name;
        }
       

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
