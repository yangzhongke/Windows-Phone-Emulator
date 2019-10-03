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
using System.Collections.Generic;
using System.Threading;
using System.Windows.Data;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections;
using Microsoft.Phone.Internals.touch;
using Microsoft.Phone.Controls.Gestures;

namespace Microsoft.Phone.Controls
{
    [TemplatePart(Name = "PivotItemPresenter", Type = typeof(ItemsPresenter)), StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PivotItem)), TemplatePart(Name = "HeadersListElement", Type = typeof(PivotHeadersControl))]
    public class Pivot : TemplatedItemsControl<PivotItem>
    {
        // Fields
        private double _actualWidth;
        private bool _animating;
        private AnimationDirection? _animationHint = null;
        internal PivotHeadersControl _clickedHeadersControl;
        private PivotHeadersControl _headers;
        private bool _ignorePropertyChange;
        private bool _isDesignTime;
        private Panel _itemsPanel;
        private ItemsPresenter _itemsPresenter;
        private TransformAnimator _panAnimator;
        private Queue<int> _queuedIndexChanges;
        private bool _skippedLoadingPivotItem;
        private bool _skippedSwapVisibleContent;
        private bool _updatingHeaderItems;
        private const string ElementHeadersRowDefinitionName = "HeadersRowDefinition";
        private const string HeadersListElement = "HeadersListElement";
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Pivot), new PropertyMetadata(null));
        internal const string ItemContainerStyleName = "ItemContainerStyle";
        internal static readonly Duration PivotAnimationDuration = new Duration(PivotAnimationTimeSpan);
        internal const double PivotAnimationSeconds = 0.25;
        private static readonly TimeSpan PivotAnimationTimeSpan = TimeSpan.FromSeconds(0.25);
        private const string PivotItemPresenterElement = "PivotItemPresenter";
        private const double pixelsPerSecondTemporary = 600.0;
        internal readonly IEasingFunction QuarticEase = new QuarticEase();
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Pivot), new PropertyMetadata(new PropertyChangedCallback(Pivot.OnSelectedIndexPropertyChanged)));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Pivot), new PropertyMetadata(null, new PropertyChangedCallback(Pivot.OnSelectedItemPropertyChanged)));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(Pivot), new PropertyMetadata(null));
        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(Pivot), new PropertyMetadata(null));
        internal static readonly Duration ZeroDuration = new Duration(TimeSpan.Zero);

        //// Events
        public event EventHandler<PivotItemEventArgs> LoadedPivotItem;
        public event EventHandler<PivotItemEventArgs> LoadingPivotItem;
        public event SelectionChangedEventHandler SelectionChanged;
        public event EventHandler<PivotItemEventArgs> UnloadedPivotItem;
        public event EventHandler<PivotItemEventArgs> UnloadingPivotItem;        

        public Pivot()
        {
            base.DefaultStyleKey = typeof(Pivot);
            base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
            this._isDesignTime = DesignerProperties.IsInDesignTool;
            if (this._isDesignTime)
            {
                return;
            }

            GestureHelper helper = GestureHelper.Create();
            helper.HorizontalDrag += new EventHandler<TouchArgs>(this.OnHorizontalDrag);
            helper.Flick += new EventHandler<TouchArgs>(this.OnFlick);
            
            this._queuedIndexChanges = new Queue<int>(5);
        }

        private void BeginAnimateContent(int newIndex, PivotItem oldItem, AnimationDirection animationDirection)
        {
            Action completionAction = null;
            if (this._isDesignTime)
            {
                this.SwapVisibleContent(oldItem, newIndex);
            }
            else if (this._itemsPresenter != null)
            {
                this._animating = true;
                TransformAnimator.EnsureAnimator(this._itemsPresenter, ref this._panAnimator);
                PivotItem item = base.GetContainer(this.SelectedItem);
                if (item != null)
                {
                    item.MoveTo(AnimationDirection.Center);
                }
                if ((this._headers != null) && (animationDirection != AnimationDirection.Center))
                {
                    this._headers.AnimationDirection = animationDirection;
                }
                if (completionAction == null)
                {
                    completionAction = delegate
                    {
                        this._panAnimator.GoTo(this.CalculateContentDestination(InvertAnimationDirection(animationDirection)), ZeroDuration);
                        this.SwapVisibleContent(oldItem, newIndex);
                        PivotItem container = this.GetContainer(this.SelectedItem);
                        if (container != null)
                        {
                            container.MoveTo(animationDirection);
                        }
                        this._panAnimator.GoTo(0.0, PivotAnimationDuration, this.QuarticEase, delegate
                        {
                            this._animationHint = null;
                            this._animating = false;
                            this.ProcessQueuedChanges();
                        });
                    };
                }
                this._panAnimator.GoTo(this.CalculateContentDestination(animationDirection), PivotAnimationDuration, completionAction);
            }
            else
            {
                this._skippedSwapVisibleContent = true;
            }
        }

        private double CalculateContentDestination(AnimationDirection direction)
        {
            double actualWidth = base.ActualWidth;
            switch (direction)
            {
                case AnimationDirection.Left:
                    return -actualWidth;

                case AnimationDirection.Right:
                    return actualWidth;
            }
            return 0.0;
        }

        private PivotHeaderItem CreateHeaderBindingControl(object item)
        {
            PivotHeaderItem item3 = new PivotHeaderItem();
            item3.ContentTemplate = this.HeaderTemplate;
            PivotHeaderItem item2 = item3;
            Binding binding2 = new Binding();
            binding2.Source = item;
            Binding binding = binding2;
            if (item is PivotItem)
            {
                binding.Path = new PropertyPath("Header", new object[0]);
            }
            try
            {
                binding.Mode = BindingMode.OneWay;
                item2.SetBinding(ContentControl.ContentProperty, binding);
                return item2;
            }
            catch
            {
                if (!this._isDesignTime)
                {
                    throw;
                }
                return null;
            }
        }

        private int GetNextIndex()
        {
            int count = base.Items.Count;
            if (count <= 0)
            {
                return 0;
            }
            int num2 = this.SelectedIndex + 1;
            if (num2 > count)
            {
                num2 = 0;
            }
            return num2;
        }

        private int GetPreviousIndex()
        {
            int count = base.Items.Count;
            if (count <= 0)
            {
                return 0;
            }
            int num2 = this.SelectedIndex - 1;
            if (num2 < 0)
            {
                num2 = count - 1;
            }
            return num2;
        }

        private static AnimationDirection InvertAnimationDirection(AnimationDirection direction)
        {
            switch (direction)
            {
                case AnimationDirection.Left:
                    return AnimationDirection.Right;

                case AnimationDirection.Right:
                    return AnimationDirection.Left;
            }
            return direction;
        }

        private void NavigateByIndexChange(int indexDelta)
        {
            if (this._animating && (this._queuedIndexChanges != null))
            {
                this._queuedIndexChanges.Enqueue(indexDelta);
            }
            else
            {
                int selectedIndex = this.SelectedIndex;
                if (selectedIndex != -1)
                {
                    this._animationHint = new AnimationDirection?((indexDelta > 0) ? AnimationDirection.Left : AnimationDirection.Right);
                    selectedIndex += indexDelta;
                    if (selectedIndex >= base.Items.Count)
                    {
                        selectedIndex = 0;
                    }
                    else if (selectedIndex < 0)
                    {
                        selectedIndex = base.Items.Count - 1;
                    }
                    if (this._clickedHeadersControl != null)
                    {
                        this._clickedHeadersControl._wasClicked = false;
                        this._clickedHeadersControl._cancelClick = true;
                    }
                    this.SelectedIndex = selectedIndex;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            if (this._headers != null)
            {
                this._headers.SelectedIndexChanged -= new EventHandler<SelectedIndexChangedEventArgs>(this.OnHeaderSelectionChanged);
            }
            base.OnApplyTemplate();
            this._itemsPresenter = base.GetTemplateChild("PivotItemPresenter") as ItemsPresenter;
            this._headers = base.GetTemplateChild("HeadersListElement") as PivotHeadersControl;
            if (this._headers != null)
            {
                this._headers.SelectedIndexChanged += new EventHandler<SelectedIndexChangedEventArgs>(this.OnHeaderSelectionChanged);
                this.UpdateHeaders();
            }
            if (base.Items.Count > 0)
            {
                if (this.SelectedIndex < 0)
                {
                    this.SelectedIndex = 0;
                }
                else
                {
                    this.UpdateSelectedIndex(-1, this.SelectedIndex);
                }
            }
            this.UpdateVisibleContent(this.SelectedIndex);
            this.SetSelectedHeaderIndex(this.SelectedIndex);
        }

        private void OnFlick(object sender, TouchArgs e)
        {
            if (this._clickedHeadersControl != null)
            {
                this._clickedHeadersControl._wasClicked = false;
                this._clickedHeadersControl._cancelClick = true;
            }
            if (this.EnoughItemsForManipulation)
            {
                if (e.GestureType == Gesture.MOVE_EAST)
                {
                    this.NavigateByIndexChange(1);
                }
                else if (e.GestureType == Gesture.MOVE_WEST)
                {
                    this.NavigateByIndexChange(-1);
                }
            }
        }

        private void OnHeaderSelectionChanged(object s, SelectedIndexChangedEventArgs e)
        {
            if (!this._updatingHeaderItems)
            {
                this._animationHint = AnimationDirection.Center;
                this.SelectedIndex = e.SelectedIndex;
            }
        }

        private void OnHorizontalDrag(object sender, TouchArgs e)
        {
            if (this._clickedHeadersControl != null)
            {
                this._clickedHeadersControl._cancelClick = true;
            }
            if ((!this._animating && this.EnoughItemsForManipulation) && (this._itemsPresenter != null))
            {
                //TransformAnimator.EnsureAnimator(this._itemsPresenter, ref this._panAnimator);
                //double num = Math.Abs(e.DeltaDistance.X);
                //if ((!e.IsTouchComplete && !this._animating) && ((this._panAnimator != null) && (this._headers != null)))
                //{
                //    TimeSpan timeSpan = TimeSpan.FromSeconds(num / 600.0);
                //    this._panAnimator.GoTo(e.CumulativeDistance.X, new Duration(timeSpan));
                //    this._headers.PanHeader(e.CumulativeDistance.X, this._actualWidth, new Duration(timeSpan));
                //}
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            int? nullable4;
            int selectedIndex = this.SelectedIndex;
            int? nullable = null;
            int count = base.Items.Count;
            if (((e == null) || (selectedIndex != e.OldStartingIndex)) && (selectedIndex < count))
            {
                goto Label_009D;
            }
            nullable = new int?(selectedIndex);
        Label_005B:
            nullable4 = nullable;
            int num3 = count;
            if ((nullable4.GetValueOrDefault() >= num3) && nullable4.HasValue)
            {
                nullable -= 1;
                goto Label_005B;
            }
            if ((nullable < 0) && (0 < count))
            {
                nullable = 0;
            }
        Label_009D:
            if (nullable.HasValue)
            {
                int? nullable6 = nullable;
                int num4 = selectedIndex;
                this._animationHint = new AnimationDirection?(((nullable6.GetValueOrDefault() < num4) && nullable6.HasValue) ? AnimationDirection.Right : AnimationDirection.Left);
                this.SetSelectedIndexInternal(nullable.Value);
            }
            if (this._headers != null)
            {
                this.UpdateHeaders();
            }
            this.OptimizeVisuals();
            base.OnItemsChanged(e);
        }

        protected virtual void OnLoadedPivotItem(PivotItem item)
        {
            SafeRaise.Raise<PivotItemEventArgs>(this.LoadedPivotItem, this, delegate
            {
                return new PivotItemEventArgs(item);
            });
            this.OptimizeVisuals();
        }

        protected virtual void OnLoadingPivotItem(PivotItem item)
        {
            if ((item != null) && (item.Visibility == Visibility.Collapsed))
            {
                item.Visibility = Visibility.Visible;
            }
            SafeRaise.Raise<PivotItemEventArgs>(this.LoadingPivotItem, this, delegate
            {
                return new PivotItemEventArgs(item);
            });
        }

        private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pivot pivot = d as Pivot;
            if (pivot._ignorePropertyChange)
            {
                pivot._ignorePropertyChange = false;
            }
            else
            {
                pivot.UpdateSelectedIndex((int)e.OldValue, (int)e.NewValue);
            }
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pivot pivot = d as Pivot;
            if (pivot._ignorePropertyChange)
            {
                pivot._ignorePropertyChange = false;
            }
            else
            {
                pivot.UpdateSelectedItem(e.OldValue, e.NewValue);
            }
        }

        protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
            if (selectionChanged != null)
            {
                selectionChanged(this, e);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this._actualWidth = base.ActualWidth;
            RectangleGeometry geometry = new RectangleGeometry();
            geometry.Rect = new Rect(0.0, 0.0, this._actualWidth, base.ActualHeight);
            base.Clip = geometry;
            if (this._isDesignTime)
            {
                this.UpdateVisibleContent(this.SelectedIndex);
            }
        }

        protected virtual void OnUnloadedPivotItem(PivotItemEventArgs e)
        {
            EventHandler<PivotItemEventArgs> unloadedPivotItem = this.UnloadedPivotItem;
            if (unloadedPivotItem != null)
            {
                unloadedPivotItem(this, e);
            }
        }

        protected virtual void OnUnloadingPivotItem(PivotItemEventArgs e)
        {
            EventHandler<PivotItemEventArgs> unloadingPivotItem = this.UnloadingPivotItem;
            if (unloadingPivotItem != null)
            {
                unloadingPivotItem(this, e);
            }
        }

        private void OptimizeVisuals()
        {
            int selectedIndex = this.SelectedIndex;
            if ((selectedIndex >= 0) && (base.Items.Count > 1))
            {
                Action action = null;
                PivotItem next = base.GetContainer(base.Items[this.RollingIncrement(selectedIndex)]);
                PivotItem previous = base.GetContainer(base.Items[this.RollingDecrement(selectedIndex)]);
                bool flag = true;
                if (((next != null) && (previous != null)) && ((next.Visibility == previous.Visibility) && (previous.Visibility == Visibility.Visible)))
                {
                    flag = false;
                }
                if (flag)
                {
                    if (action == null)
                    {
                        action = delegate
                        {
                            Action inneraction = null;
                            if ((next != null) && (next.Visibility == Visibility.Collapsed))
                            {
                                next.Visibility = Visibility.Visible;
                            }
                            if (previous != next)
                            {
                                if (inneraction == null)
                                {
                                    inneraction = delegate
                                    {
                                        if ((previous != null) && (previous.Visibility == Visibility.Collapsed))
                                        {
                                            previous.Visibility = Visibility.Visible;
                                        }
                                    };
                                }
                                this.Dispatcher.BeginInvoke(inneraction);
                            }
                        };
                    }
                    base.Dispatcher.BeginInvoke(action);
                }
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            PivotItem item2 = element as PivotItem;
            int selectedIndex = this.SelectedIndex;
            if ((selectedIndex >= 0) && (base.Items.Count > selectedIndex))
            {
                object obj2 = base.Items[selectedIndex];
                if (item == obj2)
                {
                    if ((item2 != null) && this._skippedLoadingPivotItem)
                    {
                        this.OnLoadingPivotItem(item2);
                        if (this._skippedSwapVisibleContent)
                        {
                            this.OnLoadedPivotItem(item2);
                        }
                    }
                    return;
                }
            }
            if (item2 != null)
            {
                this.UpdateItemVisibility(item2, false);
                if (item2.Visibility == Visibility.Visible)
                {
                    item2.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ProcessQueuedChanges()
        {
            if (((this._queuedIndexChanges != null) && (this._queuedIndexChanges.Count > 0)) && !this._animating)
            {
                int indexDelta = this._queuedIndexChanges.Dequeue();
                this.NavigateByIndexChange(indexDelta);
            }
        }

        private int RollingDecrement(int index)
        {
            index--;
            if (index >= 0)
            {
                return index;
            }
            return (base.Items.Count - 1);
        }

        private int RollingIncrement(int index)
        {
            index++;
            if (index >= base.Items.Count)
            {
                return 0;
            }
            return index;
        }

        private void SetSelectedHeaderIndex(int selectedIndex)
        {
            try
            {
                this._updatingHeaderItems = true;
                if ((this._headers != null) && (base.Items.Count > 0))
                {
                    this._headers.SelectedIndex = selectedIndex;
                }
            }
            finally
            {
                this._updatingHeaderItems = false;
            }
        }

        private void SetSelectedIndexInternal(int newIndex)
        {
            this._ignorePropertyChange = true;
            this.SelectedIndex = newIndex - 1;
            this.SelectedIndex = newIndex;
        }

        private void SwapVisibleContent(PivotItem oldItem, int newIndex)
        {
            if (oldItem != null)
            {
                this.OnUnloadedPivotItem(new PivotItemEventArgs(oldItem));
            }
            this.UpdateVisibleContent(newIndex);
            this.OnLoadedPivotItem(base.GetContainer(this.SelectedItem));
        }

        private bool TryHasItemsHost()
        {
            if (this._itemsPanel != null)
            {
                return true;
            }
            if (base.ItemContainerGenerator != null)
            {
                DependencyObject reference = base.ItemContainerGenerator.ContainerFromIndex(0);
                if (reference != null)
                {
                    this._itemsPanel = VisualTreeHelper.GetParent(reference) as Panel;
                    return (this._itemsPanel != null);
                }
            }
            return false;
        }

        private void UpdateHeaders()
        {
            if (this._headers != null)
            {
                List<PivotHeaderItem> list = new List<PivotHeaderItem>();
                int count = base.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    object item = base.Items[i];
                    list.Add(this.CreateHeaderBindingControl(item));
                }
                try
                {
                    this._updatingHeaderItems = true;
                    this._headers.ItemsSource = (count == 0) ? null : ((IEnumerable)list);
                }
                finally
                {
                    this._updatingHeaderItems = false;
                }
            }
        }

        protected virtual void UpdateItemVisibility(UIElement element, bool toVisible)
        {
            if (element != null)
            {
                element.Opacity = toVisible ? ((double)1) : ((double)0);
                element.IsHitTestVisible = toVisible;
                if (toVisible && (element.Visibility == Visibility.Collapsed))
                {
                    element.Visibility = Visibility.Visible;
                }
                if (this._isDesignTime)
                {
                    TranslateTransform translateTransform = TransformAnimator.GetTranslateTransform(element);
                    if (translateTransform != null)
                    {
                        translateTransform.X = toVisible ? 0.0 : -base.ActualWidth;
                    }
                }
            }
        }

        private void UpdateSelectedIndex(int oldIndex, int newIndex)
        {
            object obj2 = null;
            int count = base.Items.Count;
            if ((newIndex >= 0) && (newIndex < count))
            {
                obj2 = base.Items[newIndex];
            }
            else if ((count > 0) && !this._isDesignTime)
            {
                this._ignorePropertyChange = true;
                this.SelectedIndex = oldIndex;
                throw new ArgumentException("SelectedIndex");
            }
            if (((newIndex < 0) && (base.Items.Count > 0)) && !this._isDesignTime)
            {
                this._ignorePropertyChange = true;
                this.SelectedIndex = 0;
                throw new ArgumentException("SelectedIndex");
            }
            this.SelectedItem = obj2;
        }

        private void UpdateSelectedItem(object oldValue, object newValue)
        {
            if (((newValue == null) && (base.Items.Count > 0)) && !this._isDesignTime)
            {
                this._ignorePropertyChange = true;
                this.SelectedItem = oldValue;
                throw new ArgumentException("SelectedItem");
            }
            int index = base.Items.IndexOf(oldValue);
            int num2 = base.Items.IndexOf(newValue);
            if ((!this._animationHint.HasValue && (index != -1)) && (num2 != -1))
            {
                this._animationHint = new AnimationDirection?((this.RollingIncrement(num2) == index) ? AnimationDirection.Right : AnimationDirection.Left);
            }
            PivotItem container = base.GetContainer(newValue);
            PivotItem item = base.GetContainer(oldValue);
            if (oldValue != null)
            {
                this.OnUnloadingPivotItem(new PivotItemEventArgs(item));
            }
            List<object> removedItems = new List<object>();
            removedItems.Add(oldValue);
            List<object> addedItems = new List<object>();
            addedItems.Add(newValue);
            this.OnSelectionChanged(new SelectionChangedEventArgs(removedItems, addedItems));
            if (container != null)
            {
                this.OnLoadingPivotItem(container);
            }
            else
            {
                this._skippedLoadingPivotItem = true;
            }
            this.BeginAnimateContent(num2, item, this._animationHint.GetValueOrDefault());
            this.SetSelectedHeaderIndex(num2);
            if (this.SelectedIndex != num2)
            {
                this.SetSelectedIndexInternal(num2);
            }
        }

        private void UpdateVisibleContent(int index)
        {
            if (this.TryHasItemsHost())
            {
                for (int i = 0; i < this._itemsPanel.Children.Count; i++)
                {
                    UIElement element = this._itemsPanel.Children[i];
                    this.UpdateItemVisibility(element, i == index);
                }
            }
        }

        // Properties
        private bool EnoughItemsForManipulation
        {
            get
            {
                return (base.Items.Count > 1);
            }
        }

        public DataTemplate HeaderTemplate
        {
            get
            {
                return (base.GetValue(HeaderTemplateProperty) as DataTemplate);
            }
            set
            {
                base.SetValue(HeaderTemplateProperty, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return (int)base.GetValue(SelectedIndexProperty);
            }
            set
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
            set
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
                return (base.GetValue(TitleTemplateProperty) as DataTemplate);
            }
            set
            {
                base.SetValue(TitleTemplateProperty, value);
            }
        }
    }
}
