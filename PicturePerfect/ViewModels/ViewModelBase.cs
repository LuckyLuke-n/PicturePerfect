using PicturePerfect.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PicturePerfect.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        /// <summary>
        /// Get or set the id for the currently selected image id.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static int SelectedImageId { get; set; }
        /// <summary>
        /// Get or set the id for the currently selected image.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static ImageFile SelectedImageFile { get; set; } = new();
        /// <summary>
        /// Get or set the currently loaded images files object.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static ImageFiles LoadedImageFiles { get; set; } = new();
        /// <summary>
        /// Get or set the currently loaded categories tree object.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static CategoriesTree LoadedCategoriesTree { get; set; } = new();
        /// <summary>
        /// Get or set the locations available in the database.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static Locations LoadedLocations { get; set; } = new();
    }
}
