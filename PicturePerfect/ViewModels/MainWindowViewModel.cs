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

        #region Defaults
        public static string PlaceholderImage => "avares://PicturePerfect/Assets/Drawables/image_placeholder.jpg";
        #endregion

        public MainWindowViewModel()
        {

        }

        public int PercentageProgressBar { get; private set; } = 100;
        public string LabelProgressBar => $"{PercentageProgressBar}%";
    }
}
