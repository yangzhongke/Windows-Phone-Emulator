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
    public sealed class AlbumCollection : IEnumerable<Album>, IEnumerable, IDisposable
    {
        private List<Album> list = new List<Album>();
        public void Dispose()
        {
            IsDisposed = true;
        }

        internal void Add(Album album)
        {
            list.Add(album);
        }
        
        public IEnumerator<Album> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

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

        public Album this[int index]
        {
            get
            {
                return list[index];
            }
        }
    }
}
