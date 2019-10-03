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

namespace Microsoft.Phone.Controls
{
    /// <summary>
    /// it's not a WP7 SDK class.
    /// InputScope of TextBox in SL is unavailable,so use Attached Property instead
    /// </summary>
    public static class TextBoxEx
    {
        public static readonly DependencyProperty InputScopeProperty =
                DependencyProperty.RegisterAttached(
                    "InputScope",
                    typeof(String),
                    typeof(Control), null);
        public static void SetInputScope(Control inputCtrl, string value)
        {
            inputCtrl.SetValue(InputScopeProperty, value);
        }

        public static string GetInputScope(Control inputCtrl)
        {
            return (string)inputCtrl.GetValue(InputScopeProperty);
        }
    }
}
