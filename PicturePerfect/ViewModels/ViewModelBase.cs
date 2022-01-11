using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PicturePerfect.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        /// <summary>
        /// Get or set the id for the currently selected image.
        /// This makes it possible for the VMs to communicate with each other.
        /// </summary>
        public static int SelectedImageId { get; set; }
    }
}
