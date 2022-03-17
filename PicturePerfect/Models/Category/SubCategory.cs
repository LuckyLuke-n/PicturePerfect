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
            Id = Database.AddSubcategory(this);
        }

        /// <summary>
        /// Method to commit the property contents to the database.
        /// </summary>
        /// <returns>Returns the category corresponding to the changed subcategory.</returns>
        public Category CommitChanges()
        {
            return Database.CommitSubCategoryProperties(this);
        }
    }
}
