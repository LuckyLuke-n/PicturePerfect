using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace PicturePerfect.Models
{
    /// <summary>
    /// Class to convert JPG images to bitmaps for use in Avalonia UI image source bindings.
    /// </summary>
    public class BitmapValueConverter
    {
        public BitmapValueConverter()
        {

        }

        /// <summary>
        /// Method to convert a given path to a image into bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the bitmap image.</returns>
        public static Bitmap Convert(string path)
        {
            Uri uri = new(path, UriKind.RelativeOrAbsolute);
            string scheme = uri.IsAbsoluteUri ? uri.Scheme : "file";

            switch (scheme)
            {
                case "file":
                    return new Bitmap(path);

                default:
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    return new Bitmap(assets.Open(uri));
            }
        }
    }
}
