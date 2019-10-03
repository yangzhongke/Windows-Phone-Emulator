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
    public class GeoPosition<T>
    {
        public GeoPosition()
        {
        }

        public GeoPosition(DateTimeOffset timestamp, T position)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position", "<T> must be non null");
            }
            this.Location = position;
            this.Timestamp = timestamp;
        }

        public T Location
        {
            get;
            set;
        }

        public DateTimeOffset Timestamp
        {
            get;
            set;
        }
    }
}
