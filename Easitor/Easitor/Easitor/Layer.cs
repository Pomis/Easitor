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
        //генерация тестовой хреновины, не юзать
        Layer(EditorModel Model, int CellSize)
        {
            IsBackground = true;
            Width = 1500;
            Height = 1500;

            BitMap = new WriteableBitmap(1500, 1500, 96, 96, PixelFormats.Bgra32, null);
            byte[] Gray_1 = {227,224,220,255};
            byte[] Gray_2 = {205,201,197,255};

            //for (int i = 0; i < 1500; i++)
            //{
            //    for (int j = 0; j < 1500; j++)
            //    {
            //        Int32Rect rect1 = new Int32Rect(i, j, 1, 1);
            //        BitMap.WritePixels(rect1, Gray_1, 4, 0);
            //    }
            //}
            Int32Rect rect = new Int32Rect(0, 0, 1500, 1500);

            byte[] pixels = new byte[1500 * 1500 * BitMap.Format.BitsPerPixel / 8];
            Random rand = new Random();
            for (int y = 0; y < BitMap.PixelHeight; y++) 
            {
                for (int x = 0; x < BitMap.PixelWidth; x++)
                {
                    int alpha = 0;
                    int red = 0;
                    int green = 0;
                    int blue = 0;

                    // Определение цвета пикселя
                    if ((x % 5 == 0) || (y % 7 == 0))
                    {
                        red = (int)((double)y / BitMap.PixelHeight * 255);
                        green = rand.Next(100, 255);
                        blue = (int)((double)x / BitMap.PixelWidth * 255);
                        alpha = 255;
                    }
                    else
                    {
                        red = (int)((double)x / BitMap.PixelWidth * 255);
                        green = rand.Next(100, 255);
                        blue = (int)((double)y / BitMap.PixelHeight * 255);
                        alpha = 50;
                    }

                    int pixelOffset = (x + y * BitMap.PixelWidth) * BitMap.Format.BitsPerPixel / 8;
                    pixels[pixelOffset] = (byte)blue;
                    pixels[pixelOffset + 1] = (byte)green;
                    pixels[pixelOffset + 2] = (byte)red;
                    pixels[pixelOffset + 3] = (byte)alpha;

                                       
                }

                int stride = (BitMap.PixelWidth * BitMap.Format.BitsPerPixel) / 8;

                BitMap.WritePixels(rect, pixels, stride, 0);
            }
            Model.LayerList.Add(this);
            Model.RevercedLayerList.Insert(0, this);
        }
        #endregion
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

        string _PolylinePoints="";
        public string PolylinePoints { get { return _PolylinePoints; } set { _PolylinePoints = value; } }

        private double _angle;
        public double Angle
        {
            get { return _angle; }
            set { _angle = value; OnPropertyChanged("Angle"); }
        }

        private string _ToolTip;
        public string ToolTip { get { return _ToolTip; } set { _ToolTip = value; OnPropertyChanged("ToolTip"); } }

        
        public bool IsBackground = false;

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

        

        public void SomeUpdateFunc()
        {
            PointCollection pc = new PointCollection();

            pc.Add(new Point(100, 300));
            pc.Add(new Point(200, 300));

            this.Points = pc;
        }
    }
}
