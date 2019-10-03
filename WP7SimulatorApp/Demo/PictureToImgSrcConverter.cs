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
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Xna.Framework.Framework.Media;
using System.Windows.Media.Imaging;

namespace WP7SimulatorApp.Demo
{
    public class PictureToImgSrcConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Picture pic = (Picture)value;
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(pic.GetThumbnail());
            return bitmap; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
