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

namespace Microsoft.Phone.Controls.Primitives
{
    public class PanningTitleLayer : PanningLayer
    {
        // Methods
        protected override void UpdateWrappingRectangles()
        {
            base.UpdateWrappingRectangles();
            int num = (int)(base.Owner.ViewportWidth * 0.1);
            base.LeftWraparound.Margin = new Thickness(0.0, 0.0, this.WidthAdjustment + num, 0.0);
            base.RightWraparound.Margin = new Thickness(this.WidthAdjustment + num, 0.0, 0.0, 0.0);
            if (base.LocalTransform != null)
            {
                base.LocalTransform.X = base.IsStatic ? 0.0 : ((-base.LeftWraparound.Width - base.LeftWraparound.Margin.Left) - base.LeftWraparound.Margin.Right);
            }
        }

        public override void Wraparound(int direction)
        {
            if (direction < 0)
            {
                int targetOffset = (int)(((base.ActualOffset + base.ContentPresenter.ActualWidth) + this.WidthAdjustment) / this.PanRate);
                base.GoTo(targetOffset, PanningLayer.Immediately, null);
            }
            else
            {
                int num2 = (int)(((base.ActualOffset - base.ContentPresenter.ActualWidth) - this.WidthAdjustment) / this.PanRate);
                base.GoTo(num2, PanningLayer.Immediately, null);
            }
        }

        // Properties
        protected override double PanRate
        {
            get
            {
                double num = 1.0;
                if ((base.Owner != null) && (base.ContentPresenter != null))
                {
                    num = (Math.Max((double)base.Owner.ViewportWidth, base.ContentPresenter.ActualWidth + this.WidthAdjustment) - ((base.Owner.ViewportWidth / 5) * 4)) / ((double)Math.Max(base.Owner.ViewportWidth, base.Owner.ItemsWidth));
                }
                return num;
            }
        }

        private double WidthAdjustment
        {
            get
            {
                return (base.Owner.ViewportWidth * 0.625);
            }
        }
    }
}
