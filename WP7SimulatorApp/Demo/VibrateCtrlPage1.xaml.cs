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
using Microsoft.Phone.Devices;

namespace WP7SimulatorApp.Demo
{
    public partial class VibrateCtrlPage1 : PhoneApplicationPage
    {
        public VibrateCtrlPage1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            VibrateController.Default.Start(TimeSpan.FromSeconds(1));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            VibrateController.Default.Start(TimeSpan.FromSeconds(3));
        }
    }
}
