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
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace Microsoft.Phone.Controls.Primitives
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PivotHeaderItem)), TemplatePart(Name = "Canvas", Type = typeof(Canvas))]
    public class PivotHeadersControl : TemplatedItemsControl<PivotHeaderItem>
    {
        // Fields
        private bool _activeSelectionChange;
        private double _animatingWidth;
        internal bool _cancelClick;
        private Canvas _canvas;
        private TransformAnimator _canvasAnimator;
        private DateTime _currentItemAnimationStarted;
        private bool _ignorePropertyChange;
        private bool _isAnimating;
        private bool _isDesign;
        private Image _leftMirror;
        private TranslateTransform _leftMirrorTranslation;
        private Dictionary<Control, OpacityAnimator> _opacities;
        private Pivot _pivot;
        private int _previousVisualFirstIndex;
        private Queue<AnimationInstruction> _queuedAnimations;
        private Dictionary<Control, double> _sizes;
        private Dictionary<Control, TranslateTransform> _translations;
        internal bool _wasClicked;
        private const string CanvasName = "Canvas";
        private const double PivotSeconds = 0.5;
        internal readonly IEasingFunction QuarticEase = new QuarticEase();
        internal EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;
        internal static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(PivotHeadersControl), new PropertyMetadata(0, new PropertyChangedCallback(PivotHeadersControl.OnSelectedIndexPropertyChanged)));
        public static readonly DependencyProperty VisualFirstIndexProperty = DependencyProperty.Register("VisualFirstIndex", typeof(int), typeof(PivotHeadersControl), new PropertyMetadata(0, new PropertyChangedCallback(PivotHeadersControl.OnVisualFirstIndexPropertyChanged)));

        //// Events
        //internal event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged
        //{
        //    add
        //    {
        //        EventHandler<SelectedIndexChangedEventArgs> handler2;
        //        EventHandler<SelectedIndexChangedEventArgs> selectedIndexChanged = this.SelectedIndexChanged;
        //        do
        //        {
        //            handler2 = selectedIndexChanged;
        //            EventHandler<SelectedIndexChangedEventArgs> handler3 = (EventHandler<SelectedIndexChangedEventArgs>)Delegate.Combine(handler2, value);
        //            selectedIndexChanged = Interlocked.CompareExchange<EventHandler<SelectedIndexChangedEventArgs>>(ref this.SelectedIndexChanged, handler3, handler2);
        //        }
        //        while (selectedIndexChanged != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<SelectedIndexChangedEventArgs> handler2;
        //        EventHandler<SelectedIndexChangedEventArgs> selectedIndexChanged = this.SelectedIndexChanged;
        //        do
        //        {
        //            handler2 = selectedIndexChanged;
        //            EventHandler<SelectedIndexChangedEventArgs> handler3 = (EventHandler<SelectedIndexChangedEventArgs>)Delegate.Remove(handler2, value);
        //            selectedIndexChanged = Interlocked.CompareExchange<EventHandler<SelectedIndexChangedEventArgs>>(ref this.SelectedIndexChanged, handler3, handler2);
        //        }
        //        while (selectedIndexChanged != handler2);
        //    }
        //}

        // Methods
        public PivotHeadersControl()
        {
            base.DefaultStyleKey = typeof(PivotHeadersControl);
            this._leftMirror = new Image();
            this._leftMirror.CacheMode = new BitmapCache();
            this._sizes = new Dictionary<Control, double>();
            this._translations = new Dictionary<Control, TranslateTransform>();
            this._opacities = new Dictionary<Control, OpacityAnimator>();
            this._isDesign = DesignerProperties.IsInDesignTool;
            this._queuedAnimations = new Queue<AnimationInstruction>();
        }

        private void AnimateComplete()
        {
            if (this._queuedAnimations.Count == 0)
            {
                this._isAnimating = false;
            }
            else
            {
                AnimationInstruction instruction = this._queuedAnimations.Dequeue();
                Duration duration = new Duration(TimeSpan.FromSeconds(instruction._durationInSeconds));
                this.BeginAnimateInternal(instruction._previousIndex, instruction._index, instruction._ease, new Duration?(duration));
            }
        }

        private void BeginAnimate(int previousIndex, int newIndex)
        {
            if (this._isDesign)
            {
                this.VisualFirstIndex = newIndex;
            }
            else
            {
                if (((newIndex != this.RollingIncrement(previousIndex)) && (newIndex != this.RollingDecrement(previousIndex))) || this._wasClicked)
                {
                    int num;
                    this._wasClicked = false;
                    for (int i = previousIndex; i != newIndex; i = num)
                    {
                        num = this.RollingIncrement(i);
                        AnimationInstruction item = new AnimationInstruction(i, num);
                        item._width = this.GetItemWidth(i);
                        if (item._width > 0.0)
                        {
                            this._queuedAnimations.Enqueue(item);
                        }
                    }
                    this.UpdateActiveAndQueuedAnimations();
                }
                else
                {
                    if ((this._queuedAnimations.Count == 0) && !this._isAnimating)
                    {
                        this.BeginAnimateInternal(previousIndex, newIndex, this.QuarticEase, null);
                        return;
                    }
                    AnimationInstruction instruction2 = new AnimationInstruction(previousIndex, newIndex);
                    instruction2._ease = this.QuarticEase;
                    instruction2._width = this.GetItemWidth(previousIndex);
                    this._queuedAnimations.Enqueue(instruction2);
                    this.UpdateActiveAndQueuedAnimations();
                }
                if (!this._isAnimating)
                {
                    this.AnimateComplete();
                }
            }
        }

        private void BeginAnimateInternal(int previousIndex, int newIndex, IEasingFunction ease, Duration? optionalDuration)
        {
            Action completionAction = null;
            Action action2 = null;
            Duration currentSampleDuration;
            if (((previousIndex == newIndex) || (previousIndex < 0)) || (((previousIndex >= base.Items.Count) || this._isDesign) || (this._canvas == null)))
            {
                if (this.VisualFirstIndex != newIndex)
                {
                    this.VisualFirstIndex = newIndex;
                }
                this.AnimateComplete();
            }
            else
            {
                TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
                this._isAnimating = true;
                bool flag = (base.Items.Count != 2) ? (newIndex == this.RollingIncrement(previousIndex)) : (this.AnimationDirection == AnimationDirection.Left);
                int index = flag ? previousIndex : newIndex;
                double itemWidth = this.GetItemWidth(index);
                this._animatingWidth = itemWidth;
                this._currentItemAnimationStarted = DateTime.Now;
                double targetOffset = -itemWidth + (flag ? 0.0 : this._canvasAnimator.CurrentOffset);
                double num4 = (itemWidth == 0.0) ? itemWidth : ((itemWidth - Math.Abs(this._canvasAnimator.CurrentOffset)) / itemWidth);
                if (num4 == 0.0)
                {
                    num4 = 1.0;
                }
                currentSampleDuration = optionalDuration.HasValue ? optionalDuration.Value : new Duration(TimeSpan.FromSeconds(0.25 + Math.Abs((double)(num4 * 0.25))));
                if (flag)
                {
                    if (completionAction == null)
                    {
                        completionAction = delegate
                        {
                            this.VisualFirstIndex = newIndex;
                            this._canvasAnimator.GoTo(0.0, Pivot.ZeroDuration, new Action(this.AnimateComplete));
                        };
                    }
                    this._canvasAnimator.GoTo(targetOffset, currentSampleDuration, ease, completionAction);
                }
                else
                {
                    this.VisualFirstIndex = newIndex;
                    if (action2 == null)
                    {
                        action2 = delegate
                        {
                            this._canvasAnimator.GoTo(0.0, currentSampleDuration, ease, new Action(this.AnimateComplete));
                        };
                    }
                    this._canvasAnimator.GoTo(targetOffset, Pivot.ZeroDuration, action2);
                }
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            PivotHeaderItem item2 = (PivotHeaderItem)element;
            item2.ParentHeadersControl = null;
            item2.Item = null;
            if (!object.ReferenceEquals(element, item))
            {
                item2.Item = item;
            }
            Control key = item as Control;
            if (key != null)
            {
                key.SizeChanged -= new SizeChangedEventHandler(this.OnHeaderSizeChanged);
                this._sizes.Remove(key);
                this._translations.Remove(key);
            }
        }

        private void FadeIn(int index)
        {
            OpacityAnimator oa;
            Control key = (Control)base.Items[index];
            if (!this._opacities.TryGetValue(key, out oa))
            {
                OpacityAnimator.EnsureAnimator(key, ref oa);
                this._opacities[key] = oa;
            }
            oa.GoTo(0.0, Pivot.ZeroDuration, delegate
            {
                oa.GoTo(1.0, new Duration(TimeSpan.FromSeconds(0.125)));
            });
        }

        private void FadeInItemIfNeeded(int index, int visualFirstIndex, int previousVisualFirstIndex, int itemCount)
        {
            if ((!this._isDesign && (this.RollingIncrement(index) == visualFirstIndex)) && (index == previousVisualFirstIndex))
            {
                if ((itemCount > 1) && ((itemCount != 2) || (this.AnimationDirection != AnimationDirection.Right)))
                {
                    double num = 0.0;
                    for (int i = this.RollingIncrement(index); i != index; i = this.RollingIncrement(i))
                    {
                        num += this.GetItemWidth(i);
                    }
                    if (num < base.ActualWidth)
                    {
                        this.FadeIn(index);
                    }
                }
            }
            else
            {
                Control control = (Control)base.Items[index];
                control.Opacity = 1.0;
            }
        }

        private double GetItemWidth(int index)
        {
            Control key = (Control)base.Items[index];
            double actualWidth = 0.0;
            if (!this._sizes.TryGetValue(key, out actualWidth))
            {
                actualWidth = key.ActualWidth;
                if (!double.IsNaN(actualWidth))
                {
                    this._sizes[key] = actualWidth;
                }
            }
            return actualWidth;
        }

        private double GetLeftMirrorWidth(int index)
        {
            return this.GetItemWidth(this.GetPreviousVisualIndex(index));
        }

        private double GetNextHeaderWidth()
        {
            int index = this.VisualFirstIndex + 1;
            if (index >= base.Items.Count)
            {
                index = 0;
            }
            return this.GetItemWidth(index);
        }

        private int GetPreviousVisualIndex(int indexOfInterest)
        {
            int num = indexOfInterest - 1;
            if (num >= 0)
            {
                return num;
            }
            return (base.Items.Count - 1);
        }

        internal void NotifyHeaderItemSelected(PivotHeaderItem item, bool isSelected)
        {
            if (isSelected)
            {
                int index = base.ItemContainerGenerator.IndexFromContainer(item);
                int selectedIndex = this.SelectedIndex;
                this.SelectOne(selectedIndex, index);
                this.SelectedIndex = index;
            }
        }

        public override void OnApplyTemplate()
        {
            this._pivot = null;
            if (this._canvas != null)
            {
                this._canvas.Children.Remove(this._leftMirror);
                this._leftMirror = null;
            }
            base.OnApplyTemplate();
            this._canvas = base.GetTemplateChild("Canvas") as Canvas;
            if (this._canvas != null)
            {
                this._canvas.Children.Add(this._leftMirror);
                this._leftMirrorTranslation = TransformAnimator.GetTranslateTransform(this._leftMirror);
                if (!double.IsNaN(this._leftMirror.ActualWidth) && (this._leftMirror.ActualWidth > 0.0))
                {
                    this._leftMirrorTranslation.X = -this._leftMirror.ActualWidth;
                }
            }
            if (base.Items.Count > 0)
            {
                this.VisualFirstIndex = this.SelectedIndex;
            }
            DependencyObject reference = this;
            do
            {
                reference = VisualTreeHelper.GetParent(reference);
                this._pivot = reference as Pivot;
            }
            while ((this._pivot == null) && (reference != null));
            if (this._pivot != null)
            {
                this._pivot._clickedHeadersControl = this;
            }
        }

        internal void OnHeaderItemClicked(PivotHeaderItem item)
        {
            if (!this._isAnimating)
            {
                if (this._cancelClick)
                {
                    this._cancelClick = false;
                }
                else
                {
                    this._wasClicked = true;
                    item.IsSelected = true;
                }
            }
        }

        private void OnHeaderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            double height = e.NewSize.Height;
            if (double.IsNaN(base.Height) || (height > base.Height))
            {
                base.Height = height;
            }
            this._sizes[(Control)sender] = width;
            this.UpdateItemsLayout();
            if (this._leftMirrorTranslation.X == 0.0)
            {
                this.UpdateLeftMirrorImage(this.SelectedIndex);
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (base.Items.Count > 0)
            {
                this.UpdateItemsLayout();
            }
        }

        private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotHeadersControl control = d as PivotHeadersControl;
            int newValue = (int)e.NewValue;
            int oldValue = (int)e.OldValue;
            if (!control._activeSelectionChange)
            {
                control.SelectOne(oldValue, newValue);
            }
        }

        private static void OnVisualFirstIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotHeadersControl control = d as PivotHeadersControl;
            if (control._ignorePropertyChange)
            {
                control._ignorePropertyChange = false;
            }
            else
            {
                int newValue = (int)e.NewValue;
                control._previousVisualFirstIndex = (int)e.OldValue;
                int count = control.Items.Count;
                if ((count > 0) && (newValue >= count))
                {
                    control._ignorePropertyChange = true;
                    d.SetValue(e.Property, 0);
                }
                control.UpdateItemsLayout();
            }
        }

        internal void PanHeader(double cumulative, double contentWidth, Duration duration)
        {
            if (!this._isAnimating && (this._canvas != null))
            {
                TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
                double num = (cumulative < 0.0) ? this.GetItemWidth(this.SelectedIndex) : this.GetLeftMirrorWidth(this.SelectedIndex);
                this._canvasAnimator.GoTo((cumulative / contentWidth) * num, duration);
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            PivotHeaderItem item2 = (PivotHeaderItem)element;
            item2.ParentHeadersControl = this;
            int num = base.ItemContainerGenerator.IndexFromContainer(element);
            if (num != -1)
            {
                item2.IsSelected = this.SelectedIndex == num;
            }
            Control control = item as Control;
            if (control != null)
            {
                control.SizeChanged += new SizeChangedEventHandler(this.OnHeaderSizeChanged);
            }
        }

        internal void RestoreHeaderPosition(Duration duration)
        {
            if ((this._canvas != null) && !this._isAnimating)
            {
                TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
                this._canvasAnimator.GoTo(0.0, duration);
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

        private void SelectOne(int previousIndex, int index)
        {
            if (!this._activeSelectionChange)
            {
                this.UpdateLeftMirrorImage(index);
                if ((index >= 0) && (index < base.Items.Count))
                {
                    try
                    {
                        this._activeSelectionChange = true;
                        for (int i = 0; i < base.Items.Count; i++)
                        {
                            PivotHeaderItem item = (PivotHeaderItem)base.ItemContainerGenerator.ContainerFromIndex(i);
                            if (item != null)
                            {
                                item.IsSelected = index == i;
                            }
                        }
                    }
                    finally
                    {
                        SafeRaise.Raise<SelectedIndexChangedEventArgs>(this.SelectedIndexChanged, this, new SelectedIndexChangedEventArgs(index));
                        this._activeSelectionChange = false;
                        this.BeginAnimate(previousIndex, index);
                    }
                }
            }
        }

        private void SetItemPosition(int i, ref double offset)
        {
            double num;
            TranslateTransform translateTransform;
            Control key = (Control)base.Items[i];
            if (!this._sizes.TryGetValue(key, out num))
            {
                num = 0.0;
            }
            if (!this._translations.TryGetValue(key, out translateTransform))
            {
                translateTransform = TransformAnimator.GetTranslateTransform(key);
                this._translations[key] = translateTransform;
            }
            translateTransform.X = offset;
            offset += num;
        }

        private void UpdateActiveAndQueuedAnimations()
        {
            TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
            if (this._canvasAnimator != null)
            {
                double num = 0.0;
                foreach (AnimationInstruction instruction in this._queuedAnimations)
                {
                    num += instruction._width;
                }
                if (this._isAnimating && (this._animatingWidth > 0.0))
                {
                    num += this._animatingWidth;
                }
                int num2 = 0;
                foreach (AnimationInstruction instruction2 in this._queuedAnimations)
                {
                    num2++;
                    instruction2._durationInSeconds = (instruction2._width / ((num == 0.0) ? 1.0 : num)) * 0.5;
                    instruction2._ease = (num2 == this._queuedAnimations.Count) ? this.QuarticEase : null;
                    if (this._isAnimating)
                    {
                        this._canvasAnimator.UpdateEasingFunction(null);
                    }
                }
                if (this._isAnimating)
                {
                    TimeSpan span = (TimeSpan)(DateTime.Now - this._currentItemAnimationStarted);
                    double num4 = span.TotalSeconds / 0.5;
                    double num5 = (this._animatingWidth * num4) / ((num == 0.0) ? 1.0 : num);
                    this._canvasAnimator.UpdateDuration(new Duration(TimeSpan.FromSeconds((num5 * num4) * 0.5)));
                }
            }
        }

        private void UpdateItemsLayout()
        {
            int count = base.Items.Count;
            double offset = 0.0;
            int visualFirstIndex = this.VisualFirstIndex;
            for (int i = visualFirstIndex; i < base.Items.Count; i++)
            {
                this.FadeInItemIfNeeded(i, visualFirstIndex, this._previousVisualFirstIndex, count);
                this.SetItemPosition(i, ref offset);
            }
            if (this.VisualFirstIndex > 0)
            {
                for (int j = 0; j < this.VisualFirstIndex; j++)
                {
                    this.FadeInItemIfNeeded(j, visualFirstIndex, this._previousVisualFirstIndex, count);
                    this.SetItemPosition(j, ref offset);
                }
            }
        }

        private void UpdateLeftMirrorImage(int visualRootIndex)
        {
            if (((this._leftMirrorTranslation != null) && (this._sizes != null)) && (this._leftMirror != null))
            {
                if (base.Items.Count <= 1)
                {
                    this._leftMirror = null;
                }
                else
                {
                    int previousVisualIndex = this.GetPreviousVisualIndex(visualRootIndex);
                    PivotHeaderItem key = (PivotHeaderItem)base.Items[previousVisualIndex];
                    if (this._sizes.ContainsKey(key))
                    {
                        double num2 = this._sizes[key];
                        key.UpdateVisualStateToUnselected();
                        try
                        {
                            WriteableBitmap bitmap = new WriteableBitmap(key, new TranslateTransform());
                            this._leftMirror.Source = bitmap;
                        }
                        catch (Exception)
                        {
                            this._leftMirror.Source = null;
                        }
                        finally
                        {
                            key.RestoreVisualStates();
                            this._leftMirrorTranslation.X = -num2;
                        }
                    }
                }
            }
        }

        // Properties
        internal AnimationDirection AnimationDirection
        {
            get;
            set;
        }

        internal int SelectedIndex
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

        public int VisualFirstIndex
        {
            get
            {
                return (int)base.GetValue(VisualFirstIndexProperty);
            }
            set
            {
                base.SetValue(VisualFirstIndexProperty, value);
            }
        }

        // Nested Types
        private class AnimationInstruction
        {
            // Fields
            public double _durationInSeconds;
            public IEasingFunction _ease;
            public int _index;
            public int _previousIndex;
            public double _width;

            // Methods
            public AnimationInstruction(int previous, int next)
            {
                this._previousIndex = previous;
                this._index = next;
            }
        }
    }
}
