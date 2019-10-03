using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Internals;
using System.Resources;
using Microsoft.Phone.Shell.Internals;
using System.Windows.Controls.Primitives;

namespace Microsoft.Phone.Controls
{
    public class PhoneApplicationPage : Page
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(PageOrientation),
            typeof(PhoneApplicationPage), new PropertyMetadata(PageOrientation.Portrait, PageOrientationChange));
        public static readonly DependencyProperty SupportedOrientationsProperty  = DependencyProperty.Register("SupportedOrientations",
            typeof(SupportedPageOrientation), typeof(PhoneApplicationPage), new PropertyMetadata(SupportedPageOrientation.PortraitOrLandscape));
        public static readonly DependencyProperty ApplicationBarProperty = DependencyProperty.Register("ApplicationBar",
            typeof(IApplicationBar), typeof(PhoneApplicationPage), null);

        public event EventHandler<CancelEventArgs> BackKeyPress;

        static void PageOrientationChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //WinPhoneCtrl.Instance.frameScreen.Orientation = (PageOrientation)e.NewValue;
        }

        public PhoneApplicationPage()
        {
            this.NavigationCacheMode = System.Windows.Navigation.NavigationCacheMode.Required;//默认就是缓存页面，不创建新实例
            this.Loaded += new RoutedEventHandler(PhoneApplicationPage_Loaded);           
        }

        internal void ShellPageCallback_OnBackKeyPress(CancelEventArgs e)
        {
            if (BackKeyPress != null)
            {
                BackKeyPress(this, e);
            }
            OnBackKeyPress(e);
        }

        protected virtual void OnBackKeyPress(CancelEventArgs e)
        {
        }

        void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {            
            //因为设置PhoneApplicationPage的Background不能改变PhoneApplicationPage的背景色（UserControl都是这样）
            //必须设定PhoneApplicationPage中的Grid等元素才行。因此无法在资源字典中统一修改背景色
            //所以在设计器显示中修改背景色，看起来更好
            if (AppHelper.IsInDesignMode())
            {
                Panel ctrl = this.Content as Panel;
                if (ctrl != null)
                {
                    //从资源文件中加载背景颜色
                    ResourceDictionary rd = new ResourceDictionary();
                    Application.LoadComponent(rd, new Uri("/Microsoft.Phone;Component/System.Windows.xaml", UriKind.RelativeOrAbsolute));
                    Color color = (Color)rd["PhoneBackgroundColor"];
                    ctrl.Background = new SolidColorBrush(color);
                }                
            }
        }

        /// <summary>
        /// 当页面不再是框架中的活动页面时调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //Remove current applicationbar of this page
            Canvas canvasTouch = WinPhoneCtrl.Instance.canvasTouch;
            foreach (var child in canvasTouch.Children.ToArray())
            {
                if (child is PhoneAppBarCtrl)
                {
                    canvasTouch.Children.Remove(child);
                }
            }

            //close keyboard
            WinPhoneCtrl.Instance.CloseAllKeyBoards();
        }

        /// <summary>
        /// 当页面成为框架中的活动页面时调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ApplicationBar != null)
            {
                //attach application bar to ui
                Canvas canvasTouch = WinPhoneCtrl.Instance.canvasTouch;
                PhoneAppBarCtrl appBarCtrl = new PhoneAppBarCtrl();
                appBarCtrl.DataContext = ApplicationBar;

                //初始时候隐藏菜单栏
                appBarCtrl.Width = WinPhoneCtrl.Instance.frameScreen.Width;
                appBarCtrl.Height = PhoneAppBarCtrl.HideMenuItems_Height;//设定高度为隐藏菜单栏部分的高度
                Canvas.SetLeft(appBarCtrl, 0);
                Canvas.SetTop(appBarCtrl, PhoneAppBarCtrl.HideMenuItems_Top);

                //附加到canvasTouch，不附加到当前应用页面上，避免对应用的VisualTree造成影响
                canvasTouch.Children.Add(appBarCtrl);
            }            
        }

        public PageOrientation Orientation
        {
            get
            {
                return (PageOrientation)base.GetValue(OrientationProperty);
            }
            [EditorBrowsable(EditorBrowsableState.Never)]
            set
            {
                PageOrientation oldValue = Orientation;
                base.SetValue(OrientationProperty, value);
                
                if (oldValue != value)
                {                   
                    if (OrientationChanged != null)
                    {
                        OrientationChangedEventArgs e = new OrientationChangedEventArgs(value);
                        OrientationChanged(this, e);
                        OnOrientationChanged(e);
                    }                    
                }                
            }
        }

        public SupportedPageOrientation SupportedOrientations
        {
            get
            {
                return (SupportedPageOrientation)base.GetValue(SupportedOrientationsProperty);
            }
            set
            {
                if (value != this.SupportedOrientations)
                {
                    base.SetValue(SupportedOrientationsProperty, value);
                }
            }
        }

        protected virtual void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            
        }

        public event EventHandler<OrientationChangedEventArgs> OrientationChanged;

        public IApplicationBar ApplicationBar
        {
            get
            {
                return (IApplicationBar)base.GetValue(ApplicationBarProperty);
            }
            set
            {
                base.SetValue(ApplicationBarProperty, value);
            }
        }
    }
}
