using System.IO;

namespace PicturePerfect.ViewModels
{
    internal class InfoWindowViewModel : ViewModelBase
    {
        #region Color and font properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        public static int LargeFontSize => 23;
        #endregion

        #region Text properties
        public static string ApplicatonName => ThisApplication.ApplicationName;
        public static string ApplicationVersion => ThisApplication.ApplicationVersion;
        public static string BuildDate => "Build date: " + ThisApplication.BuildDate;

        public static string About => File.ReadAllText("Resources/about.txt");
        public static string Libraries => File.ReadAllText("Resources/libraries.txt");
        public static string License => File.ReadAllText("Resources/license.txt");
        #endregion

        /// <summary>
        /// Creates a new instance of the InfoWindowViewModel.
        /// </summary>
        public InfoWindowViewModel()
        {

        }
    }
}
