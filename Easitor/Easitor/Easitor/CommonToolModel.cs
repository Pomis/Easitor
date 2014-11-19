using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Easitor
{
    public class CommonToolModel:INPC
    {
        public ICommand Command = new ToolCommand();

        public string Name { get; set; }
        public EditorModel Model { get; set; }
        public Grid CoordinateSystem { get; set; }
        public string Description { get; set; }
        string _Icon;
        public string Icon { get { return _Icon; } set { _Icon = value; OnPropertyChanged("Icon"); } }
        string _Color;
        public string Color { get { return _Color; } set { _Color = value; OnPropertyChanged("Color"); } }
        public void Select() 
        {
            Color = GRAY_5;
            Icon = "UI/" + Name + "Bright" + ".png";
        }
        public void Deselect()
        {
            Color = GRAY_2;
            Icon = "UI/" + Name + "Dark" + ".png";
        }
    }
}
