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
using Microsoft.Phone.Shell;

namespace WP7SimulatorApp.Test1
{
    public partial class AppBarTestPage1 : PhoneApplicationPage
    {
        public AppBarTestPage1()
        {
            InitializeComponent();
        }

        private void resetonclick(object sender, EventArgs e)
        {
            
            //MessageBox.Show("reset");
        }

        private void openonclick(object sender, EventArgs e)
        {
            //MessageBox.Show("open");
            this.ApplicationBar.MenuItems.Add(new ApplicationBarMenuItem("hello"));
            ApplicationBarMenuItem mi = this.ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            mi.Text = "aaa";
        }
    }
}
