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

namespace Microsoft.Phone.Internals.touch
{
    public class TouchArgs:EventArgs
    {
        public Gesture GestureType { get; internal set; }
        public double PathAngle { get; internal set; }
        public double PathLength { get; internal set; }
        public Point Position { get; set; }
        public Point Shift { get; set; }
        public Point StartPosition { get; internal set;}
    }
}
