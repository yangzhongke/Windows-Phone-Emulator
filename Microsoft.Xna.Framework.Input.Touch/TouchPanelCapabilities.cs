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
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Input.Touch
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TouchPanelCapabilities
    {
        public bool IsConnected
        {
            get
            {
                return true;
            }
        }
        public int MaximumTouchCount
        {
            get
            {
                return 1;
            }
        }
    }
}
