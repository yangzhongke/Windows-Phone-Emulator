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

namespace WP7SimulatorApp.Demo
{
    public partial class TasksPage1 : PhoneApplicationPage
    {
        public TasksPage1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask smsTask = new SmsComposeTask();
            smsTask.To = "10086";
            smsTask.Body = "你是男的还是女的？";
            smsTask.Show();
        }

        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            task.Body = "咨询传智播客.Net培训的事情";
            task.Subject = "传智播客的老师你们好";
            task.To = "yzk@itcast.cn";
            task.Show();
        }

        private void btnCall_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask task = new PhoneCallTask();
            task.DisplayName = "中国移动客服";
            task.PhoneNumber = "10086";
            task.Show();
        }

        private void btnOpenBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.URL = "http://net.itcast.cn";
            task.Show();
        }

        private void btnSavePhone_Click(object sender, RoutedEventArgs e)
        {
            SavePhoneNumberTask savePhoneTask = new SavePhoneNumberTask();
            savePhoneTask.PhoneNumber = "10086";
            savePhoneTask.Completed += new EventHandler<TaskEventArgs>(savePhoneTask_Completed);
            savePhoneTask.Show();
        }

        void savePhoneTask_Completed(object sender, TaskEventArgs e)
        {
            MessageBox.Show(e.TaskResult.ToString());
        }

        private void btnChoosePhone_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumberChooserTask numberChooserTask = new PhoneNumberChooserTask();
            numberChooserTask.Completed += new EventHandler<PhoneNumberResult>(numberChooserTask_Completed);
            numberChooserTask.Show();
        }

        void numberChooserTask_Completed(object sender, PhoneNumberResult e)
        {
            MessageBox.Show("ChoosePhoneResult"+e.PhoneNumber);
        }
    }
}
