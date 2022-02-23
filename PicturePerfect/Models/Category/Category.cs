using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public class Category : CategoryBase
    {
        /// <summary>
        /// Get or set the list of subcategories associated with this category.
        /// </summary>
        public List<SubCategory> SubCategories { get; set; } = new() { };

        /// <summary>
        /// Creates a new instance of the top-category class.
        /// </summary>
        public Category()
        {

        }

        /// <summary>
        /// Method to add this category instance to the database.
        /// </summary>
        public void Create()
        {
            Database.AddCategory(this);
        }

        /// <summary>
        /// Method to set the private property.
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }
    }
}
