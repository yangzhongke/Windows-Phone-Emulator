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
    public interface IGeoPositionWatcher<T>
    {
        // Events
        event EventHandler<GeoPositionChangedEventArgs<T>> PositionChanged;
        event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

        // Methods
        void Start();
        void Start(bool suppressPermissionPrompt);
        void Stop();
        bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout);

        // Properties
        GeoPosition<T> Position { get; }
        GeoPositionStatus Status { get; }
    }
}
