using PicturePerfect.Models;
using PicturePerfect.Views;
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

        #region Visibilty of gui elements
        private bool isVisibleAddSubCategory1 = false;
        public bool IsVisibleAddSubCategory1
        {
            get { return isVisibleAddSubCategory1; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddSubCategory1, value); }
        }
        #endregion

        #region TreeView properties
        private object selectedCategoryObject = null;
        /// <summary>
        /// Get or set the selected object in the treeview.
        /// </summary>
        public object SelectedCategoryObject
        {
            get { return selectedCategoryObject; }
            set { this.RaiseAndSetIfChanged(ref selectedCategoryObject, value); SetGui(); }
        }

        private SubCategory subCategoryLinkedSelected;
        /// <summary>
        /// Get or set the selection from the combobox containing the linked subcategories.
        /// </summary>
        public SubCategory SubCategoryLinkedSelected
        {
            get { return subCategoryLinkedSelected; }
            set { this.RaiseAndSetIfChanged(ref subCategoryLinkedSelected, value); }
        }

        private int subCategoryLinkedIndexSelected = -1;
        /// <summary>
        /// Get or set the index for the selected linked subcategory.
        /// </summary>
        public int SubCategoryLinkedIndexSelected
        {
            get { return subCategoryLinkedIndexSelected; }
            set { this.RaiseAndSetIfChanged(ref subCategoryLinkedIndexSelected, value); }
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

        private string newSubCategoryName = string.Empty;
        /// <summary>
        /// Get or set the name for the new category.
        /// </summary>
        public string NewSubCategoryName
        {
            get { return newSubCategoryName; }
            set { this.RaiseAndSetIfChanged(ref newSubCategoryName, value); }
        }
        #endregion


        #region Commands
        public ReactiveCommand<Unit, Unit> ToggleVisibilitySubCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> UnlinkCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> LinkCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateSubcategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCategoryCommand { get; }
        #endregion

        public CategoryWindowViewModel()
        {
            ToggleVisibilitySubCategoryCommand = ReactiveCommand.Create(RunToggleVisibilitySubCategoryCommand);
            UnlinkCategoryCommand = ReactiveCommand.Create(RunUnlinkCategoryCommand);
            LinkCategoryCommand = ReactiveCommand.Create(RunLinkCategoryCommand);
            CreateSubcategoryCommand = ReactiveCommand.Create(RunCreateSubcategoryCommandAsync);
            CreateCategoryCommand = ReactiveCommand.Create(RunCreateCategoryCommand);
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
                    List<int> indicesToRemove = new();
                    int index = 0;

                    foreach (SubCategory subCategory in listAll)
                    {
                        foreach (SubCategory assignedSubCategory in SubCategoriesList)
                        {
                            if (subCategory.Id == assignedSubCategory.Id) { indicesToRemove.Add(index); break; }
                        }

                        index++;
                    }

                    // order indices to remove descending to avoid mixed up index results after removing
                    // use a distinct list for safety
                    List<int> indicesToRemoveDistinct = indicesToRemove.Distinct().ToList();
                    List<int> indicesToDeleteDistinctAndOrdered =  indicesToRemoveDistinct.OrderByDescending(i => i).ToList();

                    // remove subcategories that are already assigned
                    indicesToDeleteDistinctAndOrdered.ForEach(i => listAll.RemoveAt(i));

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
        /// Method to get the list index of the currently selected category set by the binding.
        /// </summary>
        /// <returns>Returns the list index of type int.</returns>
        private int GetListIndexOfCategory()
        {
            int index = 0;
            int listIndex = -1;
            Category selectedCategory = (Category)SelectedCategoryObject;

            // get the index of the current category in the category tree list.
            foreach (Category category in CategoriesTree.Tree)
            {
                if (category.Id == selectedCategory.Id)
                {
                    listIndex = index;
                }
                index++;
            }

            return listIndex;
        }

        /// <summary>
        /// Command to toggle the add sub category visibility.
        /// </summary>
        private void RunToggleVisibilitySubCategoryCommand()
        {
            IsVisibleAddSubCategory1 = !IsVisibleAddSubCategory1;
        }

        /// <summary>
        /// Method to get call the methods to unlink a subcategory from its category.
        /// </summary>
        private void RunUnlinkCategoryCommand()
        {           
            int listIndex = GetListIndexOfCategory();

            // call the unlink method
            if (listIndex != -1 && SubCategoryLinkedIndexSelected != -1)
            {
                // unlink is sqlite
                CategoriesTree.Tree[listIndex].UnlinkSubCategory(SubCategoryLinkedSelected);
                // in binding list
                CategoriesTree.Tree[listIndex].SubCategories.RemoveAt(SubCategoryLinkedIndexSelected);
            }   
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunLinkCategoryCommand()
        {

        }

        /// <summary>
        /// Method to call the methods for creating a new subcategory and linking it to the currently set category selection.
        /// </summary>
        private async void RunCreateSubcategoryCommandAsync()
        {
            // check selection to avoid error when no selection is made
            if (SelectedCategoryObject != null)
            {
                // category was selected
                Category selectedCategory = (Category)SelectedCategoryObject;
                if (selectedCategory.Id == 1 || selectedCategory.Id == 2)
                {
                    // all or none
                    _ = await MessageBox.Show("This category cannot have subcategories.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
                }
                else
                {
                    // create new subcategory
                    SubCategory subCategory = new();
                    subCategory.Name = "New subcategory";
                    subCategory.Create();

                    // add to categories tree observable object
                    int listIndex = GetListIndexOfCategory();
                    CategoriesTree.Tree[listIndex].LinkSubcategory(subCategory);
                }
            }
        }

        /// <summary>
        /// Method to call the methods for creating a new category.
        /// </summary>
        private void RunCreateCategoryCommand()
        {
            Category category = new();
            category.Name = "New category";
            category.Create();

            // add new category as first item in list
            CategoriesTree.Tree.Add(category);
            CategoriesTree.Tree.Move(CategoriesTree.Tree.Count - 1, 0);
        }
    }
}
