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
    public class PivotItemEventArgs : EventArgs
    {
        public PivotItemEventArgs()
        {
        }

        public PivotItemEventArgs(PivotItem item)
            : this()
        {
            this.Item = item;
        }

        // Properties
        public PivotItem Item
        {
            get;
            private set;
        }
    }
}
