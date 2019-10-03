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
using Microsoft.Xna.Framework.Framework;

namespace Microsoft.Xna.Framework.Input.Touch
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GestureSample
    {
        public GestureType GestureType
        {
            get;
            internal set;
        }
        public TimeSpan Timestamp
        {
            get;
            internal set;
        }
        public Vector2 Position
        {
            get;
            internal set;
        }
        public Vector2 Position2
        {
            get;
            internal set;
        }
        public Vector2 Delta
        {
            get;
            internal set;
        }
        public Vector2 Delta2
        {
            get;
            internal set;
        }
    }


}
