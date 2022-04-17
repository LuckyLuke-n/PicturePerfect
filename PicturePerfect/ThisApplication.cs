using Avalonia.Media.Imaging;
using PicturePerfect.Models;
using System;
using System.IO;

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
        public static Bitmap PlaceholderImage => BitmapValueConverter.Convert("avares://PicturePerfect/Assets/Drawables/image_placeholder.jpg");
        #endregion

        /// <summary>
        /// Get the current application Version.
        /// </summary>
        public static string ApplicationVersion { get; } = "V1.1.1";
        /// <summary>
        /// Get the application name.
        /// </summary>
        public static string ApplicationName { get; } = "PicturePerfect";
        /// <summary>
        /// Get the application build date.
        /// </summary>
        public static string BuildDate { get; } = "2022-04-17";

        /// <summary>
        /// Get the database version.
        /// </summary>
        public static int DatabaseVersion { get; } = 2;

        /// <summary>
        /// Get or set the currently loaded project file.
        /// </summary>
        public static ProjectFile ProjectFile { get; set; } = ProjectFile.AtStartup();

        /// <summary>
        /// Get the path to the temp folder.
        /// </summary>
        public static string TempFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LSoftware", "PicturePerfect", "temp");

        /// <summary>
        /// Method to clear the temp folder.
        /// </summary>
        public static void ClearTemp()
        {
            // check if the temp directory exists
            if (Directory.Exists(TempFolderPath))
            {
                // clear the directory
                string[] filePaths = Directory.GetFiles(TempFolderPath);
                foreach (string filePath in filePaths)
                {
                    File.Delete(filePath);
                }
            }           
        }
    }
}
