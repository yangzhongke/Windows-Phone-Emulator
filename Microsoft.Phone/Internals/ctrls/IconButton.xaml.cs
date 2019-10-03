using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Microsoft.Phone.Internals.ctrls
{
    [DefaultEvent("Click")]
    [DefaultProperty("ImageSource")]
    public partial class IconButton : UserControl
    {
        public static DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource),
            typeof(UserControl), null);
        public IconButton()
        {
            InitializeComponent();
            imgIcon.DataContext = this;
        }

        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty,value);
            }
        }

        
        public event RoutedEventHandler Click;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
        }
    }
}
