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

        private List<SubCategory> subCategoriesListUnlinked = new();
        /// <summary>
        /// Get or set the subcategory list.
        /// </summary>
        public List<SubCategory> SubCategoriesListUnlinked
        {
            get { return subCategoriesListUnlinked; }
            set { this.RaiseAndSetIfChanged(ref subCategoriesListUnlinked, value); }
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
        public ReactiveCommand<Unit, Unit> DeleteSelectedCategoryObjectCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteSubCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateSubcategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveEditsCommand { get; }
        #endregion

        public CategoryWindowViewModel()
        {
            DeleteSubCategoryCommand = ReactiveCommand.Create(RunDeleteSubCategoryCommand);
            DeleteSelectedCategoryObjectCommand = ReactiveCommand.Create(RunDeleteSelectedCategoryObjectCommandAsync);
            CreateSubcategoryCommand = ReactiveCommand.Create(RunCreateSubcategoryCommandAsync);
            CreateCategoryCommand = ReactiveCommand.Create(RunCreateCategoryCommand);
            SaveEditsCommand = ReactiveCommand.Create(RunSaveEditsCommandAsync);
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
                SubCategoriesListUnlinked = CategoriesTree.LoadUnlinkedSubCategories();

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
        /// Method to get the list index a specific category in the category list.
        /// </summary>
        /// <returns>Returns the list index of type int.</returns>
        private int GetListIndexOfCategory(Category categoryInput)
        {
            int index = 0;
            int listIndex = -1;
            //Category selectedCategory = (Category)SelectedCategoryObject;

            // get the index of the current category in the category tree list.
            foreach (Category category in CategoriesTree.Tree)
            {
                if (category.Id == categoryInput.Id)
                {
                    listIndex = index;
                }
                index++;
            }

            return listIndex;
        }

        /// <summary>
        /// Method to get the list index a specific subcategory in it's categorie's subcategory list.
        /// </summary>
        /// <param name="subCategoryInput"></param>
        /// <returns></returns>
        private int GetListIndexOfSubCategory(SubCategory subCategoryInput)
        {
            int index = 0;
            int listIndex = -1;
            Category category = Category.LoadBySubCategory(subCategoryInput);

            // get the index of the current category in the category tree list.
            foreach (SubCategory subCategory in category.SubCategories)
            {
                if (subCategory.Id == subCategoryInput.Id)
                {
                    listIndex = index;
                }
                index++;
            }

            return listIndex;
        }

        /// <summary>
        /// Method to delete the selected category object. Either a category or a subcategory.
        /// </summary>
        private async void RunDeleteSelectedCategoryObjectCommandAsync()
        {
            // check if category or subcategory
            if (SelectedCategoryObject.GetType() == typeof(Category))
            {
                // category
                Category selectedCategory = (Category)SelectedCategoryObject;

                MessageBox.MessageBoxResult result = await MessageBox.Show($"Deleting the category '{selectedCategory.Name}' removes all subcategories. All links between images and this category will be reset to the category 'None'.", null, MessageBox.MessageBoxButtons.OkCancel, MessageBox.MessageBoxIcon.Information);
                
                if (result == MessageBox.MessageBoxResult.Ok)
                {
                    // prevent deleting all and none category
                    if (selectedCategory.Id == 1 || selectedCategory.Id == 2)
                    {
                        // selected category is All or None
                        _ = await MessageBox.Show("This category cannot be deleted.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
                    }
                    else
                    {
                        // selected category is any other category than All or None
                        int listIndex = GetListIndexOfCategory(selectedCategory);
                        CategoriesTree.Tree[listIndex].Delete();
                        // remove from observable collection to update the gui
                        CategoriesTree.Tree.RemoveAt(listIndex);
                        // reload the images for the data grid binding
                        LoadedImageFiles.LoadAll();
                    }
                }
            }
            else if (SelectedCategoryObject.GetType() == typeof(SubCategory))
            {
                // subcategory
                SubCategory subCategorySelected = (SubCategory)SelectedCategoryObject;
                MessageBox.MessageBoxResult result = await MessageBox.Show($"Deleting the subcategory '{subCategorySelected.Name}' removes all links between images and this subcategory. The images will stay linked to the category", null, MessageBox.MessageBoxButtons.OkCancel, MessageBox.MessageBoxIcon.Information);

                if (result == MessageBox.MessageBoxResult.Ok)
                {
                    Category category = Category.LoadBySubCategory(subCategorySelected);

                    // list indices
                    int categoryIndex = GetListIndexOfCategory(category);
                    int subCategoryIndex = GetListIndexOfSubCategory(subCategorySelected);

                    // edit database
                    CategoriesTree.Tree[categoryIndex].SubCategories[subCategoryIndex].Delete();
                    subCategorySelected.Delete();

                    // Change the observable collection
                    category.SubCategories.RemoveAt(subCategoryIndex);
                    CategoriesTree.Tree[categoryIndex] = category;

                    // reload the images for the data grid binding
                    LoadedImageFiles.LoadAll();
                }
            }
            else
            {
                // do nothing
                _ = await MessageBox.Show("Error processing input. No changed made to project.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Method to get call the methods to delete a subcategory from its category.
        /// </summary>
        private void RunDeleteSubCategoryCommand()
        {
            int listIndex = GetListIndexOfCategory((Category)SelectedCategoryObject);

            // call the unlink method
            if (listIndex != -1 && SubCategoryLinkedIndexSelected != -1)
            {
                // delete from sqlite
                CategoriesTree.Tree[listIndex].SubCategories[SubCategoryLinkedIndexSelected].Delete();
                // in binding list
                CategoriesTree.Tree[listIndex].SubCategories.RemoveAt(SubCategoryLinkedIndexSelected);
                // reload the images for the data grid binding
                LoadedImageFiles.LoadAll();
            }
        }

        /// <summary>
        /// Method to call the methods to save changed to the category or subcategory.
        /// </summary>
        private async void RunSaveEditsCommandAsync()
        {
            // check if category or subcategory
            if (SelectedCategoryObject.GetType() == typeof(Category))
            {
                // category
                Category categorySelected = (Category)SelectedCategoryObject;
                categorySelected.Name = SelectedName;
                categorySelected.Notes = SelectedNotes;
                categorySelected.CommitChanges();

                // get the id in the list
                int index = 0;
                int counter = 0;
                foreach (Category category in LoadedCategoriesTree.Tree)
                {
                    if (category.Id == categorySelected.Id)
                    {
                        index = counter;
                    }
                    counter++;
                }

                // edit the observable collection to update the main windows and the category window
                LoadedCategoriesTree.Tree[index] = categorySelected;
            }
            else if (SelectedCategoryObject.GetType() == typeof(SubCategory))
            {
                // subcategory
                SubCategory subCategorySelected = (SubCategory)SelectedCategoryObject;
                subCategorySelected.Name = SelectedName;
                subCategorySelected.Notes = SelectedNotes;
                Category correspondingCategory = subCategorySelected.CommitChanges();

                // get the id in the list for the corresponding category
                int index = 0;
                int counter = 0;
                foreach (Category category in LoadedCategoriesTree.Tree)
                {
                    if (category.Id == correspondingCategory.Id)
                    {
                        index = counter;
                    }
                    counter++;
                }

                // edit the observable collection to update the main windows and the category window
                LoadedCategoriesTree.Tree[index] = correspondingCategory;
            }
            else
            {
                // do nothing
                _ = await MessageBox.Show("Error processing input. No changed made to project.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
            }
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
                    int listIndex = GetListIndexOfCategory(selectedCategory);
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
