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
using System.Security;
using Microsoft.Phone.Internals;

namespace Microsoft.Phone.Tasks
{
    public class PhoneCallTask
    {
        public string DisplayName
        {
            get;
            set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        [SecuritySafeCritical]
        public void Show()
        {
            AppHelper.GetCurrentPhoneAppPage().NavigationCacheMode = System.Windows.Navigation.NavigationCacheMode.Required;
            string uri = "/Microsoft.Phone;component/Internals/Pages/PhoneCallPage.xaml?displayName="
                + DisplayName + "&phoneNumber=" + PhoneNumber;
            WinPhoneCtrl.Instance.Navigate(new Uri(uri,UriKind.Relative));
        }
    }
}
