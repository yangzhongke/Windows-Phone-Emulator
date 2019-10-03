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
using Microsoft.Maps.MapControl.Core;
using Microsoft.Maps.MapControl;

namespace WP7SimulatorApp.maps
{
    public class ChineseMode: MercatorMode
    {
        private readonly MapTileLayer tileLayer;

        public ChineseMode()
            : base()
        {
            tileLayer = new MapTileLayer();
            tileLayer.TileSources.Add(
                new ChineseTileSource("http://r3.tiles.ditu.live.com/tiles/r{QuadKey}.png?g=47"));
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
