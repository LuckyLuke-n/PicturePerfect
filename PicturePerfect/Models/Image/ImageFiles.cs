using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace PicturePerfect.Models
{
    public class ImageFiles
    {
        /// <summary>
        /// Get the list of currently loaded image files.
        /// </summary>
        public ObservableCollection<ImageFile> List { get; private set; } = new();

        /// <summary>
        /// Get or set the number of metadata extraction errors.
        /// </summary>
        public int MetadataErrors { get; private set; } = 0;

        /// <summary>
        /// Creates a new instance of the images files class.
        /// </summary>
        public ImageFiles()
        {

        }

        /// <summary>
        /// Method to get the list index by the sqlite id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns and integer representing the list id.</returns>
        private int GetListIdBySqliteId(int id)
        {
            int listId = 0;
            int counter = 0;
            foreach (ImageFile imageFile in List)
            {
                if (imageFile.Id == id) { listId = counter; break; }
                counter++;
            }

            return listId;
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
        /// <param name="location">Name of the auto tag location.</param>
        public void AddImages(List<string> files, string subfolderName, Locations.Location location)
        {
            // create subfolder in \images\
            string newDirectory = Path.Combine(ThisApplication.ProjectFile.ImageFolder, subfolderName);
            Directory.CreateDirectory(newDirectory);

            MetadataErrors = 0;
            foreach (string file in files)
            {
                // create image file oject
                try
                {
                    ImageFile image = ImageFile.NewToDatabase(file, subfolderName, location);
                    List.Add(image);
                }
                catch (ArgumentException)
                {
                    MetadataErrors++;
                }
                //ImageFile image = ImageFile.NewToDatabase(file, subfolderName, location);
                //List.Add(image);
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

        /// <summary>
        /// Method to remove the list item with the given sqlite id.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveBySqliteId(int id)
        {
            List.RemoveAt(GetListIdBySqliteId(id));
        }

        /// <summary>
        /// Method to replace an image file object by its sqlite id with another image in the list of loaded items.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageFileReplacement"></param>
        public void ReplaceBySqliteId(int id, ImageFile imageFileReplacement)
        {
            List[GetListIdBySqliteId(id)] = imageFileReplacement;
        }
    }
}