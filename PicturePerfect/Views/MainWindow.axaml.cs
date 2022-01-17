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
        }

        /// <summary>
        /// Dialog types for folder and file selctions in this class.
        /// </summary>
        private enum DialogType
        {
            NewProject,
            SelectProject
        }

        /// <summary>
        /// Event for new project button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuNewProject_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.NewProject);     
        }

        /// <summary>
        /// Event for select project button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuSelectProject_Click(object sender, RoutedEventArgs e)
        {
            _ = GetPathAsync(DialogType.SelectProject);   
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
                        viewModel.PathToProjectFolder = resultFolder.ToString();
                        return resultFolder.ToString();
                    }
                    else { return null; }
                }
                else { return null; }
            }
            else // DialogType.SelectProject
            {
                // file dialog to get the path to an exisiting project file
                OpenFileDialog dialogFile = new();
                dialogFile.AllowMultiple = false;
                dialogFile.Filters.Add(new FileDialogFilter() { Name = "PicturePerfect-Project File", Extensions = { "ppp" } });
                var result = await dialogFile.ShowAsync((Window)VisualRoot);

                // check for null reference
                if (result != null)
                {
                    viewModel.PathToProjectFile = result[0].ToString();
                    return result;
                    
                }
                else { return null; }
            }
        }


        public void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            new InfoWindow().Show();
        }
    }
}
