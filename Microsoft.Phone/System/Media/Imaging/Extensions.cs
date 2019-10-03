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
using System.Security;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Internals;

namespace System.Media.Imaging
{
    public static class Extensions
    {
        [SecuritySafeCritical]
        public static void LoadJpeg(this WriteableBitmap bitmap, Stream sourceStream)
        {
            bitmap.SetSource(sourceStream);
        }
        [SecuritySafeCritical]
        // orientation is not used at the moment
        public static void SaveJpeg(this WriteableBitmap bitmap, Stream targetStream, int targetWidth, int targetHeight, int orientation, int quality)
        {
            //要渲染的Image
            Image image = new Image();
            image.Source = bitmap;

            //变型后的WriteableBitmap
            WriteableBitmap newBitmap = new WriteableBitmap(targetWidth,targetHeight);
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = targetWidth*1.0 / bitmap.PixelWidth;
            transform.ScaleY = targetHeight * 1.0 / bitmap.PixelHeight;
            //image通过transform形变后画到newBitmap上
            newBitmap.Render(image, transform);
            newBitmap.Invalidate();

            var data = AppHelper.BitMapToByte(newBitmap);
            using (MemoryStream ms = new MemoryStream(data))
            {
                ms.CopyTo(targetStream);
            }            
        }
    }
}
