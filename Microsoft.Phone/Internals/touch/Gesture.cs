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

namespace Microsoft.Phone.Internals.touch
{
    public enum Gesture
    {
        DISABLED = -1,
        NONE = 0,
        MOVE_WEST = 1,
        MOVE_NORTH = 2,
        MOVE_EAST = 3,
        MOVE_SOUTH = 4,
        MOVE_NORTHWEST = 5,
        MOVE_NORTHEAST = 6,
        MOVE_SOUTHWEST = 7,
        MOVE_SOUTHEAST = 8,
        ROTATE = 9,
        SCALE = 10,
        SLIDE_WEST = 11,
        SLIDE_NORTH = 12,
        SLIDE_EAST = 13,
        SLIDE_SOUTH = 14,
        SLIDE_NORTHWEST = 15,
        SLIDE_NORTHEAST = 16,
        SLIDE_SOUTHWEST = 17,
        SLIDE_SOUTHEAST = 18,
    }
}
