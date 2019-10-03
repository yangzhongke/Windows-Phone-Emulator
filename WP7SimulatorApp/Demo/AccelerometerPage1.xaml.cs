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
using Microsoft.Devices.Sensors;
using System.Windows.Threading;

namespace WP7SimulatorApp.Demo
{
    public partial class AccelerometerPage1 : PhoneApplicationPage
    {
        private double vx=0;
        private double vy=0;
        private double ax = 0;
        private double ay = 0;

        public AccelerometerPage1()
        {
            InitializeComponent();

            Accelerometer am = new Accelerometer();
            am.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(am_ReadingChanged);
            am.Start();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //计算位移。delta=v(初)*t+a*t*t/2;
            double deltax = vx * 0.1 + ax * 0.1 * 0.1 / 2;
            double deltay = vy * 0.1 + ay * 0.1 * 0.1 / 2;
            vx = vx + ax * 0.1;
            vy = vy + ay * -0.1;

            double newX = tr.X + deltax * 20;
            
            if (Math.Abs(newX) < this.ActualWidth / 2)
            {
                tr.X = newX;
            }
            else//如果碰壁，则转向
            {
                vx = -vx;
            }
            double newY = tr.Y + deltay * 20; ;
            if (Math.Abs(newY) < this.ActualHeight / 2)
            {
                tr.Y = newY;
            }
            else
            {
                vy = -vy;
            }
        }

        void am_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            ax = e.X;
            ay = e.Y;
            //button1.Content = e.X + "," + e.Y + "," + e.Z + "," + e.Timestamp;
            //模拟必须这样才能访问，多线程
            Dispatcher.BeginInvoke(() =>
            {
                //textBlock1.Text = e.X + "," + e.Y + "," + e.Z;// +"," + e.Timestamp;
                //tr.X = (e.Z * e.Z + e.X * e.X) * Math.Sin(e.Z) * Math.Sin(e.X) * 100 * -1;
                //tr.Y = (e.Z * e.Z + e.Y * e.Y) * Math.Sin(e.Z) * Math.Sin(e.Y) * 100;

                //lineG.X2 = lineG.X1 + (e.Z * e.Z + e.X * e.X) * Math.Sin(e.Z) * Math.Sin(e.X) * 200 * -1;
                //lineG.Y2 = lineG.Y1 + (e.Z * e.Z + e.Y * e.Y) * Math.Sin(e.Z) * Math.Sin(e.Y) * 200;
            });
        }
    }
}
