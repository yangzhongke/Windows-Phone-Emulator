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
using Microsoft.Phone.Controls;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void linkAd1_Click(object sender, RoutedEventArgs e)
        {
            AppHelper.OpenUrl("http://net.itcast.cn");
        }
    }
}
