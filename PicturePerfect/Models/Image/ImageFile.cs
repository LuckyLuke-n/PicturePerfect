using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public class ImageFile
    {
        /// <summary>
        /// Get or set the filename. Per default this is the filename and can be reset to a custom name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
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
        public double ExposureTime { get; set; }
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
        /// Create a sqlite entry for this image.
        /// </summary>
        public void CreateDatabaseEntry()
        {

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
