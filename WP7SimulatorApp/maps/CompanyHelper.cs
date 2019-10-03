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
using System.Collections.Generic;

namespace WP7SimulatorApp.maps
{
    public class CompanyHelper
    {
        public readonly static Dictionary<int, String> dict = new Dictionary<int, string>();
        static CompanyHelper()
        {
            dict[1] = "餐馆";
            dict[2] = "学校";
            dict[3] = "超市";
            dict[4] = "服装店";
            dict[5] = "美发店";
            dict[6] = "健身房";
            dict[7] = "洗浴中心";
            dict[8] = "KTV歌厅";
            dict[9] = "酒吧";
            dict[10] = "其他";
        }
    }
}
