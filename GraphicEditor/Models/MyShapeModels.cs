using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor.Models
{
    public class MyShapeModels
    {
        public string? shapeName { get; set; }
        public MyShapeModels(string name)
        {
            shapeName = name;
        }
    }
}
