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
using Microsoft.Phone.Notification;

namespace WP7SimulatorApp.Demo2
{
    public partial class PushNotifPage : PhoneApplicationPage
    {
        public PushNotifPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            HttpNotificationChannel httpChannel = HttpNotificationChannel.Find("test");
            //如果存在就删除
            if (httpChannel!=null)
            {
                httpChannel.Close();
                httpChannel.Dispose();
            }

            httpChannel = new HttpNotificationChannel("test", "NotificationServer");
            //打开连接
            httpChannel.Open();
            //绑定toast 推送服务
            httpChannel.BindToShellToast();

            //绑定Tokens (tile) 推送服务 
            httpChannel.BindToShellTile();
        }
    }
}
