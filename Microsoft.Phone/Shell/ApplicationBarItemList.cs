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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Microsoft.Phone.Shell
{
    internal class ApplicationBarItemList<TApplicationBarItem> : INotifyCollectionChanged ,IList, ICollection, IList<TApplicationBarItem>, ICollection<TApplicationBarItem>, IEnumerable<TApplicationBarItem>, IEnumerable where TApplicationBarItem : IApplicationBarMenuItem
    {
        // Fields
        internal int MAX_ITEMS;
        private List<TApplicationBarItem> mList = new List<TApplicationBarItem>();        

        // Methods
        static ApplicationBarItemList()
        {
        }

        internal ApplicationBarItemList(bool isButtonList)
        {
            
        }

        public void Add(TApplicationBarItem item)
        {
            int count = this.mList.Count;
            this.Insert(count, item);
        }

        public void Clear()
        {
            while (this.mList.Count > 0)
            {
                this.RemoveAt(0);                
            }
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(TApplicationBarItem item)
        {
            return this.mList.Contains(item);
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(this.mList.ToArray(), array, this.mList.Count);
        }

        public void CopyTo(TApplicationBarItem[] array, int arrayIndex)
        {
            this.mList.CopyTo(array, arrayIndex);
        }


        public IEnumerator<TApplicationBarItem> GetEnumerator()
        {
            return this.mList.GetEnumerator();
        }

        public int IndexOf(TApplicationBarItem item)
        {
            return this.mList.IndexOf(item);
        }

        public void Insert(int index, TApplicationBarItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if ((index < 0) || (index > this.mList.Count))
            {
                throw new ArgumentOutOfRangeException("item");
            }
            if (this.mList.Count == this.MAX_ITEMS)
            {
                throw new InvalidOperationException("Too many items in list");
            }
            if (this.mList.Contains(item))
            {
                throw new InvalidOperationException("Cannot add same item to multiple lists at once or to same list twice");
            }
            this.mList.Insert(index, item);
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Remove(TApplicationBarItem item)
        {
            int index = this.mList.IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            this.RemoveAt(index);
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return true;
        }

        public void RemoveAt(int index)
        {
            mList.RemoveAt(index);
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.mList.GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(value is TApplicationBarItem))
            {
                throw new ArgumentException("This list does not support that type");
            }
            TApplicationBarItem item = (TApplicationBarItem)value;
            this.Add(item);
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return this.mList.Count;
        }

        bool IList.Contains(object value)
        {
            if ((value == null) || !(value is TApplicationBarItem))
            {
                return false;
            }
            TApplicationBarItem item = (TApplicationBarItem)value;
            return this.Contains(item);
        }

        int IList.IndexOf(object value)
        {
            if ((value == null) || !(value is TApplicationBarItem))
            {
                return -1;
            }
            TApplicationBarItem item = (TApplicationBarItem)value;
            return this.IndexOf(item);
        }

        void IList.Insert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(value is TApplicationBarItem))
            {
                throw new ArgumentException("This list does not support that type");
            }
            TApplicationBarItem item = (TApplicationBarItem)value;
            this.Insert(index, item);
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void IList.Remove(object value)
        {
            if ((value != null) && (value is TApplicationBarItem))
            {
                TApplicationBarItem item = (TApplicationBarItem)value;
                this.Remove(item);
                FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        // Properties
        public int Count
        {
            get
            {
                return this.mList.Count;
            }
        }


        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public TApplicationBarItem this[int index]
        {
            get
            {
                return this.mList[index];
            }
            set
            {
                this.RemoveAt(index);
                this.Insert(index, value);
            }
        }


        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (TApplicationBarItem)value;
            }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #region INotifyCollectionChanged 成员

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void FireCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        #endregion
    }
}
