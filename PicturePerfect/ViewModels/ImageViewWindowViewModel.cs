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
        private int id;
        

        /// <summary>
        /// Get or set the image id for the image to be displayed in the image viewer.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { this.RaiseAndSetIfChanged(ref id, value); }
        }

        /// <summary>
        /// Created a new instance of the image view view model.
        /// </summary>
        /// <param name="id"></param>
        public ImageViewWindowViewModel(int id)
        {
            this.id = id;
        }
    }
}
