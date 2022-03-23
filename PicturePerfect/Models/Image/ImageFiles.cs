using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

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

            list.ForEach(file => List.Add(file));
        }

        /// <summary>
        /// Load images from the database by category.
        /// </summary>
        /// <param name="category"></param>
        public void LoadByCategory(Category category)
        {
            List.Clear();

            List<ImageFile> list = Database.LoadImageFilesByCategory(category);

            list.ForEach(file => List.Add(file));
        }

        /// <summary>
        /// Load images from the database by sub-category.
        /// </summary>
        public void LoadBySubCategory(SubCategory subCategory)
        {
            List.Clear();

            List<ImageFile> list = Database.LoadImageFilesBySubCategory(subCategory);

            list.ForEach(file => List.Add(file));
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
                ImageFile image = ImageFile.NewToDatabase(file, subfolderName);
                List.Add(image);
            }

            // reload all images from the sqlite database in order to get the info about the id
            LoadAll();
        }

        /// <summary>
        /// Search for specific query.
        /// </summary>
        /// <param name="query"></param>
        public void Search(string query)
        {
            List.Clear();

            List<ImageFile> searchMatches = Database.Search(query);

            searchMatches.ForEach(file => List.Add(file));
        }
    }
}
