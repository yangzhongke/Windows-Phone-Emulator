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
    public partial class CallInPage : PhoneApplicationPage
    {
        private RepeatedMediaElement ringPlayer;

        public CallInPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string number = NavigationContext.QueryString["number"];
            txtNumber.Text = number;

            mediaElementRing.SetSource(AppHelper.GetExecutingAssemblyResourceStream("Internals.audios.nokiacallin.wma"));
            ringPlayer = new RepeatedMediaElement(mediaElementRing);
            ringPlayer.Play(-1);
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            ringPlayer.Stop();
            NavigationService.GoBack();
        }
    }
}
