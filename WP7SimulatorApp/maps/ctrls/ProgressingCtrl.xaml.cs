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

namespace WP7SimulatorApp.maps.ctrls
{
    public partial class ProgressingCtrl : UserControl
    {
        public ProgressingCtrl()
        {
            InitializeComponent();
        }

        public string Message
        {
            get
            {
                return txtMsg.Text;
            }
            set
            {
                txtMsg.Text = value;
            }
        }

        public bool ShowProgressing
        {
            get
            {
                return progressBar.Visibility == System.Windows.Visibility.Visible;
            }
            set
            {
                progressBar.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }
    }
}
