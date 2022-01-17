using Avalonia.Controls;
using Avalonia.Media.Imaging;
using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reactive;
using System.Text;

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
        #endregion

        #region Page "Images" Properties
        public int SelectedImage { get; set; } = 5;
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
            set { this.RaiseAndSetIfChanged(ref bufferSize, value); } //ThisApplication.ProjectFile.BufferSize = value; }
        }
        /// <summary>
        /// Get a list of possible separators. This value will be saved to the project file.
        /// </summary>
        public List<string> Separators { get; } = new() { "_", "-", "+", ".", ",", "Space"};
        private string? separator = ThisApplication.ProjectFile.Separator;
        /// <summary>
        /// Get or set the separator character for folder naming. This value will be saved to the project file.
        /// </summary> 
        public string Separator
        {
            get { return separator; }
            set { this.RaiseAndSetIfChanged(ref separator, value); } //ThisApplication.ProjectFile.Separator = value; }
        }
        private bool useSeparator = ThisApplication.ProjectFile.UseSeparator;
        /// <summary>
        /// Get or set the value if separtor suffix folder naming should be used. This value will be saved to the project file.
        /// </summary>
        public bool UseSeparator
        {
            get { return useSeparator; }
            set { this.RaiseAndSetIfChanged(ref useSeparator, value); } //ThisApplication.ProjectFile.useSeparator = value; }
        }

        #endregion

        #region Status Bar
        public int PercentageProgressBar { get; private set; } = 100;
        public string LabelProgressBar { get; private set; } = "100%";
        public bool IsIndeterminate { get; private set; } = false;
        public string ProjectName { get; } = ThisApplication.ProjectFile.ProjectName;
        public string InWorkItem { get; private set; } = "Item name (hard coded name)";
        #endregion

        #region Input for paths from axaml.cs file
        private string pathToProjectFile = string.Empty;
        /// <summary>
        /// Get the path to the project file or set the path which will trigger the LoadProject() method.
        /// </summary>
        public string PathToProjectFile
        {
            get { return pathToProjectFile; }
            set { pathToProjectFile = value; LoadProject(); }
        }
        private string pathToProjectFolder = string.Empty;
        /// <summary>
        /// Get the path to the project folder or set the path which will trigger the NewProject() method.
        /// </summary>
        public string PathToProjectFolder
        {
            get { return pathToProjectFolder; }
            set { pathToProjectFolder = value; NewProject(); }
        }
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> ShowImageCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite1Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite2Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite3Command { get; }
        public ReactiveCommand<Unit, Unit> ShowFavorite4Command { get; }
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
        }


        private void RunShowImageCommand()
        {
            ShowImage(SelectedImage);
        }

        private async void RunShowFavorite1Command()
        {
            // set the inherited static property to make the id available to the other view models
            ShowImage(SelectedImage);
        }

        private void RunShowFavorite2Command()
        {
            ShowImage(SelectedImage);
        }

        private void RunShowFavorite3Command()
        {
            ShowImage(SelectedImage);
        }

        private void RunShowFavorite4Command()
        {
            ShowImage(SelectedImage);
        }

        private void RunUseSeparatorCommand()
        {

        }



        private void ShowImage(int id)
        {
            SelectedImageId = id;
            new ImageViewWindow().Show();
        }

        private async void LoadProject()
        {
            await MessageBox.Show(PathToProjectFile);
            ThisApplication.ProjectFile = ProjectFile.Load(PathToProjectFile);
        }

        private async void NewProject()
        {
            ThisApplication.ProjectFile = ProjectFile.New(PathToProjectFolder);
            await MessageBox.Show(ThisApplication.ProjectFile.ProjectName);
        }
    }
}
