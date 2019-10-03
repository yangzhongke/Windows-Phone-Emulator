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
using Microsoft.Phone.Controls.Primitives;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Collections.Specialized;
using Microsoft.Phone.Controls.Gestures;
using Microsoft.Phone.Internals.touch;
using Microsoft.Phone.Internals;

namespace Microsoft.Phone.Controls
{
    [TemplatePart(Name = "TitleLayer", Type = typeof(PanningLayer)), TemplatePart(Name = "BackgroundLayer", Type = typeof(PanningLayer)), TemplatePart(Name = "ItemsLayer", Type = typeof(PanningLayer)), StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PanoramaItem))]
    public class Panorama : TemplatedItemsControl<PanoramaItem>
    {
        // Fields
        private bool _adjustSelectedRequested;
        private int _cumulativeDragDelta;
        private bool _dragged;
        private int _flickDirection;
        private int _frameCount;
        private bool _loaded;
        private PanningLayer _panningBackground;
        private PanningLayer _panningItems;
        private PanningLayer _panningTitle;
        private bool _suppressSelectionChangedEvent;
        private int _targetOffset;
        private bool _updateBackgroundPending;
        private const string BackgroundLayerElement = "BackgroundLayer";
        private static readonly DependencyProperty BackgroundShadowProperty = DependencyProperty.Register("BackgroundShadow", typeof(Brush), typeof(Panorama), new PropertyMetadata(null, new PropertyChangedCallback(Panorama.OnBackgroundShadowChanged)));
        private static readonly Duration DefaultDuration = new Duration(TimeSpan.FromMilliseconds(800.0));
        public static readonly DependencyProperty DefaultItemProperty = DependencyProperty.Register("DefaultItem", typeof(object), typeof(Panorama), new PropertyMetadata(null, new PropertyChangedCallback(Panorama.OnDefaultItemChanged)));
        private static readonly Duration EntranceDuration = DefaultDuration;
        private static readonly Duration FlickDuration = DefaultDuration;
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Panorama), null);
        internal static readonly Duration Immediately = new Duration(TimeSpan.Zero);
        private const string ItemsLayerElement = "ItemsLayer";
        private static readonly Duration PanDuration = new Duration(TimeSpan.FromMilliseconds(150.0));
        internal const double PanningOpacity = 0.7;
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Panorama), new PropertyMetadata(-1));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Panorama), new PropertyMetadata(null, new PropertyChangedCallback(Panorama.OnSelectionChanged)));
        private static readonly Duration SnapDuration = DefaultDuration;
        internal const int Spacing = 0x30;
        private const string TitleLayerElement = "TitleLayer";
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(Panorama), null);
        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(Panorama), null);

        // Events
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        // Methods
        public Panorama()
        {
            this._updateBackgroundPending = true;
            base.DefaultStyleKey = typeof(Panorama);

            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }
            GestureHelper helper = GestureHelper.Create();
            helper.HorizontalDrag += delegate(object sender, TouchArgs args)
            {
                this.HorizontalDrag(args);
            };
            
            base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
            if (DesignerProperties.IsInDesignTool)
            {
                base.Loaded += new RoutedEventHandler(this.OnLoaded);
                base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
            }
            else
            {
                CompositionTarget.Rendering += new EventHandler(this.EntranceAnimationCallback);
            }
        }

        private void AdjustSelection()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                if (this._loaded)
                {
                    this._targetOffset = 0;
                    this.GoTo(this._targetOffset, Immediately);
                }
            }
            else
            {
                object selectedItem = this.SelectedItem;
                object item = null;
                bool flag = false;
                bool flag2 = false;
                if ((this.Panel != null) && (this.Panel.VisibleChildren.Count > 0))
                {
                    if (selectedItem == null)
                    {
                        item = base.GetItem(this.Panel.VisibleChildren[0]);
                    }
                    else
                    {
                        PanoramaItem container = base.GetContainer(selectedItem);
                        flag2 = true;
                        if ((container == null) || !this.Panel.VisibleChildren.Contains(container))
                        {
                            item = base.GetItem(this.Panel.VisibleChildren[0]);
                        }
                        else
                        {
                            item = selectedItem;
                        }
                    }
                }
                else
                {
                    this._targetOffset = 0;
                    this.GoTo(this._targetOffset, Immediately);
                }
                if (flag)
                {
                    this.SelectedItem = item;
                }
                else
                {
                    this.SetSelectionInternal(item);
                }
                this.UpdateItemPositions();
                if (flag2)
                {
                    PanoramaItem item2 = base.GetContainer(item);
                    if (item2 != null)
                    {
                        this._targetOffset = -item2.StartPosition;
                        this.GoTo(this._targetOffset, Immediately);
                    }
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size size = finalSize;
            size.Width = base.DesiredSize.Width;
            base.ArrangeOverride(finalSize);
            return finalSize;
        }

        private void AsyncUpdateBackground(BitmapImage img)
        {
            img.ImageOpened -= new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
            this.UpdateBackground();
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            ((PanoramaItem)element).Owner = null;
        }

        private void EntranceAnimationCallback(object sender, EventArgs e)
        {
            switch (this._frameCount++)
            {
                case 0:
                    this.GoTo(this.ViewportWidth, Immediately);
                    return;

                case 1:
                    this.GoTo(0, EntranceDuration);
                    CompositionTarget.Rendering -= new EventHandler(this.EntranceAnimationCallback);
                    return;
            }
        }

        private void Flick(TouchArgs e)
        {
            if (e.GestureType== Gesture.MOVE_WEST)
            {
                this._flickDirection = -1;
            }
            if (e.GestureType == Gesture.MOVE_EAST)
            {
                this._flickDirection = 1;
            }
        }

        private void GestureEnd()
        {
            if (this._flickDirection == 0)
            {
                if (this._dragged)
                {
                    int num;
                    int num2;
                    PanoramaItem item;
                    bool flag;
                    this.Panel.GetSnapOffset(this._targetOffset, this.ViewportWidth, Math.Sign(this._cumulativeDragDelta), out num, out num2, out item, out flag);
                    if (flag)
                    {
                        this.WrapAround(Math.Sign(this._cumulativeDragDelta));
                    }
                    object obj2 = base.GetItem(item);
                    if (obj2 != null)
                    {
                        this.SelectedItem = obj2;
                    }
                    this.UpdateItemPositions();
                    this.GoTo(num, SnapDuration);
                }
            }
            else
            {
                this.ProcessFlick();
            }
        }

        internal PanoramaItem GetDefaultItemContainer()
        {
            return base.GetContainer(this.DefaultItem);
        }

        private void GoTo(int offset)
        {
            this.GoTo(offset, (Action)null);
        }

        private void GoTo(int offset, Action completionAction)
        {
            int num = Math.Abs((int)(((int)this._panningItems.ActualOffset) - offset));
            this.GoTo(offset, TimeSpan.FromMilliseconds((double)(num * 2)), completionAction);
        }

        private void GoTo(int offset, Duration duration)
        {
            this.GoTo(offset, duration, null);
        }

        private void GoTo(int offset, Duration duration, Action completionAction)
        {
            if (this._panningBackground != null)
            {
                this._panningBackground.GoTo(offset, duration, null);
            }
            if (this._panningTitle != null)
            {
                this._panningTitle.GoTo(offset, duration, null);
            }
            if (this._panningItems != null)
            {
                this._panningItems.GoTo(offset, duration, completionAction);
            }
        }

        private void HorizontalDrag(TouchArgs args)
        {
            if (this._flickDirection == 0)
            {
                this._cumulativeDragDelta = (int)(args.Position.X-args.StartPosition.X);
                this._targetOffset += (int)(args.Position.X - args.StartPosition.X);
                if (Math.Abs(this._cumulativeDragDelta) <= this.ViewportWidth)
                {
                    this._dragged = true;
                    this.GoTo(this._targetOffset, PanDuration);
                }
            }
        }

        private void LayoutUpdatedAdjustSelection(object sender, EventArgs e)
        {
            this._adjustSelectedRequested = false;
            base.LayoutUpdated -= new EventHandler(this.LayoutUpdatedAdjustSelection);
            this.AdjustSelection();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (AppHelper.IsInDesignMode())
            {
                if (double.IsInfinity(availableSize.Width))
                {
                    availableSize.Width = this.ViewportWidth;
                }
                if (double.IsInfinity(availableSize.Height))
                {
                    availableSize.Height = this.ViewportHeight;
                }
                return availableSize;
            }
            var screen = WinPhoneCtrl.Instance.ScreenElement;
            if (screen.ActualWidth > 0.0)
            {
                this.ViewportWidth = !double.IsInfinity(availableSize.Width) ? ((int)availableSize.Width) : ((int)screen.ActualWidth);
                this.ViewportHeight = !double.IsInfinity(availableSize.Height) ? ((int)availableSize.Height) : ((int)screen.ActualHeight);
            }
            else
            {
                this.ViewportWidth = (int)Math.Min(availableSize.Width, 480.0);
                this.ViewportHeight = (int)Math.Min(availableSize.Height, 800.0);
            }
            base.MeasureOverride(new Size(double.PositiveInfinity, (double)this.ViewportHeight));
            if (double.IsInfinity(availableSize.Width))
            {
                availableSize.Width = this.ViewportWidth;
            }
            if (double.IsInfinity(availableSize.Height))
            {
                availableSize.Height = this.ViewportHeight;
            }
            return availableSize;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._panningBackground = base.GetTemplateChild("BackgroundLayer") as PanningLayer;
            this._panningTitle = base.GetTemplateChild("TitleLayer") as PanningLayer;
            this._panningItems = base.GetTemplateChild("ItemsLayer") as PanningLayer;
            if (this._panningBackground != null)
            {
                this._panningBackground.Owner = this;
            }
            if (this._panningTitle != null)
            {
                this._panningTitle.Owner = this;
            }
            if (this._panningItems != null)
            {
                this._panningItems.Owner = this;
            }
            Binding binding = new Binding("Background")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.Self)
            };
            base.SetBinding(BackgroundShadowProperty, binding);
        }

        private void OnBackgroundImageOpened(object sender, RoutedEventArgs e)
        {
            this.AsyncUpdateBackground((BitmapImage)sender);
        }

        private static void OnBackgroundShadowChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Panorama panorama = (Panorama)obj;
            if (!panorama._updateBackgroundPending)
            {
                panorama.UpdateBackground();
            }
        }

        private static void OnDefaultItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((Panorama)obj).OnDefaultItemSet();
        }

        private void OnDefaultItemSet()
        {
            if (this.Panel != null)
            {
                this.Panel.NotifyDefaultItemChanged();
                if (this.Panel.VisibleChildren.Count > 0)
                {
                    this.SelectedItem = this.DefaultItem;
                }
                if (this.Panel != null)
                {
                    this.Panel.ResetItemPositions();
                }
                this._panningItems.Refresh();
                this.UpdateItemPositions();
                this.GoTo(0, Immediately);
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (this.Panel != null)
            {
                this.Panel.ResetItemPositions();
            }
            this.RequestAdjustSelection();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this._loaded = true;
        }

        private static void OnSelectionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SafeRaise.GetEventArgs<SelectionChangedEventArgs> getEventArgs = null;
            Panorama sender = (Panorama)obj;
            sender.SelectedIndex = sender.Items.IndexOf(args.NewValue);
            if (!sender._suppressSelectionChangedEvent && sender.Items.Contains(args.NewValue))
            {
                if (getEventArgs == null)
                {
                    getEventArgs = () => new SelectionChangedEventArgs((args.OldValue == null) ? ((IList)new object[0]) : ((IList)new object[] { args.OldValue }), (args.NewValue == null) ? ((IList)new object[0]) : ((IList)new object[] { args.NewValue }));
                }
                SafeRaise.Raise<SelectionChangedEventArgs>(sender.SelectionChanged, sender, getEventArgs);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ViewportWidth = (int)e.NewSize.Width;
            this.ViewportHeight = (int)e.NewSize.Height;
            this.ItemsWidth = (int)this.Panel.ActualWidth;
            this.UpdateBackground();
            base.Clip = new RectangleGeometry { Rect = new Rect(0.0, 0.0, e.NewSize.Width, e.NewSize.Height) };
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this._loaded = false;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            PanoramaItem item2 = element as PanoramaItem;
            if (item2 != null)
            {
                item2.Owner = this;
                if ((item2.Content == null) && (item2 != item))
                {
                    item2.Content = item;
                }
                if ((item2.HeaderTemplate == null) && (element.ReadLocalValue(PanoramaItem.HeaderTemplateProperty) == DependencyProperty.UnsetValue))
                {
                    item2.HeaderTemplate = this.HeaderTemplate;
                }
                if (((item2.Header == null) && !(item is UIElement)) && (item2.ReadLocalValue(PanoramaItem.HeaderProperty) == DependencyProperty.UnsetValue))
                {
                    item2.Header = item;
                }
            }
        }

        private void ProcessFlick()
        {
            if (this._flickDirection != 0)
            {
                PanoramaPanel.ItemStop stop;
                PanoramaPanel.ItemStop stop2;
                PanoramaPanel.ItemStop stop3;
                int actualOffset = (int)this._panningItems.ActualOffset;
                this.Panel.GetStops(actualOffset, this.ItemsWidth, out stop, out stop2, out stop3);
                if (((stop != stop2) || (stop2 != stop3)) || (stop3 != null))
                {
                    this._targetOffset = (this._flickDirection < 0) ? -stop3.Position : -stop.Position;
                    if (Math.Sign((double)(this._targetOffset - this._panningItems.ActualOffset)) != Math.Sign(this._flickDirection))
                    {
                        this.WrapAround(Math.Sign(this._flickDirection));
                    }
                    this.SelectedItem = base.GetItem((this._flickDirection < 0) ? stop3.Item : stop.Item);
                    this.UpdateItemPositions();
                    this.GoTo(this._targetOffset, FlickDuration);
                }
            }
        }

        internal void RequestAdjustSelection()
        {
            if (!this._adjustSelectedRequested)
            {
                base.LayoutUpdated += new EventHandler(this.LayoutUpdatedAdjustSelection);
                this._adjustSelectedRequested = true;
            }
        }

        private void SetSelectionInternal(object selectedItem)
        {
            this._suppressSelectionChangedEvent = true;
            this.SelectedItem = selectedItem;
            this._suppressSelectionChangedEvent = false;
        }

        private void UpdateBackground()
        {
            this._updateBackgroundPending = false;
            this._panningBackground.ContentPresenter.Height = this.ViewportHeight;
            if (base.Background is SolidColorBrush)
            {
                this._panningBackground.ContentPresenter.Width = this.ViewportWidth;
                this._panningBackground.IsStatic = true;
            }
            else if (base.Background is GradientBrush)
            {
                this._panningBackground.ContentPresenter.Width = Math.Max(this.ItemsWidth, this.ViewportWidth);
                this._panningBackground.IsStatic = this._panningBackground.ContentPresenter.Width == this.ViewportWidth;
            }
            else if (base.Background is ImageBrush)
            {
                Action action = null;
                ImageBrush background = (ImageBrush)base.Background;
                BitmapImage bmp = background.ImageSource as BitmapImage;
                if ((this._panningBackground.ContentPresenter != null) && (bmp != null))
                {
                    if (bmp.PixelWidth == 0)
                    {
                        bmp.ImageOpened -= new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
                        bmp.ImageOpened += new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
                        if (action == null)
                        {
                            action = delegate
                            {
                                this.AsyncUpdateBackground(bmp);
                            };
                        }
                        base.Dispatcher.BeginInvoke(action);
                    }
                    this._panningBackground.ContentPresenter.Width = bmp.PixelWidth;
                }
                this._panningBackground.IsStatic = false;
            }
        }

        private void UpdateItemPositions()
        {
            bool flag = true;
            if (this.Panel != null)
            {
                if ((this.Panel.VisibleChildren.Count > 2) && (this.SelectedItem != null))
                {
                    PanoramaItem container = base.GetContainer(this.SelectedItem);
                    if (container != null)
                    {
                        int index = this.Panel.VisibleChildren.IndexOf(container);
                        if (index == (this.Panel.VisibleChildren.Count - 1))
                        {
                            this.Panel.ShowFirstItemOnRight();
                            flag = false;
                        }
                        else if (index == 0)
                        {
                            this.Panel.ShowLastItemOnLeft();
                            flag = false;
                        }
                    }
                }
                if (flag)
                {
                    this.Panel.ResetItemPositions();
                }
            }
        }

        private void WrapAround(int direction)
        {
            this._panningBackground.Wraparound(direction);
            this._panningTitle.Wraparound(direction);
            this._panningItems.Wraparound(direction);
        }

        // Properties
        internal int AdjustedViewportWidth
        {
            get
            {
                return Math.Max(0, this.ViewportWidth - 0x30);
            }
        }

        public object DefaultItem
        {
            get
            {
                return base.GetValue(DefaultItemProperty);
            }
            set
            {
                base.SetValue(DefaultItemProperty, value);
                this.OnDefaultItemSet();
            }
        }

        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(HeaderTemplateProperty);
            }
            set
            {
                base.SetValue(HeaderTemplateProperty, value);
            }
        }

        internal int ItemsWidth { get; set; }

        internal PanoramaPanel Panel { get; set; }

        public int SelectedIndex
        {
            get
            {
                return (int)base.GetValue(SelectedIndexProperty);
            }
            private set
            {
                base.SetValue(SelectedIndexProperty, value);
            }
        }

        public object SelectedItem
        {
            get
            {
                return base.GetValue(SelectedItemProperty);
            }
            private set
            {
                base.SetValue(SelectedItemProperty, value);
            }
        }

        public object Title
        {
            get
            {
                return base.GetValue(TitleProperty);
            }
            set
            {
                base.SetValue(TitleProperty, value);
            }
        }

        public DataTemplate TitleTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(TitleTemplateProperty);
            }
            set
            {
                base.SetValue(TitleTemplateProperty, value);
            }
        }

        internal int ViewportHeight { get; private set; }

        internal int ViewportWidth { get; private set; }
    }
}
