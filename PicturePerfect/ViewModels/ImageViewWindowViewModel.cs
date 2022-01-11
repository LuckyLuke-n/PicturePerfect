using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.ViewModels
{
    public class ImageViewWindowViewModel : ViewModelBase
    {
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
        }
    }
}
