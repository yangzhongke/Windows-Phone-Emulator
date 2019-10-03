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
    public partial class ClipPhotoPage : PhoneApplicationPage
    {
        public static byte[] ClippedData;

        public ClipPhotoPage()
        {
            InitializeComponent();
            this.NavigationCacheMode = System.Windows.Navigation.NavigationCacheMode.Disabled;
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(new MemoryStream(PhotoChooserPage.SelectedFileData));
            imgClip.Source = bitmap;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int PixelHeight = Convert.ToInt32(NavigationContext.QueryString["PixelHeight"]);
            int PixelWidth = Convert.ToInt32(NavigationContext.QueryString["PixelWidth"]);
            ClippedData = null;
            thumbClip.Width = PixelWidth;
            thumbClip.Height = PixelHeight;
        }

        private void thumbClip_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            tr.X = tr.X + e.HorizontalChange;
            tr.Y = tr.Y + e.VerticalChange;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //截取指定大小的图片
            WriteableBitmap bitmap = new WriteableBitmap((int)thumbClip.Width, (int)thumbClip.Height);
            TranslateTransform transform = new TranslateTransform();
            transform.Y = -1 * tr.Y;
            transform.X = -1 * tr.X;
            bitmap.Render(imgClip, transform);//把imgClip按照t转换画到bitmap中
            bitmap.Invalidate();
            ClippedData = AppHelper.BitMapToByte(bitmap);
            sbClipSuccess.Begin();//启动动画
        }

        private void sbClipSuccess_Completed(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();//动画播放完成返回
        }
    }
}
