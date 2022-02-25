using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public ObservableCollection<ImageFile> List { get; private set; } = new();

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
            List.Clear();

            List<ImageFile> list = Database.LoadAllImageFiles();

            foreach (ImageFile file in list)
            {
                List.Add(file);
            }
        }

        /// <summary>
        /// Load images from the database by category.
        /// </summary>
        public void LoadByCategory()
        {
            List.Clear();
        }

        /// <summary>
        /// Load images from the database by sub-category.
        /// </summary>
        public void LoadBySubCategory()
        {
            List.Clear();
        }

        /// <summary>
        /// Load images from the database by location.
        /// </summary>
        public void LoadByLocation()
        {
            List.Clear();
        }

        /// <summary>
        /// Copy image files from a folder to \images\subfolder\ and store the information to the database.
        /// </summary>
        /// <param name="files">List of files to be added.</param>
        /// <param name="subfolderName">Name of the subfolder created in \images\</param>
        public void AddImages(List<string> files, string subfolderName)
        {
            // create subfolder in \images\
            string newDirectory = Path.Combine(ThisApplication.ProjectFile.ImageFolder, subfolderName);
            Directory.CreateDirectory(newDirectory);

            foreach (string file in files)
            {
                // create image file oject
                ImageFile image = new();
                image.NewFromPath(file, subfolderName);
                List.Add(image);
            }
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
