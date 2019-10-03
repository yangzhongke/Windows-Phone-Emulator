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
    public sealed class SongCollection : IEnumerable<Song>, IEnumerable, IDisposable
    {
        private List<Song> list = new List<Song>();

        internal void Add(Song song)
        {
            list.Add(song);
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public IEnumerator<Song> GetEnumerator()
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

        public Song this[int index]
        {
            get
            {
                return list[index];
            }
        }
    }
}
