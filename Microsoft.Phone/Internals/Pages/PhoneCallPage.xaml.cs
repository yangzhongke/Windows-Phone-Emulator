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
    public partial class PhoneCallPage : PhoneApplicationPage
    {
        private RepeatedMediaElement tonePlayer;

        public PhoneCallPage()
        {
            InitializeComponent();
            //不是跳转页面，只是悬浮：PhoneCallTask
        }

        private void btnEndCall_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (tonePlayer != null)
            {
                tonePlayer.Stop();
            }            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string displayName = NavigationContext.QueryString["displayName"];
            string phoneNumber = NavigationContext.QueryString["phoneNumber"];

            txtDisplayName.Text = displayName;
            txtNumber.Text = phoneNumber;

            btnCall.IsEnabled = true;
            btnNotCall.IsEnabled = true;
            btnEndCall.IsEnabled = false;
        }

        private void btnNotCall_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnCall_Click(object sender, RoutedEventArgs e)
        {
            btnCall.IsEnabled = false;
            btnNotCall.IsEnabled = false;
            btnEndCall.IsEnabled = true;

            mediaElementTone.SetSource(AppHelper.GetExecutingAssemblyResourceStream("Internals.audios.phonecalling.wma"));
            tonePlayer = new RepeatedMediaElement(mediaElementTone);
            tonePlayer.Play(-1);
        }
    }
}
