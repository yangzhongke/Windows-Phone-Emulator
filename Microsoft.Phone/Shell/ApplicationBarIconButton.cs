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
    public class ApplicationBarIconButton : DependencyObject, IApplicationBarIconButton, IApplicationBarMenuItem
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), 
            typeof(ApplicationBarIconButton), null);

        public static DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool),
            typeof(ApplicationBarIconButton), new PropertyMetadata(true));

        public static DependencyProperty IconUriProperty = DependencyProperty.Register("IconUri", typeof(Uri),
            typeof(ApplicationBarIconButton), null);

        public event EventHandler Click;

        public ApplicationBarIconButton():this(null)
        {
            
        }

        public ApplicationBarIconButton(Uri iconUri)
        {
            //if (iconUri == null)
            //{
            //    iconUri = new Uri("");
            //}
            SetValue(IconUriProperty, iconUri);
        }

        internal void FireClickEvent()
        {
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        public Uri IconUri
        {
            get
            {
                return (Uri)GetValue(IconUriProperty);
            }
            set
            {
                SetValue(IconUriProperty, value);
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
