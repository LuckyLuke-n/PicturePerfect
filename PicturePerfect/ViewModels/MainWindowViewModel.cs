using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Linq;
using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

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

        #region Defaults
        private static string PaceholderImagePath = "avares://PicturePerfect/Assets/Drawables/image_placeholder.jpg";
        public static Bitmap PlaceholderImage => BitmapValueConverter.Convert("avares://PicturePerfect/Assets/Drawables/image_placeholder.jpg");
        #endregion

        #region Favorite images for home page
        public static Bitmap ImageNo1 { get; private set; } = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/test/P5140045_Stockerpel.jpg");
        public static Bitmap ImageNo2 { get; private set; } = BitmapValueConverter.Convert(PaceholderImagePath);
        public static Bitmap ImageNo3 { get; private set; } = BitmapValueConverter.Convert(PaceholderImagePath);
        public static Bitmap ImageNo4 { get; private set; } = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/test/P5140202_Kohlmeise.jpg");
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
        private int imageId = 1999;
        /// <summary>
        /// Get the image Id of the selected image. Set the id and the static property in the view model base for hand over to other windows.
        /// </summary>
        public int ImageId
        {
            get { return imageId; }
            set
            {
                // this.RaiseAndSetIfChanged(ref imageId, value);
                imageId = value;
                // set the properties in the view model base
                SelectedImageId = value;
            }
        }

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

        private ImageFiles imageFilesDatabase = new();
        /// <summary>
        /// Get the image files object of the images in the database. Set the object and the static property in the view model base for hand over to other windows.
        /// </summary>
        public ImageFiles ImageFilesDatabase
        { 
            get { return imageFilesDatabase; }
            set
            {
                imageFilesDatabase = value;
                // set the properties in the view model base
                LoadedImageFiles = value;
            }
        }

        private CategoriesTree categoriesTree = new();
        /// <summary>
        /// Get the categories tree object of the selected data. Set the object and the static property in the view model base for hand over to other windows.
        /// </summary>
        public CategoriesTree CategoriesTree
        {
            get { return categoriesTree; }
            set
            {
                categoriesTree = value;
                // set the properties in the view model base
                LoadedCategoriesTree = value;
            }
        }

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

        #region Settings
        public static string NamingConventionDescription => File.ReadAllText("Resources/Descriptions/naming_convention.txt");
        public static string FileTypeDescription => File.ReadAllText("Resources/Descriptions/file_types.txt");
        public static string BufferSizeDescription => File.ReadAllText("Resources/Descriptions/buffer_size.txt");

        private int bufferSize = ThisApplication.ProjectFile.BufferSize;
        /// <summary>
        /// Get or set the buffer size. This value will be saved to the project file.
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set { this.RaiseAndSetIfChanged(ref bufferSize, value); ThisApplication.ProjectFile.BufferSize = value; }
        }
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
        #endregion

        #region Status Bar
        public int PercentageProgressBar { get; private set; } = 100;
        public string LabelProgressBar { get; private set; } = "100%";
        public bool IsIndeterminate { get; private set; } = false;
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
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> ShowImageCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite1Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite2Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite3Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite4Command { get; }
        public ReactiveCommand<Unit, Unit> ToggleFileDialogCommand { get; }
        public ReactiveCommand<Unit, Unit> NewProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleLoadImagesCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadImagesCommand { get; }       
        #endregion

        /// <summary>
        /// Creates an instance of the MainWindowViewModel.
        /// </summary>
        public MainWindowViewModel()
        {
            ShowImageCommand = ReactiveCommand.Create(RunShowImageCommand);
            ShowFavorite1Command = ReactiveCommand.Create(RunShowFavorite1Command);
            ShowFavorite2Command = ReactiveCommand.Create(RunShowFavorite2Command);
            ShowFavorite3Command = ReactiveCommand.Create(RunShowFavorite3Command);
            ShowFavorite4Command = ReactiveCommand.Create(RunShowFavorite4Command);
            ToggleFileDialogCommand = ReactiveCommand.Create(RunToggleFileDialogCommand);
            NewProjectCommand = ReactiveCommand.Create(RunNewProjectCommandAsync);
            LoadProjectCommand = ReactiveCommand.Create(RunLoadProjectCommandAsync);
            LoadImagesCommand = ReactiveCommand.Create(RunLoadImagesCommandAsync);
            ToggleLoadImagesCommand = ReactiveCommand.Create(RunToggleLoadImagesCommand);
        }

        /// <summary>
        /// Method to set the control properties for the settings page.
        /// </summary>
        private void SetMainWindowPages()
        {
            // home page
            Notes = ThisApplication.ProjectFile.Notes;

            // images page
            ImageFilesDatabase.LoadAll();

            // settings page
            NefFilesChecked = ThisApplication.ProjectFile.NefFilesChecked;
            OrfFilesChecked = ThisApplication.ProjectFile.OrfFilesChecked;
            JpgFilesChecked = ThisApplication.ProjectFile.JpgFilesChecked;
            PngFilesChecked = ThisApplication.ProjectFile.PngFilesChecked;
            BufferSize = ThisApplication.ProjectFile.BufferSize;
            UseSeparator = ThisApplication.ProjectFile.UseSeparator;
            if (UseSeparator == true) { Separator = ThisApplication.ProjectFile.Separator; };
        }

        /// <summary>
        /// Method to open a new instance of the image view window.
        /// </summary>
        private void RunShowImageCommand()
        {
            // launch the view image window by using the ImageFile object stored int he ViewModelBase class
            new ImageViewWindow().Show();
        }

        private void RunShowFavorite1Command()
        {
            // set the inherited static property to make the id available to the other view models
            //ShowImage(ImageId);
        }

        private void RunShowFavorite2Command()
        {
            //ShowImage(ImageId);
        }

        private void RunShowFavorite3Command()
        {
            //ShowImage(ImageId);
        }

        private void RunShowFavorite4Command()
        {
            //ShowImage(SelectedImageId);
        }

        private void RunUseSeparatorCommand()
        {

        }

        /// <summary>
        /// Method to show the image view window.
        /// </summary>
        /// <param name="id"></param>
        private void ShowFavoriteImage(int favoriteId)
        {
            // get image file object by id from project file

            // set object in view model base

            // new ImageViewWindow().Show();
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
                    string message = $"{count} files will be added to your database." + Environment.NewLine + "Do you want to go on?";
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
                        ImageFilesDatabase.AddImages(files: filesToAdd, subfolderName: name);
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
    }
}
