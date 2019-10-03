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
using Microsoft.Phone.Internals;
using Microsoft.Phone.Internals.touch;

namespace Microsoft.Phone.Controls.Gestures
{
    internal class GestureHelper
    {
        public event EventHandler<TouchArgs> Flick;

        public event EventHandler<TouchArgs> HorizontalDrag;

        public event EventHandler<TouchArgs> VerticalDrag;

        public static GestureHelper Create()
        {
            GestureHelper helper = new GestureHelper();
            return helper;
        }

        public GestureHelper()
        {
            WinPhoneCtrl.Instance.GestureDetected += Instance_GestureDetected;
        }

        void Instance_GestureDetected(object sender, Internals.touch.TouchArgs e)
        {
            if (e.GestureType == Internals.touch.Gesture.MOVE_EAST
                || e.GestureType == Internals.touch.Gesture.MOVE_WEST)
            {
                if (HorizontalDrag != null)
                {
                    HorizontalDrag(null, e);
                }
            }
            if (e.GestureType == Internals.touch.Gesture.MOVE_SOUTH
                || e.GestureType == Internals.touch.Gesture.MOVE_NORTH)
            {
                if (VerticalDrag != null)
                {
                    VerticalDrag(null, e);
                }
            }
            if (Flick != null)
            {
                Flick(null, e);
            }
        }

    }
}
