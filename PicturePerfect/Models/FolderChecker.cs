using System;
using System.Collections.Generic;
using System.IO;

namespace PicturePerfect.Models
{
    public class FolderChecker
    {
        /// <summary>
        /// Creates a ned instance of the folder checker class.
        /// </summary>
        public FolderChecker()
        {

        }

        /// <summary>
        /// Method to count the images files in a given folder.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the number of files.</returns>
        public int CountFiles(string path)
        {
            int counter = 0;

            string[] files = Directory.GetFiles(path);

            // add the leading dot for checking with filesInfo.Extension
            // also add the extension in upper case letters
            //List<string> filesExtensions = new() { };
            //ThisApplication.ProjectFile.GetInputFileTypes().ForEach(fileType => filesExtensions.AddRange(new List<string>() { $".{fileType}", $".{fileType.ToUpper()}" }));

            foreach ( string file in files )
            {
                FileInfo fileInfo = new(file);
                if (ThisApplication.ProjectFile.GetInputFileTypes().Contains(fileInfo.Extension) == true) { counter++; }
            }

            return counter;
        }

        /// <summary>
        /// Method to count the raw files in a given folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rawTypes"></param>
        /// <returns>Returns the number of files.</returns>
        public int CountRawFiles(string path, List<string> rawTypes)
        {
            int counter = 0;

            string[] files = Directory.GetFiles(path);

            // add the leading dot for checking with filesInfo.Extension
            List<string> filesExtensions = new() { };
            rawTypes.ForEach(fileType => filesExtensions.AddRange(new List<string>() { $".{fileType}", $".{fileType.ToUpper()}" }));

            foreach (string file in files)
            {
                FileInfo fileInfo = new(file);
                if (filesExtensions.Contains(fileInfo.Extension) == true) { counter++; }
            }

            return counter;
        }

        /// <summary>
        /// Method to get the size of a folder in MB.
        /// If the backup folder does not exist the returned size will be zero.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the bytes as an integer.</returns>
        public double GetFolderSize(string path)
        {
            double size = 0;

            // check if folder exists to avoid error when project is below ot equal version V1.1.1
            if (Directory.Exists(path) == true)
            {
                // Get array of all file names.
                string[] files = Directory.GetFiles(path, "*.*");

                // Calculate total bytes of all files in a loop.
                foreach (string file in files)
                {
                    //FileInfo to get length of each file.
                    FileInfo info = new(file);
                    size += (double)info.Length / 1000000;
                }
            }

            return Math.Round(size, 3);
        }
    }
}
