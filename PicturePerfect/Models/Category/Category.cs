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
        /// Method to load a category by it's subcategory.
        /// </summary>
        /// <param name="subCategory"></param>
        /// <returns>Returns the category object.</returns>
        public static Category LoadBySubCategory(SubCategory subCategory)
        {
            return Database.LoadCategoryBySubCategory(subCategory);
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
        /// Method to commit the property contents to the database.
        /// </summary>
        public void CommitChanges()
        {
            Database.CommitCategoryProperties(category: this);
        }

        /// <summary>
        /// Method to delete this category from the database. This removes all connections to this category object.
        /// </summary>
        public void Delete()
        {
            Database.DeleteCategory(category: this);
        }
    }
}
