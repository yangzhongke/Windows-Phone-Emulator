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
using System.Security;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Device.Location.Internals;

namespace System.Device.Location
{
    public class GeoCoordinateWatcher : IDisposable, INotifyPropertyChanged, IGeoPositionWatcher<GeoCoordinate>
    {
        private GeoPositionAccuracy accuracy;
        private volatile bool disposed;
        private IntPtr locationChanged;
        private volatile bool started;
        private IntPtr statusChanged;
        private volatile bool statusNotFired;
        private double threshold;

        // Events
        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;

        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        // Methods
        public GeoCoordinateWatcher()
            : this(GeoPositionAccuracy.Default)
        {
            
        }

        public GeoCoordinateWatcher(GeoPositionAccuracy desiredAccuracy)
        {
            this.accuracy = desiredAccuracy;
            this.Init();
            MapCtrlBoard.GeoCoordinateChanged += new GeoCoordinateChangeDelegate(MapCtrlBoard_GeoCoordinateChanged);
        }

        void MapCtrlBoard_GeoCoordinateChanged(double latitude, double longitude, double altitude, double course, double speed)
        {
            if (!this.started)
            {
                return;
            }
            var geoCoordinate = new GeoCoordinate(latitude, longitude, altitude);
            geoCoordinate.Course = course;
            geoCoordinate.Speed = speed;

            var point = new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, geoCoordinate);
            GeoPositionChangedEventArgs<GeoCoordinate> e = new GeoPositionChangedEventArgs<GeoCoordinate>(point);
            OnPositionChanged(e);
        }

        [SecuritySafeCritical]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SecuritySafeCritical]
        protected virtual void Dispose(bool disposing)
        {
            MapCtrlBoard.GeoCoordinateChanged -= MapCtrlBoard_GeoCoordinateChanged;
            lock (this)
            {
                if (!this.disposed)
                {
                    this.Stop();
                    this.locationChanged = IntPtr.Zero;
                    this.statusChanged = IntPtr.Zero;
                    this.disposed = true;
                }
            }
        }

        private void DisposeCheck()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("GeoCoordinateWatcher");
            }
        }

        [SecuritySafeCritical]
        ~GeoCoordinateWatcher()
        {
            this.Dispose(false);
        }

        [SecuritySafeCritical]
        private void Init()
        {
            GeoPositionStatus noData = GeoPositionStatus.NoData;
            GeoPositionPermission unknown = GeoPositionPermission.Unknown;
            this.Position = new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, GeoCoordinate.Unknown);
            this.MovementThreshold = 0.0;
            this.Status = noData;
            this.Permission = unknown;
            this.statusNotFired = true;
        }
        protected void OnPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (this.started)
            {
                if (this.PositionChanged != null)
                {
                    this.PositionChanged(this, e);
                }
                this.OnPropertyChanged("Position");
            }
        }

        protected void OnPositionStatusChanged(GeoPositionStatusChangedEventArgs e)
        {
            if (this.started && ((this.Status != e.Status) || this.statusNotFired))
            {
                this.statusNotFired = false;
                this.Status = e.Status;
                if (this.StatusChanged != null)
                {
                    this.StatusChanged(this, e);
                }
                this.OnPropertyChanged("Status");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, args);
            }
        }

        [SecuritySafeCritical]
        public void Start()
        {
            this.DisposeCheck();
            lock (this)
            {
                if (this.started)
                {
                    return;
                }
            }
            lock (this)
            {
                this.started = true;
                this.statusNotFired = true;
            }
        }

        [SecuritySafeCritical]
        public void Start(bool suppressPermissionPrompt)
        {
            this.DisposeCheck();
            this.Start();
        }

        [SecuritySafeCritical]
        public void Stop()
        {
            this.DisposeCheck();
            lock (this)
            {
                if (this.started)
                {
                    this.started = false;
                    this.statusNotFired = true;
                }
            }
        }

        [SecuritySafeCritical]
        public bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout)
        {
            this.DisposeCheck();
            bool flag = true;
            lock (this)
            {
                if (this.started)
                {
                    return flag;
                }
            }
            flag = false;
            this.Start(suppressPermissionPrompt);
            return flag;
        }

        // Properties
        public GeoPositionAccuracy DesiredAccuracy
        {
            get
            {
                this.DisposeCheck();
                return this.accuracy;
            }
        }

        public double MovementThreshold
        {
            get
            {
                this.DisposeCheck();
                return this.threshold;
            }
            set
            {
                this.DisposeCheck();
                if ((value < 0.0) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException("value", "Argument must be non negative");
                }
                this.threshold = value;
            }
        }

        public GeoPositionPermission Permission
        {
            get;
            set;
        }

        public GeoPosition<GeoCoordinate> Position
        {
            get;
            set;
        }

        public GeoPositionStatus Status
        {
            get;
            set;
        }
    }
}
