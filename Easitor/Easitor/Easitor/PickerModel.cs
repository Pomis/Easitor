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
    public class PickerModel : CommonToolModel, ITool
    {
        bool IsPainting = false;
        MainWindow win;
        public PickerModel()
        {
            Name = "PickerTool";
            Description = "Пипеточка";
            Icon = "UI/PickerToolDark.png";
            Color = GRAY_2;
        }

        public void StartAction(EditorModel _Model, MainWindow W, MouseButtonEventArgs e)
        {
            win = W;
            Model = _Model;
        }
        public void FinishAction(){}
        public void MouseMove(MouseEventArgs e){}
    }
}
