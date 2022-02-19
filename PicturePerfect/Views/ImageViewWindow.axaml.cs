using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PicturePerfect.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PicturePerfect.Views
{
    public partial class ImageViewWindow : Window
    {
        private TextBox textBoxSelectFolder;

        public ImageViewWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new ImageViewWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textBoxSelectFolder = this.FindControl<TextBox>("textBoxSelectFolder");
        }

        /// <summary>
        /// Event to close the image view window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event to select a folder for image conversion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync();
        }

        /// <summary>
        /// Async method to get a path for a folder.
        /// </summary>
        /// <returns>Returns as string array or null.</returns>
        private async Task<object> GetPathAsync()
        {
            // file dialog to get the path to a folder
            OpenFolderDialog dialogFolder = new();
            var resultFolder = await dialogFolder.ShowAsync((Window)VisualRoot);

            // check for null reference
            if (resultFolder != null)
            {
                //viewModel.PathToProjectFolder = resultFolder.ToString();
                textBoxSelectFolder.Text = resultFolder.ToString();
                return resultFolder.ToString();
            }
            else { return null; }

        }
    }
}
