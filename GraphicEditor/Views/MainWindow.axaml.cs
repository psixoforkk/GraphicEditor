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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                var but = this.GetVisualDescendants().OfType<ListBox>().Where(but => but.Name.Equals("selectlb")).FirstOrDefault();
                mainWindowViewModel.GetSelectedItemIndex = but.SelectedIndex;
            }
        }
        public System.Text.Json.Serialization.JsonNumberHandling NumberHandling { get; }
        public enum JsonNumberHandling : byte
        {
            AllowReadingFromString = 0x1,
            WriteAsString = 0x2,
            AllowNamedFloatingPointLiterals = 0x4
        }
        public async void OpenJsonFileDialogButtonClick(object sender, RoutedEventArgs args)
        {

        }
        public async void SaveJsonFileDialogButtonClick(object sender, RoutedEventArgs args)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExtension = ".JSON";
            string? result = await saveFileDialog.ShowAsync(this);
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                if (result != null)
                {
                    using (FileStream fs = new FileStream(result, FileMode.OpenOrCreate))
                    {
                        var options = new JsonSerializerOptions
                        {
                            NumberHandling = (System.Text.Json.Serialization.JsonNumberHandling)(JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowNamedFloatingPointLiterals),
                            ReferenceHandler = ReferenceHandler.Preserve
                        };
                        string json = JsonSerializer.Serialize(mainWindowViewModel.ShapesOut, options);
                        string jsoner = JsonSerializer.Serialize(mainWindowViewModel.ShapesIn, options);
                        string jsons = json + jsoner;
                        byte[] buffer = Encoding.Default.GetBytes(jsons);
                        fs.Write(buffer, 0, buffer.Length);
                        //await JsonSerializer.SerializeAsync(fs, mainWindowViewModel.ShapesIn, options);
                    }
                }
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
            }
        }
    }
}
