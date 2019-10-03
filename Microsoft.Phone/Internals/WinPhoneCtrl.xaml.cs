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
using System.Windows.Threading;
using System.IO;
using System.Reflection;
using Microsoft.Phone.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Microsoft.Phone.Internals.ctrls;
using MIRIA.Devices.MultiTouch;
using Microsoft.Phone.Internals.touch;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Microsoft.Phone.Internals
{
    public partial class WinPhoneCtrl : UserControl
    {
        public DependencyProperty StartPageUriProperty = DependencyProperty.Register("StartPageUri",
            typeof(Uri), typeof(WinPhoneCtrl), null);

        public static WinPhoneCtrl Instance
        {
            get;
            set;
        }

        private double OriginHeight;//390
        private double OriginWidth = 228;//228

        public WinPhoneCtrl()
        {
            InitializeComponent();
            
            OriginHeight = frameScreen.Height;//读取原始尺寸，方便变换方向
            OriginWidth = frameScreen.Width;
            
            canvasTouch.GestureDetected += (sender, e) => {
                CloseAllKeyBoards();
                ////如果在非TextBox上点击鼠标，则关闭软键盘
                //if ((e.OriginalSource is TextBox) == false)
                //{
                //    CloseAllKeyBoards();
                //}
            };

            //设置剪裁区域，只显示屏幕部分
            frameScreen.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0,
                frameScreen.Width, frameScreen.Height)
            };
            canvasTouch.GestureDetected += new MIRIA.UIKit.TCanvas.GestureDetectedHandler(canvasTouch_GestureDetected);
            canvasTouch.Hold += new MIRIA.UIKit.TCanvas.HoldHandler(canvasTouch_Hold);
            if (!AppHelper.IsInDesignMode())
            {
                new TouchListener(canvasTouch);
            }

            //定时检测新的Popup，并且把没有Parent的Popup调整到屏幕内部
            DispatcherTimer timerScanPopups = new DispatcherTimer();
            timerScanPopups.Tick += (sender, e) => { PopupHellper.ScanPopups(); };
            timerScanPopups.Interval = TimeSpan.FromMilliseconds(50);
            timerScanPopups.Start();
        }

        public TouchArgs ReadCurrentTouchArgs()
        {
            if (canvasTouch.GesturesInterpreter.Fingers.Count <= 0)
            {
                return null;
            }
            var gesture = canvasTouch.GesturesInterpreter.CurrentGesture;
            var finger = canvasTouch.GesturesInterpreter.Fingers[0];
            TouchArgs touchArgs = new TouchArgs();
            touchArgs.GestureType = (Gesture)Enum.Parse(
                typeof(Gesture), gesture.ToString(), true);
            touchArgs.PathAngle = finger.PathAngle;
            touchArgs.PathLength = finger.PathLength;
            touchArgs.Position = finger.Position;
            touchArgs.Shift = finger.Shift;
            touchArgs.StartPosition = finger.StartPosition;
            return touchArgs;
        }

        void canvasTouch_Hold(object sender, MIRIA.Gestures.GestureHoldEventArgs e)
        {
            
        }

        public event EventHandler<TouchArgs> GestureDetected;

        void canvasTouch_GestureDetected(object sender, MIRIA.Gestures.GestureDetectedEventArgs e)
        {
            TouchArgs touchArgs = ReadCurrentTouchArgs();
            if (GestureDetected != null)
            {
                GestureDetected(null,touchArgs);
            }
        }

        //起始页
        public Uri StartPageUri
        {
            get
            {
                return (Uri)base.GetValue(StartPageUriProperty);
            }
            set
            {
                base.SetValue(StartPageUriProperty, value);
                frameScreen.Source = StartPageUri;
            }
        }

        private RepeatedMediaElement vibratePlayer;//震动声音播放器

        internal void Vibrate(TimeSpan duration)
        {
            int seconds = (int)duration.TotalMilliseconds / 1000;//求一共持续几秒
            if (seconds <= 1)
            {
                seconds = 1;
            }
            storyboardVibrate.AutoReverse = true;
            storyboardVibrate.RepeatBehavior = new RepeatBehavior(seconds*10); //震动音频正好1秒钟。每次动画持续0.1秒
            this.storyboardVibrate.Begin();           

            //MediaElement要求mediaplayer 10
            MediaElement media = new MediaElement();
            media.Visibility = System.Windows.Visibility.Collapsed;
            LayoutRoot.Children.Add(media);
            
            media.Volume = 1;//在0 与1 之间的线性标尺上所表示的媒体音量
            //only support wma。
            Stream stream = AppHelper.GetExecutingAssemblyResourceStream("Internals.audios.vibrate.wma");
            media.SetSource(stream);
            vibratePlayer = new RepeatedMediaElement(media);
            vibratePlayer.Play(seconds);
        }

        //停止震动
        public void StopVibrate()
        {
            storyboardVibrate.Seek(TimeSpan.Zero);
            storyboardVibrate.Stop();
            vibratePlayer.Stop();
        }

        public event PlaneProjectionChangeDelegate PlaneProjectionChange;

        public void OnPlaneProjectionChange(double x, double y, double z)
        {
            PhoneApplicationPage phonePage = (PhoneApplicationPage)frameScreen.Content;
            //如果还没有加载引用页面，则返回
            if (phonePage == null)
            {
                return;
            }
            if (PlaneProjectionChange != null)
            {
                PlaneProjectionChange(x, y, z);
            }
            if (z >= 60 && z <= 120)
            {
                LandscapeRightOrient();
            }
            else if (z >= 240 && z <= 300)
            {
                LandscapeLeftOrient();
            }
            else if (z >= 340 || z <= 20)
            {
                PortraitUpOrient();
            }
            else if (z >= 150 && z <= 210)
            {
                PortraitDownOrient();
            }
        }

        internal void PortraitUpOrient()
        {
            PhoneApplicationPage phonePage = (PhoneApplicationPage)frameScreen.Content;
            //如果不支持，则不处理
            if (phonePage.SupportedOrientations == SupportedPageOrientation.Landscape)
            {
                return;
            }
            screenProjection.CenterOfRotationX = 0;
            screenProjection.CenterOfRotationY = 0;
            screenProjection.RotationZ = 0;

            frameScreen.Height = OriginHeight;
            frameScreen.Width = OriginWidth;

            transformScreen.TranslateX = 0;
            transformScreen.TranslateY = 0;           
            
            phonePage.Orientation = PageOrientation.PortraitUp;            
        }

        internal void PortraitDownOrient()
        {
            PhoneApplicationPage phonePage = (PhoneApplicationPage)frameScreen.Content;
            //如果不支持，则不处理
            if (phonePage.SupportedOrientations == SupportedPageOrientation.Landscape)
            {
                return;
            }
            screenProjection.CenterOfRotationX = 0;
            screenProjection.CenterOfRotationY = 0;
            screenProjection.RotationZ = 180;

            frameScreen.Height = OriginHeight;
            frameScreen.Width = OriginWidth;

            transformScreen.TranslateX = -OriginWidth;
            transformScreen.TranslateY = -OriginHeight;

            phonePage.Orientation = PageOrientation.PortraitDown;
        }

        internal void LandscapeRightOrient()
        {
            PhoneApplicationPage phonePage = (PhoneApplicationPage)frameScreen.Content;
            //如果不支持，则不处理
            if (phonePage.SupportedOrientations == SupportedPageOrientation.Portrait)
            {
                return;
            }

            screenProjection.CenterOfRotationX = 0;
            screenProjection.CenterOfRotationY = 0;
            screenProjection.RotationZ = -90;

            frameScreen.Height = OriginWidth;
            frameScreen.Width = OriginHeight;

            transformScreen.TranslateX = 0;
            transformScreen.TranslateY = -OriginWidth;

            phonePage.Orientation = PageOrientation.LandscapeLeft;
        }

        internal void LandscapeLeftOrient()
        {
            PhoneApplicationPage phonePage = (PhoneApplicationPage)frameScreen.Content;
            //如果不支持，则不处理
            if (phonePage.SupportedOrientations == SupportedPageOrientation.Portrait)
            {
                return;
            }

            screenProjection.CenterOfRotationX = 0;
            screenProjection.CenterOfRotationY = 0;
            screenProjection.RotationZ = 90;

            frameScreen.Height = OriginWidth;
            frameScreen.Width = OriginHeight;

            transformScreen.TranslateY = 0;
            transformScreen.TranslateX = -OriginHeight;

            phonePage.Orientation = PageOrientation.LandscapeRight;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //尝试关闭输入法，如果存在被关闭输入法，则不继续后退
            //真机就是这样，在输入法界面Back只会关闭输入法
            if (CloseAllKeyBoards())
            {
                return;
            }
            var page = AppHelper.GetCurrentPhoneAppPage();
            if (page != null)
            {
                CancelEventArgs cancelArgs = new CancelEventArgs(false);
                page.ShellPageCallback_OnBackKeyPress(cancelArgs);
                if (cancelArgs.Cancel)
                {
                    return;
                }
            }
            if (frameScreen.CanGoBack)
            {
                frameScreen.GoBack();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigateNewApp(new Uri("/Microsoft.Phone;component/Internals/Pages/SearchPage.xaml", UriKind.Relative));
        }

        public void Navigate(Uri uri)
        {
            frameScreen.Navigate(uri);
        }

        /// <summary>
        /// 转向外部应用页面
        /// </summary>
        /// <param name="uri"></param>
        public void NavigateNewApp(Uri uri)
        {
            //禁用当前页面缓存，模拟Kill当前应用
            AppHelper.GetCurrentPhoneAppPage().NavigationCacheMode = System.Windows.Navigation.NavigationCacheMode.Disabled;
            Navigate(uri);                 
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            PhoneApplicationPage page = frameScreen.Content as PhoneApplicationPage;
            
        }

        public Uri LastPage//上一页
        {
            get;
            private set;
        }

        private void frameScreen_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (sender == null)
            {
                return;      
            }            
            NavigationService NavigationService = (NavigationService)sender;
            LastPage = NavigationService.CurrentSource;

            //如果是从应用导航回首页，则销毁应用
            if (e.Uri.ToString().Equals("/Microsoft.Phone;component/Internals/Pages/SysHomePage.xaml"))
            {
                var appPage = AppHelper.GetCurrentPhoneAppPage();
                if (appPage == null)
                {
                    return;
                }
                if (e.NavigationMode == NavigationMode.Back)
                {
                    appPage.NavigationCacheMode = NavigationCacheMode.Disabled;
                }                
            }
            //Page导航的时候将所有没有Parent的Popup关闭
            //因为Page导航走的时候Page不会自动关闭
            PopupHellper.CloseRootPopups();
        }

        public Panel RootPanel
        {
            get
            {
                return LayoutRoot;
            }
        }

        private void frameScreen_GotFocus(object sender, RoutedEventArgs e)
        {
            CloseAllKeyBoards();
            Control inputControl = e.OriginalSource as Control;
            if (inputControl is TextBox || inputControl is PasswordBox)
            {
                TextBox textbox = e.OriginalSource as TextBox;
                //如果TextBox获得焦点
                if (textbox != null && textbox.IsReadOnly)//如果文本框是只读，则不显示软键盘
                {
                    return;
                }
                KeyboardContainer kbContainer = new KeyboardContainer();
                kbContainer.CurrentInputCtrl = (Control)e.OriginalSource;//将键盘服务的控件设置为焦点的控件

                //attach application bar to ui
                Canvas canvasTouch = WinPhoneCtrl.Instance.canvasTouch;

                kbContainer.Width = WinPhoneCtrl.Instance.frameScreen.Width;
                kbContainer.Height = 228;
                Canvas.SetLeft(kbContainer, 0);


                var transform = inputControl.TransformToVisual(canvasTouch);
                var point = transform.Transform(new Point(0, 0));
                //键盘总是显示在最底下
                Canvas.SetTop(kbContainer, canvasTouch.ActualHeight - kbContainer.Height);

                //如果文本框下面放不下键盘，则将Page上移，将TextBox移到最顶端
                if (point.Y + inputControl.Height > 390 - kbContainer.Height)
                {
                    SetPageContentTransformY(-point.Y);
                }
                else
                {
                    //把界面移位归位
                    SetPageContentTransformY(0);
                }

                //附加到canvasTouch，不附加到当前应用页面上，避免对应用的VisualTree造成影响
                canvasTouch.Children.Add(kbContainer);
            }
        }

        /// <summary>
        /// 关闭所有软键盘
        /// 返回值表示是否存在被关闭的键盘
        /// </summary>
        public bool CloseAllKeyBoards()
        {
            bool hasCloseKeyBoard = false;
            Canvas canvasTouch = WinPhoneCtrl.Instance.canvasTouch;
            foreach (var child in canvasTouch.Children.ToArray())
            {
                if (child is KeyboardContainer)
                {
                    canvasTouch.Children.Remove(child);
                    hasCloseKeyBoard = true;
                }                
            }
            //把界面移位归位
            SetPageContentTransformY(0);
            return hasCloseKeyBoard;
        }

        private void SetPageContentTransformY(double y)
        {
            UIElement page = (UIElement)frameScreen.Content;
            if (page == null)
            {
                return;
            }
            TransformGroup tg = page.RenderTransform as TransformGroup;
            if (tg == null)
            {
                //如果不是TransformGroup，则将旧的RenderTransform放到TransformGroup中
                tg = new TransformGroup();
                if (page.RenderTransform != null)
                {
                    tg.Children.Add(page.RenderTransform);
                }

            }
            //将旧的TranslateTransform移除
            foreach (var t in tg.Children.ToArray())
            {
                if (t is TranslateTransform)
                {
                    tg.Children.Remove(t);
                }
            }
            //添加新的TranslateTransform
            tg.Children.Add(new TranslateTransform() { Y = y });
            page.RenderTransform = tg;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //回到首页
            NavigateNewApp(StartPageUri);
        }

        public FrameworkElement ScreenElement
        {
            get
            {
                return frameScreen;
            }
        }
    }

    public delegate void PlaneProjectionChangeDelegate(double x,double y,double z);
}
