using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Easitor
{
    public class BrushModel : CommonToolModel, ITool
    {
        double PrevX;
        double PrevY;
        double NowX;
        double NowY;
        bool IsPainting = false;
        MainWindow win;
        public BrushModel()
        {
            Name = "DottedBrush";
            Description = "Точечная кисточка";
            Icon = "UI/DottedBrushDark.png";
            Color = GRAY_2;
        }

        public void StartAction(EditorModel _Model, MainWindow W, MouseButtonEventArgs e)
        {
            win = W;
            Model = _Model;

            // Записываем в историю
            Command = new ToolCommandViewModel();
            ((ToolCommandViewModel)Command).Image = Icon;
            ((ToolCommandViewModel)Command).CommandName = Name;

            //((ToolCommand)Command).LayerListBefore = new List<Layer>
            //    (CloneClass.CloneObject<ObservableCollection<Layer>>(Model.LayerList));

            Model.SelectedLayer.BrushOpacity = Model.sliderOpacity;
            
            IsPainting = true;

            //рисуем кружочки!
            Circle AddingCircle = new Circle(Model.sliderColorView,Model.sliderRadius,Model.sliderBlur,Model.sliderOpacity, e.GetPosition(W.PaintArea).X-Model.SelectedLayer.X, e.GetPosition(W.PaintArea).Y-Model.SelectedLayer.Y);
            Model.SelectedLayer.CircleList.Add(AddingCircle);

            PrevX = e.GetPosition(W.PaintArea).X - Model.SelectedLayer.X;
            PrevY = e.GetPosition(W.PaintArea).Y - Model.SelectedLayer.Y;
        }
        public void FinishAction()
        {
            
            IsPainting = false;
            RenderTargetBitmap Renderer = new RenderTargetBitmap((int)Model.SelectedLayer.BitMap.Width, (int)Model.SelectedLayer.BitMap.Height, 96, 96, PixelFormats.Default);
            Renderer.Render(Model.RenderGrid);
            Model.SelectedLayer.BitMap = new WriteableBitmap(Renderer);
            // удаляем все старые кружочки.
            Model.SelectedLayer.CircleList.Clear();
            // Записываем в историю
            //((ToolCommand)Command).LayerListAfter=Model.LayerList.ToList();
            HistoryModel.Instance.CommandHistory.Add(Command);
            HistoryModel.Instance.CheckIfTooLong();
        }
        public void MouseMove(MouseEventArgs e) 
        {
            if (IsPainting)
            {
                Circle AddingCircle =     new Circle(Model.sliderColorView,
                                                        Model.sliderRadius,
                                                      Model.sliderBlur,
                                                   Model.sliderOpacity,
                    e.GetPosition(win.PaintArea).X - Model.SelectedLayer.X,
                    e.GetPosition(win.PaintArea).Y - Model.SelectedLayer.Y);
                Model.SelectedLayer.CircleList.Add(AddingCircle);
                
            }
        }
    }
}
