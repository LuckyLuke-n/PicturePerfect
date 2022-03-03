using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
