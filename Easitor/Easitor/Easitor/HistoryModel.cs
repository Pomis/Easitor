using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    public class HistoryModel :Singleton<HistoryModel>
    {
        private HistoryModel() { }
        private ObservableCollection<ICommand> commandHistory=new ObservableCollection<ICommand>();
        public ObservableCollection<ICommand> CommandHistory
        {
            get
            {
                return commandHistory;
            }

            set
            {
                commandHistory = value;
                OnPropertyChanged("CommandHistory");
            }
        }
    }
}
