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
    public partial class WebBrowserPage : PhoneApplicationPage
    {
        public WebBrowserPage()
        {
            InitializeComponent();
        }

        private void txtUrl_LostFocus(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigate(new Uri(txtUrl.Text));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("url"))
            {
                string url = NavigationContext.QueryString["url"];
                txtUrl.Text = url;
                webBrowser.Navigate(new Uri(url));
            }            
        }
    }
}
