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
    /// <summary>
    /// Кисточка - ежик
    /// </summary>
    public class HedgehogBrushModel : CommonToolModel, ITool
    {
     
        bool IsPainting = false;
        MainWindow win;
        public HedgehogBrushModel()
        {
            Name = "Hedgehog";
            Description = "Ёжик";
            Icon = "UI/BrushDark.png";
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

             Model.SelectedLayer.BrushOpacity = Model.sliderOpacity;
            
            IsPainting = true;

            //рисуем кружочки!
            Circle AddingCircle = new Circle(Model.sliderColorView,Model.sliderRadius,Model.sliderBlur,Model.sliderOpacity, e.GetPosition(W.PaintArea).X-Model.SelectedLayer.X, e.GetPosition(W.PaintArea).Y-Model.SelectedLayer.Y);
            Model.SelectedLayer.CircleList.Add(AddingCircle);

        }
        public void FinishAction()
        {
            if (IsPainting)
            {
                IsPainting = false;
                RenderTargetBitmap Renderer = new RenderTargetBitmap((int)Model.SelectedLayer.BitMap.Width, (int)Model.SelectedLayer.BitMap.Height, 96, 96, PixelFormats.Default);
                Renderer.Render(Model.RenderGrid);
                Model.SelectedLayer.BitMap = new WriteableBitmap(Renderer);
                // удаляем все старые кружочки.
                Model.SelectedLayer.CircleList.Clear();
                Model.SelectedLayer.ContinuedCircleList.Clear();
                // Записываем в историю
                //((ToolCommand)Command).LayerListAfter=Model.LayerList.ToList();
                HistoryModel.Instance.CommandHistory.Add(Command);
            }
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
                AddingCircle.X2 = Model.SelectedLayer.CircleList.Last().X;
                AddingCircle.Y2 = Model.SelectedLayer.CircleList.Last().Y;
                Model.SelectedLayer.ContinuedCircleList.Add(AddingCircle);
                
            }

        }
    }
}
