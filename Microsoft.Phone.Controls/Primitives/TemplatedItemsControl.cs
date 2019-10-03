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
using System.Collections.Generic;

namespace Microsoft.Phone.Controls.Primitives
{
    public class TemplatedItemsControl<T> : ItemsControl where T : FrameworkElement, new()
    {
        // Fields
        private readonly Dictionary<T, object> _containerToItem;
        private readonly Dictionary<object, T> _itemToContainer;
        public static readonly DependencyProperty ItemContainerStyleProperty;

        // Methods
        static TemplatedItemsControl()
        {
            TemplatedItemsControl<T>.ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(TemplatedItemsControl<T>), null);
        }

        public TemplatedItemsControl()
        {
            this._itemToContainer = new Dictionary<object, T>();
            this._containerToItem = new Dictionary<T, object>();
        }

        protected virtual void ApplyItemContainerStyle(DependencyObject container)
        {
            T local = container as T;
            if ((local != null) && (local.ReadLocalValue(FrameworkElement.StyleProperty) == DependencyProperty.UnsetValue))
            {
                Style itemContainerStyle = this.ItemContainerStyle;
                if (itemContainerStyle != null)
                {
                    local.Style = itemContainerStyle;
                }
                else
                {
                    local.ClearValue(FrameworkElement.StyleProperty);
                }
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            this._itemToContainer.Remove(item);
            this._containerToItem.Remove((T)element);
        }

        protected T GetContainer(object item)
        {
            T local = default(T);
            if (item != null)
            {
                this._itemToContainer.TryGetValue(item, out local);
            }
            return local;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            T container = Activator.CreateInstance<T>();
            this.ApplyItemContainerStyle(container);
            return container;
        }

        protected object GetItem(T container)
        {
            object obj2 = null;
            if (container != null)
            {
                this._containerToItem.TryGetValue(container, out obj2);
            }
            return obj2;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is T);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            this.ApplyItemContainerStyle(element);
            base.PrepareContainerForItemOverride(element, item);
            this._itemToContainer[item] = (T)element;
            this._containerToItem[(T)element] = item;
        }

        // Properties
        public Style ItemContainerStyle
        {
            get
            {
                return (base.GetValue(TemplatedItemsControl<T>.ItemContainerStyleProperty) as Style);
            }
            set
            {
                base.SetValue(TemplatedItemsControl<T>.ItemContainerStyleProperty, value);
            }
        }
    }
}
