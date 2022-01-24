using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public class CategoryBase
    {
        /// <summary>
        /// Get the category id.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Get or set the category name.
        /// </summary>
        public string Name { get; set; } =  string.Empty;
        /// <summary>
        /// Get or set the notex for this category.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Creates a new instance of the category class.
        /// </summary>
        public CategoryBase()
        {

        }
    }
}
