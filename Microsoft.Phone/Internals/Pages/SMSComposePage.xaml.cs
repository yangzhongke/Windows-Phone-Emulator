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
using System.Runtime.InteropServices.Automation;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class SMSComposePage : PhoneApplicationPage
    {
        public SMSComposePage()
        {
            InitializeComponent();
            storyboardPopupStatus.Completed += new EventHandler(storyboardPopupStatus_Completed);
        }

        void storyboardPopupStatus_Completed(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {            
            storyboardPopupStatus.Begin();            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string body = NavigationContext.QueryString["body"];
            string to = NavigationContext.QueryString["to"];
            txtPhoneNum.Text = to;
            txtMsg.Text = body;
        }
    }
}
