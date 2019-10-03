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
    public class PanningBackgroundLayer : PanningLayer
    {
        // Properties
        protected override double PanRate
        {
            get
            {
                double num = 1.0;
                if ((base.Owner != null) && (base.ContentPresenter != null))
                {
                    num = (Math.Max((double)base.Owner.ViewportWidth, base.ContentPresenter.ActualWidth) - ((base.Owner.ViewportWidth / 5) * 4)) / ((double)Math.Max(base.Owner.ViewportWidth, base.Owner.ItemsWidth));
                }
                return num;
            }
        }
    }


}
