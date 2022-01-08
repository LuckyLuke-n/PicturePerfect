using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PicturePerfect.ViewModels;

namespace PicturePerfect.Views
{
    public partial class ImageViewWindow : Window
    {
        public ImageViewWindow(int id)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new ImageViewWindowViewModel(id);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
