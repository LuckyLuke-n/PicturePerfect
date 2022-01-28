﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="fileTypes"></param>
        /// <returns>Returns the number of files.</returns>
        public int CountFiles(string path, List<string> fileTypes)
        {
            int counter = 0;

            string[] files = Directory.GetFiles(path);

            // add the leading dot for checking with filesInfo.Extension
            // also add the extension in upper case letters
            List<string> filesExtensions = new() { };
            fileTypes.ForEach(fileType => filesExtensions.AddRange(new List<string>() { $".{fileType}", $".{fileType.ToUpper()}" }));

            foreach ( string file in files )
            {
                FileInfo fileInfo = new(file);
                if (filesExtensions.Contains(fileInfo.Extension) == true) { counter++; }
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
    }
}