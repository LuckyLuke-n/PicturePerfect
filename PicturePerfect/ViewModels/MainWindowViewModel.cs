using Avalonia.Media.Imaging;
using PicturePerfect.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PicturePerfect.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        # region Color properties
        public static string DarkColor => ThisApplication.DarkColor;
        public static string MediumColor => ThisApplication.MediumColor;
        public static string LightColor => ThisApplication.LightColor;
        public static string LightFontColor => ThisApplication.LightFontColor;
        public static string DarkContrastColor => ThisApplication.DarkContrastColor;
        #endregion

        #region Fonts and Sizes
        public static int MenuBarWidth => 250;
        public static int SearchBoxWidth => MenuBarWidth - 55;
        public static int LargeFontSize => 23;
        #endregion

        #region Favorite images
        public static Bitmap ImageNo1 { get; private set; } = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/test/P5140045_Stockerpel.jpg");
        public static Bitmap ImageNo2 { get; private set; } = BitmapValueConverter.Convert(PaceholderImagePath);
        public static Bitmap ImageNo3 { get; private set; } = BitmapValueConverter.Convert(PaceholderImagePath);
        public static Bitmap ImageNo4 { get; private set; } = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/test/P5140202_Kohlmeise.jpg");
        #endregion

        #region Defaults
        private static string PaceholderImagePath = "avares://PicturePerfect/Assets/Drawables/image_placeholder.jpg";
        public static Bitmap PlaceholderImage => BitmapValueConverter.Convert("avares://PicturePerfect/Assets/Drawables/image_placeholder.jpg");
        #endregion

        /// <summary>
        /// Creates an instance of the MainWindowViewModel.
        /// </summary>
        public MainWindowViewModel()
        {

        }

        public int PercentageProgressBar { get; private set; } = 100;
        public string LabelProgressBar => $"{PercentageProgressBar}%";



    }
}
