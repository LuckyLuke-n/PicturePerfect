using PicturePerfect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect
{
    internal static class ThisApplication
    {
        #region Default theme colors
        public static string DarkColorDefault = "#232323"; // carbon
        public static string MediumColorDefault = "Gray";
        public static string LightColorDefault = "WhiteSmoke";
        public static string LightFontColorDefault = "WhiteSmoke";
        public static string DarkContrastColorDefault = "#2E3033"; // rich gray
        #endregion

        /// <summary>
        /// Get the current application Version.
        /// </summary>
        public static string ApplicationVersion { get; } = "V0.1.0";
        /// <summary>
        /// Get the array of alle released application versions.
        /// </summary>
        public static string[] ApplicationVersionList { get; } = new string[] { "V0.1.0" };

        /// <summary>
        /// Get or set the currently loaded project file.
        /// </summary>
        public static ProjectFile ProjectFile { get; set; } = ProjectFile.AtStartup();


    }
}
