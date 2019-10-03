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

namespace Microsoft.Xna.Framework.Framework.Media
{
    public sealed class Song : IEquatable<Song>, IDisposable
    {
        internal string filepath;

        public void Dispose()
        {
            IsDisposed = true;
        }

        public Album Album
        {
            get;
            internal set;
        }

        public bool Equals(Song other)
        {
            if (other == null)
            {
                return false;
            }
            return this.filepath.Equals(other.filepath);
        }

        public override bool Equals(object obj)
        {
            Song other = obj as Song;
            return Equals(other);
        }

        public static Song FromUri(string name, Uri uri)
        {
            string myMusicDir = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            Song song = new Song();
            song.filepath = System.IO.Path.Combine(myMusicDir,uri.ToString());
            return song;
        }

        public override int GetHashCode()
        {
            return filepath.GetHashCode();
        }

        public static bool operator ==(Song first, Song second)
        {
            if ((object)first == null && (object)second == null)
            {
                return true;
            }
            else if ((object)first != null & (object)second != null)
            {
                return first.filepath == second.filepath;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(Song first, Song second)
        {
            return !(first==second);
        }

        public override string ToString()
        {
            return System.IO.Path.GetFileName(filepath);
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
                return System.IO.Path.GetFileNameWithoutExtension(filepath);
            } 
        }

    }
}
