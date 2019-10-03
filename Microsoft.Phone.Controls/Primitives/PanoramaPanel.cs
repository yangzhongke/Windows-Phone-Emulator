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
    public class PanoramaPanel : Panel
    {
        // Fields
        private readonly List<ItemStop> _itemStops = new List<ItemStop>();
        private Panorama _owner;
        private PanoramaItem _selectedItem;
        private readonly List<PanoramaItem> _visibleChildren = new List<PanoramaItem>();
        private const int SnapThresholdDivisor = 3;

        // Methods
        public PanoramaPanel()
        {
            base.SizeChanged += new SizeChangedEventHandler(this.PanoramaPanel_SizeChanged);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._itemStops.Clear();
            double num = 0.0;
            Rect finalRect = new Rect(0.0, 0.0, 0.0, finalSize.Height);
            for (int i = 0; i < this._visibleChildren.Count; i++)
            {
                PanoramaItem item = this._visibleChildren[i];
                finalRect.X = item.StartPosition = (int)num;
                this._itemStops.Add(new ItemStop(item, i, item.StartPosition));
                if (item.Orientation == Orientation.Vertical)
                {
                    finalRect.Width = this.Owner.AdjustedViewportWidth;
                }
                else
                {
                    finalRect.Width = Math.Max((double)this.Owner.AdjustedViewportWidth, item.DesiredSize.Width);
                    if (finalRect.Width > this.Owner.AdjustedViewportWidth)
                    {
                        this._itemStops.Add(new ItemStop(item, i, (item.StartPosition + ((int)finalRect.Width)) - this.Owner.AdjustedViewportWidth));
                    }
                }
                item.ItemWidth = (int)finalRect.Width;
                item.Arrange(finalRect);
                num += finalRect.Width;
            }
            this.Owner.RequestAdjustSelection();
            return finalSize;
        }

        private void FindOwner()
        {
            Panorama panorama = null;
            FrameworkElement reference = this;
            do
            {
                reference = (FrameworkElement)VisualTreeHelper.GetParent(reference);
                panorama = reference as Panorama;
            }
            while ((reference != null) && (panorama == null));
            this.Owner = panorama;
        }

        private static int GetBestIndex(int n0, int n1, int n2)
        {
            if (n0 >= 0)
            {
                return n0;
            }
            if (n1 >= 0)
            {
                return n1;
            }
            if (n2 < 0)
            {
                throw new InvalidOperationException("No best index.");
            }
            return n2;
        }

        private int GetDefaultItemIndex()
        {
            PanoramaItem defaultItemContainer = this.Owner.GetDefaultItemContainer();
            int num = (defaultItemContainer != null) ? base.Children.IndexOf(defaultItemContainer) : 0;
            if (num < 0)
            {
                num = 0;
            }
            return num;
        }

        private void GetItemsInView(int offset, int viewportWidth, out int leftIndex, out int leftInView, out int centerIndex, out int rightIndex, out int rightInView)
        {
            int num5;
            int num6;
            int num7;
            rightInView = num5 = -1;
            rightIndex = num6 = num5;
            centerIndex = num7 = num6;
            leftIndex = leftInView = num7;
            int count = this.VisibleChildren.Count;
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    PanoramaItem item = this._visibleChildren[i];
                    int num3 = item.StartPosition + offset;
                    int num4 = (num3 + item.ItemWidth) - 1;
                    if ((num3 <= 0) && (num4 >= 0))
                    {
                        leftIndex = i;
                        leftInView = Math.Min(viewportWidth, item.ItemWidth + num3);
                    }
                    if ((num3 < viewportWidth) && (num4 >= viewportWidth))
                    {
                        rightIndex = i;
                        rightInView = Math.Min(viewportWidth, viewportWidth - num3);
                    }
                    if ((num3 > 0) && (num4 < viewportWidth))
                    {
                        centerIndex = i;
                    }
                    if ((i == 0) && (leftInView == -1))
                    {
                        leftInView = num3;
                    }
                    if ((i == (count - 1)) && (rightInView == -1))
                    {
                        rightInView = (viewportWidth - num4) - 1;
                    }
                }
            }
        }

        private static int GetLeftAlignedOffset(PanoramaItem movingTo, int viewportWidth)
        {
            return -movingTo.StartPosition;
        }

        private static int GetRightAlignedOffset(PanoramaItem movingTo, int viewportWidth)
        {
            if (movingTo.Orientation != Orientation.Vertical)
            {
                return (((-movingTo.ItemWidth + viewportWidth) - movingTo.StartPosition) - 0x30);
            }
            return -movingTo.StartPosition;
        }

        internal void GetSnapOffset(int offset, int viewportWidth, int direction, out int snapTo, out int newDirection, out PanoramaItem newSelection, out bool wraparound)
        {
            int num = viewportWidth / 3;
            wraparound = false;
            snapTo = offset;
            newDirection = direction;
            newSelection = this._selectedItem;
            if (this.VisibleChildren.Count != 0)
            {
                int num2;
                int num3;
                int num4;
                int num5;
                int num6;
                foreach (ItemStop stop in this._itemStops)
                {
                    if (stop.Position == -offset)
                    {
                        newSelection = stop.Item;
                        return;
                    }
                }
                this.GetItemsInView(offset, viewportWidth, out num2, out num3, out num4, out num5, out num6);
                if ((num2 != num5) || (num2 == -1))
                {
                    int num7;
                    bool flag = false;
                    if (num2 == -1)
                    {
                        flag = true;
                        num2 = this._visibleChildren.Count - 1;
                    }
                    bool flag2 = false;
                    if (num5 == -1)
                    {
                        flag2 = true;
                        num5 = 0;
                    }
                    if (direction < 0)
                    {
                        if (num6 > num)
                        {
                            num7 = GetBestIndex(num4, num5, num2);
                            newDirection = -1;
                        }
                        else
                        {
                            num7 = GetBestIndex(num2, num4, num5);
                            newDirection = 1;
                        }
                    }
                    else if (direction > 0)
                    {
                        if (num3 > num)
                        {
                            num7 = GetBestIndex(num2, num4, num5);
                            newDirection = 1;
                        }
                        else
                        {
                            num7 = GetBestIndex(num4, num5, num2);
                            newDirection = -1;
                        }
                    }
                    else if (num4 != -1)
                    {
                        num7 = num4;
                        newDirection = -1;
                    }
                    else if (num3 > num6)
                    {
                        num7 = num2;
                        newDirection = -1;
                    }
                    else
                    {
                        num7 = num5;
                        newDirection = 1;
                    }
                    this._selectedItem = this._visibleChildren[num7];
                    if (newDirection < 0)
                    {
                        snapTo = GetLeftAlignedOffset(this._selectedItem, viewportWidth);
                    }
                    else
                    {
                        snapTo = GetRightAlignedOffset(this._selectedItem, viewportWidth);
                    }
                    newSelection = this._selectedItem;
                    if (((num7 == num2) && flag) || ((num7 == num5) && flag2))
                    {
                        wraparound = true;
                    }
                }
                else
                {
                    newSelection = this._selectedItem = this._visibleChildren[num2];
                }
            }
        }

        internal void GetStops(int offset, int totalWidth, out ItemStop previous, out ItemStop current, out ItemStop next)
        {
            int num2;
            int num3;
            ItemStop stop2;
            int num = num2 = num3 = -1;
            previous = (ItemStop)(stop2 = null);
            next = current = stop2;
            if (this.VisibleChildren.Count != 0)
            {
                int num4 = -offset % totalWidth;
                int num5 = 0;
                foreach (ItemStop stop in this._itemStops)
                {
                    if (stop.Position < num4)
                    {
                        num = num5;
                    }
                    else
                    {
                        if (stop.Position > num4)
                        {
                            num3 = num5;
                            break;
                        }
                        if (stop.Position == num4)
                        {
                            num2 = num5;
                        }
                    }
                    num5++;
                }
                if (num == -1)
                {
                    num = this._itemStops.Count - 1;
                }
                if (num3 == -1)
                {
                    num3 = 0;
                }
                previous = this._itemStops[num];
                current = (num2 != -1) ? this._itemStops[num2] : null;
                next = this._itemStops[num3];
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.FindOwner();
            int defaultItemIndex = this.GetDefaultItemIndex();
            Size size = new Size(0.0, availableSize.Height);
            int adjustedViewportWidth = this.Owner.AdjustedViewportWidth;
            int num3 = (int)Math.Min(availableSize.Height, (double)this.Owner.ViewportHeight);
            Size size2 = new Size((double)adjustedViewportWidth, (double)num3);
            Size size3 = new Size(double.PositiveInfinity, (double)num3);
            int count = base.Children.Count;
            this._visibleChildren.Clear();
            for (int i = 0; i < count; i++)
            {
                int num6 = (i + defaultItemIndex) % count;
                PanoramaItem item = (PanoramaItem)base.Children[num6];
                if (item.Visibility == Visibility.Visible)
                {
                    this._visibleChildren.Add(item);
                    item.Measure((item.Orientation == Orientation.Vertical) ? size2 : size3);
                    if (item.Orientation == Orientation.Vertical)
                    {
                        size.Width += adjustedViewportWidth;
                    }
                    else
                    {
                        size.Width += Math.Max((double)adjustedViewportWidth, item.DesiredSize.Width);
                    }
                }
            }
            return size;
        }

        internal void NotifyDefaultItemChanged()
        {
            base.InvalidateMeasure();
            base.InvalidateArrange();
            base.UpdateLayout();
        }

        private void PanoramaPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Owner.ItemsWidth = (int)e.NewSize.Width;
        }

        internal void ResetItemPositions()
        {
            foreach (PanoramaItem item in this.VisibleChildren)
            {
                item.RenderTransform = null;
            }
        }

        internal void ShowFirstItemOnRight()
        {
            this.ResetItemPositions();
            if (this.VisibleChildren.Count > 0)
            {
                PanoramaItem item = this.VisibleChildren[0];
                item.RenderTransform = new TranslateTransform { X = base.ActualWidth };
            }
        }

        internal void ShowLastItemOnLeft()
        {
            this.ResetItemPositions();
            if (this.VisibleChildren.Count > 0)
            {
                PanoramaItem item = this.VisibleChildren[this.VisibleChildren.Count - 1];
                item.RenderTransform = new TranslateTransform { X = -base.ActualWidth };
            }
        }

        // Properties
        private Panorama Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                if (this._owner != value)
                {
                    if (this._owner != null)
                    {
                        this._owner.Panel = null;
                    }
                    this._owner = value;
                    if (this._owner != null)
                    {
                        this._owner.Panel = this;
                    }
                }
            }
        }

        internal IList<PanoramaItem> VisibleChildren
        {
            get
            {
                return this._visibleChildren;
            }
        }

        // Nested Types
        internal class ItemStop
        {
            // Methods
            public ItemStop(PanoramaItem item, int index, int position)
            {
                this.Item = item;
                this.Index = index;
                this.Position = position;
            }

            // Properties
            public int Index { get; private set; }

            public PanoramaItem Item { get; private set; }

            public int Position { get; private set; }
        }
    }
}
