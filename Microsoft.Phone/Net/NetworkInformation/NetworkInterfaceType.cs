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

namespace Microsoft.Phone.Net.NetworkInformation
{
    public enum NetworkInterfaceType
    {
        AsymmetricDsl = 0x5e,
        Atm = 0x25,
        BasicIsdn = 20,
        Ethernet = 6,
        Ethernet3Megabit = 0x1a,
        FastEthernetFx = 0x45,
        FastEthernetT = 0x3e,
        Fddi = 15,
        GenericModem = 0x30,
        GigabitEthernet = 0x75,
        HighPerformanceSerialBus = 0x90,
        IPOverAtm = 0x72,
        Isdn = 0x3f,
        Loopback = 0x18,
        MobileBroadbandCdma = 0x92,
        MobileBroadbandGsm = 0x91,
        MultiRateSymmetricDsl = 0x8f,
        None = 0,
        Ppp = 0x17,
        PrimaryIsdn = 0x15,
        RateAdaptDsl = 0x5f,
        Slip = 0x1c,
        SymmetricDsl = 0x60,
        TokenRing = 9,
        Tunnel = 0x83,
        Unknown = 1,
        VeryHighSpeedDsl = 0x61,
        Wireless80211 = 0x47
    }
}
