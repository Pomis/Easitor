using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Easitor
{
    public interface ITool
    {
        string Name { get; set; }
        string Description { get; set; }
        string Icon { get; set; }
        string Color { get; set; }
        void StartAction(EditorModel _Model, MainWindow W, MouseButtonEventArgs e);
        void FinishAction();
        void Select();
        void Deselect();
        void MouseMove(MouseEventArgs e);
    }
}
