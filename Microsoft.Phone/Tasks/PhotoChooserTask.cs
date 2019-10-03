using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Internals;
using Microsoft.Phone.Internals.Pages;
using System.IO;

namespace Microsoft.Phone.Tasks
{
    public class PhotoChooserTask : ChooserBase<PhotoResult>
    {
        public PhotoChooserTask()
        {
            PixelHeight = 0;
            PixelWidth = 0;
            ShowCamera = false;
        }

        public override void Show()
        {
            base.Show();
            WinPhoneCtrl winPhoneCtrl = WinPhoneCtrl.Instance;
            string uri = "/Microsoft.Phone;component/Internals/Pages/PhotoChooserPage.xaml?PixelHeight="
                + PixelHeight + "&PixelWidth=" + PixelWidth + "&ShowCamera=" + ShowCamera;
            winPhoneCtrl.Navigate(new Uri(uri, UriKind.Relative));
        }

        internal override void FireChooseComplete(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.FireChooseComplete(e);
            PhotoResult result = new PhotoResult();
            if (PhotoChooserPage.SelectedFileData == null)
            {
                result.TaskResult = TaskResult.Cancel;
            }
            else
            {
                result.ChosenPhoto = new MemoryStream(PhotoChooserPage.SelectedFileData);
                result.Error = null;
                result.OriginalFileName = PhotoChooserPage.SelectedFileName;
                result.TaskResult = TaskResult.OK;
            }
            //调用事件监听的方法，模拟事件触发
            FireEventHandler(e, result);
        }

        public int PixelHeight
        {
            get;
            set;
        }

        public int PixelWidth
        {
            get;
            set;
        }

        public bool ShowCamera
        {
            get;
            set;
        }

    }
}
