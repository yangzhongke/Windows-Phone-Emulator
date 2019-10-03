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
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Internals.Pages;
using Microsoft.Phone.Controls;
using System.IO;

namespace Microsoft.Phone.Tasks
{
    public class CameraCaptureTask : ChooserBase<PhotoResult>
    {
        public override void Show()
        {
            base.Show();
            WinPhoneCtrl winPhoneCtrl = WinPhoneCtrl.Instance;
            winPhoneCtrl.Navigate(new Uri("/Microsoft.Phone;component/Internals/Pages/CameraCapturePage.xaml",
                UriKind.Relative));

        }

        internal override void FireChooseComplete(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.FireChooseComplete(e);
            PhotoResult result = new PhotoResult();
            if (CameraCapturePage.CaptureResult == null)
            {
                result.TaskResult = TaskResult.Cancel;
            }
            else
            {
                result.ChosenPhoto = new MemoryStream(CameraCapturePage.CaptureResult);
                result.Error = null;
                result.OriginalFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpeg";
                result.TaskResult = TaskResult.OK;
            }
            //调用事件监听的方法，模拟事件触发
            FireEventHandler(e, result);
        }
    }
}
