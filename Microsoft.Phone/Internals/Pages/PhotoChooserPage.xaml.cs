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
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class PhotoChooserPage : PhoneApplicationPage
    {
        internal static string SelectedFileName;
        internal static byte[] SelectedFileData;

        private int PixelHeight;
        private int PixelWidth;

        public PhotoChooserPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string lastPageUri = WinPhoneCtrl.Instance.LastPage.ToString();
            string pagePrefix = "/Microsoft.Phone;component/Internals/Pages/";
            if (lastPageUri.Contains(pagePrefix+"CameraCapturePage.xaml"))//从拍照回来
            {
                //do nothing，因为cameraTask_Completed已经处理了
            }
            else if (lastPageUri.Contains(pagePrefix + "ClipPhotoPage.xaml"))//从截取页面回来
            {
                SelectedFileData = ClipPhotoPage.ClippedData;//保存截取结果
                NavigationService.GoBack();//返回应用
            }
            else if (!lastPageUri.StartsWith(pagePrefix + "PhotoChooserPage.xaml"))//从应用页面过来
            {                
                SelectedFileName = null;
                SelectedFileData = null;

                PixelHeight = Convert.ToInt32(NavigationContext.QueryString["PixelHeight"]);
                PixelWidth = Convert.ToInt32(NavigationContext.QueryString["PixelWidth"]);
                bool ShowCamera = Convert.ToBoolean(NavigationContext.QueryString["ShowCamera"]);
                btnCamera.Visibility = ShowCamera.ToVisibility();
            }            
        }

        //不能用image1.Source = new BitmapImage(new Uri(path,UriKind.Absolute))，否则显示不出来
        private static ImageSource ToImageSource(string filename)
        {
            using (Stream stream = File.OpenRead(filename))
            {
                BitmapImage img = new BitmapImage();
                img.SetSource(stream);
                return img;
            }
        }        

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //加载电脑中“我的图片中的图片”做为模拟的图片库
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var data = from filename in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                       where AppHelper.IsImgFile(filename)
                       select filename;
            wrapPanelPhotos.Children.Clear();           
            foreach (var filename in data)
            {
                var bitmap = ToImageSource(filename);
                Image image = new Image();
                image.Source = bitmap;
                image.Height = 50;
                image.Width = 50;
                image.Tag = filename;// System.IO.Path.GetFileName(filename);
                image.MouseLeftButtonDown += new MouseButtonEventHandler(image_MouseLeftButtonDown);
                wrapPanelPhotos.Children.Add(image);
            }
        }

        void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image =  sender as Image;
            string fullpath = image.Tag.ToString();
            SelectedFileName = System.IO.Path.GetFileName(fullpath);
            SelectedFileData = File.ReadAllBytes(fullpath);
            //如果PixelHeight、PixelWidth不为0，则需要截取
            if (PixelHeight > 0 && PixelWidth > 0)
            {
                string uri = "/Microsoft.Phone;component/Internals/Pages/ClipPhotoPage.xaml?PixelHeight=" + PixelHeight
                    + "&PixelWidth=" + PixelWidth;
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
            else
            {
                //如果不需要截取则返回应用
                NavigationService.GoBack();
            }
        }

        private void btnCamera_Click(object sender, RoutedEventArgs e)
        {
            //拍照
            CameraCaptureTask cameraTask = new CameraCaptureTask();
            cameraTask.Completed += new EventHandler<PhotoResult>(cameraTask_Completed);
            cameraTask.Show();
        }

        void cameraTask_Completed(object sender, PhotoResult e)
        {
            SelectedFileName = e.OriginalFileName;
            SelectedFileData = e.ChosenPhoto.ToBytes();
            //如果PixelHeight、PixelWidth不为0，则需要截取
            if (PixelHeight > 0 && PixelWidth > 0)
            {
                string uri = "/Microsoft.Phone;component/Internals/Pages/ClipPhotoPage.xaml?PixelHeight=" + PixelHeight
                    + "&PixelWidth=" + PixelWidth;
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
            else
            {                
                NavigationService.GoBack();
            }            
        }
    }
}
