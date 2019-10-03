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
using Microsoft.Maps.MapControl;
using Microsoft.Maps.MapControl.Core;
using System.IO;

namespace System.Device.Location.Internals
{
    public class ItcastTileSource : LocationRectTileSource
    {
        private string tileUriFormat;

        public ItcastTileSource(string titleUriFormat)
        {
            this.tileUriFormat = titleUriFormat;
        }

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            //调用LocationRectTileSource的GetUri返回的是null，估计这个就是OOB下显示不出地图的原因，因此自己计算Uri返回
            string quadkey = new QuadKey(x, y, zoomLevel).Key;
            string uri = tileUriFormat.Replace("{X}", x.ToString()).Replace("{Y}", y.ToString())
                .Replace("{ZoomLevel}", zoomLevel.ToString()).Replace("{QuadKey}", quadkey);
            return new Uri(uri);
        }
    }
}
