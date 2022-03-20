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
    internal class LocationWindowViewModel : ViewModelBase
    {
        #region Color and font properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        public static int LargeFontSize => 23;
        #endregion

        #region ListView properties
        /// <summary>
        /// Linked to the observable collection in the class Locations.
        /// </summary>
        public Locations Locations => LoadedLocations;

        private Locations.Location locationSeleted;
        /// <summary>
        /// Get or set the selected location object.
        /// </summary>
        public Locations.Location LocationSelected
        {
            get { return locationSeleted; }
            set { this.RaiseAndSetIfChanged(ref locationSeleted, value); SetGui(); }
        }

        private int locationSeletedIndex = -1;
        /// <summary>
        /// Get or set the list index for the selected location.
        /// </summary>
        public int LocationSeletedIndex
        {
            get { return locationSeletedIndex; }
            set { this.RaiseAndSetIfChanged(ref locationSeletedIndex, value); }
        }
        #endregion

        #region Location properties
        private bool isUnProtectedLocation = true;
        /// <summary>
        /// Get or set weather the selected element is a unprotected location object or not. "None" is protected.
        /// </summary>
        public bool IsUnProtectedLocation
        {
            get { return isUnProtectedLocation; }
            set { this.RaiseAndSetIfChanged(ref isUnProtectedLocation, value); }
        }

        private int locationIdSelected = 0;
        /// <summary>
        /// Get or set the id of the selected location.
        /// </summary>
        public int LocationIdSelected
        {
            get { return locationIdSelected; }
            set { this.RaiseAndSetIfChanged(ref locationIdSelected, value); }
        }

        private string locationNameSelected = string.Empty;
        /// <summary>
        /// Get or set the name of the selected location.
        /// </summary>
        public string LocationNameSelected
        {
            get { return locationNameSelected; }
            set { this.RaiseAndSetIfChanged(ref locationNameSelected, value); }
        }

        private string locationGeoTagSelected = string.Empty;
        /// <summary>
        /// Get or set the geo tag of the selected location.
        /// </summary>
        public string LocationGeoTagSelected
        {
            get { return locationGeoTagSelected; }
            set { this.RaiseAndSetIfChanged(ref locationGeoTagSelected, value); }
        }

        private string locationNotesSelected = string.Empty;
        /// <summary>
        /// Get or set the notes of the selected location.
        /// </summary>
        public string LocationNotesSelected
        {
            get { return locationNotesSelected; }
            set { this.RaiseAndSetIfChanged(ref locationNotesSelected, value); }
        }
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> AddLocationCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteSelectedLocationCommand { get; }
        public ReactiveCommand<Unit, Unit> EditLocationCommand { get; }
        #endregion

        public LocationWindowViewModel()
        {
            AddLocationCommand = ReactiveCommand.Create(RunAddLocationCommand);
            DeleteSelectedLocationCommand = ReactiveCommand.Create(RunDeleteSelectedLocationCommandAsync);
            EditLocationCommand = ReactiveCommand.Create(RunEditLocationCommand);
        }

        /// <summary>
        /// Method to set the gui elements with the currently selected location object.
        /// </summary>
        private void SetGui()
        {
            // check if location is "None" --> prevent editing
            if (LocationSelected.Id == 1)
            {
                // set elements in gui to disabled
                IsUnProtectedLocation = false;
            }
            else
            {
                // any other property
                LocationIdSelected = LocationSelected.Id;
                LocationNameSelected = LocationSelected.Name;
                LocationGeoTagSelected = LocationSelected.GeoTag;
                LocationNotesSelected = LocationSelected.Notes;
                IsUnProtectedLocation = true;
            }
        }

        /// <summary>
        /// Method to call the methods to add a loaction.
        /// </summary>
        private void RunAddLocationCommand()
        {

        }

        /// <summary>
        /// Method to call the methods to delete a loaction.
        /// </summary>
        private async void RunDeleteSelectedLocationCommandAsync()
        {
            MessageBox.MessageBoxResult result = await MessageBox.Show($"Deleting the location '{LocationSelected.Name}' will unlink all images from this location and remove the location itself.", null, MessageBox.MessageBoxButtons.OkCancel, MessageBox.MessageBoxIcon.Question);

            if (result == MessageBox.MessageBoxResult.Ok)
            {
                // check if selected location is "None"
                if (LocationSelected.Id == 1)
                {
                    // None
                    _ = await MessageBox.Show("This location cannot be deleted.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
                }
                else
                {
                    // any other location
                    // remove from database and observable collection
                    Locations.List[LocationSeletedIndex].Delete();
                    Locations.List.Remove(LocationSelected);
                }
            }
        }

        /// <summary>
        /// Method to call the methods to edit a loaction.
        /// </summary>
        private void RunEditLocationCommand()
        {

        }
    }
}
