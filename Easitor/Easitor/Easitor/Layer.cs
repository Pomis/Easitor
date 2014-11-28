using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
namespace Easitor
{
    public class Layer:PlacableModel
    {
        #region Конструкторы
        // Новый пустой слой.
        public Layer(EditorModel Model)
        {
            _Model=Model;
            X = 0;
            Y = 0;
            Model.LayerList.Add(this);
            Model.RevercedLayerList.Insert(0, this);
            ToolTip = (1+Model.LayersAdded++).ToString();
            BitMap = new WriteableBitmap(new BitmapImage(new Uri("UI/Empty.png", UriKind.Relative)));
            Width = BitMap.Width;
            Height = BitMap.Height;
        }
        // Загрузка изображения из файла в слой.
        public Layer(string path, EditorModel Model)
        {
            _Model=Model;
            BitMap = new WriteableBitmap(new BitmapImage(new Uri(path,UriKind.Relative)));
            Model.LayerList.Add(this);
            Model.RevercedLayerList.Insert(0, this);
            Width = BitMap.Width;
            Height = BitMap.Height;
            X = 0;
            Y = 0;
            ToolTip = (1+Model.LayersAdded++).ToString();
        }
        #endregion
        #region Поля
        
        WriteableBitmap _BitMap;
        public WriteableBitmap BitMap { get { return _BitMap; } set { _BitMap = value; OnPropertyChanged("BitMap"); } }

        double _BrushOpacity;
        public double BrushOpacity { get { return _BrushOpacity; } set { _BrushOpacity = value; OnPropertyChanged("BrushOpacity"); } }

        string _Background;
        public string Background { get { return _Background; } set { _Background = value; OnPropertyChanged("Background"); } }

        ObservableCollection<Circle> _CircleList = new ObservableCollection<Circle>();
        public ObservableCollection<Circle> CircleList { get { return _CircleList; } set { _CircleList = value; OnPropertyChanged("CircleList"); } }

        ObservableCollection<Circle> _ContinuedCircleList = new ObservableCollection<Circle>();
        public ObservableCollection<Circle> ContinuedCircleList { get { return _ContinuedCircleList; } set { _ContinuedCircleList = value; OnPropertyChanged("ContinuedCircleList"); } }

        private PointCollection points = new PointCollection();

        public PointCollection Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged("Points");
            }
        }

        private double _angle;
        public double Angle
        {
            get { return _angle; }
            set { _angle = value; OnPropertyChanged("Angle"); }
        }

        private string _ToolTip;
        public string ToolTip { get { return _ToolTip; } set { _ToolTip = value; OnPropertyChanged("ToolTip"); } }

        string _Visibility="Visible";
        public string Visibility
        {
            get
            {
                return _Visibility;
            }

            set
            {
                _Visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        string _EyeImage="UI/EyePressed.png";
        public string EyeImage
        {
            get
            {
                return _EyeImage;
            }

            set
            {
                _EyeImage = value;
                OnPropertyChanged("EyeImage");
            }
        }
        public bool IsBackground = false;
        #endregion
        #region Методы
        public Color GetPixelColor(WriteableBitmap source, int x, int y)
        {
            Color c = Colors.White;
            if (source != null)
            {
                try
                {
                    CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
                    var pixels = new byte[4];
                    cb.CopyPixels(pixels, 4, 0);
                    c = Color.FromRgb(pixels[2], pixels[1], pixels[0]);
                }
                catch (Exception) { }
            }
            return c;
        }
        public void RenderThumbnail() { }
        public void HideOrUnhide()
        {
            if (Visibility == "Hidden")
            {
                Visibility = "Visible";
                EyeImage = "UI/EyePressed.png";
            }
            else
            { 
                Visibility = "Hidden";
                EyeImage = "UI/Eye.png";
            }
        }
        #endregion
    }
}
