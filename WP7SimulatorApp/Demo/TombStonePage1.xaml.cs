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
using Microsoft.Phone.Shell;

namespace WP7SimulatorApp.Demo
{
    public partial class TombStonePage1 : PhoneApplicationPage
    {
        public TombStonePage1()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (checkBox1.IsChecked.HasValue &&
                checkBox1.IsChecked.Value)
            {
                PhoneApplicationService.Current.State["TextBox1Text"] = textBox1.Text;
                PhoneApplicationService.Current.State["EnabledTombStone"] = true;
            }
            else
            {
                PhoneApplicationService.Current.State["EnabledTombStone"] = false;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("TextBox1Text"))
            {
                textBox1.Text = PhoneApplicationService.Current.State["TextBox1Text"].ToString();
            }
            if (PhoneApplicationService.Current.State.ContainsKey("EnabledTombStone"))
            {
                checkBox1.IsChecked = (bool)PhoneApplicationService.Current.State["EnabledTombStone"];
            }
        }
    }
}
