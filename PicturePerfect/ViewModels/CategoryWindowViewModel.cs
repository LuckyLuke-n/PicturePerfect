using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.ViewModels
{
    internal class CategoryWindowViewModel : ViewModelBase
    {
        #region Color and font properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        public static int LargeFontSize => 23;
        #endregion

        public string test = "";


        public CategoryWindowViewModel()
        {
             
        }
    }
}
