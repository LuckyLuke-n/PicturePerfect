using ImageMagick;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Exif.Makernotes;
using PicturePerfect.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Location { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the category for this window.
        /// </summary>
        public string Category { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the sub category #1.
        /// </summary>
        public string SubCategory1 { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the sub category #2.
        /// </summary>
        public string SubCategory2 { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the notes for this image.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
        /// <summary>
        /// Get the image id. This is the id number from the sqlite database.
        /// </summary>
        public int Id { get; private set; }

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
        /// Save changed properties to the database.
        /// </summary>
        public void CommitChanges()
        {

        }

        /// <summary>
        /// Method to convert a raw file image to jpg-format.
        /// </summary>
        /// <param name="outputType"></param>
        /// <returns>Returns the converted images of type jpg, png, or bitmap.</returns>
        public object Convert(ImageTypes outputType = ImageTypes.jpg)
        {
        object convertedImage = "";


        return convertedImage;
        }

        /// <summary>
        /// Method to delete this images from the database.
        /// </summary>
        public void DeleteFromDatabase()
        {

        }
    }
}
