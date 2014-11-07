using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    public class Circle:PlacableModel
    {
        public Circle(string color, double diameter, double blur, double opacity, double x, double y)
        {
            Color = color;
            Width = diameter;
            Height = diameter;
            BlurRadius = blur;
            X = x;
            Y = y;
            Opacity = opacity;
        }
        string _Color;
        public string Color { get { return _Color; } set { _Color = value; OnPropertyChanged("Color"); } }
    }
}
