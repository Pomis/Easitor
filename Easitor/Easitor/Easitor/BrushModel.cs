using System;
using System.Collections.Generic;
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
        bool IsPainting = false;
        MainWindow win;
        public BrushModel()
        {
            Name = "Brush";
            Description = "Кисточка";
            Icon = "UI/BrushDark.png";
            Color = GRAY_2;
        }

        public void StartAction(EditorModel _Model, MainWindow W, MouseButtonEventArgs e)
        {
            win = W;
            Model = _Model;
            
            Model.SelectedLayer.BrushOpacity = Model.sliderOpacity;
            
            IsPainting = true;
            //попиксельно оч медленно работает 
            #region попиксельно (useless)
            //var Map = Model.SelectedLayer.BitMap;
            //byte[] test={0};
            //for (int i = 150; i < 200; i++)
            //{
            //    for (int j = 150; j < 200; j++)
            //    {
            //        System.Windows.Media.Color Clr = Model.SelectedLayer.GetPixelColor(Map, i, j);
            //        byte[] testpix = {Clr.B,201,197,255};
            //        Map.WritePixels(new System.Windows.Int32Rect(i, j, 1, 1), testpix, 4, 0);
            //    }
            //}
            //Model.SelectedLayer.BitMap.CopyPixels(test, (int)Model.SelectedLayer.BitMap.Width * 4, 0);
            #endregion

            //рисуем кружочки!
            Circle AddingCircle = new Circle(Model.sliderColorView,Model.sliderRadius,Model.sliderBlur,Model.sliderOpacity, e.GetPosition(W.PaintArea).X-Model.SelectedLayer.X, e.GetPosition(W.PaintArea).Y-Model.SelectedLayer.Y);
            Model.SelectedLayer.CircleList.Add(AddingCircle);
        }
        public void FinishAction()
        {
            IsPainting = false;
            //Model.SelectedLayer.BitMap
            RenderTargetBitmap Renderer = new RenderTargetBitmap((int)Model.SelectedLayer.BitMap.Width, (int)Model.SelectedLayer.BitMap.Height, 96, 96, PixelFormats.Default);
            Renderer.Render(Model.RenderGrid);
            Model.SelectedLayer.BitMap = new WriteableBitmap(Renderer);
            //удаляем все старые кружочки.
            Model.SelectedLayer.CircleList.Clear();
        }
        public void MouseMove(MouseEventArgs e) 
        {
            if (IsPainting)
            {
                Circle AddingCircle = new Circle(Model.sliderColorView, Model.sliderRadius, Model.sliderBlur, Model.sliderOpacity, e.GetPosition(win.PaintArea).X - Model.SelectedLayer.X, e.GetPosition(win.PaintArea).Y - Model.SelectedLayer.Y);
                Model.SelectedLayer.CircleList.Add(AddingCircle);
            }
        }
    }
}
