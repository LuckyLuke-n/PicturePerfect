﻿using PicturePerfect.Models;
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
        public static string DarkColorDefault => "#232323"; // carbon
        public static string MediumColorDefault => "Gray";
        public static string LightColorDefault => "WhiteSmoke";
        public static string LightFontColorDefault => "WhiteSmoke";
        public static string DarkContrastColorDefault => "#2E3033"; // rich gray
        #endregion

        /// <summary>
        /// Get the current application Version.
        /// </summary>
        public static string ApplicationVersion { get; } = "V0.1.0";
        /// <summary>
        /// Get the application name.
        /// </summary>
        public static string ApplicationName { get; } = "PicturePerfect";
        /// <summary>
        /// Get the application build date.
        /// </summary>
        public static string BuildDate { get; } = "2022-01-15";

        /// <summary>
        /// Get or set the currently loaded project file.
        /// </summary>
        public static ProjectFile ProjectFile { get; set; } = ProjectFile.AtStartup();


    }
}
