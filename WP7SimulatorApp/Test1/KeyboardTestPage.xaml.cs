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
using System.Runtime.InteropServices.Automation;
using System.Threading;

namespace WP7SimulatorApp.Test1
{
    public partial class KeyboardTestPage : PhoneApplicationPage
    {
        public KeyboardTestPage()
        {
            InitializeComponent();
        }

        private void LayoutRoot_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (e.OriginalSource is TextBox)
            //{
            //    TextBox txt = (TextBox)e.OriginalSource;
            //    txt.Background = new SolidColorBrush(Colors.Red);
            //}
        }

        private void LayoutRoot_TextInputStart(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void LayoutRoot_TextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void textBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("textBox2_LostFocus");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //SmsComposeTask task = new SmsComposeTask();
            //task.Show();
            //dynamic ie = AutomationFactory.CreateObject("Shell.Application");
            //ie.Open("c:/");
            //ie.Visible = true;
            //ie.Navigate2("http://net.itcast.cn",null,null,null,null);
            //Thread.Sleep(5000);
        }
    }
}
