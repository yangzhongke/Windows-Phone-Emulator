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
using System.Collections.Generic;
using System.Windows.Threading;

namespace Microsoft.Phone.Info
{
    [SecuritySafeCritical]
    public static class DeviceExtendedProperties
    {
        private static Dictionary<string, object> data = new Dictionary<string, object>();

        static DeviceExtendedProperties()
        {
            data["DeviceManufacturer"] = "传智播客.Net培训";
            data["DeviceName"] = "ItcastWindowsPhoneSimulator";
            data["DeviceUniqueId"] = new byte[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            data["DeviceFirmwareVersion"] = "0.0.0.0";
            data["DeviceHardwareVersion"] = "0.0.0.0";
            data["DeviceTotalMemory"] = Convert.ToInt64(390*1024*1024);
            data["ApplicationCurrentMemoryUsage"] = 0L;
            data["ApplicationPeakMemoryUsage"] = 0L;

            //定时更新内存占用率等模拟数据的Timer
            DispatcherTimer updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(100);
            updateTimer.Tick += new EventHandler(updateTimer_Tick);
            updateTimer.Start();
        }

        static void updateTimer_Tick(object sender, EventArgs e)
        {
            long totalMemory = Convert.ToInt64(data["DeviceTotalMemory"]);
            data["ApplicationCurrentMemoryUsage"] = Convert.ToInt64(totalMemory * new Random().NextDouble());
            data["ApplicationPeakMemoryUsage"] = Convert.ToInt64(totalMemory * new Random().NextDouble());
        }

        public static object GetValue(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException();
            }
            if (data.ContainsKey(propertyName))
            {
                return data[propertyName];
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        public static bool TryGetValue(string propertyName, out object propertyValue)
        {
            return data.TryGetValue(propertyName, out propertyValue);
        }
    }

 

}
