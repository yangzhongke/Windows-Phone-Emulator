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

namespace Microsoft.Phone.Controls
{
    internal sealed class OpacityAnimator
    {
        // Fields
        private readonly DoubleAnimation _daRunning = new DoubleAnimation();
        private Action _oneTimeAction;
        private readonly Storyboard _sbRunning = new Storyboard();
        private bool _suppressChangeNotification;
        private static readonly PropertyPath OpacityPropertyPath = new PropertyPath("Opacity", new object[0]);

        // Methods
        public OpacityAnimator(UIElement target)
        {
            this._sbRunning.Completed += new EventHandler(this.OnCompleted);
            this._sbRunning.Children.Add(this._daRunning);
            Storyboard.SetTarget(this._daRunning, target);
            Storyboard.SetTargetProperty(this._daRunning, OpacityPropertyPath);
        }

        public static void EnsureAnimator(UIElement targetElement, ref OpacityAnimator animator)
        {
            if (animator == null)
            {
                animator = new OpacityAnimator(targetElement);
            }
            if (animator == null)
            {
                throw new InvalidOperationException("The animation system could not be prepared for the target element.");
            }
        }

        public void GoTo(double targetOpacity, Duration duration)
        {
            this.GoTo(targetOpacity, duration, null, null);
        }

        public void GoTo(double targetOpacity, Duration duration, Action completionAction)
        {
            this.GoTo(targetOpacity, duration, null, completionAction);
        }

        public void GoTo(double targetOpacity, Duration duration, IEasingFunction easingFunction, Action completionAction)
        {
            this._daRunning.To = new double?(targetOpacity);
            this._daRunning.Duration = duration;
            this._daRunning.EasingFunction = easingFunction;
            this._sbRunning.Begin();
            this._suppressChangeNotification = true;
            this._sbRunning.SeekAlignedToLastTick(TimeSpan.Zero);
            this._oneTimeAction = completionAction;
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            Action action = this._oneTimeAction;
            if (action != null)
            {
                this._oneTimeAction = null;
                action();
            }
            if (!this._suppressChangeNotification)
            {
                this._suppressChangeNotification = false;
            }
        }
    }
}
