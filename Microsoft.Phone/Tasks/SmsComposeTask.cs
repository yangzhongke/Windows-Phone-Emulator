﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Internals;

namespace Microsoft.Phone.Tasks
{
    public class SmsComposeTask
    {
        public void Show()
        {
            WinPhoneCtrl winPhoneCtrl = WinPhoneCtrl.Instance;
            winPhoneCtrl.Navigate(new Uri("/Microsoft.Phone;component/Internals/Pages/SMSComposePage.xaml?body=" + Body + "&to=" + To, 
                UriKind.Relative));
        }

        public string Body
        {
            get;
            set;
        }

        public string To
        {
            get;
            set;
        }
    }
}
