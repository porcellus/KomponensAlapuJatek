using System;
using System.Globalization;
using System.Windows.Data;

namespace ClientGUI.ViewModel
{
    internal class BooleanToConnectionStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool) value;
            if (val)
            {
                return "ONLINE";
            }
            return "OFFLINE";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}