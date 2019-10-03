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

namespace Microsoft.Phone.Shell
{
    public interface IApplicationBarMenuItem
    {
        // Events
        event EventHandler Click;

        // Properties
        bool IsEnabled { get; set; }
        string Text { get; set; }
    }
}
