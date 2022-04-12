using Avalonia.Media.Imaging;
using System.Linq;
using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.ComponentModel;
using static PicturePerfect.Models.RawConverter;

namespace PicturePerfect.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        # region Color properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        #endregion

        #region Fonts and Sizes
        public static int MenuBarWidth => 250;
        public static int SearchBoxWidth => MenuBarWidth - 55;
        public static int LargeFontSize => 23;
        #endregion

        #region Favorite images for home page
        private Bitmap imageNo1 = ThisApplication.PlaceholderImage;
        public Bitmap ImageNo1
        {
            get { return imageNo1; }
            private set { this.RaiseAndSetIfChanged(ref imageNo1, value); }
        }

        private Bitmap imageNo2 = ThisApplication.PlaceholderImage;
        public Bitmap ImageNo2
        {
            get { return imageNo2; }
            private set { this.RaiseAndSetIfChanged(ref imageNo2, value); }
        }

        private Bitmap imageNo3 = ThisApplication.PlaceholderImage;
        public Bitmap ImageNo3
        {
            get { return imageNo3; }
            private set { this.RaiseAndSetIfChanged(ref imageNo3, value); }
        }

        private Bitmap imageNo4 = ThisApplication.PlaceholderImage;
        public Bitmap ImageNo4
        {
            get { return imageNo4; }
            private set { this.RaiseAndSetIfChanged(ref imageNo4, value); }
        }

        private static string notes = string.Empty;
        /// <summary>
        /// Get the notes to be displayed in the home tab. When the setter is used, the notes will be saved to the project file when a project is loaded.
        /// </summary>
        public string Notes
        {
            get { return notes; }
            set { this.RaiseAndSetIfChanged(ref notes, value); if (ProjectIsLoaded == true) { ThisApplication.ProjectFile.Notes = value; } }
        }
        #endregion

        #region Page "Images" Properties
        private ImageFile imageFile = new();
        /// <summary>
        /// Get the image file object of the selected image. Set the object and the static property in the view model base for hand over to other windows.
        /// </summary>
        public ImageFile ImageFile
        {
            get { return imageFile; }
            set
            {
                imageFile = value;
                // set the properties in the view model base
                SelectedImageFile = value;
            }
        }

        /// <summary>
        /// Get or set the index of the selected image in the collection. This will also set the property in the view model base to enable hand over to other windows.
        /// </summary>
        public int SelectedIndex
        {
            get { return SelectedImageIndex; }
            set
            {
                //selectedIndex = value;
                // set the properties in the view model base
                SelectedImageIndex = value;
            }
        }

        /// <summary>
        /// Get the image files object of the images in the database from the view model base.
        /// </summary>
        public ImageFiles ImageFilesDatabase => LoadedImageFiles;

        /// <summary>
        /// Get the categories tree object of the selected data from the view model base.
        /// </summary>
        public CategoriesTree CategoriesTree => LoadedCategoriesTree;

        /// <summary>
        /// Get the locations from the base view model.
        /// </summary>
        public Locations Locations => LoadedLocations;

        private bool hideImagesDialog = false;
        /// <summary>
        /// Get or set the property to hide or show the select images section.
        /// </summary>
        public bool HideImagesDialog
        {
            get { return hideImagesDialog; }
            set { this.RaiseAndSetIfChanged(ref hideImagesDialog, value); }
        }
        #endregion

        #region Page "RawConverter" properties
        private bool hideRawFilesDialog = false;
        /// <summary>
        /// Get or set the property to hide or show the select raw files section.
        /// </summary>
        public bool HideRawFilesDialog
        {
            get { return hideRawFilesDialog; }
            set { this.RaiseAndSetIfChanged(ref hideRawFilesDialog, value); }
        }

        private int selectedIndexRawConverter;
        /// <summary>
        /// Get or set the selected index in the raw converter list.
        /// </summary>
        public int SelectedIndexRawConverter
        {
            get { return selectedIndexRawConverter; }
            set { this.RaiseAndSetIfChanged(ref selectedIndexRawConverter, value); }
        }

        /// <summary>
        /// Get the image files loaded into the raw converter from the view model base.
        /// </summary>
        public RawConverter RawConverter => LoadedRawConverter;

        /// <summary>
        /// Get or set the background worker for the raw converter.
        /// </summary>
        private BackgroundWorker BackgroundWorkerRawConverter { get; set; } = new();
        #endregion

        #region Settings
        public static string NamingConventionDescription => File.ReadAllText("Resources/Descriptions/naming_convention.txt");
        public static string FileTypeDescription => File.ReadAllText("Resources/Descriptions/file_types.txt");
        public static string ViewDescription => File.ReadAllText("Resources/Descriptions/view_settings.txt");
        public static string ExternalViewerDescription => File.ReadAllText("Resources/Descriptions/external_viewer.txt");

        /// <summary>
        /// Get a list of possible separators. This value will be saved to the project file.
        /// </summary>
        public List<string> Separators { get; } = new() { "_", "-", "+", ".", ",", "Space"};
        private string? separator = null;
        /// <summary>
        /// Get or set the separator character for folder naming. This value will be saved to the project file.
        /// </summary> 
        public string Separator
        {
            get { return separator; }
            set { this.RaiseAndSetIfChanged(ref separator, value); ThisApplication.ProjectFile.Separator = value; }
        }
        private bool useSeparator = false;
        /// <summary>
        /// Get or set the value if separtor suffix folder naming should be used. This value will be saved to the project file.
        /// </summary>
        public bool UseSeparator
        {
            get { return useSeparator; }
            set { this.RaiseAndSetIfChanged(ref useSeparator, value); ThisApplication.ProjectFile.UseSeparator = value; }
        }

        private bool nefFilesChecked = false;
        /// <summary>
        /// Get or set the property for using raw files as input.
        /// </summary>
        public bool NefFilesChecked
        {
            get { return nefFilesChecked; }
            set { this.RaiseAndSetIfChanged(ref nefFilesChecked, value); ThisApplication.ProjectFile.NefFilesChecked = value; }
        }

        private bool orfFilesChecked = false;
        /// <summary>
        /// Get or set the property for using orf files as input.
        /// </summary>
        public bool OrfFilesChecked
        {
            get { return orfFilesChecked; }
            set { this.RaiseAndSetIfChanged(ref orfFilesChecked, value); ThisApplication.ProjectFile.OrfFilesChecked = value; }
        }

        private bool jpgFilesChecked = false;
        /// <summary>
        /// Get or set the property for using jpg files as input.
        /// </summary>
        public bool JpgFilesChecked
        {
            get { return jpgFilesChecked; }
            set { this.RaiseAndSetIfChanged(ref jpgFilesChecked, value); ThisApplication.ProjectFile.JpgFilesChecked = value; }
        }

        private bool pngFilesChecked = false;
        /// <summary>
        /// Get or set the property for using png files as input.
        /// </summary>
        public bool PngFilesChecked
        {
            get { return pngFilesChecked; }
            set { this.RaiseAndSetIfChanged(ref pngFilesChecked, value); ThisApplication.ProjectFile.PngFilesChecked = value; }
        }

        private bool tifFilesChecked = false;
        /// <summary>
        /// Get or set the property for using tif files as input.
        /// </summary>
        public bool TifFilesChecked
        {
            get { return tifFilesChecked; }
            set { this.RaiseAndSetIfChanged(ref tifFilesChecked, value); ThisApplication.ProjectFile.TifFilesChecked = value; }
        }

        private bool imageViewFullScreenChecked = false;
        /// <summary>
        /// Get or set the property for opening the image view window in full screen.
        /// </summary>
        public bool ImageViewFullScreenChecked
        {
            get { return imageViewFullScreenChecked; }
            set { this.RaiseAndSetIfChanged(ref imageViewFullScreenChecked, value); ThisApplication.ProjectFile.ImageViewFullScreenChecked = value; }
        }

        private string pathToExternalViewer = "";
        /// <summary>
        /// Get or set the path to the external image viewer.
        /// </summary>
        public string PathToExternalViewer
        {
            get { return pathToExternalViewer; }
            set { this.RaiseAndSetIfChanged(ref pathToExternalViewer, value); ThisApplication.ProjectFile.PathToExternalViewer = value; }
        }

        #endregion

        #region Status Bar and search box
        private int percentageProgressBar = 0;
        public int PercentageProgressBar
        {
            get { return percentageProgressBar; }
            private set { this.RaiseAndSetIfChanged(ref percentageProgressBar, value); }
        }

        private string labelProgressBar = "0%";
        public string LabelProgressBar
        {
            get { return labelProgressBar; }
            private set { this.RaiseAndSetIfChanged(ref labelProgressBar, value); }
        }

        private bool isIndeterminate = false;
        /// <summary>
        /// Get or set weather the progressbar is indeterminate or not.
        /// </summary>
        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
            set { this.RaiseAndSetIfChanged(ref isIndeterminate, value); }
        }

        private bool hideFileDialog = false;
        /// <summary>
        /// Get or set the property to hilde or show the file dialog in the bottom row.
        /// </summary>
        public bool HideFileDialog
        {
            get { return hideFileDialog; }
            private set { this.RaiseAndSetIfChanged(ref hideFileDialog, value); }
        }

        public string inWorkItem = "No project loaded";
        /// <summary>
        /// Get the current in work project.
        /// </summary>
        public string InWorkProject
        {
            get { return inWorkItem; }
            private set { this.RaiseAndSetIfChanged(ref inWorkItem, value); }
        }

        private bool projectIsLoaded = false;
        /// <summary>
        /// Get the property indicating weather a project is loaded.
        /// This is used to make the settings page read-only in case no project file is loaded.
        /// </summary>
        public bool ProjectIsLoaded
        {
            get { return projectIsLoaded; }
            set { this.RaiseAndSetIfChanged(ref projectIsLoaded, value); }
        }

        private string searchQuery = string.Empty;
        /// <summary>
        /// Get or set the search query.
        /// </summary>
        public string SearchQuery
        {
            get { return searchQuery; }
            set { this.RaiseAndSetIfChanged(ref searchQuery, value); }
        }
        #endregion

        #region Input for paths from axaml.cs file
        private string pathToProjectFile = "Select a project file";
        /// <summary>
        /// Get and set the path to the project file or set the path.
        /// </summary>
        public string PathToProjectFile
        {
            get { return pathToProjectFile; }
            set { this.RaiseAndSetIfChanged(ref pathToProjectFile, value); }
        }

        private string pathToProjectFolder = "Select a folder for your project";
        /// <summary>
        /// Get and set the path to the project folder or set the path.
        /// </summary>
        public string PathToProjectFolder
        {
            get { return pathToProjectFolder; }
            set { this.RaiseAndSetIfChanged(ref pathToProjectFolder, value); }
        }

        private string pathToImageSourceFolder = "Select a source folder";
        /// <summary>
        /// Get and set the path to the project load images folder or set the path.
        /// </summary>
        public string PathToImageSourceFolder
        {
            get { return pathToImageSourceFolder; }
            set { this.RaiseAndSetIfChanged(ref pathToImageSourceFolder, value); }
        }

        private string pathToConvertInputFolder = "Select a source folder";
        /// <summary>
        /// Get and set the path to the folder containg the images for the raw converter input.
        /// </summary>
        public string PathToConvertInputFolder
        {
            get { return pathToConvertInputFolder; }
            set { this.RaiseAndSetIfChanged(ref pathToConvertInputFolder, value); }
        }

        private string pathToConvertOutputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RawConverter_out");
        /// <summary>
        /// Get and set the path to the folder for the raw converter output.
        /// </summary>
        public string PathToConvertOutputFolder
        {
            get { return pathToConvertOutputFolder; }
            set { this.RaiseAndSetIfChanged(ref pathToConvertOutputFolder, value); }
        }

        private int defaultocationIndexSelected = -1;
        /// <summary>
        /// Get or set the list index of the selected location.
        /// </summary>
        public int DefaultLocationIndexSelected
        {
            get { return defaultocationIndexSelected; }
            set { this.RaiseAndSetIfChanged(ref defaultocationIndexSelected, value); }
        }
        #endregion

        #region TreeView category properties
        private object selectedCategoryObject;
        /// <summary>
        /// Get or set the selected object in the treeview.
        /// </summary>
        public object SelectedCategoryObject
        {
            get { return selectedCategoryObject; }
            set { this.RaiseAndSetIfChanged(ref selectedCategoryObject, value); LoadImagesByCategoryObject(); }
        }
        #endregion

        #region Commands images
        public ReactiveCommand<Unit, Unit> ShowImageCommand { get; }
        public ReactiveCommand<Unit, Unit> MarkAsFavorite1Command { get; }
        public ReactiveCommand<Unit, Unit> MarkAsFavorite2Command { get; }
        public ReactiveCommand<Unit, Unit> MarkAsFavorite3Command { get; }
        public ReactiveCommand<Unit, Unit> MarkAsFavorite4Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite1Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite2Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite3Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite4Command { get; }
        public ReactiveCommand<Unit, Unit> ToggleFileDialogCommand { get; }
        public ReactiveCommand<Unit, Unit> NewProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleLoadImagesCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadImagesCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteImageCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCategoriesCommand { get; }
        public ReactiveCommand<Unit, Unit> EditLocationsCommand { get; }
        #endregion

        #region Command RawConverter
        public ReactiveCommand<Unit, Unit> ToggleRawFileDialogCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadIntoRawConverterCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearRawConverterListCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearRawConverterItemCommand { get; }
        public ReactiveCommand<Unit, Unit> StartRawConverterCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelRawConverterCommand { get; }
        #endregion


        /// <summary>
        /// Creates an instance of the MainWindowViewModel.
        /// </summary>
        public MainWindowViewModel()
        {
            // commands for images section
            ShowImageCommand = ReactiveCommand.Create(RunShowImageCommandAsync);
            MarkAsFavorite1Command = ReactiveCommand.Create(RunMarkAsFavorite1CommandAsync);
            MarkAsFavorite2Command = ReactiveCommand.Create(RunMarkAsFavorite2CommandAsync);
            MarkAsFavorite3Command = ReactiveCommand.Create(RunMarkAsFavorite3CommandAsync);
            MarkAsFavorite4Command = ReactiveCommand.Create(RunMarkAsFavorite4CommandAsync);
            ShowFavorite1Command = ReactiveCommand.Create(RunShowFavorite1Command);
            ShowFavorite2Command = ReactiveCommand.Create(RunShowFavorite2Command);
            ShowFavorite3Command = ReactiveCommand.Create(RunShowFavorite3Command);
            ShowFavorite4Command = ReactiveCommand.Create(RunShowFavorite4Command);
            ToggleFileDialogCommand = ReactiveCommand.Create(RunToggleFileDialogCommand);
            NewProjectCommand = ReactiveCommand.Create(RunNewProjectCommandAsync);
            LoadProjectCommand = ReactiveCommand.Create(RunLoadProjectCommandAsync);
            LoadImagesCommand = ReactiveCommand.Create(RunLoadImagesCommandAsync);
            ToggleLoadImagesCommand = ReactiveCommand.Create(RunToggleLoadImagesCommand);
            DeleteImageCommand = ReactiveCommand.Create(RunDeleteImageCommandAsync);
            SearchCommand = ReactiveCommand.Create(RunSearchCommandAsync);
            EditCategoriesCommand = ReactiveCommand.Create(RunEditCategoriesCommandAsync);
            EditLocationsCommand = ReactiveCommand.Create(RunEditLocationsCommandAsync);

            // commands for raw converter section
            ToggleRawFileDialogCommand = ReactiveCommand.Create(RunToggleRawFileDialogCommand);
            LoadIntoRawConverterCommand = ReactiveCommand.Create(RunLoadIntoRawConverterCommandAsync);
            ClearRawConverterListCommand = ReactiveCommand.Create(RunClearRawConverterListCommand);
            ClearRawConverterItemCommand = ReactiveCommand.Create(RunClearRawConverterItemCommand);
            StartRawConverterCommand = ReactiveCommand.Create(RunStartRawConverterCommandAsync);
            CancelRawConverterCommand = ReactiveCommand.Create(RunCancelRawConverterCommand);

            // clear the temp folder
            ThisApplication.ClearTemp();
        }

        /// <summary>
        /// Method to set the control properties for the settings page.
        /// </summary>
        private void SetMainWindowPages()
        {
            // home page
            Notes = ThisApplication.ProjectFile.Notes;

            // images page
            LoadedImageFiles.LoadAll();
            LoadedCategoriesTree.LoadTree();
            LoadedLocations.LoadList();
            SetFavoriteImages();

            // load images section
            DefaultLocationIndexSelected = LoadedLocations.GetIndexForDefault();

            // settings page
            NefFilesChecked = ThisApplication.ProjectFile.NefFilesChecked;
            OrfFilesChecked = ThisApplication.ProjectFile.OrfFilesChecked;
            JpgFilesChecked = ThisApplication.ProjectFile.JpgFilesChecked;
            PngFilesChecked = ThisApplication.ProjectFile.PngFilesChecked;
            TifFilesChecked = ThisApplication.ProjectFile.TifFilesChecked;
            ImageViewFullScreenChecked = ThisApplication.ProjectFile.ImageViewFullScreenChecked;
            UseSeparator = ThisApplication.ProjectFile.UseSeparator;
            if (UseSeparator == true) { Separator = ThisApplication.ProjectFile.Separator; };
            PathToExternalViewer = ThisApplication.ProjectFile.PathToExternalViewer;
        }

        /// <summary>
        /// Set the favorite images in a background worker.
        /// </summary>
        private void SetFavoriteImages()
        {
            // new backgroundworker
            BackgroundWorker backgroundWorker = new();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;

            // run the worker
            backgroundWorker.RunWorkerAsync();


            void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
            {
                PercentageProgressBar = 0;
                LabelProgressBar = PercentageProgressBar.ToString() + "%";
                int i = 0;
                int[] favoriteIds = { ThisApplication.ProjectFile.Favorite1Id, ThisApplication.ProjectFile.Favorite2Id, ThisApplication.ProjectFile.Favorite3Id, ThisApplication.ProjectFile.Favorite4Id };
                Bitmap[] favoriteImages = { ImageNo1, ImageNo2, ImageNo3, ImageNo4 };

                // load image as bitmaps
                foreach (int id in favoriteIds)
                {
                    if (id == 0)
                    {
                        // no image set
                        favoriteImages[i] = ThisApplication.PlaceholderImage;
                    }
                    else
                    {
                        // set image if file exists
                        // if file does not exists (maybe deleted) set the place holder image
                        switch (i)
                        {
                            case 0:
                                if (File.Exists(ImageFile.LoadById(id).AbsolutePath)) { ImageNo1 = ImageFile.LoadById(id).ToBitmap(); }
                                else { ImageNo1 = ThisApplication.PlaceholderImage; }
                                break;
                            case 1:
                                if (File.Exists(ImageFile.LoadById(id).AbsolutePath)) { ImageNo2 = ImageFile.LoadById(id).ToBitmap(); }
                                else { ImageNo2 = ThisApplication.PlaceholderImage; }
                                break;
                            case 2:
                                if (File.Exists(ImageFile.LoadById(id).AbsolutePath)) { ImageNo3 = ImageFile.LoadById(id).ToBitmap(); }
                                else { ImageNo3 = ThisApplication.PlaceholderImage; }
                                break;
                            case 3:
                                if (File.Exists(ImageFile.LoadById(id).AbsolutePath)) { ImageNo4 = ImageFile.LoadById(id).ToBitmap(); }
                                else { ImageNo4 = ThisApplication.PlaceholderImage; }
                                break;
                            default:
                                break;
                        }                      
                    }
                    i++;
                    double percentage = (double)i / 4 * 100;
                    backgroundWorker.ReportProgress((int)percentage);
                }
            }

            void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                //PercentageProgressBar = 100;
            }

            void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                PercentageProgressBar = e.ProgressPercentage;
                LabelProgressBar = PercentageProgressBar.ToString() + "%";
            }
        }

        /// <summary>
        /// Method to open a new instance of the image view window.
        /// </summary>
        private async void RunShowImageCommandAsync()
        {
            // launch the view image window by using the ImageFile object stored int he ViewModelBase class
            if (ProjectIsLoaded == true)
            {
                new ImageViewWindow().Show();               
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to show the favorite image 1.
        /// </summary>
        private void RunShowFavorite1Command()
        {
            if (ThisApplication.ProjectFile.Favorite1Id == 0)
            {
                // no favorite selected
                MessageBox.Show("No favorite selected yet.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
            else
            {
                // set the static property in the view model base and in this vew model
                ImageFile = ImageFile.LoadById(ThisApplication.ProjectFile.Favorite1Id);
                RunShowImageCommandAsync();
            }
        }

        /// <summary>
        /// Method to show the favorite image 2.
        /// </summary>
        private void RunShowFavorite2Command()
        {
            if (ThisApplication.ProjectFile.Favorite2Id == 0)
            {
                // no favorite selected
                MessageBox.Show("No favorite selected yet.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
            else
            {
                // set the static property in the view model base and in this vew model
                ImageFile = ImageFile.LoadById(ThisApplication.ProjectFile.Favorite2Id);
                RunShowImageCommandAsync();
            }
        }

        /// <summary>
        /// Method to show the favorite image 3.
        /// </summary>
        private void RunShowFavorite3Command()
        {
            if (ThisApplication.ProjectFile.Favorite3Id == 0)
            {
                // no favorite selected
                MessageBox.Show("No favorite selected yet.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
            else
            {
                // set the static property in the view model base and in this vew model
                ImageFile = ImageFile.LoadById(ThisApplication.ProjectFile.Favorite3Id);
                RunShowImageCommandAsync();
            }
        }

        /// <summary>
        /// Method to show the favorite image 4.
        /// </summary>
        private void RunShowFavorite4Command()
        {
            if (ThisApplication.ProjectFile.Favorite4Id == 0)
            {
                // no favorite selected
                MessageBox.Show("No favorite selected yet.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
            else
            {
                // set the static property in the view model base and in this vew model
                ImageFile = ImageFile.LoadById(ThisApplication.ProjectFile.Favorite4Id);
                RunShowImageCommandAsync();
            }
        }

        /// <summary>
        /// Method to set the seleted image file as favorite 1.
        /// </summary>
        private async void RunMarkAsFavorite1CommandAsync()
        {
            if (ProjectIsLoaded == true)
                {
                ThisApplication.ProjectFile.Favorite1Id = ImageFile.Id;
                ImageNo1 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to set the seleted image file as favorite 2.
        /// </summary>
        private async void RunMarkAsFavorite2CommandAsync()
        {
            if (ProjectIsLoaded == true)
            {
                ThisApplication.ProjectFile.Favorite2Id = ImageFile.Id;
                ImageNo2 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to set the seleted image file as favorite 3.
        /// </summary>
        private async void RunMarkAsFavorite3CommandAsync()
        {
            if (ProjectIsLoaded == true)
            {
                ThisApplication.ProjectFile.Favorite3Id = ImageFile.Id;
                ImageNo3 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to set the seleted image file as favorite 4.
        /// </summary>
        private async void RunMarkAsFavorite4CommandAsync()
        {
            if (ProjectIsLoaded == true)
            {
                ThisApplication.ProjectFile.Favorite4Id = ImageFile.Id;
                ImageNo4 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to toggle the show file dialog bool.
        /// </summary>
        private void RunToggleFileDialogCommand()
        {
            HideFileDialog = !HideFileDialog;
            PathToProjectFolder = "Select a folder for your project";
            PathToProjectFile = "Select a project file";
        }

        /// <summary>
        /// Method to create a new project.
        /// </summary>
        private async void RunNewProjectCommandAsync()
        {
            if (PathToProjectFolder != "Select a folder for your project")
            {
                // set the global project file property
                ThisApplication.ProjectFile = ProjectFile.New(PathToProjectFolder);
                InWorkProject = ThisApplication.ProjectFile.ProjectName;
                // hide menu bar and clear boxes
                RunToggleFileDialogCommand();
                ProjectIsLoaded = true;

                // create new database before setting the main window pages
                Database.NewDatabase();

                SetMainWindowPages();
            }
            else
            {
                _ = await MessageBox.Show("Please select a path.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }      
        }

        /// <summary>
        /// Method to load an existing project.
        /// </summary>
        private async void RunLoadProjectCommandAsync()
        {
            if (PathToProjectFile != "Select a project file")
            {
                // set the global project file property
                ThisApplication.ProjectFile = ProjectFile.Load(PathToProjectFile);
                InWorkProject = ThisApplication.ProjectFile.ProjectName;
                // hide menu bar and clear boxes
                RunToggleFileDialogCommand();
                ProjectIsLoaded = true;
                SetMainWindowPages();
            }
            else
            {
                _ = await MessageBox.Show("Please select a path.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Method to select a folder to load images from.
        /// </summary>
        private async void RunLoadImagesCommandAsync()
        {
            if (ProjectIsLoaded == true)
            {
                if (PathToImageSourceFolder != "Select a source folder")
                {
                    // count files in the folder
                    FolderChecker folderChecker = new();
                    int count = folderChecker.CountFiles(PathToImageSourceFolder);
                    string message = $"{count} files will be added to your database. All images with be linked to the location '{LoadedLocations.List[DefaultLocationIndexSelected].Name}'." + Environment.NewLine +
                        "This might take some time depending on the writing speed of your hard drive." + Environment.NewLine +
                        "Do you want to go on?";
                    MessageBox.MessageBoxResult result = await MessageBox.Show(message, null, MessageBox.MessageBoxButtons.OkCancel, MessageBox.MessageBoxIcon.Question);

                    // check user selection and image count
                    if (result == MessageBox.MessageBoxResult.Ok && count > 0)
                    {
                        List<string> filesToAdd = new();
                        List<DateTime> creationDates = new();
                        // check if files are of correct files tye.
                        foreach (string path in Directory.GetFiles(PathToImageSourceFolder))
                        {
                            FileInfo fileInfo = new(path);
                            // check if the file is the type of the selected input files
                            if (ThisApplication.ProjectFile.GetInputFileTypes().Contains(fileInfo.Extension) == true)
                            {
                                // files is of correct file type
                                // can be added to database
                                filesToAdd.Add(path);
                                
                                // last write time is a work around since it was not possible to read the create date from exifdirectory
                                // last write time is the creation date for un.edited files.
                                // this list is needed for the folder naming in the next step.
                                creationDates.Add(fileInfo.LastWriteTime);
                            }
                        }

                        // subfolder name string based on settings
                        string name = string.Empty;

                        // get min and max date
                        string minDate = creationDates.Min(date => date).ToString("yyyy-MM-dd");
                        string maxDate = creationDates.Max(date => date).ToString("yyyy-MM-dd");

                        // check folder naming convention
                        if (ThisApplication.ProjectFile.UseSeparator == true)
                        {
                            // get the name of the directory and split it by using the separator. Take string before first separator occurence.
                            string prefix = new DirectoryInfo(PathToImageSourceFolder).Name.Split(ThisApplication.ProjectFile.Separator)[0];
                            name = $"{prefix}_{minDate}_to_{maxDate}";
                        }
                        else
                        {
                            name = $"{minDate}_to_{maxDate}";
                        }

                        // add files to database
                        Locations.Location autoTagLocation = LoadedLocations.List[DefaultLocationIndexSelected];
                        LoadedImageFiles.AddImages(files: filesToAdd, subfolderName: name, location: autoTagLocation);

                        // handle the errors and notify user
                        if (LoadedImageFiles.MetadataErrors > 0)
                        {
                            _ = await MessageBox.Show($"Error while extracting advanced meta data in {LoadedImageFiles.MetadataErrors} case/s.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
                        }

                    }
                    // hide load folder section
                    RunToggleLoadImagesCommand();
                }
                else
                {
                    _ = await MessageBox.Show("Please select a path.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
                }
            }
            else
            {
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to hide or show the load images section.
        /// </summary>
        private void RunToggleLoadImagesCommand()
        {
            HideImagesDialog = !HideImagesDialog;
            PathToImageSourceFolder = "Select a source folder";
        }

        /// <summary>
        /// Method to load images by category or subcategory.
        /// </summary>
        private void LoadImagesByCategoryObject()
        {
            if (SelectedCategoryObject.GetType() == typeof(Category))
            {
                // Selection was a category
                Category category = (Category)SelectedCategoryObject;
                LoadedImageFiles.LoadByCategory(category);
            }
            else
            {
                // selection was a subcategory
                SubCategory subCategory = (SubCategory)SelectedCategoryObject;
                LoadedImageFiles.LoadBySubCategory(subCategory);
            }
        }

        /// <summary>
        /// Method to run delete the selected image.
        /// </summary>
        private async void RunDeleteImageCommandAsync()
        {
            if (ProjectIsLoaded == true)
            {
                string message = $"Are you sure you want to delete the image {ImageFile.Name} from the database?";
                MessageBox.MessageBoxResult result = await MessageBox.Show(message, null, MessageBox.MessageBoxButtons.YesNo, MessageBox.MessageBoxIcon.Question);

                if (result == MessageBox.MessageBoxResult.Yes)
                {
                    // delete from database
                    ImageFile.DeleteFromDatabase();

                    // remove from list
                    LoadedImageFiles.RemoveBySqliteId(ImageFile.Id);
                }
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to run the search against the database.
        /// </summary>
        private async void RunSearchCommandAsync()
        {
            if (ProjectIsLoaded == true)
            {
                LoadedImageFiles.Search(SearchQuery);
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to open the window for editing the categories.
        /// </summary>
        private async void RunEditCategoriesCommandAsync()
        {
            if (ProjectIsLoaded)
            {
                new CategoryWindow().Show();
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to open a new window for editing the locations.
        /// </summary>
        private async void RunEditLocationsCommandAsync()
        {
            if (ProjectIsLoaded)
            {
                new LocationWindow().Show();
            }
            else
            {
                // no project file loaded
                _ = await MessageBox.Show("Please load a project file to go on.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Method to toggle the show raw file dialog bool.
        /// </summary>
        private void RunToggleRawFileDialogCommand()
        {
            HideRawFilesDialog = !HideRawFilesDialog;
            PathToConvertInputFolder = "Select a source folder";
            PathToConvertOutputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RawConverter_out");
        }

        /// <summary>
        /// Method add files to the raw converter.
        /// </summary>
        private async void RunLoadIntoRawConverterCommandAsync()
        {
            if (PathToConvertInputFolder != "Select a source folder")
            {
                List<string> filesToAdd = new();
                // check if files are of correct files tye.
                foreach (string path in Directory.GetFiles(PathToConvertInputFolder))
                {
                    filesToAdd.Add(path);
                }

                // add files to converter list
                int numberOfFilesAdded = LoadedRawConverter.AddToRawConverter(files: filesToAdd);

                // hide load folder section
                RunToggleRawFileDialogCommand();
                _ = await MessageBox.Show($"{numberOfFilesAdded} raw images added to the RawConverter.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
            else
            {
                _ = await MessageBox.Show("Please select a path.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Method to clear the list of raw converter items.
        /// </summary>
        private void RunClearRawConverterListCommand()
        {
            LoadedRawConverter.RawFiles.Clear();
        }

        /// <summary>
        /// Method to delete a selected file from the raw converter list.
        /// </summary>
        private void RunClearRawConverterItemCommand()
        {
            LoadedRawConverter.RawFiles.RemoveAt(SelectedIndexRawConverter);
        }

        /// <summary>
        /// Method to run the async process of converting all images in the raw converter list.
        /// </summary>
        private void RunStartRawConverterCommandAsync()
        {
            // convert all files in the list
            void BackgroundWorkerRawConverter_DoWork(object sender, DoWorkEventArgs e)
            {
                // create folder
                Directory.CreateDirectory(PathToConvertOutputFolder);

                // initiate counter and progress bar
                int counter = 0;
                LabelProgressBar = "0%";
                PercentageProgressBar = 0;

                // convert all files
                foreach (RawFile rawImage in LoadedRawConverter.RawFiles)
                {
                    // check if cancellation was requested
                    if (BackgroundWorkerRawConverter.CancellationPending == true)
                    {
                        // abort requested
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        // no cancellation
                        rawImage.Convert(toFolder: PathToConvertOutputFolder);
                        counter++;

                        // current job percentage
                        // this must be a double value in order to prevent the percentage being 0 in case file count is >100
                        double percentProgress = (double)counter / LoadedRawConverter.RawFiles.Count * 100;
                        BackgroundWorkerRawConverter.ReportProgress((int)percentProgress);
                    }
                }
            }

            // all converted
            void BackgroundWorkerRawConverter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                // set progress bar properties
                LabelProgressBar = "100%";
                PercentageProgressBar = 100;
            }

            // update progressbar and value
            void BackgroundWorkerRawConverter_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                // set progress bar properties
                LabelProgressBar = $"{e.ProgressPercentage}%";
                PercentageProgressBar = e.ProgressPercentage;
            }

            // check if files where seleted
            if (LoadedRawConverter.RawFiles.Count > 0)
            {
                // pictures in list
                // new backgroundworker
                BackgroundWorkerRawConverter = new()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                BackgroundWorkerRawConverter.DoWork += BackgroundWorkerRawConverter_DoWork;
                BackgroundWorkerRawConverter.RunWorkerCompleted += BackgroundWorkerRawConverter_RunWorkerCompleted;
                BackgroundWorkerRawConverter.ProgressChanged += BackgroundWorkerRawConverter_ProgressChanged;

                // run the worker
                BackgroundWorkerRawConverter.RunWorkerAsync();
            }
            else
            {
                // no pictures have been added to the raw converter
                _ = MessageBox.Show("No items in converter list.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Method to cancel the background worker if it is running.
        /// </summary>
        private void RunCancelRawConverterCommand()
        {
            if (BackgroundWorkerRawConverter.IsBusy == true)
            {
                BackgroundWorkerRawConverter.CancelAsync();
            }
        }
    }
}
