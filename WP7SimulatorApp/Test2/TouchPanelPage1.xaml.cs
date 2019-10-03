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
using System.Windows.Threading;
using Microsoft.Xna.Framework.Input.Touch;

namespace WP7SimulatorApp.Test2
{
    public partial class TouchPanelPage1 : PhoneApplicationPage
    {
        public TouchPanelPage1()
        {
            InitializeComponent();
            TouchPanel.EnabledGestures = GestureType.Hold |
                GestureType.HorizontalDrag | GestureType.VerticalDrag |
                GestureType.Tap |GestureType.Flick;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            var sample = TouchPanel.ReadGesture();
             
            PageTitle.Text = sample.GestureType.ToString();
        }
    }
}
