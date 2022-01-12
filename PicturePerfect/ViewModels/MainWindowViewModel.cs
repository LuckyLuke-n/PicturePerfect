using Avalonia.Media.Imaging;
using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        #region Status Bar
        public int PercentageProgressBar { get; private set; } = 100;
        public string LabelProgressBar { get; private set; } = "100%";
        public bool IsIndeterminate { get; private set; } = false;
        public string ProjectName { get; } = ThisApplication.ProjectFile.ProjectName;
        public string InWorkItem { get; private set; } = "Item name (hard coded name)";
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
            ShowImage(1);
        }

        private async void RunShowFavorite1Command()
        {
            // set the inherited static property to make the id available to the other view models
            SelectedImageId = SelectedImage;
            new ImageViewWindow().Show();
            //var res = await MessageBox.Show(text: "Test message box", buttons: MessageBox.MessageBoxButtons.YesNoCancel, icon: MessageBox.MessageBoxIcon.Information);
        }

        private void RunShowFavorite2Command()
        {
            ShowImage(1);
        }

        private void RunShowFavorite3Command()
        {
            ShowImage(1);
        }

        private void RunShowFavorite4Command()
        {
            ShowImage(1);
        }

        private void ShowImage(int id)
        {

        }
    }
}
