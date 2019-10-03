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
using System.IO;
using System.Windows.Browser;

namespace Microsoft.Xna.Framework.Framework.Media
{
    public sealed class Album : IEquatable<Album>, IDisposable
    {
        internal string dirPath;

        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool Equals(Album other)
        {
            if (other == null)
            {
                return false;
            }
            return dirPath == other.dirPath;
        }

        public override bool Equals(object obj)
        {
            Album other = obj as Album;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return dirPath.GetHashCode();
        }
        
        public static bool operator ==(Album first, Album second)
        {
            //http://www.cnblogs.com/cruisoring/archive/2009/11/04/1595958.html
            if ((object)first == null && (object)second == null)
            {
                return true;
            }
            else if ((object)first != null & (object)second != null)
            {
                return first.dirPath == second.dirPath;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(Album first, Album second)
        {
            return !(first == second);
        }

        public override string ToString()
        {
            return dirPath;
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

        public SongCollection Songs
        {
            get
            {
                SongCollection songs = new SongCollection();
                foreach (var filepath in Directory.EnumerateFiles(dirPath,"*.mp3"))
                {
                    string filename = System.IO.Path.GetFileName(filepath);
                    Uri fileUri = new Uri(filepath,UriKind.Absolute);
                    string myMusicDir = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)+@"\";
                    //最后必须加"\"，否则计算出来的相对路径不对，估计不加\是被当成了文件
                    Uri uriMusicDir = new Uri(myMusicDir, UriKind.Absolute);
                    string relPath = uriMusicDir.MakeRelativeUri(fileUri).ToString();//计算相对路径
                    //因为MakeRelativeUri计算出来的路径是被UrlEncode的，所以传递之前需要再UrlDecode
                    Uri relUri = new Uri(HttpUtility.UrlDecode(relPath), UriKind.Relative);

                    Song song = Song.FromUri(filename, relUri);
                    song.Album = this;
                    songs.Add(song);
                }
                return songs;
            }
        }
    }
}
