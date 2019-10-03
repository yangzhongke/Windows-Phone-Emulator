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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Internals.people;

namespace WP7SimulatorApp.Test1
{
    public partial class TaskTest1Page : PhoneApplicationPage
    {
        public TaskTest1Page()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //PhoneCallTask task = new PhoneCallTask();
            //task.DisplayName = "中国移动";
            //task.PhoneNumber = "10086";
            //task.Show();

            //EmailComposeTask task = new EmailComposeTask();
            //task.Subject = "传智播客.Net培训，三个月学费只要5800元";
            //task.To = "itcast@itcast.cn";
            //task.Body = "详细请访问http://net.itcast.cn";
            //task.Show();

            WebBrowserTask task = new WebBrowserTask();
            task.URL = "http://net.itcast.cn";
            task.Show();

            //var list = PeopleMgr.LoadPeoples();
            //PeopleItemInfo p1 = new PeopleItemInfo();
            //p1.Name = "传智播客";
            //p1.PhoneNumberList.Add(new KeyValuePair<string, string>("咨询电话1", "010-51552111"));
            //p1.PhoneNumberList.Add(new KeyValuePair<string, string>("咨询电话2", "010-51552112"));
            //p1.EmailAddressList.Add(new KeyValuePair<string, string>("公司邮箱", "itcast@itcast.cn"));
            //list.Add(p1);

            //PeopleMgr.SavePeoples(list);
        }
    }
}
