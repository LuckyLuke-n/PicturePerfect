using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PicturePerfect.ViewModels;
using System.Diagnostics;

namespace PicturePerfect.Views
{
    public partial class ImageViewWindow : Window
    {
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
        }
    }
}
