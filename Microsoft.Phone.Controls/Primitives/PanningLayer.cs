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
using System.Windows.Media.Imaging;

namespace Microsoft.Phone.Controls.Primitives
{
    [TemplatePart(Name = "LeftWraparound", Type = typeof(Rectangle)), TemplatePart(Name = "RightWraparound", Type = typeof(Rectangle)), TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter)), TemplatePart(Name = "LocalTransform", Type = typeof(TranslateTransform)), TemplatePart(Name = "PanningTransform", Type = typeof(TranslateTransform))]
    public class PanningLayer : ContentControl
    {
        // Fields
        private readonly IEasingFunction _easingFunction = new ExponentialEase { Exponent = 5.0 };
        private TransformAnimator animator;
        private ContentPresenter contentPresenter;
        private const string ContentPresenterName = "ContentPresenter";
        protected static readonly Duration Immediately = new Duration(TimeSpan.Zero);
        private bool isStatic;
        private const string LeftWraparoundName = "LeftWraparound";
        private const string LocalTransformName = "LocalTransform";
        private const string PanningTransformName = "PanningTransform";
        private const string RightWraparoundName = "RightWraparound";

        // Methods
        public PanningLayer()
        {
            base.DefaultStyleKey = typeof(PanningLayer);
        }

        public void GoTo(int targetOffset, Duration duration, Action completionAction)
        {
            if ((this.animator != null) && !this.IsStatic)
            {
                this.animator.GoTo((double)((int)(targetOffset * this.PanRate)), duration, this._easingFunction, completionAction);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.LocalTransform = base.GetTemplateChild("LocalTransform") as TranslateTransform;
            this.PanningTransform = base.GetTemplateChild("PanningTransform") as TranslateTransform;
            this.LeftWraparound = base.GetTemplateChild("LeftWraparound") as Rectangle;
            this.RightWraparound = base.GetTemplateChild("RightWraparound") as Rectangle;
            this.ContentPresenter = base.GetTemplateChild("ContentPresenter") as ContentPresenter;
            this.animator = (this.PanningTransform != null) ? new TransformAnimator(this.PanningTransform) : null;
        }

        private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateWrappingRectangles();
        }

        internal void Refresh()
        {
            this.UpdateWrappingRectangles();
        }

        protected virtual void UpdateWrappingRectangles()
        {
            bool flag = (!(base.Content is ItemsPresenter) || (this.Owner.Panel == null)) || (this.Owner.Panel.VisibleChildren.Count < 3);
            this.LeftWraparound.Visibility = this.RightWraparound.Visibility = (this.IsStatic || !flag) ? Visibility.Collapsed : Visibility.Visible;
            if (!this.IsStatic && flag)
            {
                this.RightWraparound.Height = this.LeftWraparound.Height = this.ContentPresenter.ActualHeight;
                this.RightWraparound.Width = this.LeftWraparound.Width = this.Owner.ViewportWidth;
                this.LeftWraparound.Margin = this.RightWraparound.Margin = this.ContentPresenter.Margin;
                WriteableBitmap bitmap = new WriteableBitmap(this.Owner.ViewportWidth, (int)this.ContentPresenter.ActualHeight);
                TranslateTransform transform = new TranslateTransform();
                bitmap.Render(this.ContentPresenter, transform);
                bitmap.Invalidate();
                this.RightWraparound.Fill = new ImageBrush { ImageSource = bitmap };
                bitmap = new WriteableBitmap(this.Owner.ViewportWidth, (int)this.ContentPresenter.ActualHeight);
                transform.X = this.Owner.ViewportWidth - this.ContentPresenter.ActualWidth;
                bitmap.Render(this.ContentPresenter, transform);
                bitmap.Invalidate();
                this.LeftWraparound.Fill = new ImageBrush { ImageSource = bitmap };
            }
            if (this.LocalTransform != null)
            {
                double num = (this.LeftWraparound.Visibility == Visibility.Visible) ? (-this.LeftWraparound.Width - this.LeftWraparound.Margin.Left) : 0.0;
                this.LocalTransform.X = this.IsStatic ? 0.0 : (num - this.LeftWraparound.Margin.Right);
            }
        }

        public virtual void Wraparound(int direction)
        {
            if (direction < 0)
            {
                this.GoTo((int)((this.ActualOffset + this.ContentPresenter.ActualWidth) / this.PanRate), Immediately, null);
            }
            else
            {
                this.GoTo((int)((this.ActualOffset - this.ContentPresenter.ActualWidth) / this.PanRate), Immediately, null);
            }
        }

        // Properties
        internal double ActualOffset
        {
            get
            {
                if (this.PanningTransform == null)
                {
                    return 0.0;
                }
                return this.PanningTransform.X;
            }
            private set
            {
                if (this.PanningTransform != null)
                {
                    this.PanningTransform.X = value;
                }
            }
        }

        protected internal ContentPresenter ContentPresenter
        {
            get
            {
                return this.contentPresenter;
            }
            set
            {
                if (this.contentPresenter != null)
                {
                    this.contentPresenter.SizeChanged -= new SizeChangedEventHandler(this.OnContentSizeChanged);
                }
                this.contentPresenter = value;
                if (this.contentPresenter != null)
                {
                    this.contentPresenter.SizeChanged += new SizeChangedEventHandler(this.OnContentSizeChanged);
                }
            }
        }

        internal bool IsStatic
        {
            get
            {
                return this.isStatic;
            }
            set
            {
                if (value != this.isStatic)
                {
                    this.isStatic = value;
                    this.UpdateWrappingRectangles();
                    if (this.isStatic)
                    {
                        this.ActualOffset = 0.0;
                    }
                }
            }
        }

        protected Rectangle LeftWraparound { get; set; }

        protected TranslateTransform LocalTransform { get; set; }

        internal Panorama Owner { get; set; }

        protected TranslateTransform PanningTransform { get; set; }

        protected virtual double PanRate
        {
            get
            {
                return 1.0;
            }
        }

        protected Rectangle RightWraparound { get; set; }
    }
}
