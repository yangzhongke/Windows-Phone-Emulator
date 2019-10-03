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
using System.Linq;

namespace Microsoft.Phone.Controls
{
    internal sealed class TransformAnimator
{
    // Fields
    private readonly DoubleAnimation _daRunning = new DoubleAnimation();
    private Action _oneTimeAction;
    private readonly Storyboard _sbRunning = new Storyboard();
    private TranslateTransform _transform;
    private static readonly PropertyPath TranslateXPropertyPath = new PropertyPath("X", new object[0]);

    // Methods
    public TransformAnimator(TranslateTransform translateTransform)
    {
        this._transform = translateTransform;
        this._sbRunning.Completed += new EventHandler(this.OnCompleted);
        this._sbRunning.Children.Add(this._daRunning);
        Storyboard.SetTarget(this._daRunning, this._transform);
        Storyboard.SetTargetProperty(this._daRunning, TranslateXPropertyPath);
    }

    public static void EnsureAnimator(FrameworkElement targetElement, ref TransformAnimator animator)
    {
        if (animator == null)
        {
            TranslateTransform translateTransform = GetTranslateTransform(targetElement);
            if (translateTransform != null)
            {
                animator = new TransformAnimator(translateTransform);
            }
        }
        if (animator == null)
        {
            throw new InvalidOperationException("The animation system could not be prepared for the target element.");
        }
    }

    public static TranslateTransform GetTranslateTransform(UIElement container)
    {
        if (container == null)
        {
            throw new ArgumentNullException("container");
        }
        TranslateTransform renderTransform = container.RenderTransform as TranslateTransform;
        if (renderTransform == null)
        {
            if (container.RenderTransform == null)
            {
                renderTransform = new TranslateTransform();
                container.RenderTransform = renderTransform;
                return renderTransform;
            }
            if (container.RenderTransform is TransformGroup)
            {
                TransformGroup group = container.RenderTransform as TransformGroup;
                //if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                //{
                //    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Transform, bool>(null, (IntPtr) <GetTranslateTransform>b__0);
                //}
                //if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                //{
                //    CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Transform, TranslateTransform>(null, (IntPtr) <GetTranslateTransform>b__1);
                //}
                //renderTransform = Enumerable.Select<Transform, TranslateTransform>(Enumerable.Where<Transform>(group.Children, CS$<>9__CachedAnonymousMethodDelegate2), CS$<>9__CachedAnonymousMethodDelegate3).FirstOrDefault<TranslateTransform>();
                if (renderTransform == null)
                {
                    renderTransform = new TranslateTransform();
                    group.Children.Add(renderTransform);
                }
                return renderTransform;
            }
            TransformGroup group2 = new TransformGroup();
            Transform transform2 = container.RenderTransform;
            container.RenderTransform = null;
            group2.Children.Add(transform2);
            renderTransform = new TranslateTransform();
            group2.Children.Add(renderTransform);
            container.RenderTransform = group2;
        }
        return renderTransform;
    }

    public void GoTo(double targetOffset, Duration duration)
    {
        this.GoTo(targetOffset, duration, null, null);
    }

    public void GoTo(double targetOffset, Duration duration, Action completionAction)
    {
        this.GoTo(targetOffset, duration, null, completionAction);
    }

    public void GoTo(double targetOffset, Duration duration, IEasingFunction easingFunction)
    {
        this.GoTo(targetOffset, duration, easingFunction, null);
    }

    public void GoTo(double targetOffset, Duration duration, IEasingFunction easingFunction, Action completionAction)
    {
        this._daRunning.To = new double?(targetOffset);
        this._daRunning.Duration = duration;
        this._daRunning.EasingFunction = easingFunction;
        this._sbRunning.Begin();
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
    }

    public void UpdateDuration(Duration duration)
    {
        if (this._daRunning != null)
        {
            this._daRunning.Duration = duration;
        }
    }

    public void UpdateEasingFunction(IEasingFunction ease)
    {
        if ((this._daRunning != null) && (this._daRunning.EasingFunction != ease))
        {
            this._daRunning.EasingFunction = ease;
        }
    }

    // Properties
    public double CurrentOffset
    {
        get
        {
            return this._transform.X;
        }
    }
}
}
