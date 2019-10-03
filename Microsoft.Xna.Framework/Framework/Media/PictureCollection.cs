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
using System.Collections;

namespace Microsoft.Xna.Framework.Framework.Media
{
    public sealed class PictureCollection : IEnumerable<Picture>, IEnumerable, IDisposable
    {
        private List<Picture> list = new List<Picture>();

        internal void Add(Picture picture)
        {
            list.Add(picture);
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public IEnumerator<Picture> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        // Properties
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public Picture this[int index]
        {
            get
            {
                return list[index];
            }
        }
    }
}
