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
using WP7SimulatorApp.maps.API;
using System.IO;
using System.Media.Imaging;

namespace WP7SimulatorApp.maps
{
    public partial class UploadCompanyPhotoPage : PhoneApplicationPage
    {
        public UploadCompanyPhotoPage()
        {
            InitializeComponent();
        }

        private void btnCamera_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureTask cameraTask = new CameraCaptureTask();
            cameraTask.Completed += new EventHandler<PhotoResult>(cameraTask_Completed);
            cameraTask.Show();
        }

        void cameraTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage img = new BitmapImage();
                img.SetSource(e.ChosenPhoto);
                imgCompany.Source = img;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int companyId = Convert.ToInt32(this.NavigationContext.QueryString["id"]);

            UploadCompanyPhotoAPI uploadPhotoAPI = new UploadCompanyPhotoAPI(SessionManager.SessionState);
            uploadPhotoAPI.OK += new EventHandler(uploadPhotoAPI_OK);
            uploadPhotoAPI.UnkownResponse += new EventHandler<ResponseEventArgs>(uploadPhotoAPI_UnkownResponse);
            uploadPhotoAPI.Upload(companyId, ConvertToBytes((BitmapImage)imgCompany.Source));            
        }

        void uploadPhotoAPI_UnkownResponse(object sender, ResponseEventArgs e)
        {
            this.HandleUnkownResponse(e);
        }

        public static byte[] ConvertToBytes(BitmapImage bitmapImage)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                WriteableBitmap wBitmap = new WriteableBitmap(bitmapImage);
                wBitmap.SaveJpeg(stream, wBitmap.PixelWidth, wBitmap.PixelHeight, 0, 100);
                stream.Seek(0, SeekOrigin.Begin);
                data = stream.GetBuffer();
            }

            return data;
        }

        void uploadPhotoAPI_OK(object sender, EventArgs e)
        {
            Helpers.ShowMsgDelayClose("上传图片成功！");
            this.Navigate("/maps/MapMainPage.xaml");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Navigate("/maps/MapMainPage.xaml");
        }
    }
}
