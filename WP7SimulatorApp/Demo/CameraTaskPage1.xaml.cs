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

namespace WP7SimulatorApp.Demo
{
    public partial class CameraTaskPage1 : PhoneApplicationPage
    {
        public CameraTaskPage1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureTask task = new CameraCaptureTask();
            task.Completed += new EventHandler<PhotoResult>(task_Completed);
            task.Show();   

        }

        void task_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK)
            {
                return;
            }
            CameraCaptureTask task = sender as CameraCaptureTask;
            task.Completed -= task_Completed;
            BitmapImage bitmap = new BitmapImage();
            using (e.ChosenPhoto)
            {
                bitmap.SetSource(e.ChosenPhoto);
                image1.Source = bitmap;
            }
        }
    }
}
