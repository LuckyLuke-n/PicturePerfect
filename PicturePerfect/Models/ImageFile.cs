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
        public string Name { get; set; }
        /// <summary>
        /// Get or set the subfolder name located in .../images/
        /// </summary>
        public string Subfolder { get; set; }
        /// <summary>
        /// Get or set the file type.
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// Get or set the date when the image was taken.
        /// </summary>
        public DateTime DateTaken { get; set; }
        /// <summary>
        /// Get or set the file size of this image.
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// Get or set the camera manufacturer and model.
        /// </summary>
        public string Camera { get; set; }
        /// <summary>
        /// Get or set the ISO value set for this image.
        /// </summary>
        public int ISO { get; set; }
        /// <summary>
        /// Get or set the F-stop value for this image.
        /// </summary>
        public string FStop { get; set; }
        /// <summary>
        /// Get or set the exposure time in 1/xxx sec.
        /// </summary>
        public string ExposureTime { get; set; }
        /// <summary>
        /// Get or set the exposure bias in steps for this image.
        /// </summary>
        public string ExposureBias { get; set; }
        /// <summary>
        /// Get or set the focal length in mm for this image.
        /// </summary>
        public int FocalLength { get; set; }

        /// <summary>
        /// Get or set the loacation where this image was taken.
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Get or set the category for this window.
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Get or set the sub category #1.
        /// </summary>
        public string SubCategory1 { get; set; }
        /// <summary>
        /// Get or set the sub category #2.
        /// </summary>
        public string SubCategory2 { get; set; }
        /// <summary>
        /// Get or set the notes for this image.
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Get the image id. This is the id number from the sqlite database.
        /// </summary>
        public int Id { get; private set; }

        public ImageFile()
        {

        }
    }
}
