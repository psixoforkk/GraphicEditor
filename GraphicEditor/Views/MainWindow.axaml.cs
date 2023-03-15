using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using GraphicEditor.ViewModels;

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
    }
}
