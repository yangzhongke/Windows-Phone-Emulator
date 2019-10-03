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
    public class ApplicationBarMenuItem :DependencyObject, IApplicationBarMenuItem
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string),
            typeof(ApplicationBarMenuItem), null);

        public static DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool),
            typeof(ApplicationBarMenuItem), new PropertyMetadata(true));

        public event EventHandler Click;

        public ApplicationBarMenuItem()
            : this(null)
        {
        }

        public ApplicationBarMenuItem(string text)
        {
            this.Text = text;
            this.IsEnabled = true;
        }

        internal void FireClickEvent()
        {
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return (bool)GetValue(IsEnabledProperty);
            }
            set
            {
                SetValue(IsEnabledProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }


}
