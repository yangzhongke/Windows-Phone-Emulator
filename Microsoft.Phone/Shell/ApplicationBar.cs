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
using System.Windows.Markup;
using System.ComponentModel;
using System.Collections;
using Microsoft.Phone.Controls;

namespace Microsoft.Phone.Shell
{
    [ContentProperty("Buttons")]
    public sealed class ApplicationBar : DependencyObject, IApplicationBar
    {
        internal static readonly int MaxIconButtons = 4;
        internal static readonly int MaxMenuItems = 50;

        private ApplicationBarItemList<IApplicationBarMenuItem> mMenuItems;
        private ApplicationBarItemList<IApplicationBarIconButton> mButtons;

        public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;

        public ApplicationBar()
        {
            this.mButtons = new ApplicationBarItemList<IApplicationBarIconButton>(true);
            this.mButtons.MAX_ITEMS = MaxIconButtons;
            this.mMenuItems = new ApplicationBarItemList<IApplicationBarMenuItem>(false);
            this.mMenuItems.MAX_ITEMS = MaxMenuItems;
        }

        private static bool IsInDesignMode()
        {
            if ((Application.Current != null) && (Application.Current.GetType() != typeof(Application)))
            {
                return DesignerProperties.IsInDesignTool;
            }
            return true;
        }

        public static DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color),
            typeof(ApplicationBar), new PropertyMetadata(Colors.Black));

        public Color BackgroundColor
        {
            get
            {
                return (Color)GetValue(BackgroundColorProperty);
            }
            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }

        public IList Buttons
        {
            get
            {
                return this.mButtons;
            }
        }

        public static DependencyProperty ForegroundColorProperty = DependencyProperty.Register("ForegroundColor", typeof(Color),
            typeof(ApplicationBar), new PropertyMetadata(Colors.White));

        public Color ForegroundColor
        {
            get
            {
                return (Color)GetValue(ForegroundColorProperty);
            }
            set
            {
                SetValue(ForegroundColorProperty, value);
            }
        }

        public static DependencyProperty IsMenuEnabledProperty = DependencyProperty.Register("IsMenuEnabled", typeof(bool),
            typeof(ApplicationBar), new PropertyMetadata(true));

        public bool IsMenuEnabled
        {
            get
            {
                return (bool)GetValue(IsMenuEnabledProperty);
            }
            set
            {
                SetValue(IsMenuEnabledProperty, value);
            }
        }

        public static DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool),
            typeof(ApplicationBar), new PropertyMetadata(true));

        public bool IsVisible
        {
            get
            {
                return (bool)GetValue(IsVisibleProperty);
            }
            set
            {
                SetValue(IsVisibleProperty, value);
            }
        }

        public IList MenuItems
        {
            get
            {
                return this.mMenuItems;
            }
        }

        public static DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(double),
           typeof(ApplicationBar), new PropertyMetadata(1.0));

        public double Opacity
        {
            get
            {
                return (double)GetValue(OpacityProperty);
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > 1)
                {
                    value = 1;
                }
                SetValue(OpacityProperty, value);
            }
        }
    }

}
