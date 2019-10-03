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

namespace System.Device.Location
{
    public class GeoPositionChangedEventArgs<T> : EventArgs
    {
        public GeoPositionChangedEventArgs(GeoPosition<T> position)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position", "GeoLocation<T> must be non null");
            }
            this.Position = position;
        }

        public GeoPosition<T> Position
        {
            get;
            set;
        }
    }
}
