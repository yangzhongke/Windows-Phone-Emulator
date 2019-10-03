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

namespace Microsoft.Phone.Net.NetworkInformation
{
    [SecuritySafeCritical]
    public sealed class NetworkInterface : System.Net.NetworkInformation.NetworkInterface
    {
        private static NetworkInterface _singleInstance;
        private static object _sync = new object();

        private NetworkInterface()
        {
        }

        public static NetworkInterface GetInternetInterface()
        {
            if (_singleInstance == null)
            {
                lock (_sync)
                {
                    if (_singleInstance == null)
                    {
                        _singleInstance = new NetworkInterface();
                    }
                }
            }
            return _singleInstance;
        }

        // Properties
        public static NetworkInterfaceType NetworkInterfaceType
        {
            get
            {
                return NetworkInterfaceType.MobileBroadbandGsm;
            }
        }
    }
}
