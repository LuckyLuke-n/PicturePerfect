using Avalonia.Media.Imaging;
using ImageMagick;
using System;
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
        public string CustomName { get; set; }
        /// <summary>
        /// Get the filename. Per default this is the filename.
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the subfolder name located in .../images/
        /// </summary>
        public string Subfolder { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the file type.
        /// </summary>
        public string FileType { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the date when the image was taken.
        /// </summary>
        public DateTime DateTaken { get; set; }
        /// <summary>
        /// Get or set the file size of this image.
        /// </summary>
        public double Size { get; set; }
        /// <summary>
        /// Get or set the camera manufacturer and model.
        /// </summary>
        public string Camera { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the ISO value set for this image.
        /// </summary>
        public int ISO { get; set; }
        /// <summary>
        /// Get or set the F-stop value for this image.
        /// It should be displayed as f/[value]
        /// </summary>
        public double FStop { get; set; }
        /// <summary>
        /// Get or set the exposure time in milli-sec.
        /// </summary>
        public int ExposureTime { get; set; }
        /// <summary>
        /// Get or set the exposure bias in steps for this image.
        /// </summary>
        public double ExposureBias { get; set; }
        /// <summary>
        /// Get or set the focal length in mm for this image.
        /// </summary>
        public double FocalLength { get; set; }

        /// <summary>
        /// Get or set the loacation where this image was taken.
        /// </summary>
        public Locations.Location Location { get; set; } = new();
        /// <summary>
        /// Get or set the category for this window.
        /// </summary>
        public Category Category { get; set; } = new();
        /// <summary>
        /// Get or set the sub category #1.
        /// </summary>
        public SubCategory SubCategory1 { get; set; } = new();
        /// <summary>
        /// Get or set the sub category #2.
        /// </summary>
        public SubCategory SubCategory2 { get; set; } = new();
        /// <summary>
        /// Get or set the notes for this image.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
        /// <summary>
        /// Get the image id. This is the id number from the sqlite database.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Get the absolute path to this image file.
        /// </summary>
        public string AbsolutePath => Path.Combine(ThisApplication.ProjectFile.ImageFolder, Subfolder, Name);

        // file type arrays
        private readonly string[] orf = { ".orf", ".ORF" };
        private readonly string[] nef = { ".nef", ".NEF" };
        private readonly string[] jpg = { ".jpg", ".JPG" };
        private readonly string[] png = { ".png", ".PNG" };


        /// <summary>
        /// Creates a new instance of the image file class.
        /// </summary>
        public ImageFile()
        {

        }

        /// <summary>
        /// Method to populate the properties with file info from a given path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="subfolderName"></param>
        public void NewFromPath(string path, string subfolderName)
        {
            FileInfo fileInfo = new(path);
            CustomName = fileInfo.Name;
            Name = fileInfo.Name;
            Subfolder = subfolderName;
            FileType = fileInfo.Extension;
            DateTaken = fileInfo.LastWriteTime; // Last write time is the creation date for un.edited files. This is a work around since it was not possible to read the create date from exifdirectory.
            Size = Math.Round(fileInfo.Length/1000000.00, 3);

            // create the entry
            CreateDatabaseEntry(this, path);
        }

        /// <summary>
        /// Method to copy the files and create a sqlite entry for this image.
        /// </summary>
        private void CreateDatabaseEntry(ImageFile imageFile, string path)
        {
            // copy the file to the image folder's subfolder
            string destination = Path.Combine(ThisApplication.ProjectFile.ImageFolder, imageFile.Subfolder, imageFile.Name);
            File.Copy(path, destination, true);

            // add to sqlite
            Database.AddImage(this);
        }

        /// <summary>
        /// Method to set the value for the Id property. The property has a private setter to avoid mis-use.
        /// This method is a work around.
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Method to set the value for the Name property. The property has a private setter to avoid mis-use.
        /// This method is a work around.
        /// </summary>
        /// <param name="name"></param>
        public void SetFileName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Save changes made to the image meta data.
        /// </summary>
        public void CommitCustomFileNameChange()
        {
            Database.SetCustomName(imageFile: this);
        }

        /// <summary>
        /// Save changes made to the image location.
        /// </summary>
        public void CommitLocationChange()
        {

        }

        /// <summary>
        /// Save changes made to the image category.
        /// </summary>
        public void CommitCategoryChange()
        {

        }

        /// <summary>
        /// Save changes made to the image sub category 1.
        /// </summary>
        public void CommitSubCategory1Change()
        {

        }

        /// <summary>
        /// Save changes made to the image sub category 2.
        /// </summary>
        public void CommitSubCategory2Change()
        {

        }



        /// <summary>
        /// Method to get this image to a bitmap.
        /// </summary>
        /// <returns>Returns the bitmap.</returns>
        public Bitmap ToBitmap()
        {
            Bitmap bitmap;

            if (orf.Contains(FileType) || nef.Contains(FileType))
            {
                MagickImage magickImage = new(AbsolutePath);
                bitmap = magickImage.ToBitmap();
            }
            else if (jpg.Contains(FileType) || png.Contains(FileType))
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
            else if ((nef.Contains(outputType) || orf.Contains(outputType)) & jpg.Contains(FileType))
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
                    if (jpg.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Jpg);
                    }
                    else if (png.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Png);
                    }
                    else if (nef.Contains(outputType))
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
        /// Method to delete this images from the database.
        /// </summary>
        public void DeleteFromDatabase()
        {

        }
    }
}
