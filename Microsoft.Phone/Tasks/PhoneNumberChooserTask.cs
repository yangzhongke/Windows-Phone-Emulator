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

namespace Microsoft.Phone.Tasks
{
    public class PhoneNumberChooserTask : ChooserBase<PhoneNumberResult>
    {
        public override void Show()
        {
            base.Show();
            WinPhoneCtrl winPhoneCtrl = WinPhoneCtrl.Instance;
            winPhoneCtrl.Navigate(new Uri("/Microsoft.Phone;component/Internals/Pages/PhoneNumberChooserPage.xaml",
                UriKind.Relative));

        }

        internal override void FireChooseComplete(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.FireChooseComplete(e);
            PhoneNumberResult result = new PhoneNumberResult();
            if (PhoneNumberChooserPage.PhoneNumber == null)
            {
                result.TaskResult = TaskResult.Cancel;
            }
            else
            {
                result.PhoneNumber = PhoneNumberChooserPage.PhoneNumber;
                result.Error = null;
                result.TaskResult = TaskResult.OK;
            }
            //调用事件监听的方法，模拟事件触发
            FireEventHandler(e, result);
        }
    }
}
