using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public class ImageFiles
    {
        /// <summary>
        /// Get the list of currently loaded image files.
        /// </summary>
        public ObservableCollection<ImageFiles> List { get; private set; } = new();

        /// <summary>
        /// Creates a new instance of the images files class.
        /// </summary>
        public ImageFiles()
        {

        }

        /// <summary>
        /// Method to load all images from the database.
        /// </summary>
        public void LoadAll()
        {

        }

        /// <summary>
        /// Load images from the database by category.
        /// </summary>
        public void LoadByCategory()
        {

        }

        /// <summary>
        /// Load images from the database by sub-category.
        /// </summary>
        public void LoadBySubCategory()
        {

        }

        /// <summary>
        /// Load images from the database by location.
        /// </summary>
        public void LoadByLocation()
        {

        }

        /// <summary>
        /// Load image files from a folder and store them to the database.
        /// </summary>
        public void ReadFromFolder()
        {

        }

        /// <summary>
        /// Method to remove a images file from the list.
        /// This will not delete the item from the database.
        /// </summary>
        public void RemoveFromList()
        {

        }

        /// <summary>
        /// Method to delete the image from the database.
        /// </summary>
        public void DeleteFromDatabase()
        {

        }
    }
}
