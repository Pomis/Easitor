using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    public class ToolCommandViewModel :ToolCommand
    {

        private string image;
        public string Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        string background;
        public string Background
        {
            get
            {
                return background;
            }

            set
            {
                background = value;
                OnPropertyChanged("Background");
            }
        }

        string commandName;
        public string CommandName
        {
            get
            {
                return commandName;
            }

            set
            {
                commandName = value;
                OnPropertyChanged("CommandName");
            }
        }

        


    }
}
