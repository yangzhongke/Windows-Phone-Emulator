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

namespace Microsoft.Xna.Framework.Input.Touch
{
    public static class TouchPanel
    {
        public static TouchPanelCapabilities GetCapabilities()
        {
            return new TouchPanelCapabilities();
        }
        
        public static GestureSample ReadGesture()
        {
            var args = WinPhoneCtrl.Instance.ReadCurrentTouchArgs();
            if (args == null)
            {
                return new GestureSample() { GestureType= GestureType.None};
            }
            GestureSample sample = new GestureSample();
            if (args.GestureType == Phone.Internals.touch.Gesture.MOVE_EAST ||
                args.GestureType == Phone.Internals.touch.Gesture.MOVE_WEST)
            {
                sample.GestureType = GestureType.HorizontalDrag;
            }
            if (args.GestureType == Phone.Internals.touch.Gesture.MOVE_NORTH ||
                args.GestureType == Phone.Internals.touch.Gesture.MOVE_SOUTH)
            {
                sample.GestureType = GestureType.VerticalDrag;
            }
            sample.Position = new Framework.Vector2() 
                { X = (float)args.Position.X, Y = (float)args.Position.Y };
            sample.Delta = new Framework.Vector2() 
                { 
                    X=(float)(args.Position.X-args.StartPosition.X),
                  Y = (float)(args.Position.Y - args.StartPosition.Y)
                };
            //http://dotnet.chinaitlab.com/CSharp/808869.html
            //判断EnabledGestures中是否启用了指定的GestureType
            //如果没指定相应的GestureType，则调整GestureType为None
            if ((EnabledGestures & sample.GestureType) != sample.GestureType)
            {
                sample.GestureType = GestureType.None;
            }
            return sample;
        }

        public static GestureType EnabledGestures
        {
            get;
            set;
        }

        public static bool IsGestureAvailable
        {
            get
            {
                var args = WinPhoneCtrl.Instance.ReadCurrentTouchArgs();
                if (args == null)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
