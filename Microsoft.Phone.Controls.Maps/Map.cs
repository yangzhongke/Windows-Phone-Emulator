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
using MSMap = Microsoft.Maps.MapControl;
using System.Windows.Threading;
using System.Collections.Generic;
using Microsoft.Maps.MapControl.Navigation;
using Microsoft.Maps.MapControl.Overlays;
using Microsoft.Maps.MapControl.Core;

namespace Microsoft.Phone.Controls.Maps
{
    public class Map : MSMap.Map
    {
        public static readonly DependencyProperty ZoomBarVisibilityProperty = DependencyProperty.Register("ZoomBarVisibility", typeof(Visibility), typeof(Map), new PropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(Map.OnOverlayVisibilityChangedCallback)));

        private static void OnOverlayVisibilityChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Map)d).OnOverlayVisibilityChanged(e);
        }

        private Canvas canvasZoomBar;

        public Map()
        {
            //因为BMSL自带的RoadMode不能OOB中运行，所以需要使用自己的RoadMode代替
            this.Mode = new RoadMode();

            //定时隐藏错误消息
            DispatcherTimer timerHideErrorMsg = new DispatcherTimer();
            timerHideErrorMsg.Interval = TimeSpan.FromMilliseconds(200);
            timerHideErrorMsg.Tick += new EventHandler(timerHideErrorMsg_Tick);
            timerHideErrorMsg.Start();

            //默认隐藏导航
            this.NavigationVisibility = System.Windows.Visibility.Collapsed;

            //缩放面板
            canvasZoomBar = new Canvas();
            canvasZoomBar.Visibility = ZoomBarVisibility;
            this.Children.Add(canvasZoomBar);

            //放大、缩小按钮是黑色的
            Brush zoomBarForeColor = new SolidColorBrush(Colors.Black);

            //放大、缩小按钮
            Button btnZoomIn = new Button();
            btnZoomIn.Height = 50;
            btnZoomIn.Width = 50;
            btnZoomIn.Content = "+";
            btnZoomIn.BorderBrush = zoomBarForeColor;
            btnZoomIn.Foreground = zoomBarForeColor;
            Canvas.SetLeft(btnZoomIn, 50);
            Canvas.SetTop(btnZoomIn, 250);
            btnZoomIn.Click += (sender, e) => {
                this.ZoomLevel++;
            };
            canvasZoomBar.Children.Add(btnZoomIn);

            Button btnZoomOut = new Button();
            btnZoomOut.Height = 50;
            btnZoomOut.Width = 50;
            btnZoomOut.Content = "-";
            btnZoomOut.BorderBrush = zoomBarForeColor;
            btnZoomOut.Foreground = zoomBarForeColor;
            Canvas.SetLeft(btnZoomOut, 100);
            Canvas.SetTop(btnZoomOut, 250);
            btnZoomOut.Click += (sender, e) =>
            {
                this.ZoomLevel--;
            };
            canvasZoomBar.Children.Add(btnZoomOut);
        }

        protected override void OnOverlayVisibilityChanged(DependencyPropertyChangedEventArgs eventArgs)
        {
            base.OnOverlayVisibilityChanged(eventArgs);
            //控制缩放条的可见性
            if (canvasZoomBar != null)
            {
                canvasZoomBar.Visibility = ZoomBarVisibility;
            }            
        }

        public Visibility ZoomBarVisibility
        {
            get
            {
                return (Visibility)base.GetValue(ZoomBarVisibilityProperty);
            }
            set
            {
                base.SetValue(ZoomBarVisibilityProperty, value);
            }
        }

        void timerHideErrorMsg_Tick(object sender, EventArgs e)
        {
            //因为BMSL在OOB中运行的时候有错误消息，所以定时隐藏
            var items = FindDesendants<LoadingErrorMessage>(this);
            foreach (var item in items)
            {
                item.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// 查找baseObj中类型为T的子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseObj"></param>
        /// <returns></returns>
        static IEnumerable<T> FindDesendants<T>(DependencyObject baseObj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(baseObj); i++)
            {
                var child = VisualTreeHelper.GetChild(baseObj, i);
                if (child is T)
                {
                    yield return (T)child;
                }
                foreach (var c in FindDesendants<T>(child))
                {
                    yield return (T)c;
                }
            }
        }
    }
}
