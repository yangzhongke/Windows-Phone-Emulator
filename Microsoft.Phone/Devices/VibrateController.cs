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

namespace Microsoft.Phone.Devices
{
    public class VibrateController
    {
        // Fields
        private static VibrateController instance = new VibrateController();
        internal static readonly TimeSpan MaximumDuration = TimeSpan.FromSeconds(5.0);
        internal static readonly TimeSpan MinimumDuration = TimeSpan.FromSeconds(0.0);

        // Methods
        private VibrateController()
        {
            
        }

        public void Start(TimeSpan duration)
        {
            if (duration.CompareTo(MaximumDuration) > 0)
            {
                throw new ArgumentOutOfRangeException("duration", "Duration is more than the max allowed duration");
            }
            if (duration.CompareTo(MinimumDuration) < 0)
            {
                throw new ArgumentOutOfRangeException("duration", "Duration cannot be negative");
            }
            if (duration.TotalMilliseconds >= 1.0)
            {
                WinPhoneCtrl.Instance.Vibrate(duration);
            }
        }

        public void Stop()
        {
            WinPhoneCtrl.Instance.StopVibrate();
        }

        // Properties
        public static VibrateController Default
        {
            get
            {
                return instance;
            }
        }
    }
}
