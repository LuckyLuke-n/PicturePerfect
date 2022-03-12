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

        #region Commands
        public ReactiveCommand<Unit, Unit> ToggleVisibilitySubCategory1Command { get; }
        #endregion

        public CategoryWindowViewModel()
        {
            ToggleVisibilitySubCategory1Command = ReactiveCommand.Create(RunToggleVisibilitySubCategory1Command);
        }

        /// <summary>
        /// Command to toggle the add sub category 1 visibility.
        /// </summary>
        private void RunToggleVisibilitySubCategory1Command()
        {
            IsVisibleAddSubCategory1 = !IsVisibleAddSubCategory1;
        }
    }
}
