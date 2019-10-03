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
using Microsoft.Maps.MapControl;
using System.Windows.Threading;
using Microsoft.Maps.MapControl.Overlays;
using Microsoft.Maps.MapControl.Navigation;
using System.Runtime.InteropServices.Automation;

namespace System.Device.Location.Internals
{
    public partial class MapCtrlBoard : UserControl
    {
        private DateTime lastReportTime = DateTime.MinValue;
        private Microsoft.Maps.MapControl.Location lastReportLocation = null;

        internal static event GeoCoordinateChangeDelegate GeoCoordinateChanged;

        public MapCtrlBoard()
        {
            InitializeComponent();

            var itcastPosition = MapLayer.GetPosition(canvasItcast);//取传智播客的位置
            map.SetView(itcastPosition, 16);//显示传智播客的位置

            //string tileUriFormat = "http://localhost:8080/{QuadKey}.png"; //使用自己搭建的TileServer
            //string tileUriFormat = "http://r3.tiles.ditu.live.com/tiles/r{QuadKey}.png?g=47";
            map.Mode = new ItcastMode(new MapSettings().TileServerUriFormat);

            canvasItcast.MouseLeftButtonUp += (sender, e) =>
            {
                AboutCommand aboutCmd = new AboutCommand(this);
                aboutCmd.Execute(map);
            };

            map.MapForeground.TemplateApplied += new EventHandler(MapForeground_TemplateApplied);

            //定时隐藏错误消息
            DispatcherTimer timerHideErrorMsg = new DispatcherTimer();
            timerHideErrorMsg.Interval = TimeSpan.FromMilliseconds(200);
            timerHideErrorMsg.Tick += new EventHandler(timerHideErrorMsg_Tick);
            timerHideErrorMsg.Start();
        }
        
        internal void ClearRoute()
        {
            foreach (var ellipse in map.Children.ToArray())
            {
                if (ellipse is Ellipse)
                {
                    map.Children.Remove(ellipse);
                }
            }
            lastReportTime = DateTime.MinValue;
            lastReportLocation = null;
        }

        internal void ShowAbout()
        {
            var itcastPosition = MapLayer.GetPosition(canvasItcast);//取传智播客的位置
            map.SetView(itcastPosition, 16);//显示传智播客的位置
            MessageBox.Show(@"传智播客.Net培训出品！传智播客.Net培训，口碑最好的.Net培训机构！
                              .Net培训视频教程免费下载
                              http://net.itcast.cn。传智播客的网址已经复制到剪贴板，欢迎访问！");
            Clipboard.SetText("http://net.itcast.cn");
            dynamic shell =AutomationFactory.CreateObject("Shell.Application");
            shell.ShellExecute("http://net.itcast.cn");
        }

        void MapForeground_TemplateApplied(object sender, EventArgs e)
        {
            map.MapForeground.NavigationBar.TemplateApplied += new EventHandler(NavigationBar_TemplateApplied);
        }

        void NavigationBar_TemplateApplied(object sender, EventArgs e)
        {
            //Remove the buttons for mode selection
            NavigationBar navControl = map.MapForeground.NavigationBar;
            navControl.HorizontalPanel.Children.Clear();

            CommandButton btnAbout = new CommandButton(new AboutCommand(this), "About");
            navControl.HorizontalPanel.Children.Add(btnAbout);

            CommandButton btnBings = new CommandButton(new ClearRouteCommand(this), "Clear");
            navControl.HorizontalPanel.Children.Add(btnBings);
        }

        void timerHideErrorMsg_Tick(object sender, EventArgs e)
        {
            //因为BMSL在OOB中运行的时候有错误消息，所以定时隐藏
            var items = CommonHelper.FindDesendants<LoadingErrorMessage>(LayoutRoot);
            foreach (var item in items)
            {
                item.Visibility = Windows.Visibility.Collapsed;
            }            
        }

        private void map_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //获得鼠标点击的点在本上的控件坐标
            var point = e.GetPosition(map);
            //将控件坐标转换为地理坐标
            var location = map.ViewportPointToLocation(new Point(point.X, point.Y));

            //一个轨迹点
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(Colors.Red);
            ellipse.Width = 5;
            ellipse.Height = 5;
            map.Children.Add(ellipse);
            //设定为MapLayer定位，这样会自动移动、缩放
            MapLayer.SetPosition(ellipse, location);
            e.Handled = true;//阻止弹出右键菜单

            ReportData(location);            
        }

        private void ReportData(Microsoft.Maps.MapControl.Location location)
        {
            if (lastReportLocation == null)
            {
                //not report when the first point
                lastReportTime = DateTime.Now;
                lastReportLocation = location;
                return;
            }

            double distance = CommonHelper.distance(lastReportLocation.Latitude, lastReportLocation.Longitude,
                location.Latitude, location.Longitude, 'M');
            double course = CommonHelper.CalcCourse(lastReportLocation, location);
            if (course < 0.0)
            {
                course = 360.0 - Math.Abs(course);
            }
            TimeSpan timeSpan = DateTime.Now - lastReportTime;
            double speed = distance / timeSpan.TotalMinutes;//一分钟算一个小时，模拟
            lastReportTime = DateTime.Now;
            lastReportLocation = location;
            if (GeoCoordinateChanged != null)
            {
                GeoCoordinateChanged(location.Latitude, location.Longitude, location.Altitude, course, speed);
            }            
        }
    }

    internal delegate void GeoCoordinateChangeDelegate(double latitude,double longitude,double altitude,double course,double speed);
}
