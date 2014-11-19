using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Easitor
{
    public class ToolCommand :INPC, ICommand
    {

        public void Execute()
        {
            EditorModel.StaticModel.LayerList = new ObservableCollection<Layer>(LayerListAfter); 
        }
        public void UnExecute()
        {
            EditorModel.StaticModel.LayerList = new ObservableCollection<Layer>(LayerListBefore);
        }
        public WriteableBitmap Bitmap;
        public List<Layer> LayerListBefore = new List<Layer>();
        public List<Layer> LayerListAfter = new List<Layer>();
        public double dX=0;
        public double dY=0;

    }
}
