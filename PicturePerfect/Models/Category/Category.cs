using System.Collections.Generic;

namespace PicturePerfect.Models
{
    public class Category : CategoryBase
    {
        /// <summary>
        /// Get or set the list of subcategories associated with this category.
        /// </summary>
        public List<SubCategory> SubCategories { get; set; } = new();

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
            Id = Database.AddCategory(this);
        }

        /// <summary>
        /// Method to set the private property.
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Method to link this category instance to a subcategory.
        /// </summary>
        /// <param name="subCategory"></param>
        public void LinkSubcategory(SubCategory subCategory)
        {
            Database.LinkCategoryToSubCategory(category: this, subCategory: subCategory);
            SubCategories.Add(subCategory);
        }

        /// <summary>
        /// Method to unlink a subcategory from a category. The subcategory's images are unlinked too.
        /// </summary>
        public void UnlinkSubCategory(SubCategory subCategory)
        {
            Database.UnlinkSubCategoryFromCategory(category: this, subCategory: subCategory);
        }

        /// <summary>
        /// Method to commit the property contents to the database.
        /// </summary>
        public void CommitChanges()
        {
            Database.CommitCategoryProperties(category: this);
        }
    }
}
