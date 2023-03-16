using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using GraphicEditor.ViewModels;
using SkiaSharp;
using System.Drawing.Imaging;
using System.Linq;

namespace GraphicEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void DeleteShape(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                var bb = sender as Button;
                var ss = bb.Parent;
                var dd = ss.Parent;
                var gg = dd.Parent as ListBox;
                int ggitem = gg.SelectedIndex;
                mainWindowViewModel.GetSelectedItemIndex = ggitem;
            }
        }
        public async void SavePngFileDialogButtonClick(object sender, RoutedEventArgs args)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExtension = ".PNG";
            string? result = await saveFileDialog.ShowAsync(this);
            var canvas = this.GetVisualDescendants().OfType<Canvas>().Where(canvas => canvas.Name.Equals("canvas")).FirstOrDefault();
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                if (result != null)
                {
                    //var newCanvas = this.FindControl<ItemsControl>("convers");
                    //var ss = newCanvas.ItemsPanel.GetType();
                    var pxsize = new PixelSize((int)canvas.Bounds.Width, (int)canvas.Bounds.Height);
                    var size = new Size(canvas.Bounds.Width, canvas.Bounds.Height);
                    using (RenderTargetBitmap bitmap = new RenderTargetBitmap(pxsize, new Avalonia.Vector(96, 96)))
                    {
                        canvas.Measure(size);
                        canvas.Arrange(new Rect(size));
                        bitmap.Render(canvas);
                        bitmap.Save(result);
                    }
                }
                else
                {
                    mainWindowViewModel.Path = "Dialog was canceled";
                }
            }
        }
    }
}
