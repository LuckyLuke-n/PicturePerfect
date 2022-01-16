﻿using Avalonia.Media.Imaging;
using PicturePerfect.Models;
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
        public static Bitmap ImageNo1 { get; private set; } = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/test/P5140045_Stockerpel.jpg");
        #endregion

        #region Visibilty of gui elements
        private bool isVisibleAddLocation = false;
        private bool isVisibleAddCategory = false;
        private bool isVisibleAddSubCategory1 = false;
        private bool isVisibleAddSubCategory2 = false;
        public bool IsVisibleAddLocation
        {
            get { return isVisibleAddLocation; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddLocation, value); }
        }
        public bool IsVisibleAddCategory
        {
            get { return isVisibleAddCategory; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddCategory, value); }
        }
        public bool IsVisibleAddSubCategory1
        {
            get { return isVisibleAddSubCategory1; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddSubCategory1, value); }
        }
        public bool IsVisibleAddSubCategory2
        {
            get { return isVisibleAddSubCategory2; }
            set { this.RaiseAndSetIfChanged(ref isVisibleAddSubCategory2, value); }
        }
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> ToggleVisibilityLocationCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleVisibilityCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleVisibilitySubCategory1Command { get; }
        public ReactiveCommand<Unit, Unit> ToggleVisibilitySubCategory2Command { get; }

        public ReactiveCommand<Unit, Unit> SaveLocationCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveSubCategory1Command { get; }
        public ReactiveCommand<Unit, Unit> SaveSubCategory2Command { get; }
        #endregion


        private int imageId;
        /// <summary>
        /// Get or set the image id for the image to be displayed in the image viewer.
        /// </summary>
        public int ImageId
        {
            get { return imageId; }
            set { this.RaiseAndSetIfChanged(ref imageId, value); }
        }

        /// <summary>
        /// Created a new instance of the image view view model.
        /// </summary>
        /// <param name="id"></param>
        public ImageViewWindowViewModel()
        {
            // inherited from base view model
            ImageId = SelectedImageId;

            ToggleVisibilityLocationCommand = ReactiveCommand.Create(RunToggleVisibilityLocationCommand);
            ToggleVisibilityCategoryCommand = ReactiveCommand.Create(RunToggleVisibilityCategoryCommand);
            ToggleVisibilitySubCategory1Command = ReactiveCommand.Create(RunToggleVisibilitySubCategory1Command);
            ToggleVisibilitySubCategory2Command = ReactiveCommand.Create(RunToggleVisibilitySubCategory2Command);

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
        /// Command save the new location location.
        /// </summary>
        private void RunSaveLocationCommand()
        {
            RunToggleVisibilityLocationCommand();
        }

        /// <summary>
        /// Command to save the new category.
        /// </summary>
        private void RunSaveCategoryCommand()
        {
            RunToggleVisibilityCategoryCommand();
        }

        /// <summary>
        /// Command to save the new sub category 1.
        /// </summary>
        private void RunSaveSubCategory1Command()
        {
            RunToggleVisibilitySubCategory1Command();
        }

        /// <summary>
        /// Command to save the new sub category 2.
        /// </summary>
        private void RunSaveSubCategory2Command()
        {
            RunToggleVisibilitySubCategory2Command();
        }
    }
}
