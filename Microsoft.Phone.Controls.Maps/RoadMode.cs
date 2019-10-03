﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Maps.Internals;

namespace Microsoft.Phone.Controls.Maps
{
    public class RoadMode : WP7Mode
    {
        public RoadMode()
            : base("http://t0.tiles.virtualearth.net/tiles/r{QuadKey}.png?g=213")
        {
        }
    }
}
