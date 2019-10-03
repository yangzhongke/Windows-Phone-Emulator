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
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;

namespace Microsoft.Xna.Framework.Framework.Media
{
    internal class MediaHelper
    {
        /// <summary>
        /// 得到album父节点一直到根的路径
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public static IEnumerable<PictureAlbum> GetPath(PictureAlbum album)
        {
            if (album.Parent != null)
            {
                yield return album.Parent;
                foreach (var item in GetPath(album.Parent))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 得到album父节点一直到根的路径的字符串表示形式。
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetPathStrings(PictureAlbum album)
        {
            return from item in GetPath(album)
                   select item.Name;
        }

        /// <summary>
        /// 创建stream代表图片的缩略图
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Image CreateThumbnailImage(Stream stream, int width)
        {
            double thumbnailWidth, thumbnailHeight;
            WriteableBitmap wb = CreateThumbnailBitmap(stream,width,out thumbnailWidth,out thumbnailHeight);
            Image thumbnail = new Image();
            thumbnail.Width = thumbnailWidth;
            thumbnail.Height = thumbnailHeight;
            thumbnail.Source = wb;
            return thumbnail;
        }

        /// <summary>
        /// 创建stream代表图片的缩略图，
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="width">建议的宽度</param>
        /// <param name="thumbnailWidth">最终缩略图的宽度</param>
        /// <param name="thumbnailHeight">最终缩略图的高度</param>
        /// <returns></returns>
        public static WriteableBitmap CreateThumbnailBitmap(Stream stream, int width,
            out double thumbnailWidth, out double thumbnailHeight)
        {
            BitmapImage bi = new BitmapImage();
            bi.SetSource(stream);
            double cx = width;
            double cy = bi.PixelHeight * (cx / bi.PixelWidth);
            Image image = new Image();
            image.Source = bi;
            WriteableBitmap wb1 = new WriteableBitmap((int)cx, (int)cy);
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = cx / bi.PixelWidth;
            transform.ScaleY = cy / bi.PixelHeight;
            wb1.Render(image, transform);
            wb1.Invalidate();
            WriteableBitmap wb2 = new WriteableBitmap((int)cx, (int)cy);
            for (int i = 0; i < wb2.Pixels.Length; i++)
            { 
                wb2.Pixels[i] = wb1.Pixels[i]; 
            }
            wb2.Invalidate();
            thumbnailHeight = cy;
            thumbnailWidth = cx;
            return wb2;
        }
    }
}
