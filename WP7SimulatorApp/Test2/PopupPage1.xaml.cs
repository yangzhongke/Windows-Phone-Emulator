﻿using System;
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
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Internals;

namespace WP7SimulatorApp.Test2
{
    public partial class ToolkitPage1 : PhoneApplicationPage
    {
        public ToolkitPage1()
        {
            InitializeComponent();
            Popup pop = new Popup();
            pop.Child = new Button() { Content="fff"};
            pop.IsOpen = true;

            //WinPhoneCtrl.Instance.RootPanel.Children.Add(pop);
            //listPicker1.ListPickerMode = ListPickerMode.Full;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            e.Cancel = true;
        }
    }
}
