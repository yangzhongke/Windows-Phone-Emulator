using System;
using System.Net;
using Microsoft.Maps.MapControl;
using Microsoft.Maps.MapControl.Core;

namespace Microsoft.Phone.Controls.Maps.Internals
{
    /// <summary>
    /// OOB下不能使用任何的Mode，所以需要使用这个Mode
    /// http://www.cnblogs.com/rupeng/archive/2011/02/19/1958631.html
    /// </summary>
    public class WP7Mode : MercatorMode
    {
        private readonly MapTileLayer tileLayer;
 
        public WP7Mode(string tileUriFormat)
            : base()
        {
            tileLayer = new MapTileLayer();
            tileLayer.TileSources.Add(new WP7TileSource(tileUriFormat));
        }

        public override System.Windows.UIElement Content
        {
            get
            {
                return tileLayer;
            }
        }
    }
}
