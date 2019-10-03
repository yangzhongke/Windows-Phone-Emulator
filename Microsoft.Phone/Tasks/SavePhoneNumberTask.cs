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
    public class SavePhoneNumberTask : ChooserBase<TaskEventArgs>
    {
        public override void Show()
        {
            base.Show();
            WinPhoneCtrl winPhoneCtrl = WinPhoneCtrl.Instance;
            winPhoneCtrl.Navigate(new Uri("/Microsoft.Phone;component/Internals/Pages/SavePhoneNumberPage.xaml?phoneNumber="+PhoneNumber,
                UriKind.Relative));
        }

        public string PhoneNumber { get; set; }
 
        internal override void FireChooseComplete(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.FireChooseComplete(e);
            TaskEventArgs result = new TaskEventArgs();
            result.TaskResult = SavePhoneNumberPage.TaskResult;
            //调用事件监听的方法，模拟事件触发
            FireEventHandler(e, result);
        }
    }
}
