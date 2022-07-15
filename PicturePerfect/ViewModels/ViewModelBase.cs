using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;

namespace PicturePerfect.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        /// <summary>
        /// Get or set the id for the currently selected image id.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static int SelectedImageIndex { get; set; }
        /// <summary>
        /// Get or set the id for the currently selected image.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static ImageFile SelectedImageFile { get; set; } = new();
        /// <summary>
        /// Get or set the currently loaded images files object.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static ImageFiles LoadedImageFiles { get; set; } = new();
        /// <summary>
        /// Get or set the currently loaded categories tree object.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static CategoriesTree LoadedCategoriesTree { get; set; } = new();
        /// <summary>
        /// Get or set the locations available in the database.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static Locations LoadedLocations { get; set; } = new();
        /// <summary>
        /// Get or set the images loaded for the raw converter.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static RawConverter LoadedRawConverter { get; set; } = new();
        /// <summary>
        /// Get or set the filtering object selected in the treeviews. This can either be a location, category, or subcategory.
        /// </summary>
        public static object? SelectedFilteringObject { get; set; } = null;

        /// <summary>
        /// Method to load images by a category or location object. The images will be added to the LoadedImageFiles list.
        /// </summary>
        public void LoadImagesByFilter()
        {
            try
            {
                if (SelectedFilteringObject == null)
                {
                    // no selection made by user --> category selection is "All"
                    LoadedImageFiles.LoadAll();
                }
                else if (SelectedFilteringObject.GetType() == typeof(Category))
                {
                    // Selection was a category
                    Category category = (Category)SelectedFilteringObject;
                    LoadedImageFiles.LoadByCategory(category);
                }
                else if (SelectedFilteringObject.GetType() == typeof(SubCategory))
                {
                    // selection was a subcategory
                    SubCategory subCategory = (SubCategory)SelectedFilteringObject;
                    LoadedImageFiles.LoadBySubCategory(subCategory);
                }
                else if (SelectedFilteringObject.GetType() == typeof(Locations.Location))
                {
                    // selection was a location
                    LoadedImageFiles.LoadByLocation((Locations.Location)SelectedFilteringObject);
                }
                else
                {
                    string text = "An error occured. Selection could not be recognized as category or location.";
                    MessageBox.Show(text, null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string text = "An error occured. Filtering was von possible" + Environment.NewLine + ex.Message;
                MessageBox.Show(text, null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
            }
        }
    }
}
