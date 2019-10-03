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

namespace Microsoft.Devices.Sensors
{
    public class AccelerometerReadingEventArgs : EventArgs
    {
        

        // Properties
        public DateTimeOffset Timestamp
        {
            get;
            set;
        }

        public double X
        {
            get;
            internal set;
        }

        public double Y
        {
            get;
            internal set;
        }

        public double Z
        {
            get;
            internal set;
        }
    }


}
