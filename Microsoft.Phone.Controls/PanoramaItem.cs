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
    public class PanoramaItem : ContentControl
    {
        // Fields
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(PanoramaItem), null);
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(PanoramaItem), null);
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PanoramaItem), new PropertyMetadata(Orientation.Vertical, new PropertyChangedCallback(PanoramaItem.OnOrientationChanged)));

        // Methods
        public PanoramaItem()
        {
            base.DefaultStyleKey = typeof(PanoramaItem);
        }

        private static void OnOrientationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PanoramaItem reference = (PanoramaItem)obj;
            reference.InvalidateMeasure();
            FrameworkElement parent = VisualTreeHelper.GetParent(reference) as FrameworkElement;
            if (parent != null)
            {
                parent.InvalidateMeasure();
            }
        }

        // Properties
        public object Header
        {
            get
            {
                return base.GetValue(HeaderProperty);
            }
            set
            {
                base.SetValue(HeaderProperty, value);
            }
        }

        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(HeaderTemplateProperty);
            }
            set
            {
                base.SetValue(HeaderTemplateProperty, value);
            }
        }

        internal int ItemWidth { get; set; }

        public Orientation Orientation
        {
            get
            {
                return (Orientation)base.GetValue(OrientationProperty);
            }
            set
            {
                base.SetValue(OrientationProperty, value);
            }
        }

        internal Panorama Owner { get; set; }

        internal int StartPosition { get; set; }
    }
}
