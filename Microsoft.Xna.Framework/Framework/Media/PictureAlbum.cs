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
using System.Linq;
using System.IO;
using Microsoft.Phone.Internals;

namespace Microsoft.Xna.Framework.Framework.Media
{
    public sealed class PictureAlbum : IEquatable<PictureAlbum>, IDisposable
    {
        private string dirPath;

        public PictureAlbum(string dirPath, PictureAlbum parent)
        {
            this.dirPath = dirPath;
            this.Parent = parent;
        }


        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool Equals(PictureAlbum other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Name == this.Name && other.Parent == this.Parent;
        }

        public override bool Equals(object obj)
        {
            PictureAlbum other = obj as PictureAlbum;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Parent.GetHashCode()+this.Name.GetHashCode();
        }

        public static bool operator ==(PictureAlbum first, PictureAlbum second)
        {
            if ((object)first == null && (object)second == null)
            {
                return true;
            }
            else if ((object)first != null && (object)second != null)
            {
                var pathFirst = MediaHelper.GetPathStrings(first);
                var pathSecond = MediaHelper.GetPathStrings(second);
                return pathFirst.SequenceEqual(pathSecond);
            }
            else//一个为null，一个不为
            {
                return false;
            }
        }

        public static bool operator !=(PictureAlbum first, PictureAlbum second)
        {
            return !(first == second);
        }

        public override string ToString()
        {
            return Name;
        }

        public PictureAlbumCollection Albums
        {
            get
            {
                PictureAlbumCollection albumCollection = new PictureAlbumCollection();
                var childDirs = Directory.EnumerateDirectories(dirPath);
                foreach (var childDir in childDirs)
                {
                    albumCollection.Add(new PictureAlbum(childDir,this));
                }
                return albumCollection;
            }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetDirectoryName(dirPath);
            }
        }

        public PictureAlbum Parent
        {
            get;
            private set;
        }

        public PictureCollection Pictures
        {
            get
            {
                PictureCollection collection = new PictureCollection();
                var files = Directory.EnumerateFiles(dirPath);
                foreach (var filename in files)
                {
                    if (AppHelper.IsImgFile(filename))
                    {
                        collection.Add(new Picture(filename, this));
                    }                    
                }
                return collection;
            }
        }
    }
}
