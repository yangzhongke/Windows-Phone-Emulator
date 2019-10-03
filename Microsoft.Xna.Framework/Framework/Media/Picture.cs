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
using System.Windows.Media.Imaging;
using Microsoft.Phone.Internals;
using System.Runtime.InteropServices.Automation;
using FluxJpeg.Core.Decoder;

namespace Microsoft.Xna.Framework.Framework.Media
{
    public sealed class Picture : IEquatable<Picture>, IDisposable
    {
        internal string filepath;

        internal Picture(string filepath, PictureAlbum album)
        {
            this.filepath = filepath;
            this.Album = album;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool Equals(Picture other)
        {
            if (other == null)
            {
                return false;
            }
            return other.filepath == this.filepath;
        }

        public override bool Equals(object obj)
        {
            Picture other = obj as Picture;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return filepath.GetHashCode();
        }

        public Stream GetImage()
        {
            return File.OpenRead(filepath);
        }

        public Stream GetThumbnail()
        {
            double thumbnailWidth, thumbnailHeight;
            WriteableBitmap wb = MediaHelper.CreateThumbnailBitmap(GetImage(), 48, 
                out thumbnailWidth, out thumbnailHeight);
            byte[] buffer = AppHelper.BitMapToByte(wb);
            return new MemoryStream(buffer);
        }

        public static bool operator ==(Picture first, Picture second)
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

        public static bool operator !=(Picture first, Picture second)
        {
            return !(first==second);
        }

        public override string ToString()
        {
            return filepath;
        }

        public PictureAlbum Album
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get
            {
                return File.GetLastWriteTime(filepath);
            }
        }

        public int Height
        {
            get
            {
                using (Stream stream = File.OpenRead(filepath))
                {
                    JpegDecoder jpeg = new JpegDecoder(stream);
                    return jpeg.Decode().Image.Height;
                }       
            }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        //todo：需要真机验证name中是否包含后缀
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(filepath);
            }
        }

        public int Width
        {
            get
            {
                using (Stream stream = File.OpenRead(filepath))
                {
                    JpegDecoder jpeg = new JpegDecoder(stream);
                    return jpeg.Decode().Image.Width;
                } 
            }
        }
    }
}
