using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PicturePerfect.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        public static string Date => DateTime.Today.ToString("yyyy-MM-dd");
        public static string CalendarWeek => "CW " + ISOWeek.GetWeekOfYear(DateTime.Now).ToString();

        # region Color properties
        public static string DarkColor => ThisApplication.DarkColor;
        public static string MediumColor => ThisApplication.MediumColor;
        public static string LightColor => ThisApplication.LightColor;
        public static string LightFontColor => ThisApplication.LightFontColor;
        public static string DarkContrastColor => ThisApplication.DarkContrastColor;
        #endregion

        #region Fonts
        public static int LargeFontSize => 23;
        #endregion
    }
}
