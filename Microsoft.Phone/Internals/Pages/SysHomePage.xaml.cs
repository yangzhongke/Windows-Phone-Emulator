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
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;
using System.Threading;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class SysHomePage : PhoneApplicationPage
    {
        private LaunchScreen _currentLaunchScreen;

        private ApplicationModel ApplicationInfo;
        public SysHomePage()
        {
            InitializeComponent();
            this._currentLaunchScreen = LaunchScreen.TileList;

            this.ApplicationInfo = new ApplicationModel();

            this.ApplicationInfo.DisplayName = WMAppConfiguration.Instance.AppTitle;
            this.ApplicationInfo.PageUrl = new Uri("/" + WMAppConfiguration.Instance.NavigationPage, UriKind.Relative);
            //加载图标，图标的BuildAction必须是“内容Content”
            this.ApplicationInfo.IconUrl = new Uri("/"+WMAppConfiguration.Instance.AppIconPath,UriKind.Relative);
            this.ApplicationInfo.TemplateType5BackgroundImageURI = new Uri("/" + WMAppConfiguration.Instance.TemplateType5BackgroundImageURI, UriKind.Relative);
            this.ApplicationInfo.TemplateType5Title =
                WMAppConfiguration.Instance.TemplateType5Title;

            this.DataContext = ApplicationInfo;
        }

        //private static BitmapImage GetBitmapImage(string iconPath)
        //{
        //    Assembly appAsm = Application.Current.GetType().Assembly;//当前应用的Assembly
        //    Stream iconStream = AppHelper.GetAssemblyResourceStream(appAsm, iconPath);
        //    BitmapImage icon = new BitmapImage();
        //    icon.SetSource(iconStream);
        //    return icon;
        //}

        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            //显示Splash图片
            Popup popup = new Popup();
            Image img = new Image();
            img.Width = WinPhoneCtrl.Instance.frameScreen.Width;
            img.Height = WinPhoneCtrl.Instance.frameScreen.Height;
            img.Source = new BitmapImage(new Uri("/SplashScreenImage.jpg", UriKind.Relative));
            popup.Child = img;
            popup.IsOpen = true;
            Thread thread = new Thread(() =>
            {
                //模拟启动耗时1秒
                Thread.Sleep(1000);
                //Sleep完成后再启动应用
                Dispatcher.BeginInvoke(() =>
                {
                    //不用管关闭，因为WinPhoneCtrl的frameScreen_Navigating中实现了导航结束以后会自动关闭未关闭的Popup
                    //popup.IsOpen = false;
                    WinPhoneCtrl.Instance.NavigateNewApp(ApplicationInfo.PageUrl);
                });
            });
            thread.Start();                 
        }

        private void btnShowApplicationsList_Click(object sender, RoutedEventArgs e)
        {
            if (this._currentLaunchScreen == LaunchScreen.TileList)
            {
                this._currentLaunchScreen = LaunchScreen.ApplicationIconList;
                this.resShowApplicationListStoryboard.Begin();
            }
            else if (this._currentLaunchScreen == LaunchScreen.ApplicationIconList)
            {
                this._currentLaunchScreen = LaunchScreen.TileList;
                this.resShowTileListStoryboard.Begin();
            }
        }

        private enum LaunchScreen
        {
            TileList,
            ApplicationIconList,
            Animating
        }

        private void imgSampleTile_IE_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartNewApp("/Microsoft.Phone;component/Internals/Pages/WebBrowserPage.xaml");
        }

        private static void StartNewApp(string uri)
        {
            WinPhoneCtrl.Instance.NavigateNewApp(new Uri(uri, UriKind.Relative));
        }

        private void imgSampleTile_Phone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartNewApp("/Microsoft.Phone;component/Internals/Pages/AboutPage.xaml");
        }

        private void imgSampleTile_Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartNewApp("/Microsoft.Phone;component/Internals/Pages/SettingsPage.xaml");
        }
    }

    public class ApplicationModel
    {
        public string DisplayName { get; set; }
        public Uri IconUrl { get; set; }
        public Uri PageUrl { get; set; }
        public string TemplateType5Title { get; set; }
        public Uri TemplateType5BackgroundImageURI { get; set; }
    }
}
