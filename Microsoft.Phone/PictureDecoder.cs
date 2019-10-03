namespace Microsoft.Phone
{
    using System;
    using System.IO;
    using System.Security;
    using System.Windows.Media.Imaging;

    /// <summary>An application will use this type to decode a JPEG file into a WriteableBitmap.</summary>
    [SecuritySafeCritical]
    public static class PictureDecoder
    {
        /// <summary>This method decodes a JPEG into a WriteableBitmap object.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Windows.Media.Imaging.WriteableBitmap" />
        /// .</returns>
        /// <param name="source">The image data stream.</param>
        [SecuritySafeCritical]
        public static WriteableBitmap DecodeJpeg(Stream source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return DecodeJpegInternal(source, 0, 0);
        }

        /// <summary>This method decodes a JPEG into a WriteableBitmap object. Also, this method has parameters for setting the maximum height and width of the output.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Windows.Media.Imaging.WriteableBitmap" />
        /// . </returns>
        /// <param name="source">The image data stream.</param>
        /// <param name="maxPixelWidth">The maximum width of the output.</param>
        /// <param name="maxPixelHeight">The maximum height of the output.</param>
        [SecuritySafeCritical]
        public static WriteableBitmap DecodeJpeg(Stream source, int maxPixelWidth, int maxPixelHeight)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (maxPixelWidth <= 0)
            {
                throw new ArgumentOutOfRangeException("maxPixelWidth");
            }
            if (maxPixelHeight <= 0)
            {
                throw new ArgumentOutOfRangeException("maxPixelHeight");
            }
            return DecodeJpegInternal(source, (uint) maxPixelHeight, (uint) maxPixelHeight);
        }

        [SecuritySafeCritical]
        private static WriteableBitmap DecodeJpegInternal(Stream source, uint maxPixelWidth, uint maxPixelHeight)
        {
            StreamHelper helper = new StreamHelper(source);
            uint widthNeeded = 0;
            uint heightNeeded = 0;
            string tempFile = helper.GetTempFile();
            WriteableBitmap bitmap  = new WriteableBitmap((int) widthNeeded, (int) heightNeeded);
            Stream memStream = new MemoryStream();
            helper.WriteToStream(memStream);
            memStream.Position = 0;
            bitmap.SetSource(memStream);
            return bitmap;
        }
    }
}

