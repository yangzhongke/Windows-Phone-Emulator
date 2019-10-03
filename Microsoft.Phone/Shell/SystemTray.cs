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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Internals;

namespace Microsoft.Phone.Shell
{
    public class SystemTray : DependencyObject
    {
        // Fields
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(SystemTray), new PropertyMetadata(false, new PropertyChangedCallback(SystemTray.OnIsVisiblePropertyChanged)));

        // Methods
        private static PhoneApplicationPage GetActivePage()
        {
            PhoneApplicationPage page = WinPhoneCtrl.Instance.frameScreen.Content as PhoneApplicationPage;
            return page;
        }

        public static bool GetIsVisible(DependencyObject element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void OnIsVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            PhoneApplicationPage page = dependencyObject as PhoneApplicationPage;
            if (page != null)
            {
                //page.SystemTrayIsVisible = (bool)e.NewValue;
            }
        }

        public static void SetIsVisible(DependencyObject element, bool isVisible)
        {
            element.SetValue(IsVisibleProperty, isVisible);
        }

        // Properties
        public static bool IsVisible
        {
            get
            {
                PhoneApplicationPage activePage = GetActivePage();
                if (activePage == null)
                {
                    throw new InvalidOperationException("To get SystemTray visibility the RootVisual must be a PhoneApplicationFrame and its Content must be a PhoneApplicationPage");
                }
                return GetIsVisible(activePage);
            }
            set
            {
                PhoneApplicationPage activePage = GetActivePage();
                if (activePage == null)
                {
                    throw new InvalidOperationException("To set SystemTray visibility the RootVisual must be a PhoneApplicationFrame and its Content must be a PhoneApplicationPage");
                }
                SetIsVisible(activePage, value);
            }
        }
    }
}
