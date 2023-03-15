using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Utilities;
using GraphicEditor.Models;
using GraphicEditor.ViewModels.Pages;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Text;

namespace GraphicEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> ShapesIn { get; set; } = new ObservableCollection<Shape>();
        public ObservableCollection<MyShapeModels> ShapesOut { get; set; } = new ObservableCollection<MyShapeModels>();
        private object content;
        private ObservableCollection<ViewModelBase> viewModelCollection;
        public ObservableCollection<MyColor> ListOfBrushes { get; set; } = new ObservableCollection<MyColor>();
        public ReactiveCommand<Unit, Unit> AddButton { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        private bool color_flag;
        private bool fcolor_flag;
        public string nameText;
        public string startPointText;
        public string endPointText;
        public string polyLineText;
        public string rectWidth;
        public string rectHeight;
        public string pathShapeText;
        public int numericUpDownText;
        public int getIndex;
        public int getFillIndex;

        public MainWindowViewModel()
        {
            LoadColors();
            viewModelCollection = new ObservableCollection<ViewModelBase>();
            viewModelCollection.Add(new LineViewModel("Линия"));
            viewModelCollection.Add(new PolyLineViewModel("Ломаная линия"));
            viewModelCollection.Add(new PolygonViewModel("Многоугольник"));
            viewModelCollection.Add(new RectangleViewModel("Прямоугольник"));
            viewModelCollection.Add(new RectangleViewModel("Эллипс"));
            viewModelCollection.Add(new PathShapeViewModel("Составная фигура"));
            Content = viewModelCollection[0];
            AddButton = ReactiveCommand.Create(() =>
            {
                if (Content == viewModelCollection[0]) LineAdd();
                if (Content == viewModelCollection[1]) PolyLineAdd();
                if (Content == viewModelCollection[2]) PolygonAdd();
                if (Content == viewModelCollection[3]) RectangleAdd(true);
                if (Content == viewModelCollection[4]) RectangleAdd(false);
                if (Content == viewModelCollection[5]) PathAdd();
            });
            ResetCommand = ReactiveCommand.Create(() =>
            {
                NameText = "";
                StartPointText = "";
                EndPointText = "";
                PolyLineText = "";
                RectHeight = "";
                RectWidth = "";
                PathShapeText = "";
                GetFillIndex = 1;
                GetIndex = 1;
                NumericUpDownText = 1;
            });
        }
        private void PathAdd()
        {
            Path newShape = new Path
            {
                Data = Geometry.Parse(pathShapeText),
                Fill = ListOfBrushes[GetFillIndex].Brush,
                Stroke = ListOfBrushes[GetIndex].Brush,
                StrokeThickness = NumericUpDownText
            };
            ShapesIn.Add(newShape);
            ShapesOut.Add(new MyShapeModels(NameText));
        }
        private void RectangleAdd(bool flag)
        {
            if (flag)
            {
                Rectangle newShape = new Rectangle
                {
                    Margin = Avalonia.Thickness.Parse(startPointText),
                    Width = int.Parse(rectWidth),
                    Height = int.Parse(rectHeight),
                    Fill = ListOfBrushes[GetFillIndex].Brush,
                    Stroke = ListOfBrushes[GetIndex].Brush,
                    StrokeThickness = NumericUpDownText
                };
                ShapesIn.Add(newShape);
            }
            else
            {
                Ellipse newShape = new Ellipse
                {
                    Margin = Avalonia.Thickness.Parse(startPointText),
                    Width = int.Parse(rectWidth),
                    Height = int.Parse(rectHeight),
                    Fill = ListOfBrushes[GetFillIndex].Brush,
                    Stroke = ListOfBrushes[GetIndex].Brush,
                    StrokeThickness = NumericUpDownText
                };
                ShapesIn.Add(newShape);
            }
            ShapesOut.Add(new MyShapeModels(NameText));
        }
        private void PolygonAdd()
        {
            List<Avalonia.Point> listOfPolyLinePoints = new List<Avalonia.Point>();
            string[] words = polyLineText.Split(' ');
            foreach (string word in words)
            {
                listOfPolyLinePoints.Add(Avalonia.Point.Parse(word));
            }
            Polygon newShape = new Polygon
            {
                Points = listOfPolyLinePoints,
                Fill = ListOfBrushes[GetFillIndex].Brush,
                Stroke = ListOfBrushes[GetIndex].Brush,
                StrokeThickness = NumericUpDownText
            };
            ShapesIn.Add(newShape);
            ShapesOut.Add(new MyShapeModels(NameText));
        }
        private void PolyLineAdd()
        {
            List<Avalonia.Point> listOfPolyLinePoints = new List<Avalonia.Point>();
            string[] words = polyLineText.Split(' ');
            foreach (string word in words)
            {
                listOfPolyLinePoints.Add(Avalonia.Point.Parse(word));
            }
            Polyline newShape = new Polyline
            {
                Points = listOfPolyLinePoints,
                Stroke = ListOfBrushes[GetIndex].Brush,
                StrokeThickness = NumericUpDownText
            };
            ShapesIn.Add(newShape);
            ShapesOut.Add(new MyShapeModels(NameText));
        }
        private void LineAdd()
        {
            Line newShape = new Line
            {
                StartPoint = Avalonia.Point.Parse(startPointText),
                EndPoint = Avalonia.Point.Parse(endPointText),
                Stroke = ListOfBrushes[GetIndex].Brush,
                StrokeThickness = NumericUpDownText,

            };
            ShapesIn.Add(newShape);
            ShapesOut.Add(new MyShapeModels(NameText));
        }
        public void LoadColors()
        {
            PropertyInfo[] colorProps = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo colorProp in colorProps)
            {
                if (colorProp.PropertyType == typeof(Color))
                {
                    Color color = (Color)colorProp.GetValue(null, null);
                    string colorName = colorProp.Name;
                    SolidColorBrush brush = new SolidColorBrush(color);
                    MyColor item = new MyColor() { Brush = brush, Name = colorName };
                    ListOfBrushes.Add(item);
                }
            }
        }
        public string PathShapeText
        {
            get { return pathShapeText; }
            set { this.RaiseAndSetIfChanged(ref pathShapeText, value); }
        }
        public string RectWidth
        {
            get { return rectWidth; }
            set { this.RaiseAndSetIfChanged(ref rectWidth, value); }
        }  
        public string RectHeight
        {
            get { return rectHeight; }
            set { this.RaiseAndSetIfChanged(ref rectHeight, value); }
        }
        public string PolyLineText
        {
            get { return polyLineText; }
            set { this.RaiseAndSetIfChanged(ref polyLineText, value); }
        }
        public string NameText
        {
            get { return nameText; }
            set { this.RaiseAndSetIfChanged(ref nameText, value); }
        }
        public string StartPointText
        {
            get { return startPointText; }
            set { this.RaiseAndSetIfChanged(ref startPointText, value); }
        }
        public string EndPointText
        {
            get { return endPointText; }
            set { this.RaiseAndSetIfChanged(ref endPointText, value); }
        }
        public int NumericUpDownText
        {
            get { return numericUpDownText; }
            set { this.RaiseAndSetIfChanged(ref numericUpDownText, value); }
        }
        public int GetFillIndex
        {
            get
            {
                if (fcolor_flag == true)
                {
                    getFillIndex = 1;
                    fcolor_flag = false;
                }
                return getFillIndex;
            }
            set
            { this.RaiseAndSetIfChanged(ref getFillIndex, value); }
        }
        public int GetIndex
        {
            get
            {
                if (color_flag == true)
                {
                    getIndex = 1;
                    color_flag = false;
                }
                return getIndex;
            }
            set { this.RaiseAndSetIfChanged(ref getIndex, value); }
        }
        public object Content
        {
            get 
            {
                return content; 
            }
            set
            {
                NameText = "";
                StartPointText = "";
                EndPointText = "";
                PolyLineText = "";
                RectHeight = "";
                RectWidth = "";
                PathShapeText = "";
                color_flag = true;
                fcolor_flag = true;
                NumericUpDownText = 1;
                this.RaiseAndSetIfChanged(ref content, value); 
            }
        }
        public ObservableCollection<ViewModelBase> ViewModelCollection
        {
            get { return viewModelCollection; }
            set
            {
                this.RaiseAndSetIfChanged(ref viewModelCollection, value);
            }
        }
    }
}
