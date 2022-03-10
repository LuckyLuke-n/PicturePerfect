using Avalonia.Media.Imaging;
using ImageMagick;
using System.IO;

namespace PicturePerfect.Models
{
    /// <summary>
    /// MackickImageExtensions according to https://github.com/dlemstra/Magick.NET/issues/543.
    /// </summary>
    public static class MagickImageExtensions
    {
        /// <summary>
        /// Convert the image magick object to type bitmap.
        /// </summary>
        /// <param name="imageMagick"></param>
        /// <returns>Returns a bitmap image object.</returns>
        public static Bitmap ToBitmap(this MagickImage imageMagick)
        {
            imageMagick.Format = MagickFormat.Bmp;

            MemoryStream memStream = new MemoryStream();
            imageMagick.Write(memStream);
            memStream.Position = 0;

            /* Do not dispose the memStream, the bitmap owns it. */
            var bitmap = new Bitmap(memStream);

            return bitmap;
        }
    }
}
