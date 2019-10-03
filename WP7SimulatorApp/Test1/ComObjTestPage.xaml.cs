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
using System.Runtime.InteropServices.Automation;
using Microsoft.Xna.Framework.Framework.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace WP7SimulatorApp.Test1
{
    public partial class ComObjTestPage : PhoneApplicationPage
    {
        public ComObjTestPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //dynamic ie = AutomationFactory.CreateObject("Shell.Application");
            //ie.Open("c:/");
            //using (dynamic shell = AutomationFactory.CreateObject("WScript.Shell"))
            //{
            //    shell.Run(@"%windir%\notepad", 5);
            //}

            //using (dynamic ie = AutomationFactory.CreateObject("Itcast.Net.Phone.InteropServices.PhoneInterop"))
            //{
            //    return ie.GetImageSize(filepath).Height;
            //}

            //MediaLibrary ml = new MediaLibrary();
            //BitmapImage img = new BitmapImage();
            //var pics = ml.RootPictureAlbum.Pictures;
            //img.SetSource(pics[new Random().Next(0, pics.Count)].GetThumbnail());
            //image1.Source = img;
            //MessageBox.Show(pics[0].Height.ToString());

            //foreach (var p in ml.RootPictureAlbum.Albums[0].Pictures)
            //{
            //    MessageBox.Show("" + p.Date + p.Height);
            //}

            //ml.SavePicture("a.jpg", new byte[20]);
            //MessageBox.Show(ml.SavedPictures[0].Name);

            MediaLibrary ml = new MediaLibrary();
            //foreach (var album in ml.Albums)
            //{
            //    MessageBox.Show(album.Songs.Count.ToString());
            //}
           MediaPlayer.Play(ml.Albums[0].Songs[2]);

            
        }
    }
}
