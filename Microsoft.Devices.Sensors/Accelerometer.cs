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
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;

namespace Microsoft.Devices.Sensors
{
    public sealed class Accelerometer : IDisposable
    {
        // Events
        public event EventHandler<AccelerometerReadingEventArgs> ReadingChanged;

        private bool isClosed = true;
        // Methods
        public Accelerometer()
        {
            this.State = SensorState.Ready;
            WinPhoneCtrl.Instance.PlaneProjectionChange += OnReadingChanged;
        }

        public void Dispose()
        {
            try
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
            }
        }

        private void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.Stop();
                }
            }
            catch (Exception)
            {
            }
        }

        ~Accelerometer()
        {
            this.Dispose(false);
        }

        internal void OnReadingChanged(double x, double y, double z)
        {
            if (isClosed)
            {
                return;
            }

            if (ReadingChanged != null)
            {
                //换算成重力加速度。静止状态加速度三个的平方和是1
                //http://www.cnblogs.com/imobiler/archive/2010/12/10/1902609.html 
                double gy = Math.Round(Sin(360 - x), 2);
                double gz = Math.Round(-Cos(360 - x) * Cos(y), 2);
                double gx = Math.Round(-Sin(y),2);
                //todo:支持晃动的动态加速度。暂不支持

                var eventArg = new AccelerometerReadingEventArgs() { X = gx, Y = gy, Z = gz, Timestamp = DateTime.Now };
                BackgroundWorker bw = new BackgroundWorker();
                //模拟器中也是readingChanged不是在UI线程触发的
                bw.DoWork += (sender, e) =>
                {
                    ReadingChanged(this, (AccelerometerReadingEventArgs)e.Argument);
                };
                bw.RunWorkerCompleted += (sender, e) => 
                { 
                    if(e.Error!=null)
                        throw e.Error; 
                };
                //readingChanged中发生异常则不会到Application_UnhandledException
                //中，所以需要监听一下再抛出。

                bw.RunWorkerAsync(eventArg);                            
            }
        }

        private static double Sin(double angle)
        {
            return Math.Sin(ToRadians(angle));
        }

        private static double Cos(double angle)
        {
            return Math.Cos(ToRadians(angle));
        }

        private static double ToRadians(double angle)
        {
            return Math.PI*angle/180.0;
        }

        public void Start()
        {
            if (this.State == SensorState.NoPermissions)
            {
                throw new UnauthorizedAccessException("Caller does not have the capability to access Accelerometer Sensor.");
            }
            isClosed = false;
        }

        public void Stop()
        {
            if (this.State == SensorState.NoPermissions)
            {
                throw new UnauthorizedAccessException("Caller does not have the capability to access Accelerometer Sensor.");
            }
            isClosed = true;
        }

        // Properties
        public SensorState State
        {
            get;
            set;
        }
    }


}
