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
    public class OrientationChangedEventArgs : EventArgs
    {
        // Fields
        private PageOrientation mOrientation;

        // Methods
        public OrientationChangedEventArgs(PageOrientation orientation)
        {
            this.mOrientation = orientation;
        }

        // Properties
        public PageOrientation Orientation
        {
            get
            {
                return this.mOrientation;
            }
        }
    }
}
