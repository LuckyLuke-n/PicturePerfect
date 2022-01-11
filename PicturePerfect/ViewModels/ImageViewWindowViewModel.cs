using Avalonia.Media.Imaging;
using PicturePerfect.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.ViewModels
{
    public class ImageViewWindowViewModel : ViewModelBase
    {
        #region Color properties
        public static string DarkColor => ThisApplication.DarkColor;
        public static string MediumColor => ThisApplication.MediumColor;
        public static string LightColor => ThisApplication.LightColor;
        public static string LightFontColor => ThisApplication.LightFontColor;
        public static string DarkContrastColor => ThisApplication.DarkContrastColor;
        public static Bitmap ImageNo1 { get; private set; } = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/test/P5140045_Stockerpel.jpg");
        #endregion

        private int imageId;
        



        /// <summary>
        /// Get or set the image id for the image to be displayed in the image viewer.
        /// </summary>
        public int ImageId
        {
            get { return imageId; }
            set { this.RaiseAndSetIfChanged(ref imageId, value); }
        }

        /// <summary>
        /// Created a new instance of the image view view model.
        /// </summary>
        /// <param name="id"></param>
        public ImageViewWindowViewModel()
        {
            // inherited from base view model
            ImageId = SelectedImageId;
        }
    }
}
