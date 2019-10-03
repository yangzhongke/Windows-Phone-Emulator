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

namespace Microsoft.Phone.Controls.Primitives
{
    [TemplateVisualState(Name = "Unselected", GroupName = "SelectionStates"), TemplateVisualState(Name = "Selected", GroupName = "SelectionStates")]
    public class PivotHeaderItem : ContentControl
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PivotHeaderItem), new PropertyMetadata(false, new PropertyChangedCallback(PivotHeaderItem.OnIsSelectedPropertyChanged)));
        private const string SelectedState = "Selected";
        private const string SelectionStatesGroup = "SelectionStates";
        private const string UnselectedState = "Unselected";

        // Methods
        public PivotHeaderItem()
        {
            base.DefaultStyleKey = typeof(PivotHeaderItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.UpdateVisualStates(false);
        }

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotHeaderItem item = d as PivotHeaderItem;
            if (item.ParentHeadersControl != null)
            {
                item.ParentHeadersControl.NotifyHeaderItemSelected(item, (bool)e.NewValue);
                item.UpdateVisualStates(true);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if ((e != null) && !e.Handled)
            {
                e.Handled = true;
                if (this.ParentHeadersControl != null)
                {
                    this.ParentHeadersControl.OnHeaderItemClicked(this);
                }
            }
        }

        internal void RestoreVisualStates()
        {
            this.UpdateVisualStates(false);
        }

        private void UpdateVisualStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, this.IsSelected ? "Selected" : "Unselected", useTransitions);
        }

        internal void UpdateVisualStateToUnselected()
        {
            VisualStateManager.GoToState(this, "Unselected", false);
        }

        // Properties
        public bool IsSelected
        {
            get
            {
                return (bool)base.GetValue(IsSelectedProperty);
            }
            set
            {
                base.SetValue(IsSelectedProperty, value);
            }
        }

        internal object Item
        {
            get;
            set;
        }

        internal PivotHeadersControl ParentHeadersControl
        {
            get;
            set;
        }
    }
}
