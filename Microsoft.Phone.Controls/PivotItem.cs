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

namespace Microsoft.Phone.Controls
{
    [TemplateVisualState(Name = "Left", GroupName = "Position States"), TemplateVisualState(Name = "Right", GroupName = "Position States"), TemplateVisualState(Name = "Center", GroupName = "Position States")]
    public class PivotItem : ContentControl
    {
        // Fields
        private AnimationDirection _direction;
        private AnimationDirection? _firstAnimation = null;
        private const string ContentName = "Content";
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(PivotItem), new PropertyMetadata(null));
        private const string PivotPositionsGroupName = "Position States";
        private const string PivotStateCenter = "Center";
        private const string PivotStateLeft = "Left";
        private const string PivotStateRight = "Right";

        // Methods
        public PivotItem()
        {
            base.DefaultStyleKey = typeof(PivotItem);
        }

        internal void MoveTo(AnimationDirection direction)
        {
            bool useTransitions = direction != AnimationDirection.Center;
            this._direction = direction;
            if (!this._firstAnimation.HasValue && useTransitions)
            {
                this._firstAnimation = new AnimationDirection?(direction);
            }
            else
            {
                this.UpdateVisualStates(useTransitions);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AnimationDirection? nullable = this._firstAnimation;
            this.MoveTo(AnimationDirection.Center);
            if (nullable.HasValue)
            {
                this.MoveTo(nullable.Value);
            }
            this._firstAnimation = 0;
        }

        private static string StateNameFromAnimationDirection(AnimationDirection direction)
        {
            switch (direction)
            {
                case AnimationDirection.Center:
                    return "Center";

                case AnimationDirection.Left:
                    return "Left";

                case AnimationDirection.Right:
                    return "Right";
            }
            throw new InvalidOperationException();
        }

        private void UpdateVisualStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, StateNameFromAnimationDirection(this._direction), useTransitions);
        }

        // Properties
        public object Header
        {
            get
            {
                return base.GetValue(HeaderProperty);
            }
            set
            {
                base.SetValue(HeaderProperty, value);
            }
        }
    }
}
