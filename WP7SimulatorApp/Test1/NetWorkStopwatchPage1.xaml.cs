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
using Microsoft.Phone.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;

namespace WP7SimulatorApp.Test1
{
    public partial class NetWorkStopwatchPage1 : PhoneApplicationPage
    {
        public NetWorkStopwatchPage1()
        {
            InitializeComponent();
        }

        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                textBlockNetwork.Text = "网络连通，类型"
                    + NetworkInterface.NetworkInterfaceType;
            }
            else
            {
                textBlockNetwork.Text = "网络未连通";
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Thread.Sleep(3000);
            sw.Stop();
            textBlock1.Text = sw.Elapsed.ToString();
        }
    }
}
