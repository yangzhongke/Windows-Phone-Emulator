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
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
using Microsoft.Phone.Internals;

namespace Microsoft.Phone.Internals
{
    public partial class WinPhoneBoard : UserControl
    {
        public WinPhoneBoard()
        {
            InitializeComponent();
            WinPhoneCtrl.Instance = this.winPhoneCtrl1;
        }

        //报告当前的倾斜方向
        private void ReportProjection()
        {
            winPhoneCtrl1.OnPlaneProjectionChange(phoneProjection.RotationX, 
                phoneProjection.RotationY, phoneProjection.RotationZ);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.IsRunningOutOfBrowser)
            {
                throw new Exception("模拟器必须以OutOfBrowser方式运行。请在项目上点击右键，选择“属性”，在打开页面的“Silverlight”选项卡，先将“允许在浏览器外运行应用程序”前的复选框的勾选去掉，再将复选框重新选中即可。");
            }
            this.Focus();//只有Focus一下，KeyDown才会触发，否则必须等到一个文本控件获得焦点以后才会被触发
            //设置启动页
            winPhoneCtrl1.StartPageUri = new Uri("/Microsoft.Phone;component/Internals/Pages/SysHomePage.xaml",
                UriKind.Relative);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                phoneProjection.RotationY = (phoneProjection.RotationY+5)%360;
                ReportProjection();
            }
            else if (e.Key == Key.Right)
            {
                phoneProjection.RotationY = SimplifyAngle(phoneProjection.RotationY - 5);
                ReportProjection();
            }
            else if (e.Key == Key.Up)
            {
                phoneProjection.RotationX = SimplifyAngle(phoneProjection.RotationX - 5);
                ReportProjection();
            }
            else if (e.Key == Key.Down)
            {
                phoneProjection.RotationX = SimplifyAngle(phoneProjection.RotationX + 5);
                ReportProjection();
            }
            else if (e.Key == Key.PageUp)
            {
                phoneProjection.RotationZ = SimplifyAngle(phoneProjection.RotationZ + 5);
                ReportProjection();
            }
            else if (e.Key == Key.PageDown)
            {
                phoneProjection.RotationZ = SimplifyAngle(phoneProjection.RotationZ - 5);
                ReportProjection();
            }
        }

        //调整角度到0-360之间
        private static double SimplifyAngle(double a)
        {
            a = a % 360;
            if (a < 0)
            {
                a = a + 360;
            }
            return a;
        }

        private void btnCallIn_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Microsoft.Phone;component/Internals/Pages/CallInPage.xaml?number="+txtComingInCall.Text,
                UriKind.Relative);
            winPhoneCtrl1.NavigateNewApp(uri);
        }

        private void toggleBtnCtrlPanel_Checked(object sender, RoutedEventArgs e)
        {
            canvasCtrlPanel.Visibility = System.Windows.Visibility.Visible;
            toggleBtnCtrlPanel.Content = ">>";
        }

        private void toggleBtnCtrlPanel_Unchecked(object sender, RoutedEventArgs e)
        {
            canvasCtrlPanel.Visibility = System.Windows.Visibility.Collapsed;
            toggleBtnCtrlPanel.Content = "<<";
        }
    }
}
