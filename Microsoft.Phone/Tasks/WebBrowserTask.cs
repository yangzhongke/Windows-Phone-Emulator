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
using Microsoft.Phone.Internals;

namespace Microsoft.Phone.Tasks
{
    public class WebBrowserTask
    {
        public string URL
        {
            get;
            set;
        }

        public void Show()
        {
            WinPhoneCtrl winPhoneCtrl = WinPhoneCtrl.Instance;
            string url = "/Microsoft.Phone;component/Internals/Pages/WebBrowserPage.xaml?url=" + URL;
            winPhoneCtrl.Navigate(new Uri(url,UriKind.Relative));
        }
    }
}
