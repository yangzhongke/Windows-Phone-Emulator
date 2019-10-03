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
using Microsoft.Phone.Internals;

namespace WP7SimulatorApp
{
    public partial class DemoPage1 : PhoneApplicationPage
    {
        public DemoPage1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbItem = listBox1.SelectedItem as  ListBoxItem;
            if (lbItem == null)
            {
                return;
            }
            WinPhoneCtrl.Instance.NavigateNewApp(new Uri(lbItem.Tag.ToString(),UriKind.Relative));
        }
    }
}
