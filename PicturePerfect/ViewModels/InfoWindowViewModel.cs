using PicturePerfect.Models;
using PicturePerfect.Views;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;

namespace PicturePerfect.ViewModels
{
    internal class InfoWindowViewModel : ViewModelBase
    {
        #region Color and font properties
        public static string DarkColor => ThisApplication.ProjectFile.DarkColor;
        public static string MediumColor => ThisApplication.ProjectFile.MediumColor;
        public static string LightColor => ThisApplication.ProjectFile.LightColor;
        public static string LightFontColor => ThisApplication.ProjectFile.LightFontColor;
        public static string DarkContrastColor => ThisApplication.ProjectFile.DarkContrastColor;
        public static int LargeFontSize => 23;
        #endregion

        #region Text properties
        public static string ApplicatonName => ThisApplication.ApplicationName;
        public static string ApplicationVersion => ThisApplication.ApplicationVersion;
        public static string BuildDate => "Build date: " + ThisApplication.BuildDate;
        public static string DatabaseVersion => $"Database version of currently loaded project: {Database.CurrentVersion}";

        public static string About => File.ReadAllText("Resources/about.txt");
        public static string Libraries => File.ReadAllText("Resources/libraries.txt");
        public static string License => File.ReadAllText("Resources/license.txt");

        public static string GitHubLink => "https://github.com/LuckyLuke-n/PicturePerfect";
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> OpenGitHubCommand { get; }
        #endregion

        /// <summary>
        /// Creates a new instance of the InfoWindowViewModel.
        /// </summary>
        public InfoWindowViewModel()
        {
            OpenGitHubCommand = ReactiveCommand.Create(RunOpenGitHubCommand);
        }

        /// <summary>
        /// Method to open the github page of this application.
        /// </summary>
        private async void RunOpenGitHubCommand()
        {
            try
            {
                Process.Start("explorer.exe", GitHubLink);
            }
            catch (Exception exception)
            {
                string text = "We encoutered an error opening the link in your default browser." + Environment.NewLine +
                    $"Original message: {exception.Message}";
                _ = await MessageBox.Show(text, null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
            }         
        }
    }
}
