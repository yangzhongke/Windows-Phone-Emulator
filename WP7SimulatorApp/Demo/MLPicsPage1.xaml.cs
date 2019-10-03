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
using Microsoft.Xna.Framework.Framework.Media;
using Microsoft.Phone.Tasks;

namespace WP7SimulatorApp.Demo
{
    public partial class MLPicsPage1 : PhoneApplicationPage
    {
        public MLPicsPage1()
        {
            InitializeComponent();

            
            //ml.SavedPictures[0].
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MediaLibrary ml = new MediaLibrary();
            listBox1.DataContext = ml.SavedPictures;
        }
        private void btnTaskSnap_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureTask task = new CameraCaptureTask();
            task.Completed += new EventHandler<PhotoResult>(task_Completed);
            task.Show();
        }

        void task_Completed(object sender, PhotoResult e)
        {
            MediaLibrary ml = new MediaLibrary();
            ml.SavePicture(e.OriginalFileName,e.ChosenPhoto);
        }
    }
}
