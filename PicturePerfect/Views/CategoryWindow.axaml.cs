using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PicturePerfect.ViewModels;

namespace PicturePerfect.Views
{
    public partial class CategoryWindow : Window
    {
        public CategoryWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new CategoryWindowViewModel();
            Title = ThisApplication.ApplicationName;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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
    }
}
