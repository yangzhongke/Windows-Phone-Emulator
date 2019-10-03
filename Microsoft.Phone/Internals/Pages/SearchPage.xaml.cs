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
using System.Windows.Browser;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //WebBrowser不能旋转，可以用WebBrowserBrush解决这个问题
            string url = "http://www.bing.com/search?q=" + HttpUtility.UrlEncode(txtKeywords.Text);
            webBrowser.Navigate(new Uri(url, UriKind.Absolute));
        }
    }
}
