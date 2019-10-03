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
using Microsoft.Phone.Devices.Radio;

namespace WP7SimulatorApp
{
    public partial class RadioTestPage : PhoneApplicationPage
    {
        public RadioTestPage()
        {
            InitializeComponent();
            TryOpen();

            List<RadioItem> list = new List<RadioItem>();
            list.Add(new RadioItem { Name="CRI",Frequence=91.5});
            list.Add(new RadioItem { Name = "HitFM", Frequence = 88.7 });
            list.Add(new RadioItem { Name = "传智播客广播", Frequence = 91.2 });
            list.Add(new RadioItem { Name = "飞鱼秀", Frequence = 103.8 });
            list.Add(new RadioItem { Name = "ABCMusic", Frequence = 102.3 });
            listBoxRadioItems.DataContext = list;
        }

        private void TryOpen()
        {
            if (FMRadio.Instance.PowerMode == RadioPowerMode.Off)
            {
                FMRadio.Instance.PowerMode = RadioPowerMode.On;
            }
        }

        private void btnPower_Click(object sender, RoutedEventArgs e)
        {
            if (FMRadio.Instance.PowerMode == RadioPowerMode.Off)
            {
                FMRadio.Instance.PowerMode = RadioPowerMode.On;
                btnPower.Content = "Off";
            }
            else
            {
                FMRadio.Instance.PowerMode = RadioPowerMode.Off;
                btnPower.Content = "On";
            }
        }

        private void GoToFrequency(double f)
        {
            TryOpen();
            FMRadio.Instance.Frequency = f;
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            GoToFrequency(Convert.ToDouble(textBox1.Text));
        }

        private void listBoxRadioItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadioItem item = listBoxRadioItems.SelectedValue as RadioItem;
            GoToFrequency(item.Frequence);
        }
    }

    public class RadioItem
    {
        public string Name { get; set; }
        public double Frequence { get; set; }
    }
}
