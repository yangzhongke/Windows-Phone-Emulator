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
    public class EmailComposeTask
    {
        public void Show()
        {
            WinPhoneCtrl winPhoneCtrl = WinPhoneCtrl.Instance;
            string uri = "/Microsoft.Phone;component/Internals/Pages/EmailComposePage.xaml?Body=" + Body+"&Cc="+Cc
                +"&Subject="+ Subject+"&To="+To;
            winPhoneCtrl.Navigate(new Uri(uri,UriKind.Relative));
        }
        public string Body { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }

    }
}
