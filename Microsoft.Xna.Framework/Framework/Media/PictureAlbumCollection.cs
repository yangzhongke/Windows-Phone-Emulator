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
    public sealed class PictureAlbumCollection : IEnumerable<PictureAlbum>, IEnumerable, IDisposable
    {
        private List<PictureAlbum> list = new List<PictureAlbum>();

        internal void Add(PictureAlbum pictureAlbum)
        {
            list.Add(pictureAlbum);
        }

        // Methods
        public void Dispose()
        {
            IsDisposed = true;
        }

        public IEnumerator<PictureAlbum> GetEnumerator()
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

        public PictureAlbum this[int index]
        {
            get
            {
                return list[index];
            }
        }
    }
}
