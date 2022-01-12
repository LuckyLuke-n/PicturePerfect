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

        private enum DialogType
        {
            NewProject,
            SelectProject
        }

        public void MenuNewProject_Click(object sender, RoutedEventArgs e)
        {
            var path = GetPathAsync(DialogType.NewProject);
        }

        public void MenuSelectProject_Click(object sender, RoutedEventArgs e)
        {
            var path = GetPathAsync(DialogType.SelectProject);
        }

        private async Task<string> GetPathAsync(DialogType dialogType)
        {

            if (dialogType == DialogType.NewProject)
            {
                OpenFolderDialog dialogFolder = new();
                var result = await dialogFolder.ShowAsync((Window)VisualRoot);

                if (result != null)
                {
                    MessageBox.Show(result.ToString());
                    
                }

                return result.ToString();
            }
            else
            {
                OpenFileDialog dialogFile = new();
                dialogFile.AllowMultiple = false;
                dialogFile.Filters.Add(new FileDialogFilter() { Name = "PicturePerfect-Project File", Extensions = { "ppp" } });
                var result = await dialogFile.ShowAsync((Window)VisualRoot);

                if (result != null)
                {
                    MessageBox.Show(result[0].ToString());
                    
                }

                return result.ToString();
            }
        }
    }
}
