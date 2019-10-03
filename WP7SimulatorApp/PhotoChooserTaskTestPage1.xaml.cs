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
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using System.Device.Location;
using System.IO.IsolatedStorage;

namespace WP7SimulatorApp
{
    public partial class TestPage1 : PhoneApplicationPage
    {
        public TestPage1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.PixelHeight = (int)image1.ActualHeight;
            task.PixelWidth = (int)image1.ActualWidth;
            task.Completed += new EventHandler<PhotoResult>(task_Completed);
            task.ShowCamera = true;
            task.Show();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            PhoneApplicationService.Current.State["txt"] = textBox1.Text;
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("txt"))
            {
                textBox1.Text = Convert.ToString(PhoneApplicationService.Current.State["txt"]);
            }            
        }

        void task_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var bitmapImg = new BitmapImage();
                bitmapImg.SetSource(e.ChosenPhoto);
                image1.Source = bitmapImg;
            }
            else
            {
                button1.Content = "取消";
            }           
        }
    }
}
