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

namespace Microsoft.Phone.Controls
{
    [TemplatePart(Name = "ClientArea", Type = typeof(FrameworkElement))]
    public class PhoneApplicationFrame : Frame
    {
        // Fields
        private FrameworkElement _clientArea;
        private PageOrientation _layoutOrientation;
        private TranslateTransform _offsetTranslateTransform;
        private RotateTransform _orientationRotateTransform;
        private TransformGroup _orientationTransforms;
        private TranslateTransform _orientationTranslateTransform;
        //private RECT _visibleRegion;
        private const PageOrientation DefaultFrameOrientation = PageOrientation.PortraitUp;
        public static readonly DependencyProperty OrientationProperty = 
            DependencyProperty.Register("Orientation", typeof(PageOrientation), typeof(PhoneApplicationFrame), new PropertyMetadata(DefaultFrameOrientation,new PropertyChangedCallback(PhoneApplicationFrame.OrientationPropertyChanged)));
        private const string PART_ClientArea = "ClientArea";
        private const int PortraitUpHeight = 800;
        private const int PortraitUpWidth = 480;
        internal static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(PhoneApplicationFrame), new PropertyMetadata(new PropertyChangedCallback(PhoneApplicationFrame.VerticalOffsetPropertyChanged)));

        // Events
        //public event EventHandler<ObscuredEventArgs> Obscured;

        public event EventHandler<OrientationChangedEventArgs> OrientationChanged;

        public event EventHandler Unobscured;

        //// Methods
        //public PhoneApplicationFrame()
        //{
        //    Action action = null;
        //    PerfUtil.BeginLogMarker(MarkerEvents.TH_INIT_FRAME, "Frame initialized");
        //    ShellFrame.Initialize();
        //    base.DefaultStyleKey = typeof(PhoneApplicationFrame);
        //    this._orientationTransforms = new TransformGroup();
        //    this._offsetTranslateTransform = new TranslateTransform();
        //    this._orientationTranslateTransform = new TranslateTransform();
        //    this._orientationRotateTransform = new RotateTransform();
        //    this._orientationTransforms.Children.Add(this._offsetTranslateTransform);
        //    this._orientationTransforms.Children.Add(this._orientationRotateTransform);
        //    this._orientationTransforms.Children.Add(this._orientationTranslateTransform);
        //    base.RenderTransform = this._orientationTransforms;
        //    this.Orientation = PageOrientation.PortraitUp;
        //    RECT rect = new RECT
        //    {
        //        left = 0,
        //        top = 0,
        //        right = 0,
        //        bottom = 0
        //    };
        //    this._visibleRegion = rect;
        //    base._navigationService = new NavigationService(this);
        //    if (!Frame.IsInDesignMode() && !base._hostInfo.Rehydrated)
        //    {
        //        if (action == null)
        //        {
        //            action = delegate
        //            {
        //                base.Load();
        //            };
        //        }
        //        Deployment.Current.Dispatcher.BeginInvoke(action);
        //    }
        //}

        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    if (this.RotateOnClientArea)
        //    {
        //        Rect rect2 = new Rect
        //        {
        //            Height = finalSize.Width,
        //            Width = finalSize.Height
        //        };
        //        Rect finalRect = rect2;
        //        this._clientArea.Arrange(finalRect);
        //        return finalSize;
        //    }
        //    return base.ArrangeOverride(finalSize);
        //}

        //~PhoneApplicationFrame()
        //{
        //    ShellFrame.Uninitialize();
        //    PerfUtil.EndLogMarker(MarkerEvents.TH_INIT_FRAME, "Frame uninitialized");
        //}

        internal void FireOrientationChanged(PageOrientation value)
        {
            WinPhoneCtrl.Instance.frameScreen.Orientation = value;
            OrientationChangedEventArgs e = new OrientationChangedEventArgs(value);
            EventHandler<OrientationChangedEventArgs> orientationChanged = this.OrientationChanged;
            if (orientationChanged != null)
            {
                orientationChanged(this, e);
            }
        }

        //private Size GetMaxFiniteRotatedSize(Size desired, Size actual)
        //{
        //    if (double.IsInfinity(actual.Width))
        //    {
        //        actual.Width = 0.0;
        //    }
        //    if (double.IsInfinity(actual.Height))
        //    {
        //        actual.Height = 0.0;
        //    }
        //    return new Size { Width = Math.Max(actual.Width, desired.Height), Height = Math.Max(actual.Height, desired.Width) };
        //}

        //internal override void InternalUpdateOrientationAndMarginForPage(PhoneApplicationPage visiblePage)
        //{
        //    if (visiblePage != null)
        //    {
        //        visiblePage.UpdateCurrentVisualState();
        //        this.UpdateMargin(visiblePage.VisibleRegion);
        //        this.Orientation = visiblePage.Orientation;
        //        this.LayoutOrientation = visiblePage.LayoutOrientation;
        //    }
        //}

        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    if (this.RotateOnClientArea)
        //    {
        //        Size size3 = new Size
        //        {
        //            Height = availableSize.Width,
        //            Width = availableSize.Height
        //        };
        //        Size size = size3;
        //        this._clientArea.Measure(size);
        //        return this.GetMaxFiniteRotatedSize(this._clientArea.DesiredSize, availableSize);
        //    }
        //    return base.MeasureOverride(availableSize);
        //}

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    this._clientArea = base.GetTemplateChild("ClientArea") as FrameworkElement;
        //    this.UpdateMargin(this._visibleRegion);
        //}

        //internal override void OnBeginLayoutChanged(object sender, OrientationChangedEventArgs args)
        //{
        //    this.LayoutOrientation = args.Orientation;
        //}

        //internal override void OnBeginOrientationChanged(object sender, OrientationChangedEventArgs args)
        //{
        //    this.Orientation = args.Orientation;
        //}

        //internal override void OnVisibleRegionChanged(object sender, VisibleRegionChangeEventArgs args)
        //{
        //    if (args != null)
        //    {
        //        this.UpdateMargin(args.rc);
        //    }
        //}

        private static void OrientationPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            PhoneApplicationFrame frame = obj as PhoneApplicationFrame;
            frame.FireOrientationChanged(frame.Orientation);
        }

        //internal void ShellPageManager_OnLockStateChange(object sender, LockStateChangeEventArgs args)
        //{
        //    if (!base.Dispatcher.CheckAccess())
        //    {
        //        this.ShellPageManager_OnLockStateChange(sender, args);
        //    }
        //    else if (args.IsLocked)
        //    {
        //        PerfUtil.BeginLogMarker(MarkerEvents.TH_LOCK_SCREEN, "Phone locked");
        //        base.IsObscured = true;
        //        ObscuredEventArgs args2 = new ObscuredEventArgs(true);
        //        Frame.FireEventHandler<ObscuredEventArgs>(this.Obscured, this, args2);
        //    }
        //    else
        //    {
        //        PerfUtil.EndLogMarker(MarkerEvents.TH_LOCK_SCREEN, "Phone unlocked");
        //    }
        //}

        //internal void ShellPageManager_OnObscurityChange(object sender, ObscurityChangeEventArgs args)
        //{
        //    if (!base.Dispatcher.CheckAccess())
        //    {
        //        this.ShellPageManager_OnObscurityChange(sender, args);
        //    }
        //    else if (args.Obscured != base.IsObscured)
        //    {
        //        base.IsObscured = args.Obscured;
        //        if (args.Obscured)
        //        {
        //            ObscuredEventArgs args2 = new ObscuredEventArgs(false);
        //            Frame.FireEventHandler<ObscuredEventArgs>(this.Obscured, this, args2);
        //            PerfUtil.BeginLogMarker(MarkerEvents.TH_OBSCURE_SCREEN, "App obscured");
        //        }
        //        else
        //        {
        //            PerfUtil.EndLogMarker(MarkerEvents.TH_OBSCURE_SCREEN, "App unobscured");
        //            Frame.FireEventHandler(this.Unobscured, this);
        //        }
        //    }
        //}

        //internal void UpdateMargin(RECT visibleRegion)
        //{
        //    if (this._clientArea != null)
        //    {
        //        this._clientArea.Margin = new Thickness((double)visibleRegion.left, (double)visibleRegion.top, (double)visibleRegion.right, (double)visibleRegion.bottom);
        //    }
        //    this._visibleRegion = visibleRegion;
        //}

        //internal void UpdateOrientationTransform()
        //{
        //    Action action = null;
        //    if (!base.Dispatcher.CheckAccess())
        //    {
        //        if (action == null)
        //        {
        //            action = delegate
        //            {
        //                this.UpdateOrientationTransform();
        //            };
        //        }
        //        base.Dispatcher.BeginInvoke(action);
        //    }
        //    else
        //    {
        //        switch (this.Orientation)
        //        {
        //            case PageOrientation.PortraitUp:
        //                this._orientationRotateTransform.Angle = 0.0;
        //                this._orientationTranslateTransform.X = 0.0;
        //                this._orientationTranslateTransform.Y = 0.0;
        //                break;

        //            case PageOrientation.LandscapeLeft:
        //                this._orientationRotateTransform.Angle = 90.0;
        //                this._orientationTranslateTransform.X = 480.0;
        //                this._orientationTranslateTransform.Y = 0.0;
        //                break;

        //            case PageOrientation.LandscapeRight:
        //                this._orientationRotateTransform.Angle = 270.0;
        //                this._orientationTranslateTransform.X = 0.0;
        //                this._orientationTranslateTransform.Y = 800.0;
        //                break;
        //        }
        //        base.InvalidateMeasure();
        //        base.UpdateLayout();
        //    }
        //}

        private static void VerticalOffsetPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            PhoneApplicationFrame frame = obj as PhoneApplicationFrame;
            frame.OffsetTranslateTransform.Y = (double)e.NewValue;
        }

        // Properties
        internal PageOrientation LayoutOrientation
        {
            get
            {
                return this._layoutOrientation;
            }
            set
            {
                if (this._layoutOrientation != value)
                {
                    this._layoutOrientation = value;
                    //this.UpdateOrientationTransform();
                }
            }
        }

        private TranslateTransform OffsetTranslateTransform
        {
            get
            {
                return this._offsetTranslateTransform;
            }
        }

        public PageOrientation Orientation
        {
            get
            {
                return (PageOrientation)base.GetValue(OrientationProperty);
            }
            internal set
            {
                if (this.Orientation != value)
                {
                    base.SetValue(OrientationProperty, value);
                }
            }
        }

        internal bool RotateOnClientArea
        {
            get
            {
                if (this._orientationRotateTransform.Angle != 90.0)
                {
                    return (this._orientationRotateTransform.Angle == 270.0);
                }
                return true;
            }
        }

        internal double VerticalOffset
        {
            get
            {
                return (double)base.GetValue(VerticalOffsetProperty);
            }
            set
            {
                if (this.VerticalOffset != value)
                {
                    base.SetValue(VerticalOffsetProperty, value);
                }
            }
        }
    }
}
