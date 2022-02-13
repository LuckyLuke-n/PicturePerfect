using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public class SubCategory : CategoryBase
    {
        /// <summary>
        /// Creates a new instance of the class sub-category.
        /// </summary>
        public SubCategory()
        {

        }

        /// <summary>
        /// Method to add a new subcategory to the database.
        /// </summary>
        public void Create()
        {
            Database.AddSubcategory(this);
        }
    }
}
