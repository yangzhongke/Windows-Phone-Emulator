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
    internal class SelectedIndexChangedEventArgs : EventArgs
    {
        // Methods
        public SelectedIndexChangedEventArgs(int index)
        {
            this.SelectedIndex = index;
        }

        // Properties
        public int SelectedIndex
        {
            get;
            private set;
        }
    }
}
