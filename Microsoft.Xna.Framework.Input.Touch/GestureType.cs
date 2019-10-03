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

namespace Microsoft.Xna.Framework.Input.Touch
{
    [Flags]
    public enum GestureType
    {
        DoubleTap = 2,
        DragComplete = 0x100,
        Flick = 0x80,
        FreeDrag = 0x20,
        Hold = 4,
        HorizontalDrag = 8,
        None = 0,
        Pinch = 0x40,
        PinchComplete = 0x200,
        Tap = 1,
        VerticalDrag = 0x10
    }
}
