using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Microsoft.Phone.Shell.Internals
{
    public class BoolVisibilityConverter:IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool b = (bool)value;
            return b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility v = (Visibility)value;
            return v == Visibility.Visible;
        }

        #endregion
    }
}
