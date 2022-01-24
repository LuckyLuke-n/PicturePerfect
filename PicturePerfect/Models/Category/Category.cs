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
        /// Get the list of subcategories associated with this category.
        /// </summary>
        public List<SubCategory> SubCategories { get; private set; } = new() { };

        /// <summary>
        /// Creates a new instance of the top-category class.
        /// </summary>
        public Category()
        {

        }
    }
}
