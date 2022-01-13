using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.ViewModels
{
    internal class InfoWindowViewModel : ViewModelBase
    {
        #region Color properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        #endregion

        #region Text properties
        public static string ApplicatonName => ThisApplication.ApplicationName;



        public static string License => "Test";
        #endregion

        /// <summary>
        /// Creates a new instance of the InfoWindowViewModel.
        /// </summary>
        public InfoWindowViewModel()
        {

        }
    }
}
