using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
namespace Easitor
{
    public class EditorModel:INPC
    {
        #region Поля

        #region Поля слайдеров

        public double sliderRadius { get; set; }
        public double sliderOpacity { get; set; }
        public string sliderColor { get; set; }
        public double sliderBlur {get; set;}

        string _sliderRadiusView;
        public string sliderRadiusView { get { return _sliderRadiusView; } set { _sliderRadiusView = value; OnPropertyChanged("sliderRadiusView"); } }
        string _sliderOpacityView;
        public string sliderOpacityView { get { return _sliderOpacityView; } set { _sliderOpacityView = value; OnPropertyChanged("sliderOpacityView"); } }
        string _sliderColorView;
        public string sliderColorView { get { return _sliderColorView; } set { _sliderColorView = value; OnPropertyChanged("sliderColorView"); } }
        string _sliderBlurView;
        public string sliderBlurView { get { return _sliderBlurView; } set { _sliderBlurView = value; OnPropertyChanged("sliderBlurView"); } }

        double _sliderRadiusWidth;
        public double sliderRadiusWidth { get { return _sliderRadiusWidth; } set { _sliderRadiusWidth = value; OnPropertyChanged("sliderRadiusWidth"); } }
        double _sliderOpacityWidth;
        public double sliderOpacityWidth { get { return _sliderOpacityWidth; } set { _sliderOpacityWidth = value; OnPropertyChanged("sliderOpacityWidth"); } }
        double _sliderColorWidth;
        public double sliderColorWidth { get { return _sliderColorWidth; } set { _sliderColorWidth = value; OnPropertyChanged("sliderColorWidth"); } }
        double _sliderBlurWidth;
        public double sliderBlurWidth { get { return _sliderBlurWidth; } set { _sliderBlurWidth = value; OnPropertyChanged("sliderBlurWidth"); } }

        #endregion

        public System.Windows.Controls.Grid RenderGrid;

        public int LayersAdded = 0;
        public Layer SelectedLayer { get; set; }

        double _LeftColumnWidth;
        public double LeftColumnWidth { get { return _LeftColumnWidth; } set { _LeftColumnWidth = value; OnPropertyChanged("LeftColumnWidth"); } }
        double _RightColumnWidth;
        public double RightColumnWidth { get { return _RightColumnWidth; } set { _RightColumnWidth = value; OnPropertyChanged("RightColumnWidth"); } }

        public double Width { get; set; }
        public double Height { get; set; }

        string _LeftPanelButtonBackground=GRAY_2;
        public string LeftPanelButtonBackground { get { return _LeftPanelButtonBackground; } set { _LeftPanelButtonBackground = value; OnPropertyChanged("LeftPanelButtonBackground"); } }
        string _LeftPanelButtonImage="UI/HistoryDark.png";
        public string LeftPanelButtonImage { get { return _LeftPanelButtonImage; } set { _LeftPanelButtonImage = value; OnPropertyChanged("LeftPanelButtonImage"); } }
        string _RightPanelButtonBackground = GRAY_2;
        public string RightPanelButtonBackground { get { return _RightPanelButtonBackground; } set { _RightPanelButtonBackground = value; OnPropertyChanged("RightPanelButtonBackground"); } }
        string _RightPanelButtonImage="UI/LayersDark.png";
        public string RightPanelButtonImage { get { return _RightPanelButtonImage; } set { _RightPanelButtonImage = value; OnPropertyChanged("RightPanelButtonImage"); } }

        ObservableCollection<Layer> _LayerList = new ObservableCollection<Layer>();
        public ObservableCollection<Layer> LayerList { get { return _LayerList; } set { _LayerList = value; OnPropertyChanged("LayerList"); } }
        ObservableCollection<Layer> _RevercedLayerList = new ObservableCollection<Layer>();
        public ObservableCollection<Layer> RevercedLayerList { get { return _RevercedLayerList; } set { _RevercedLayerList = value; OnPropertyChanged("RevercedLayerList"); } }
        ObservableCollection<ITool> _ToolList = new ObservableCollection<ITool>();
        public ObservableCollection<ITool> ToolList { get { return _ToolList; } set { _ToolList = value; OnPropertyChanged("ToolList"); } }

        public ITool SelectedTool;

        protected bool IsLeftPanelExpanding = false;
        protected bool IsLeftPanelShrinking = false;
        protected bool IsRightPanelExpanding = false;
        protected bool IsRightPanelShrinking = false;
        #endregion
        #region Статическая ссылка
        static public EditorModel StaticModel;
        void MakeStaticLink()
        {
            StaticModel = this;
        }
        #endregion

        #region Инициализация
        public EditorModel()
        {
            GetTypes();
            Layer FirstLayer = new Layer(this);
            ChooseLayer(FirstLayer);
            MakeStaticLink();
            InitializeSliders();
        }
        #endregion
        #region Методы
        public void MoveLeftPanel()
        {
            if (LeftColumnWidth == 0 || IsLeftPanelShrinking)
            {
                IsLeftPanelExpanding = true;
                IsLeftPanelShrinking = false;
            }
            else if (LeftColumnWidth >= MAX_LEFT_COLUMN_WIDTH || IsLeftPanelExpanding)
            {
                IsLeftPanelExpanding = false;
                IsLeftPanelShrinking = true;
            }
        }
        public void MoveRightPanel()
        {
            if (RightColumnWidth == 0 || IsRightPanelShrinking)
            {
                IsRightPanelExpanding = true;
                IsRightPanelShrinking = false;
            }
            else if (RightColumnWidth >= MAX_RIGHT_COLUMN_WIDTH || IsRightPanelExpanding)
            {
                IsRightPanelExpanding = false;
                IsRightPanelShrinking = true;
            }
        }

        // Выбрать инструмент по описанию
        public void SelectTool(string _Description)
        {
            if (SelectedTool!=null)SelectedTool.Deselect();
            foreach (ITool tool in ToolList)
            {
                if (tool.Description == _Description)
                {
                    SelectedTool = tool;
                    SelectedTool.Select();
                }
            }
        }


        public void MoveSlider(double Factor, string ToolTip)
        {
            if (ToolTip == "Радиус выбранного интрумента")
            {
                sliderRadius = Factor * 200;
                sliderRadiusView = "Радиус: " + Factor * 100;
                sliderRadiusWidth = Factor * 200;
            }
            else if (ToolTip == "Непрозрачность наносимого изображения")
            {
                sliderOpacity = Factor*1.1;
                if (Factor * 1.1 < 1) sliderOpacityView = "Непрозрачность: " + Factor * 110 + "%";
                else sliderOpacityView = "Непрозрачность: 100%";
                sliderOpacityWidth = Factor * 200;
            }
            else if (ToolTip == "Чёткость границы")
            {
                if (Factor * 1.1 < 1)
                {
                    sliderBlurView = "Жёсткость: " + Factor * 110 + "%";
                    sliderBlur = (1 - Factor) * sliderRadius;
                }
                else
                {
                    sliderBlurView = "Жёсткость: 100%";
                    sliderBlur = 0;
                }
                sliderBlurWidth = Factor * 200;
            }
        }
        void InitializeSliders()
        {
            sliderRadius = 5;
            sliderRadiusView = "Радиус: 5";
            sliderRadiusWidth = 5 * 2;

            sliderBlur = 0;
            sliderBlurView = "Жёсткость: 100%";
            sliderBlurWidth = 200;

            sliderOpacity = 1;
            sliderOpacityWidth = 200;
            sliderOpacityView = "Непрозрачность: 100%";

            sliderColorView = "#343546";
        }

        public void Blur()
        {
            if(SelectedLayer!=null)
                SelectedLayer.BlurRadius = 5;
        }

        public void NewLayer()
        {
            Layer L = new Layer(this);
            LayerList.Add(L);
            ChooseLayer(L);
        }
        public void ImportImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                new Layer(filename, this);
            }
        }

        public BitmapImage ConvertToBitmapImage(RenderTargetBitmap renderTargetBitmap)
        {
            //var renderTargetBitmap = getRenderTargetBitmap();
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        List<Grid> Grids = new List<Grid>();
        public void CreateGridList(Grid Grid)
        {
            RenderGrid = Grid;
            if (Grids.Count() == 0) Grids.Add(Grid);
            bool IsOK = false;
            foreach (Grid grid in Grids)
            {
                if (!Grid.Equals(grid, Grid))
                {
                    IsOK = true;
                }
            }
            if (IsOK) Grids.Add(Grid);
        }

        public void ChooseLayer(string _ToolTip)
        {
            foreach (Layer L in LayerList)
            {
                L.Background = "";
                if (L.ToolTip == _ToolTip)
                {
                    L.Background = GRAY_2;
                    SelectedLayer = L;
                    foreach (Grid grid in Grids)
                    {
                        if (SelectedLayer != null && grid.Tag.ToString() == SelectedLayer.ToolTip)
                        {
                            RenderGrid = grid;
                        }
                    }
                }
            }
        }
        public void ChooseLayer(Layer _L)
        {
            foreach (Layer L in LayerList)
            {
                L.Background = "";
                
            }
            _L.Background = GRAY_2;
            SelectedLayer = _L;
            
        }


        public void RunAutomator()
        {
            InterpretatorWindow W = new InterpretatorWindow();
            W.Show();
            W.Focus();
        }
        #region Рефлексия
        public void GetTypes()
        {
            var t = typeof(ITool);
            var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(typ => typ.GetTypes())
                .Where(x => t.IsAssignableFrom(x));
            //на всякий случай избавляемся от интерфейсов
            List<Type> toolTypes = new List<Type>();
            foreach (Type type in assemblyTypes)
            {
                   if (type.IsInterface || type.IsAbstract)
                   {
                        continue;
                   }
                   else
                   {
                       toolTypes.Add(type);
                   }    
            }
            foreach (Type type in toolTypes)
            {
                ITool plugin = (ITool)Activator.CreateInstance(type);
                ToolList.Add(plugin);
            }
            // Пусть первый инструмент будет выбран по умолчанию!
            ToolList[0].Select();
            SelectedTool = ToolList[0];
        }
        #endregion
        #endregion
    }
}
