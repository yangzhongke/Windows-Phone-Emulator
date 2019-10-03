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
using System.Windows.Navigation;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Ink;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Internals;
using Microsoft.Phone.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Devices.Radio;
using System.Windows.Media.Imaging;

namespace WP7SimulatorApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            Accelerometer am = new Accelerometer();
            am.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(am_ReadingChanged);
            am.Start();
            //am.Stop();  
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/PhonePageTemplate.xaml",UriKind.Relative));
            //using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("3.txt", FileMode.OpenOrCreate,
            //    IsolatedStorageFile.GetUserStoreForApplication()))
            //{
            //    using (StreamWriter sw = new StreamWriter(stream))
            //    {
            //        sw.WriteLine("afdasdfasfdasdf");
            //    }                
            //}    
            

            //NavigationService.Navigate(new Uri("/PhonePageTemplate.xaml",UriKind.Relative));
            //SmsComposeTask smsTask = new SmsComposeTask();
            //smsTask.To = "10086";
            //smsTask.Body = "你是男的还是女的？";
            //smsTask.Show();

            CameraCaptureTask task = new CameraCaptureTask();
            task.Completed += new EventHandler<PhotoResult>(task_Completed);
            task.Show();   
   
            //VibrateController.Default.Start(TimeSpan.FromSeconds(2));
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

        void am_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            //button1.Content = e.X + "," + e.Y + "," + e.Z + "," + e.Timestamp;
            //模拟必须这样才能访问，多线程
            Dispatcher.BeginInvoke(() =>
            {
                button1.Content = e.X + "," + e.Y + "," + e.Z;// +"," + e.Timestamp;
                tr.X = (e.Z*e.Z+e.X*e.X)*Math.Sin(e.Z)*Math.Sin(e.X)*100*-1;
                tr.Y = (e.Z * e.Z + e.Y * e.Y) * Math.Sin(e.Z) * Math.Sin(e.Y) * 100;

                lineG.X2 = lineG.X1+ (e.Z * e.Z + e.X * e.X) * Math.Sin(e.Z) * Math.Sin(e.X) * 200 * -1;
                lineG.Y2 =  lineG.Y1+ (e.Z * e.Z + e.Y * e.Y) * Math.Sin(e.Z) * Math.Sin(e.Y) * 200;
            });
        }

        private void thumb1_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            tr.X = tr.X + e.HorizontalChange;
            tr.Y = tr.Y + e.VerticalChange;
        }

        private Stroke stroke;
        private void inkPresenter1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            inkPresenter1.CaptureMouse();
            stroke = new Stroke();
            stroke.DrawingAttributes.Color = Colors.Red;
            inkPresenter1.Strokes.Add(stroke);
        }

        private void inkPresenter1_MouseMove(object sender, MouseEventArgs e)
        {
            if (stroke != null)
            {
                stroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkPresenter1));
            }            
        }

        private void inkPresenter1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            inkPresenter1.ReleaseMouseCapture();
            stroke = null;
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            //label1.Content = e.Orientation.ToString();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RadioTestPage.xaml",UriKind.Relative));
        }
    }
}
