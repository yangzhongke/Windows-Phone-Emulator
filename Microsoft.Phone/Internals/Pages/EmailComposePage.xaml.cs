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
    public partial class EmailComposePage : PhoneApplicationPage
    {
        public EmailComposePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string Body = NavigationContext.QueryString["Body"];
            string Cc = NavigationContext.QueryString["Cc"];
            string Subject = NavigationContext.QueryString["Subject"];
            string To = NavigationContext.QueryString["To"];

            txtTo.Text = To;
            txtSubject.Text = Subject;
            txtBody.Text = Body;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string Body = NavigationContext.QueryString["Body"];
            string Cc = NavigationContext.QueryString["Cc"];
            string Subject = NavigationContext.QueryString["Subject"];
            string To = NavigationContext.QueryString["To"];

            using (dynamic phoneInterop = AppHelper.TryCreatePhoneInteropServices("PhoneInterop"))
            {
                //在手机上增加Settings，设置用户名密码等
                phoneInterop.SendMail("smtp.163.com", "itcast0420", "123456", "itcast0420@163.com", To, Subject, Body);
            }

            NavigationService.GoBack();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
