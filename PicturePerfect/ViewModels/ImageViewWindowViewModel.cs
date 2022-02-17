using Avalonia.Media.Imaging;
using Avalonia.Threading;
using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.ViewModels
{
    public class ImageViewWindowViewModel : ViewModelBase
    {
        #region Color properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        #endregion

        #region Visibilty of gui elements
        private bool isVisibleAddLocation = false;
        public bool IsVisibleAddLocation
        {
            get { return isVisibleAddLocation; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddLocation, value); }
        }

        private bool isVisibleAddCategory = false;
        public bool IsVisibleAddCategory
        {
            get { return isVisibleAddCategory; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddCategory, value); }
        }

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

        private bool moreInfoVisible = false;
        public bool MoreInfoVisible
        {
            get { return moreInfoVisible; }
            set { this.RaiseAndSetIfChanged(ref moreInfoVisible, value); }
        }

        private bool isIndeterminateBar = false;
        /// <summary>
        /// Get or set weather the progressbar is indeterminate or not.
        /// </summary>
        public bool IsIndeterminateBar
        {
            get { return isIndeterminateBar; }
            set { this.RaiseAndSetIfChanged(ref isIndeterminateBar, value); }
        }
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> ToggleVisibilityLocationCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleVisibilityCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleVisibilitySubCategory1Command { get; }
        public ReactiveCommand<Unit, Unit> ToggleVisibilitySubCategory2Command { get; }
        public ReactiveCommand<Unit, Unit> ToggleVisibilityMoreInfoCommand { get; }

        public ReactiveCommand<Unit, Unit> SaveLocationCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveSubCategory1Command { get; }
        public ReactiveCommand<Unit, Unit> SaveSubCategory2Command { get; }
        #endregion

        #region Image info
        private int imageId;
        /// <summary>
        /// Get or set the image id for the image to be displayed in the image viewer.
        /// </summary>
        public int ImageId
        {
            get { return imageId; }
            set { this.RaiseAndSetIfChanged(ref imageId, value); }
        }

        private string moreInfo = "";
        /// <summary>
        /// Get or set the more information section in the image view window. 
        /// </summary>
        public string MoreInfo
        {
            get => GetMoreInfo();
            set { this.RaiseAndSetIfChanged(ref moreInfo, value); }
        }

        private ImageFile imageFile = new();
        /// <summary>
        /// Get or set the image file for the image to currently be viewed.
        /// </summary>
        public ImageFile ImageFile
        {
            get { return imageFile; }
            set { this.RaiseAndSetIfChanged(ref this.imageFile, value); }
        }

        private Bitmap bitmapToDraw = ThisApplication.PlaceholderImage;
        /// <summary>
        /// Get the bitmap object for the image file object.
        /// </summary>
        public Bitmap BitmapToDraw
        {
            //get { return ImageFile.ToBitmap(); }
            get { return bitmapToDraw; }
            private set { this.RaiseAndSetIfChanged(ref bitmapToDraw, value); }
        }

        /// <summary>
        /// Get the categories tree object of the selected data. Set the object and the static property in the view model base for hand over to other windows.
        /// </summary>
        public CategoriesTree CategoriesTree => LoadedCategoriesTree;

        /// <summary>
        /// Get the locations available in the database.
        /// This is inherited from the ViewModelBase.
        /// </summary>
        public Locations Locations => LoadedLocations;
        #endregion Image info

        #region new location catrogory or sub-category
        private string newLocationName = string.Empty;
        /// <summary>
        /// Get or set the name for the new location.
        /// </summary>
        public string NewLocationName
        {
            get { return newLocationName; }
            set { this.RaiseAndSetIfChanged(ref newLocationName, value); }
        }

        private string newCategoryName = string.Empty;
        /// <summary>
        /// Get or set the name for the new category.
        /// </summary>
        public string NewCategoryName
        {
            get { return newCategoryName; }
            set { this.RaiseAndSetIfChanged(ref newCategoryName, value); }
        }

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


        /// <summary>
        /// Created a new instance of the image view view model.
        /// </summary>
        /// <param name="id"></param>
        public ImageViewWindowViewModel()
        {
            // inherited from base view model
            ImageFile = SelectedImageFile;

            // new backgroundworker
            BackgroundWorker backgroundWorker = new();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            // run the worker
            backgroundWorker.RunWorkerAsync();

            // convert and load the bitmap into the gui
            void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
            {
                IsIndeterminateBar = true;
                BitmapToDraw = ImageFile.ToBitmap();
            }

            // reset the progress bar property
            void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                IsIndeterminateBar = false;
            }


            ToggleVisibilityLocationCommand = ReactiveCommand.Create(RunToggleVisibilityLocationCommand);
            ToggleVisibilityCategoryCommand = ReactiveCommand.Create(RunToggleVisibilityCategoryCommand);
            ToggleVisibilitySubCategory1Command = ReactiveCommand.Create(RunToggleVisibilitySubCategory1Command);
            ToggleVisibilitySubCategory2Command = ReactiveCommand.Create(RunToggleVisibilitySubCategory2Command);
            ToggleVisibilityMoreInfoCommand = ReactiveCommand.Create(RunToggleVisibilityMoreInfoCommand);

            SaveLocationCommand = ReactiveCommand.Create(RunSaveLocationCommand);
            SaveCategoryCommand = ReactiveCommand.Create(RunSaveCategoryCommand);
            SaveSubCategory1Command = ReactiveCommand.Create(RunSaveSubCategory1Command);
            SaveSubCategory2Command = ReactiveCommand.Create(RunSaveSubCategory2Command);
        }

        /// <summary>
        /// Command to toggle the add location line visiblity.
        /// </summary>
        private void RunToggleVisibilityLocationCommand()
        {
            IsVisibleAddLocation = !IsVisibleAddLocation;
        }

        /// <summary>
        /// Command to toggle the add category visibility.
        /// </summary>
        private void RunToggleVisibilityCategoryCommand()
        {
            IsVisibleAddCategory = !IsVisibleAddCategory;
        }

        /// <summary>
        /// Command to toggle the add sub category 1 visibility.
        /// </summary>
        private void RunToggleVisibilitySubCategory1Command()
        {
            IsVisibleAddSubCategory1 = !IsVisibleAddSubCategory1;
        }

        /// <summary>
        /// Command to toggle the add subcategory 2 visibilty.
        /// </summary>
        private void RunToggleVisibilitySubCategory2Command()
        {
            IsVisibleAddSubCategory2 = !IsVisibleAddSubCategory2;
        }

        /// <summary>
        /// Command to toggle the visibility of the more info section.
        /// </summary>
        private void RunToggleVisibilityMoreInfoCommand()
        {
            MoreInfoVisible = !MoreInfoVisible;
        }

        /// <summary>
        /// Command save the new location location.
        /// </summary>
        private void RunSaveLocationCommand()
        {
            Locations.Location location = new();
            location.Name = NewLocationName;
            location.Create();

            // add new location as first item in list
            Locations.List.Add(location);
            Locations.List.Move(Locations.List.Count-1, 0);

            RunToggleVisibilityLocationCommand();
        }

        /// <summary>
        /// Command to save the new category.
        /// </summary>
        private void RunSaveCategoryCommand()
        {
            Category category = new();
            category.Name = NewCategoryName;
            category.Create();

            // add new category as first item in list
            CategoriesTree.Tree.Add(category);
            CategoriesTree.Tree.Move(CategoriesTree.Tree.Count-1, 0);

            RunToggleVisibilityCategoryCommand();
        }

        /// <summary>
        /// Command to save the new sub category 1.
        /// </summary>
        private void RunSaveSubCategory1Command()
        {
            SaveSubCategory(NewSubCategory1Name);
            RunToggleVisibilitySubCategory1Command();
        }

        /// <summary>
        /// Command to save the new sub category 2.
        /// </summary>
        private void RunSaveSubCategory2Command()
        {
            SaveSubCategory(NewSubCategory2Name);
            RunToggleVisibilitySubCategory2Command();
        }

        /// <summary>
        /// Method to get more metadate info.
        /// </summary>
        /// <returns>Returns a string containg the metadata.</returns>
        private string GetMoreInfo()
        {
            string moreInfo = string.Empty;

            if (ImageFile == null)
            {
                return moreInfo = "";
            }
            else
            {
                moreInfo = $"Camera maker: {ImageFile.Camera}" + Environment.NewLine +
                    $"ISO: {ImageFile.ISO}" + Environment.NewLine +
                    $"F-stop: {ImageFile.FStop}" + Environment.NewLine +
                    $"Exposure time: {ImageFile.ExposureTime}" + Environment.NewLine +
                    $"Exposure bias: {ImageFile.ExposureBias}" + Environment.NewLine +
                    $"Focal length: {ImageFile.FocalLength}";

                return moreInfo;
            }
           
        }

        /// <summary>
        /// Method to save a new subcategory to the database.
        /// </summary>
        /// <param name="name"></param>
        private void SaveSubCategory(string name)
        {
            SubCategory subCategory = new();
            subCategory.Name = name;
            subCategory.Create();
        }
    }
}
