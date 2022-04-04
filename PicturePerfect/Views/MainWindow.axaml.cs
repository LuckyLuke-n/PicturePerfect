using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PicturePerfect.ViewModels;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel = new();
        private TextBox textBoxNewProject;
        private TextBox textBoxLoadProject;
        private TextBox textBoxLoadImages;
        private TextBox textBoxRawConverterIn;
        private TextBox textBoxRawConverterOut;
        private TextBox textBoxExternalViewer;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textBoxNewProject = this.FindControl<TextBox>("textBoxNewProject");
            textBoxLoadProject = this.FindControl<TextBox>("textBoxLoadProject");
            textBoxLoadImages = this.FindControl<TextBox>("textBoxLoadImages");
            textBoxRawConverterIn = this.FindControl<TextBox>("textBoxRawConverterIn");
            textBoxRawConverterOut = this.FindControl<TextBox>("textBoxRawConverterOut");
            textBoxExternalViewer = this.FindControl<TextBox>("textBoxExternalViewer");
        }

        /// <summary>
        /// Dialog types for folder and file selctions in this class.
        /// </summary>
        private enum DialogType
        {
            NewProject,
            SelectProject,
            LoadImages,
            RawConverterInput,
            RawConverterOutput,
            PathToExternalViewer
        }

        /// <summary>
        /// Event for new project button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonNewProject_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.NewProject);
        }

        /// <summary>
        /// Event for select project button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSelectProject_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.SelectProject);
        }

        /// <summary>
        /// Event for selecting images to load them into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonLoadImages_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.LoadImages);
        }

        /// <summary>
        /// Event for selecting images to load into the raw converter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonRawConverterInput_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.RawConverterInput);
        }

        /// <summary>
        /// Event for selecting the output folder for the raw converter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonRawConverterOutput_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.RawConverterOutput);
        }

        /// <summary>
        /// Event for selecting the path to the executeable for the external image viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonExternalViewer_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.PathToExternalViewer);
        }

        /// <summary>
        /// Async method to get a path for a folder or file selection.
        /// </summary>
        /// <param name="dialogType"></param>
        /// <returns>Returns as string array or null.</returns>
        private async Task<object> GetPathAsync(DialogType dialogType)
        {
            if (dialogType == DialogType.NewProject)
            {
                string text = "Please select a folder for your new project. The folder name will be the name of your project.";
                MessageBox.MessageBoxResult result = await MessageBox.Show(text, null, MessageBox.MessageBoxButtons.OkCancel, MessageBox.MessageBoxIcon.Information);

                if (result == MessageBox.MessageBoxResult.Ok)
                {
                    // file dialog to get the path to a new project folder
                    OpenFolderDialog dialogFolder = new();
                    var resultFolder = await dialogFolder.ShowAsync((Window)VisualRoot);

                    // check for null reference
                    if (resultFolder != null)
                    {
                        //viewModel.PathToProjectFolder = resultFolder.ToString();
                        textBoxNewProject.Text = resultFolder.ToString();
                        return resultFolder.ToString();
                    }
                    else { return null; }
                }
                else { return null; }
            }
            else if (dialogType == DialogType.SelectProject)
            {
                // file dialog to get the path to an exisiting project file
                OpenFileDialog dialogFile = new();
                dialogFile.AllowMultiple = false;
                dialogFile.Filters.Add(new FileDialogFilter() { Name = "PicturePerfect-Project File", Extensions = { "ppp" } });
                var result = await dialogFile.ShowAsync((Window)VisualRoot);

                // check for null reference
                if (result != null)
                {
                    textBoxLoadProject.Text = result[0].ToString();
                    return result;                  
                }
                else { return null; }
            }
            else if (dialogType == DialogType.LoadImages)
            {
                // file dialog to get the path to a folder
                OpenFolderDialog dialogFolder = new();
                var resultFolder = await dialogFolder.ShowAsync((Window)VisualRoot);

                // check for null reference
                if (resultFolder != null)
                {
                    //viewModel.PathToProjectFolder = resultFolder.ToString();
                    textBoxLoadImages.Text = resultFolder.ToString();
                    return resultFolder.ToString();
                }
                else { return null; }
            }
            else if (dialogType == DialogType.RawConverterInput)
            {
                // file dialog to get the path to a folder
                OpenFolderDialog dialogFolder = new();
                var resultFolder = await dialogFolder.ShowAsync((Window)VisualRoot);

                // check for null reference
                if (resultFolder != null)
                {
                    //viewModel.PathToProjectFolder = resultFolder.ToString();
                    textBoxRawConverterIn.Text = resultFolder.ToString();
                    return resultFolder.ToString();
                }
                else { return null; }
            }
            else if (dialogType == DialogType.RawConverterOutput)
            {
                // file dialog to get the path to a folder
                OpenFolderDialog dialogFolder = new();
                var resultFolder = await dialogFolder.ShowAsync((Window)VisualRoot);

                // check for null reference
                if (resultFolder != null)
                {
                    //viewModel.PathToProjectFolder = resultFolder.ToString();
                    textBoxRawConverterOut.Text = resultFolder.ToString();
                    return resultFolder.ToString();
                }
                else { return null; }
            }        
            else if (dialogType == DialogType.PathToExternalViewer)
            {
                // file dialog to get the path to an exisiting project file
                OpenFileDialog dialogFile = new();
                dialogFile.AllowMultiple = false;
                var result = await dialogFile.ShowAsync((Window)VisualRoot);

                // check for null reference
                if (result != null)
                {
                    textBoxExternalViewer.Text = result[0].ToString();
                    return result;
                }
                else { return null; }
            }
            else
            {
                _ = MessageBox.Show("An error occured while trying to get the path from the selection window.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Event to trigger the info window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            new InfoWindow().Show();
        }
    }
}
