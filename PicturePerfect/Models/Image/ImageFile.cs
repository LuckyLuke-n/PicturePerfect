﻿using Avalonia.Media.Imaging;
using ImageMagick;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PicturePerfect.Models
{
    public class ImageFile
    {
        /// <summary>
        /// Get or set the custom name. This will not change the file name.
        /// Per default this equals the file name.
        /// </summary>
        public string CustomName { get; private set; }
        /// <summary>
        /// Get the filename. Per default this is the filename.
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the subfolder name located in .../images/
        /// </summary>
        public string Subfolder { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the file type.
        /// </summary>
        public string FileType { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the date when the image was taken.
        /// </summary>
        public DateTime DateTaken { get; private set; }
        /// <summary>
        /// Get or set the file size of this image.
        /// </summary>
        public double Size { get; private set; }
        /// <summary>
        /// Get or set the camera manufacturer and model.
        /// </summary>
        public string Camera { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the ISO value set for this image.
        /// </summary>
        public int ISO { get; private set; }
        /// <summary>
        /// Get or set the F-stop value for this image.
        /// It should be displayed as f/[value]
        /// </summary>
        public double FStop { get; private set; }
        /// <summary>
        /// Get or set the exposure time in milli-sec.
        /// </summary>
        public int ExposureTime { get; private set; }
        /// <summary>
        /// Get or set the exposure bias in steps for this image.
        /// </summary>
        public double ExposureBias { get; private set; }
        /// <summary>
        /// Get or set the focal length in mm for this image.
        /// </summary>
        public double FocalLength { get; private set; }

        /// <summary>
        /// Get or set the loacation where this image was taken.
        /// </summary>
        public Locations.Location Location { get; private set; } = new();
        /// <summary>
        /// Get or set the category for this window.
        /// </summary>
        public Category Category { get; private set; } = new();
        /// <summary>
        /// Get or set the sub category #1.
        /// </summary>
        public SubCategory SubCategory1 { get; private set; } = new();
        /// <summary>
        /// Get or set the sub category #2.
        /// </summary>
        public SubCategory SubCategory2 { get; private set; } = new();
        /// <summary>
        /// Get or set the notes for this image.
        /// </summary>
        public string Notes { get; private set; } = string.Empty;
        /// <summary>
        /// Get the image id. This is the id number from the sqlite database.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Get the absolute path to this image file.
        /// </summary>
        public string AbsolutePath => Path.Combine(ThisApplication.ProjectFile.ImageFolder, Subfolder, Name);

        // file type arrays
        public static string[] OrfStrings => new string[] { ".orf", ".ORF" };
        public static string[] NefStrings => new string[] { ".nef", ".NEF" };
        private static string[] JpgStrings => new string[] { ".jpg", ".JPG" };
        private static string[] PngStrings => new string[] { ".png", ".PNG" };


        /// <summary>
        /// Creates a new instance of the image file class.
        /// </summary>
        public ImageFile()
        {

        }

        /// <summary>
        /// Creates a new image file object with information from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="customName"></param>
        /// <param name="subfolderName"></param>
        /// <param name="fileType"></param>
        /// <param name="dateTaken"></param>
        /// <param name="size"></param>
        /// <param name="camera"></param>
        /// <param name="fStop"></param>
        /// <param name="iso"></param>
        /// <param name="exposureTime"></param>
        /// <param name="exposureBias"></param>
        /// <param name="focalLength"></param>
        /// <param name="notes"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile NewFromDatabase(int id, string name, string customName, string subfolderName, string fileType, DateTime dateTaken, double size, string camera, double fStop, int iso, int exposureTime, double exposureBias, double focalLength, string notes, Locations.Location location, Category category, SubCategory subCategory1, SubCategory subCategory2)
        {
            ImageFile imageFile = new()
            {
                Id = id,
                Name = name,
                CustomName = customName,
                Subfolder = subfolderName,
                FileType = fileType,
                DateTaken = dateTaken,
                Size = size,
                Camera = camera,
                ISO = iso,
                FStop = fStop,
                ExposureTime = exposureTime,
                ExposureBias = exposureBias,
                FocalLength = focalLength,
                Notes = notes,
                Location = location,
                Category = category,
                SubCategory1 = subCategory1,
                SubCategory2 = subCategory2
            };

            return imageFile;
        }

        /// <summary>
        /// Method to create an image file object from a path and save the properties to the database.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="subfolderName"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile NewToDatabase(string path, string subfolderName, Locations.Location location)
        {
            // create an image file from the path
            ImageFile imageFile = CreateFromPath(path);
            imageFile.Subfolder = subfolderName;
            imageFile.Location = location; // this is either the default location "None" or a user selection.

            // copy the file to the image folder's subfolder
            string destination = Path.Combine(ThisApplication.ProjectFile.ImageFolder, imageFile.Subfolder, imageFile.Name);
            File.Copy(path, destination, true);

            // add to sqlite
            Database.AddImage(imageFile);

            return imageFile;
        }

        /// <summary>
        /// Method to load the image file object from the database by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile LoadById(int id)
        {
            return Database.LoadImageFileById(id);
        }

        /// <summary>
        /// Method to create an image file object from a given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the image file object.</returns>
        private static ImageFile CreateFromPath(string path)
        {
            ImageFile imageFile = new();

            FileInfo fileInfo = new(path);
            imageFile.CustomName = fileInfo.Name;
            imageFile.Name = fileInfo.Name;
            imageFile.FileType = fileInfo.Extension;
            imageFile.DateTaken = fileInfo.LastWriteTime; // Last write time is the creation date for un.edited files. This is a work around since it was not possible to read the create date from exifdirectory.
            imageFile.Size = Math.Round(fileInfo.Length / 1000000.00, 3);


            // extract metadata, but this is not possible at all times, since the output dor the tags might differ
            try
            {
                ExtractMetaData();
            }
            catch (Exception ex)
            {
                // throw the exception for further handling
                throw new ArgumentException("Error while extracting metadata.", ex);
            }

            // function to extragt the meta data
            void ExtractMetaData()
            {
                IReadOnlyList<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(path);
                ExifSubIfdDirectory? subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

                // check which meta data extration is to be performed
                if (subIfdDirectory != null && OrfStrings.Contains(imageFile.FileType))
                {
                    // file is a orf file
                    imageFile.Camera = subIfdDirectory.Tags[32].Description; // lens model as string
                    imageFile.FStop = double.Parse(subIfdDirectory.Tags[1].Description.Split('/')[1]); // e.g. f/8,0
                    imageFile.ISO = Convert.ToInt32(subIfdDirectory.Tags[3].Description); // e.g.250
                    string exposureTime = subIfdDirectory.Tags[0].Description.Split(' ')[0]; // e.g. 1/500 sec
                    imageFile.ExposureTime = (int)(Convert.ToDouble(exposureTime.Split('/')[0]) / Convert.ToDouble(exposureTime.Split('/')[1]) * 1000);
                    imageFile.ExposureBias = double.Parse(subIfdDirectory.Tags[11].Description.Split(' ')[0]); // e.g. 38 0 EV;
                    imageFile.FocalLength = double.Parse(subIfdDirectory.Tags[16].Description.Split(' ')[0]); // e.g. 38 mm;
                }
                else if (subIfdDirectory != null && JpgStrings.Contains(imageFile.FileType))
                {
                    // file is a jpg file
                    imageFile.Camera = "not available";
                    imageFile.FStop = double.Parse(subIfdDirectory.Tags[1].Description.Split('/')[1]); // e.g. f/1,8
                    imageFile.ISO = Convert.ToInt32(subIfdDirectory.Tags[0].Description); // e.g.250
                    string exposureTime = subIfdDirectory.Tags[2].Description.Split(' ')[0]; // e.g. 1/20 sec
                    imageFile.ExposureTime = (int)(Convert.ToDouble(subIfdDirectory.Tags[2].Description.Split(' ')[0]) * 1000);
                    imageFile.ExposureBias = double.Parse(subIfdDirectory.Tags[15].Description.Split(' ')[0]); // e.g. 38 0 EV;
                    imageFile.FocalLength = double.Parse(subIfdDirectory.Tags[12].Description.Split(' ')[0]); // e.g. 38 mm;
                }
                else
                {
                    // no meta data will be set
                }
            }

            return imageFile;
        }

        /// <summary>
        /// Save changes made to the image meta data.
        /// </summary>
        /// <param name="newName"></param>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitCustomFileNameChange(string newName)
        {
            CustomName = newName;
            Database.SetCustomName(imageFile: this);

            return this;
        }

        /// <summary>
        /// Save the changed made to the notex property to the database.
        /// </summary>
        /// <param name="newNotes"></param>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitNotesChange(string newNotes)
        {
            Notes = newNotes;
            Database.SetNotes(imageFile: this);

            return this;
        }

        /// <summary>
        /// Save changes made to the image location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitLocationChange(Locations.Location location)
        {
            Location = location;
            Database.LinkImageToLocation(this, location);

            return this;
        }

        /// <summary>
        /// Save changes made to the image category.
        /// </summary>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitCategoryChange(Category category)
        {
            Category = category;
            Database.LinkImageToCategory(this, category);

            return this;
        }

        /// <summary>
        /// Save changes made to the image sub category 1.
        /// </summary>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitSubCategory1Change(SubCategory newSubCategory, SubCategory oldSubCategory)
        {
            oldSubCategory = SubCategory1;
            SubCategory1 = newSubCategory;
            Database.LinkImageToSubCategory(this, newSubCategory, oldSubCategory);

            return this;
        }

        /// <summary>
        /// Save changes made to the image sub category 2.
        /// </summary>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitSubCategory2Change(SubCategory newSubCategory, SubCategory oldSubCategory)
        {
            oldSubCategory = SubCategory2;
            SubCategory2 = newSubCategory;
            Database.LinkImageToSubCategory(this, newSubCategory, oldSubCategory);

            return this;
        }

        /// <summary>
        /// Method to get this image to a bitmap.
        /// </summary>
        /// <returns>Returns the bitmap.</returns>
        public Bitmap ToBitmap()
        {
            Bitmap bitmap;

            if (OrfStrings.Contains(FileType) || NefStrings.Contains(FileType))
            {
                MagickImage magickImage = new(AbsolutePath);
                bitmap = magickImage.ToBitmap();
            }
            else if (JpgStrings.Contains(FileType) || PngStrings.Contains(FileType))
            {
                bitmap = BitmapValueConverter.Convert(AbsolutePath);
            }
            else
            {
                bitmap = ThisApplication.PlaceholderImage;
            }
            return bitmap;
        }

        /// <summary>
        /// Method to convert a raw file image to jpg-format. The image will be savd to the specified path.
        /// </summary>
        /// <param name="toFolder"></param>
        /// <param name="outputType"></param>
        /// <returns>Returns true if conversion was successful.</returns>
        public bool Export(string toFolder, string outputType = ".jpg")
        {
            // create new file name with new extension based on custom or initial name
            string GetOutputFileName()
            {
                string newFileName = string.Empty;

                // check custom or initial name
                if (Name != CustomName)
                {
                    newFileName = $"{CustomName.Split(".").First()}{outputType}";
                }
                else
                {
                    newFileName = $"{Name.Split(".").First()}{outputType}";
                }

                string fileName = Path.Combine(paths: new string[] { toFolder, newFileName });

                return fileName;
            }

            if (outputType == FileType || outputType.ToUpper() == FileType)
            {
                // only copy the file
                string outputFileName = Path.Combine(toFolder, Name);
                File.Copy(AbsolutePath, outputFileName, true);
                return true;
            }
            else if ((NefStrings.Contains(outputType) || OrfStrings.Contains(outputType)) & JpgStrings.Contains(FileType))
            {
                // jpg will not be converted to raw on output!
                return false;
            }
            else
            {
                // type is some sort of raw file that will be converted
                // decode the image using the MagickImage library
                using (MagickImage rawImage = new(this.AbsolutePath))
                {
                    if (JpgStrings.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Jpg);
                    }
                    else if (PngStrings.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Png);
                    }
                    else if (NefStrings.Contains(outputType))
                    {
                        // no conversion
                        // rawImage.Write(GetOutputFileName(), MagickFormat.Nef);
                    }
                    else
                    {
                        // no conversion
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Method to delete this images from the database and the folder on the drive.
        /// </summary>
        public void DeleteFromDatabase()
        {
            Database.DeleteImage(this);
            File.Delete(AbsolutePath);
        }

        /// <summary>
        /// Method to open the image in an external image viewer.
        /// </summary>
        /// <param name="pathToExe"></param>
        public void OpenInExternalViewer(string pathToExe)
        {
            // copy the image to a temp folder
            string path = Path.Combine(ThisApplication.TempFolderPath, Name);
            System.IO.Directory.CreateDirectory(ThisApplication.TempFolderPath); // create folder if it does no exist
            File.Copy(sourceFileName: AbsolutePath, destFileName: path, overwrite: true);

            // open the file in the selected image viewer
            Process process = new();
            process.StartInfo.FileName = pathToExe;
            process.StartInfo.Arguments = path;
            process.Start();
        }
    }
}
