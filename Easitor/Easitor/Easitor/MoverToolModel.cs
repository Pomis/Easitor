using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Easitor
{
    /// <summary>
    /// Инструмент-двигалка
    /// </summary>
    public class MoverToolModel : CommonToolModel, ITool
    {
        private double TempX = 0;
        private double TempY = 0;
        private bool AutoLayerSelection = false;

        public MoverToolModel()
        {
            Name = "MoverTool";
            Description = "Перетаскиватель слоёв";
            Icon = "UI/MoverToolDark.png";
            Color = GRAY_2;
        }
        Layer MovingLayer;
        
        public void StartAction(EditorModel _Model, MainWindow W, MouseButtonEventArgs e)
        {
            Model = _Model;
            CoordinateSystem=W.PaintArea;
            double MouseX=e.GetPosition(CoordinateSystem).X;
            double MouseY=e.GetPosition(CoordinateSystem).Y;
            //Выбор слоя){
            if (AutoLayerSelection)
            {
                foreach (Layer L in Model.LayerList)
                {
                    if ((L.X + L.Width >= MouseX && L.X <= MouseX || L.Y + L.Height >= MouseY && L.Y <= MouseY) && !L.IsBackground)
                    {
                        MovingLayer = L;

                        TempX = L.X + L.Width - MouseX;
                        TempY = L.Y + L.Height - MouseY;
                    }
                }
            }
            else
            {
                MovingLayer = Model.SelectedLayer;
                TempX = MovingLayer.X + MovingLayer.Width - MouseX;
                TempY = MovingLayer.Y + MovingLayer.Height - MouseY;
            }
        }
        public void MouseMove(MouseEventArgs e)
        {
            if (MovingLayer != null)
            {
                double MouseX = e.GetPosition(CoordinateSystem).X;
                double MouseY = e.GetPosition(CoordinateSystem).Y;
                //наркоман
                MovingLayer.X = MouseX-Math.Abs(MovingLayer.Width-TempX);
                MovingLayer.Y = MouseY - Math.Abs(MovingLayer.Height - TempY);
            }
        }
        public void FinishAction()
        {
            MovingLayer = null;
            TempY = 0;
            TempX = 0;
        }
    }
}
