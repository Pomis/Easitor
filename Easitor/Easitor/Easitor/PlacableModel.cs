using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    /// <summary>
    /// Родительский класс для классов, размещаемых на экране
    /// </summary>
    public class PlacableModel:INPC
    {
        public EditorModel _Model;

        private double _x;
        public double X
        {
            get { return _x; }
            set { _x = value; OnPropertyChanged("X"); }
        }
        private double _y;
        public double Y
        {
            get { return _y; }
            set { _y = value; OnPropertyChanged("Y"); }
        }

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set { _x2 = value; OnPropertyChanged("X2"); }
        }
        private double _y2;
        public double Y2
        {
            get { return _y2; }
            set { _y2 = value; OnPropertyChanged("Y2"); }
        }

        private string _Margins;
        public string Margins
        {
            get { return _Margins; }
            set { _Margins = value; OnPropertyChanged(Margins); }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged("Height"); }
        }

        double _Opacity;
        public double Opacity { get { return _Opacity; } set { _Opacity = value; OnPropertyChanged("Opacity"); } }

        double _BlurRadius;
        public double BlurRadius { get { return _BlurRadius; } set { _BlurRadius = value; OnPropertyChanged("BlurRadius"); } }

    }
}
