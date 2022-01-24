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
        public ObservableCollection<Category> Tree { get; private set; } = new() { new Category() { Name = "All" }, new Category() { Name = "None" } };

        /// <summary>
        /// Creates a new instance of the categories tree class.
        /// </summary>
        public CategoriesTree()
        {

        }

        /// <summary>
        /// Method to load all categories from the database.
        /// </summary>
        private void LoadTree()
        {

        }
    }
}
