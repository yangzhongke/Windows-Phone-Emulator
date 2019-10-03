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
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Reflection;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Xna.Framework.Framework;
using System.Windows.Threading;

namespace WP7SimulatorApp.Test1
{
    public partial class RecorderPage : PhoneApplicationPage
    {
        private Microphone mp;
        public RecorderPage()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += (sender, e) => { FrameworkDispatcher.Update(); };
            timer.Start();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {            
            mp.Start();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mp = Microphone.Default;
            mp.BufferReady += new EventHandler<EventArgs>(mp_BufferReady);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            mp.BufferReady -= new EventHandler<EventArgs>(mp_BufferReady);
        }

        void mp_BufferReady(object sender, EventArgs e)
        {
            int size = 0;
            byte[] buffer = new byte[1024];
            MemoryStream ms = new MemoryStream();
            while ((size = mp.GetData(buffer)) > 0)
            {
                ms.Write(buffer, 0, size);
            }
            ms.Position = 0;
            var sei = SoundEffect.FromStream(ms).CreateInstance();
            sei.Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Microphone mp = Microphone.Default;
            mp.Stop();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(NetworkInterface.GetIsNetworkAvailable().ToString());
            //Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WP7SimulatorApp.Test1.nokiacallin.wma");
            //var sei = SoundEffect.FromStream(stream).CreateInstance();
            //sei.Play();

            //WaveMediaStreamSource wavMss = new WaveMediaStreamSource(stream);
           // mediaElement1.SetSource(wavMss);
        }
    }
}
