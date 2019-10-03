using System;
using System.Net;
using Microsoft.Maps.MapControl;

namespace System.Device.Location.Internals
{
    /// <summary>
    /// OOB下不能使用任何的Mode，所以需要使用这个Mode
    /// http://www.cnblogs.com/rupeng/archive/2011/02/19/1958631.html
    /// </summary>
    public class ItcastMode : RoadMode
    {
        public ItcastMode(string tileUriFormat)
            : base()
        {
            var tileLayer = (MapTileLayer)this.Content;
            var tileSources = tileLayer.TileSources;
            tileSources.Clear();
            tileSources.Add(new ItcastTileSource(tileUriFormat));
        }
    }
}
