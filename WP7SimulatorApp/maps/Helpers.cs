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
using System.Windows.Resources;
using System.IO;
using System.Windows.Markup;
using WP7SimulatorApp.maps.ctrls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Threading;
using WP7SimulatorApp.maps.API;
using Microsoft.Phone.Devices;

namespace WP7SimulatorApp.maps
{
    public static class Helpers
    {
        public static void HandleUnkownResponse(this Page page,
            ResponseEventArgs e)
        {
            Helpers.ShortVibrate();
            MessageBox.Show("程序发生未知错误，抱歉，这个页面用一个报错页面会更好看，todo。出错Url：" + e.RequestUrl);
        }

        public static Storyboard LoadStoryboard(string name)
        {
            string path = "/Microsoft.Phone.Controls.Toolkit;component/Transitions/Storyboards/" + name + ".xaml";
            Uri uri = new Uri(path, UriKind.Relative);
            StreamResourceInfo streamResourceInfo = Application.GetResourceStream(uri);
            using (StreamReader streamReader = new StreamReader(streamResourceInfo.Stream))
            {
                string xaml = streamReader.ReadToEnd();
                return XamlReader.Load(xaml) as Storyboard;
            }
        }

        private static Popup popupMsg = new Popup();
        /// <summary>
        /// 动态显示消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="showProgressing">是否显示进度条</param>
        public static void ShowMsg(string msg,bool showProgressing=true)
        {
            ProgressingCtrl ctrl = new ProgressingCtrl();
            popupMsg.Margin = new Thickness(50, 50, 50, 50);
            ctrl.Message = msg;
            ctrl.ShowProgressing = showProgressing;
            popupMsg.Child = ctrl;
            popupMsg.IsOpen = true;
            Storyboard sb = Helpers.LoadStoryboard("SlideDownFadeIn");
            popupMsg.RenderTransform = new TranslateTransform();
            Storyboard.SetTarget(sb, popupMsg);
            sb.Begin();
        }

        /// <summary>
        /// 演示执行action委托
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        public static void DelayExecute(TimeSpan ts,Action action)
        {            
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = ts;
            timer.Tick += (sender, e) => {
                timer.Stop();
                action();
            };
            timer.Start();
        }

        /// <summary>
        /// 关闭消息
        /// </summary>
        public static void ClosePopMsg()
        {
            Storyboard sb = Helpers.LoadStoryboard("SlideUpFadeOut");
            popupMsg.RenderTransform = new TranslateTransform();
            Storyboard.SetTarget(sb, popupMsg);
            sb.Completed += (sender, e) =>
            {
                //动画播放完成关闭popup
                popupMsg.IsOpen = false;
            };
            sb.Begin();            
        }

        /// <summary>
        /// 显示消息，然后延时关闭
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ts"></param>
        public static void ShowMsgDelayClose(string msg,TimeSpan ts)
        {
            Helpers.ShowMsg(msg,false);
            Helpers.DelayExecute(ts,
                () =>
                {
                    Helpers.ClosePopMsg();
                });
        }

        /// 显示消息，然后延时3秒钟关闭
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ts"></param>
        public static void ShowMsgDelayClose(string msg)
        {
            ShowMsgDelayClose(msg, TimeSpan.FromSeconds(3));
        }

        public static void Navigate(this Page page,string pageUri)
        {
            page.NavigationService.Navigate(new Uri(pageUri,UriKind.Relative));
        }

        public static void ShortVibrate()
        {
            VibrateController.Default.Start(TimeSpan.FromMilliseconds(500));
        }
    }
}
