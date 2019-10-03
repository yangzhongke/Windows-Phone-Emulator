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

namespace Microsoft.Phone.Shell
{
    public class ApplicationBarStateChangedEventArgs : EventArgs
    {
        // Fields
        private bool mMenuVisible;

        // Methods
        public ApplicationBarStateChangedEventArgs(bool isMenuVisible)
        {
            this.mMenuVisible = isMenuVisible;
        }

        // Properties
        public bool IsMenuVisible
        {
            get
            {
                return this.mMenuVisible;
            }
        }
    }
}
