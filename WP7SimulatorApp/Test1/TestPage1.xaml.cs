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
using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Framework;

namespace WP7SimulatorApp.Test1
{
    public partial class TestPage1 : PhoneApplicationPage
    {
        private Stopwatch sw = new Stopwatch();
        public TestPage1()
        {
            InitializeComponent();
        }

        private SoundEffectInstance sei;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            sw.Start();
            wc.DownloadStringAsync(new Uri("http://www.rupeng.com"));

            FrameworkDispatcher.Update();

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WP7SimulatorApp.Test1.nokiacallin.wma");
            SoundEffect se = SoundEffect.FromStream(stream);
            sei = se.CreateInstance();
            sei.Play();
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            sw.Stop();
            button1.Content = sw.Elapsed.ToString();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sei.State.ToString());
            sei.Stop();
        }
    }
}
