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
    /// <summary>
    /// Основная логика приложен
    /// </summary>
    public class EditorModel:INPC
    {
        #region Поля
        /// <summary>
       /// Слайдеры радиуса, прозрачности, размытия, цвета
       /// </summary>
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
        /// <summary>
        /// Выбранный цвет
        /// </summary>
        string _SelectedColor;
        public string SelectedColor { get { return _SelectedColor; } set { _SelectedColor = value; OnPropertyChanged("SelectedColor"); } }
        /// <summary>
        /// Грид для рендера
        /// </summary>
        public System.Windows.Controls.Grid RenderGrid;
        /// <summary>
        /// Количество добавленных слоев (не равно количеству слоев)
        /// </summary>
        public int LayersAdded = 0;
        /// <summary>
        /// Выбранный слой
        /// </summary>
        public Layer SelectedLayer { get; set; }
        /// <summary>
        /// Ширина выезжающий панелей
        /// </summary>
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

        ObservableCollection<ICommand> _CommandList = new ObservableCollection<ICommand>();
        public ObservableCollection<ICommand> CommandList
        {
            get
            {
                return _CommandList;
            }

            set
            {
                _CommandList = value;
                OnPropertyChanged("CommandList");
            }
        }

        public ITool SelectedTool;

        protected bool IsLeftPanelExpanding = false;
        protected bool IsLeftPanelShrinking = false;
        protected bool IsRightPanelExpanding = false;
        protected bool IsRightPanelShrinking = false;
        #endregion
        #region Делегаты и события
        public delegate void NewDocumentEventHandler();
        public delegate void EasterEggEventHandler();
        public event EasterEggEventHandler LogoClicked;
        public event NewDocumentEventHandler NewDocumentCreation;
        #endregion
        #region Статическая ссылка
        /// <summary>
        /// Статическая ссылка на экземпляр класса
        /// </summary>
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
            CommandList = HistoryModel.Instance.CommandHistory;
        }
        #endregion
        #region Методы
        #region Панели инструментов
        /// <summary>
        /// Анимация движения левой панельки
        /// </summary>
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
        /// <summary>
        /// Анимация движения правой панельки
        /// </summary>
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
            if (SelectedTool != null) SelectedTool.Deselect();
            foreach (ITool tool in ToolList)
            {
                if (tool.Description == _Description)
                {
                    SelectedTool = tool;
                    SelectedTool.Select();
                }
            }
        }
        /// <summary>
        /// Двигать слайдер
        /// </summary>
        /// <param name="Factor">Коэффициент умножения параметров</param>
        /// <param name="ToolTip">Подсказка</param>
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
                sliderOpacity = Factor * 1.1;
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
        /// <summary>
        /// Инициализация слайдеров
        /// </summary>
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
        /// <summary>
        /// Размытие Гаусса (пункт меню)
        /// </summary>
        public void Blur()
        {
            if (SelectedLayer != null)
                SelectedLayer.BlurRadius = 5;
        }
        #endregion

        #region Работа со слоями
        /// <summary>
        /// Новый слой
        /// </summary>
        public void NewLayer()
        {
            Layer L = new Layer(this);
            ChooseLayer(L);
        }
        // Скрыть или показать слой
        public void HideOrUnhide(string LayerToolTip)
        {
            foreach (Layer L in RevercedLayerList)
            {
                if (L.ToolTip == LayerToolTip)
                {
                    L.HideOrUnhide();
                }
            }
        }
        /// <summary>
        /// Выбрать слой
        /// </summary>
        /// <param name="_ToolTip">Подсказка</param>
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
        /// <summary>
        /// Выбрать слой
        /// </summary>
        /// <param name="_L">Слой</param>
        public void ChooseLayer(Layer _L)
        {
            foreach (Layer L in LayerList)
            {
                L.Background = "";

            }
            _L.Background = GRAY_2;
            SelectedLayer = _L;

        }
        /// <summary>
        /// Удалить выбраннй слой
        /// </summary>
        public void DeleteSelectedLayer()
        {
            RevercedLayerList.Remove(SelectedLayer);
            LayerList.Remove(SelectedLayer);
            SelectedLayer.Destroy();
            SelectedLayer = null;
            ChooseLayer(RevercedLayerList[0]);
        }

        // Передвинуть выбранный слой на топ
        public void MoveToTop()
        {
            RevercedLayerList.Move(RevercedLayerList.IndexOf(SelectedLayer), 0);
            LayerList.Move(LayerList.IndexOf(SelectedLayer), LayerList.Count - 1);
        }
        //  Передвинуть выбранный слой на фон
        public void MoveToBackground()
        {
            RevercedLayerList.Move(RevercedLayerList.IndexOf(SelectedLayer), RevercedLayerList.Count - 1);
            LayerList.Move(LayerList.IndexOf(SelectedLayer), 0);
        }
        #endregion

        #region Работа с изображениями, файлами
        /// <summary>
        /// Импорт изображения
        /// </summary>
        public void ImportImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                new Layer(filename, this);
            }
        }
        /// <summary>
        /// Конвертировать RenderTargetBitmap в BitmapImage
        /// </summary>
        /// <param name="renderTargetBitmap">конвертируемый объект</param>
        /// <returns></returns>
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
        /// <summary>
        /// Создать список гридов
        /// </summary>
        /// <param name="Grid">Добавляемый грид</param>
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
        public CustomFileDialog DialogWindow;
        /// <summary>
        /// Вызов окошка сохранения
        /// </summary>
        public void Save()
        {
            
            DialogWindow = new CustomFileDialog(DialogMode.Save);
            DialogWindow.Show();

           
        }
        /// <summary>
        /// Вызов окошка загрузки
        /// </summary>
        public void Load()
        {
            //ProjectEncoderModel Saver = new ProjectEncoderModel();
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.InitialDirectory = "";
            //dlg.DefaultExt = ".ead";
            //dlg.Filter = "Easitor Document Files (*.ead)|*.ead";
            //Nullable<bool> result = dlg.ShowDialog();
            //if (result == true)
            //{
                //Saver.DeserializeObject(dlg.FileName);
                //ChooseLayer(LayerList[0]);
            DialogWindow = new CustomFileDialog(DialogMode.Open);
            DialogWindow.Show();

            
        }
        #endregion

        #region Разное
        public void UndoUpTo(string Tag)
        {
            //HistoryModel.Instance.UndoUpTo(Tag);
        }

        

        // Новый документ
        public void NewDocument()
        {
            NewDocumentCreation(); // Запуск события
            LayerList.Clear();
            RevercedLayerList.Clear();
            //CommandList.Clear();
            LayersAdded = 0;
            Layer FirstLayer = new Layer(this);
            ChooseLayer(FirstLayer);
        }
        /// <summary>
        /// Пасхалка
        /// </summary>
        public void ShowEasterEgg()
        {
            System.Windows.MessageBox.Show
                ("Я — Албанский вирус, но в связи с очень плохим развитием технологии в моей стране к сожалению я не могу причинить вред вашему компьютеру.\n Пожалуйста будьте так любезны стереть один из важных файлов с вашего компьютера самостоятельно.\nЗаранее благодарен за понимание и сотрудничество.");
        }
        /// <summary>
        /// Запустить Автоматизатор
        /// </summary>
        public void RunAutomator()
        {
            InterpretatorWindow W = new InterpretatorWindow();
            W.Show();
            W.Focus();
        }
        #endregion  

        #region Рефлексия
        /// <summary>
        /// Загрузить классы, реализующие интерфейс
        /// </summary>
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
