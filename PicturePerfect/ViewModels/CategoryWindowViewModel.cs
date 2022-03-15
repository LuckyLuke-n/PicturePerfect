using PicturePerfect.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.ViewModels
{
    internal class CategoryWindowViewModel : ViewModelBase
    {
        #region Color and font properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        public static int LargeFontSize => 23;
        #endregion

        #region New subcategory
        private string newSubCategory1Name = string.Empty;
        /// <summary>
        /// Get or set the name for the new subcategory 1.
        /// </summary>
        public string NewSubCategory1Name
        {
            get { return newSubCategory1Name; }
            set { this.RaiseAndSetIfChanged(ref newSubCategory1Name, value); }
        }

        private string newSubCategory2Name = string.Empty;
        /// <summary>
        /// Get or set the name for the new subcategory 2.
        /// </summary>
        public string NewSubCategory2Name
        {
            get { return newSubCategory2Name; }
            set { this.RaiseAndSetIfChanged(ref newSubCategory2Name, value); }
        }
        #endregion

        #region Visibilty of gui elements
        private bool isVisibleAddSubCategory1 = false;
        public bool IsVisibleAddSubCategory1
        {
            get { return isVisibleAddSubCategory1; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddSubCategory1, value); }
        }

        private bool isVisibleAddSubCategory2 = false;
        public bool IsVisibleAddSubCategory2
        {
            get { return isVisibleAddSubCategory2; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddSubCategory2, value); }
        }
        #endregion

        #region TreeView properties
        private object selectedCategoryObject;
        /// <summary>
        /// Get or set the selected object in the treeview.
        /// </summary>
        public object SelectedCategoryObject
        {
            get { return selectedCategoryObject; }
            set { this.RaiseAndSetIfChanged(ref selectedCategoryObject, value); SetGui(); }
        }

        /// <summary>
        /// Get the categories tree object of the selected data from the view model base.
        /// </summary>
        public CategoriesTree CategoriesTree => LoadedCategoriesTree;
        #endregion

        #region Category or Subcategory properties
        private int selectedId;
        /// <summary>
        /// Get or set the selected id.
        /// </summary>
        public int SelectedId
        {
            get { return selectedId; }
            set { this.RaiseAndSetIfChanged(ref selectedId, value); }
        }

        private string selectedName = string.Empty;
        /// <summary>
        /// Get or set the selected name.
        /// </summary>
        public string SelectedName
        {
            get { return selectedName; }
            set { this.RaiseAndSetIfChanged(ref selectedName, value); }
        }

        private string selectedNotes = string.Empty;
        /// <summary>
        /// Get or set the selected notes.
        /// </summary>
        public string SelectedNotes
        {
            get { return selectedNotes; }
            set { this.RaiseAndSetIfChanged(ref selectedNotes, value); }
        }

        private bool isCategory = false;
        /// <summary>
        /// Get or set weather the selected element is a category object or not.
        /// </summary>
        public bool IsCategory
        {
            get { return isCategory; }
            set { this.RaiseAndSetIfChanged(ref isCategory, value); }
        }

        private bool isUnProtectedCategory = true;
        /// <summary>
        /// Get or set weather the selected element is a unprotected category object or not. "All" and "None" are protected.
        /// </summary>
        public bool IsUnProtectedCategory
        {
            get { return isUnProtectedCategory; }
            set { this.RaiseAndSetIfChanged(ref isUnProtectedCategory, value); }
        }

        private List<SubCategory> subCategoriesList = new();
        /// <summary>
        /// Get or set the subcategory list.
        /// </summary>
        public List<SubCategory> SubCategoriesList
        {
            get { return subCategoriesList; }
            set { this.RaiseAndSetIfChanged(ref subCategoriesList, value); }
        }

        private List<SubCategory> subCategoriesListAll = new();
        /// <summary>
        /// Get or set the subcategory list.
        /// </summary>
        public List<SubCategory> SubCategoriesListAll
        {
            get { return subCategoriesListAll; }
            set { this.RaiseAndSetIfChanged(ref subCategoriesListAll, value); }
        }
        #endregion


        #region Commands
        public ReactiveCommand<Unit, Unit> ToggleVisibilitySubCategoryCommand { get; }
        #endregion

        public CategoryWindowViewModel()
        {
            ToggleVisibilitySubCategoryCommand = ReactiveCommand.Create(RunToggleVisibilitySubCategoryCommand);
        }

        /// <summary>
        /// Method to set the blanks in the category window
        /// </summary>
        private void SetGui()
        {
            if (SelectedCategoryObject.GetType() == typeof(Category))
            {
                // Selection was a category
                IsCategory = true;
                IsUnProtectedCategory = true;
                Category category = (Category)SelectedCategoryObject;
                SelectedId = category.Id;
                SelectedName = category.Name;
                SelectedNotes = category.Notes;
                SubCategoriesList = LoadedCategoriesTree.Tree[LoadedCategoriesTree.Tree.IndexOf(category)].SubCategories;
                SubCategoriesListAll = AllSubCategories();

                // Load a list of all subcategories except the already linked subcategories
                List<SubCategory> AllSubCategories()
                {
                    List<SubCategory> listAll = CategoriesTree.LoadAllSubcategories();

                    foreach (SubCategory subCategory in SubCategoriesList)
                    {
                        // remove subcategory from list of all subcategories if possible
                        listAll.Remove(subCategory);
                    }

                    return listAll;
                };

                if (category.Id == 1 || category.Id == 2)
                {
                    // category "All" or category "None"
                    IsCategory = false;
                    IsUnProtectedCategory = false;
                }

            }
            else
            {
                // selection was a subcategory
                IsCategory = false;
                IsUnProtectedCategory = true;
                SubCategory subCategory = (SubCategory)SelectedCategoryObject;
                SelectedId = subCategory.Id;
                SelectedName = subCategory.Name;
                SelectedNotes = subCategory.Notes;
            }
        }

        /// <summary>
        /// Command to toggle the add sub category 1 visibility.
        /// </summary>
        private void RunToggleVisibilitySubCategoryCommand()
        {
            IsVisibleAddSubCategory1 = !IsVisibleAddSubCategory1;
        }
    }
}
