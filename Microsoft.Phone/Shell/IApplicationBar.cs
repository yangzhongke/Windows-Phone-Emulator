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
using System.Collections;

namespace Microsoft.Phone.Shell
{
    public interface IApplicationBar
    {
        // Events
        event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;

        // Properties
        Color BackgroundColor { get; set; }
        IList Buttons { get; }
        Color ForegroundColor { get; set; }
        bool IsMenuEnabled { get; set; }
        bool IsVisible { get; set; }
        IList MenuItems { get; }
        double Opacity { get; set; }
    }
}
