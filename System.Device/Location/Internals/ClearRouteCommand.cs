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
using Microsoft.Maps.MapControl.Navigation;
using Microsoft.Maps.MapControl.Core;
using Microsoft.Maps.MapControl;

namespace System.Device.Location.Internals
{
    public class ClearRouteCommand : NavigationBarCommandBase
    {
        private MapCtrlBoard mapCtrlBoard;

        public ClearRouteCommand(MapCtrlBoard mapCtrlBoard)
        {
            this.mapCtrlBoard = mapCtrlBoard;
        }

        public override void Execute(Microsoft.Maps.MapControl.Core.MapBase map)
        {
            mapCtrlBoard.ClearRoute();
        }
    }
}
