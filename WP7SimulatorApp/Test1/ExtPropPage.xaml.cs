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
using Microsoft.Phone.Info;
using System.Windows.Threading;

namespace WP7SimulatorApp.Test1
{
    public partial class ExtPropPage : PhoneApplicationPage
    {
        public ExtPropPage()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.Text = (string)DeviceExtendedProperties.GetValue("DeviceManufacturer");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage"));
        }
    }
}
