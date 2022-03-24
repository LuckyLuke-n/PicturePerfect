using ImageMagick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace PicturePerfect.Models
{
    public class RawConverter
    {
        /// <summary>
        /// Get the list of currently loaded raw files.
        /// </summary>
        public ObservableCollection<RawFile> RawFiles { get; private set; } = new();

        /// <summary>
        /// Class for raw files.
        /// </summary>
        public class RawFile
        {
            public string Name { get; private set; }
            public double Size { get; private set; }
            public string Type { get; private set; }
            public DateTime DateTaken { get; private set; }
            public string AbsolutePath { get; private set; }

            // file type arrays
            public static string[] OrfStrings => new string[] { ".orf", ".ORF" };
            public static string[] NefStrings => new string[] { ".nef", ".NEF" };
            private static string[] JpgStrings => new string[] { ".jpg", ".JPG" };
            private static string[] PngStrings => new string[] { ".png", ".PNG" };

            /// <summary>
            /// Creates a new instance of the raw file class
            /// </summary>
            /// <param name="name"></param>
            /// <param name="size"></param>
            /// <param name="type"></param>
            /// <param name="dateCreated"></param>
            /// <param name="path"></param>
            public RawFile(string name, double size, string type, DateTime dateTaken, string path)
            {
                Name = name;
                Size = size;
                Type = type;
                DateTaken = dateTaken;
                AbsolutePath = path;
            }

            /// <summary>
            /// Method to convert this raw file instance to a .jpg file and store it in a given folder.
            /// </summary>
            /// <param name="toFolder"></param>
            /// <param name="outputType"></param>
            public void Convert(string toFolder, string outputType = ".jpg")
            {
                // create new file name with new extension based on custom or initial name
                string GetOutputFileName()
                {
                    // check custom or initial name
                    string newFileName = $"{Name.Split(".").First()}{outputType}";

                    string fileName = Path.Combine(paths: new string[] { toFolder, newFileName });

                    return fileName;
                }

                // decode the image using the MagickImage library
                using (MagickImage rawImage = new(AbsolutePath))
                {
                    if (JpgStrings.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Jpg);
                    }
                    else if (PngStrings.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Png);
                    }
                    else
                    {
                        // no conversion
                    }
                }
            }
        }

        /// <summary>
        /// Method to add raw files to the images files list.
        /// </summary>
        /// <param name="files"></param>
        /// <returns>Returns the number of pictures added to the list.</returns>
        public int AddToRawConverter(List<string> files)
        {
            int counter = 0;

            // go through files in list
            foreach (string file in files)
            {
                // create image file oject
                FileInfo fileInfo = new FileInfo(file);
                RawFile image = new(name: fileInfo.Name, size: Math.Round((double)fileInfo.Length/1000000, 3), type: fileInfo.Extension, dateTaken: fileInfo.LastWriteTime, path: fileInfo.FullName);

                // add to list if file is a raw file
                if (RawFile.OrfStrings.Contains(image.Type) || RawFile.NefStrings.Contains(image.Type))
                {
                    // image is a raw file
                    RawFiles.Add(image);
                    counter++;
                }
            }

            return counter;
        }
    }
}
