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
using System.Windows.Media.Imaging;
using System.IO;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class CameraCapturePage : PhoneApplicationPage
    {
        CaptureSource _captureSource;
        VideoBrush _videoBrush = new VideoBrush();//新建视频画刷

        MediaElement mediaFakeVideo;//模拟视频

        static internal byte[] CaptureResult
        {
            get;
            private set;
        }

        public CameraCapturePage()
        {
            InitializeComponent();
        }


        private void LayoutRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_captureSource != null)
            {
                _captureSource.Stop();
            }
            
            StopFakeVideo();
        }

        private void StopFakeVideo()
        {
            if (mediaFakeVideo != null)
            {
                mediaFakeVideo.Stop();//停止模拟摄像头
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            CaptureResult = null;//清空旧的数据

            if (CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices().Count <= 0)
            {
                MessageBox.Show("VideoCaptureDevice not found,please DragDrop pictures or wmv videos instead.");
                //必须设置Fill，否则LayoutRoot_Drop不会被触发
                rectPreview.Fill = new SolidColorBrush(Colors.Black);
                return;
            }
            _captureSource = new CaptureSource();
            VideoCaptureDevice _video = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
            if (_video != null)
            {
                _captureSource.VideoCaptureDevice = _video;                
                _videoBrush.SetSource(_captureSource);
            }
            if (CaptureDeviceConfiguration.AllowedDeviceAccess ||
                CaptureDeviceConfiguration.RequestDeviceAccess())
            {
                rectPreview.Fill = _videoBrush;//画刷画到矩形中
                _captureSource.Start();//启动
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            //播放拍照声
            Stream stream = AppHelper.GetExecutingAssemblyResourceStream("Internals.audios.shutter.wma");
            shutterSoundPlayer.SetSource(stream);
            //拍照声播放完毕以后再返回数据
        }

        private void shutterSoundPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            //拍照声结束后获得数据返回
            WriteableBitmap bitmap = new WriteableBitmap(rectPreview, null);
            CaptureResult = AppHelper.BitMapToByte(bitmap);
            NavigationService.GoBack();
        }

        private void LayoutRoot_Drop(object sender, DragEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            //获得拖放过来的文件。用拖过来的wmv视频或者图片当作摄像头源，以满足没有摄像头的缺陷。
            FileInfo[] files = e.Data.GetData(DataFormats.FileDrop) as FileInfo[];
            if (files.Length <= 0)
            {
                return;
            }
            if (_captureSource != null)
            {
                _captureSource.Stop();//停止电脑摄像头捕捉
            }            

            FileInfo file = files[0];
            string fileext = System.IO.Path.GetExtension(file.Name);

            //用wmv视频做模拟摄像头
            if (fileext == ".wmv")
            {
                mediaFakeVideo = new MediaElement();
                mediaFakeVideo.Volume = 0;
                mediaFakeVideo.SetSource(files[0].OpenRead());
                _videoBrush.SetSource(mediaFakeVideo);//用视频做源

                rectPreview.Fill = _videoBrush;
            }
            //用图片做模拟摄像头
            else if (fileext == ".jpg" || fileext == ".bmp"
                || fileext == ".jpeg" || fileext == ".png" || fileext == ".gif")
            {
                StopFakeVideo();

                //读取图片
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(files[0].OpenRead());

                //创建图片画刷
                ImageBrush imgBrush = new ImageBrush();
                imgBrush.Stretch = Stretch.None;                
                imgBrush.ImageSource = bitmap;

                rectPreview.Fill = imgBrush;//用图片画刷填充rectPreview
            }
            else
            {
                MessageBox.Show("不支持的文件类型!");
            }
        }
    }
}
