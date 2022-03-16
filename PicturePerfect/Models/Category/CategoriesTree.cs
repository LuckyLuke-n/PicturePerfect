using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PicturePerfect.Models
{
    public class CategoriesTree
    {
        /// <summary>
        /// Get a list of categories.
        /// </summary>
        public ObservableCollection<Category> Tree { get; private set; } = new ObservableCollection<Category>();

        /// <summary>
        /// Creates a new instance of the categories tree class.
        /// </summary>
        public CategoriesTree()
        {
        }

        /// <summary>
        /// Method to load all categories from the database.
        /// </summary>
        public void LoadTree()
        {
            Tree.Clear();
            List<Category> categories = Database.LoadAllCategories();

            // repopulate list with category objects
            categories.ForEach(categroy => Tree.Add(categroy));
        }

        /// <summary>
        /// Method to load a list containing all unlinked subcategories.
        /// </summary>
        /// <returns>Returns a list of subcategories.</returns>
        public List<SubCategory> LoadUnlinkedSubCategories()
        {
            return Database.LoadUnlinkedSubCategories();
        }
    }
}
