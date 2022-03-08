using Avalonia.Media.Imaging;
using System.Linq;
using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using Avalonia.Threading;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

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

        #region TreeView properties
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

        #region Commands
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
        #endregion


        /// <summary>
        /// Creates an instance of the MainWindowViewModel.
        /// </summary>
        public MainWindowViewModel()
        {
            ShowImageCommand = ReactiveCommand.Create(RunShowImageCommandAsync);
            MarkAsFavorite1Command = ReactiveCommand.Create(RunMarkAsFavorite1Command);
            MarkAsFavorite2Command = ReactiveCommand.Create(RunMarkAsFavorite2Command);
            MarkAsFavorite3Command = ReactiveCommand.Create(RunMarkAsFavorite3Command);
            MarkAsFavorite4Command = ReactiveCommand.Create(RunMarkAsFavorite4Command);
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
            //ImageFilesDatabase.LoadAll();
            LoadedImageFiles.LoadAll();
            LoadedCategoriesTree.LoadTree();
            LoadedLocations.LoadList();
            SetFavoriteImages();

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
                        // set image
                        switch (i)
                        {
                            case 0:
                                ImageNo1 = BitmapValueConverter.Convert(ImageFile.LoadById(id).AbsolutePath);
                                break;
                            case 1:
                                ImageNo2 = BitmapValueConverter.Convert(ImageFile.LoadById(id).AbsolutePath);
                                break;
                            case 2:
                                ImageNo3 = BitmapValueConverter.Convert(ImageFile.LoadById(id).AbsolutePath);
                                break;
                            case 3:
                                ImageNo4 = BitmapValueConverter.Convert(ImageFile.LoadById(id).AbsolutePath);
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

        private void RunUseSeparatorCommand()
        {

        }

        /// <summary>
        /// Method to set the seleted image file as favorite 1.
        /// </summary>
        private void RunMarkAsFavorite1Command()
        {
            ThisApplication.ProjectFile.Favorite1Id = ImageFile.Id;
            ThisApplication.ProjectFile.Save();
            ImageNo1 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
        }

        /// <summary>
        /// Method to set the seleted image file as favorite 2.
        /// </summary>
        private void RunMarkAsFavorite2Command()
        {
            ThisApplication.ProjectFile.Favorite2Id = ImageFile.Id;
            ThisApplication.ProjectFile.Save();
            ImageNo2 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
        }

        /// <summary>
        /// Method to set the seleted image file as favorite 3.
        /// </summary>
        private void RunMarkAsFavorite3Command()
        {
            ThisApplication.ProjectFile.Favorite3Id = ImageFile.Id;
            ThisApplication.ProjectFile.Save();
            ImageNo3 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
        }

        /// <summary>
        /// Method to set the seleted image file as favorite 4.
        /// </summary>
        private void RunMarkAsFavorite4Command()
        {
            ThisApplication.ProjectFile.Favorite4Id = ImageFile.Id;
            ThisApplication.ProjectFile.Save();
            ImageNo4 = BitmapValueConverter.Convert(ImageFile.AbsolutePath);
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
                        LoadedImageFiles.AddImages(files: filesToAdd, subfolderName: name);
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
                MessageBox.Show(subCategory.GetType().Name + "....." + subCategory.Name);
            }
        }
    }
}
