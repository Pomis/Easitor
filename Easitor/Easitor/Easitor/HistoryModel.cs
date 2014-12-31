using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    /// <summary>
    /// История действий, является одиночкой
    /// </summary>
    public class HistoryModel :Singleton<HistoryModel>
    {
        private HistoryModel()
        {
            CommandHistory.CollectionChanged += CommandHistory_CollectionChanged;
        }
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

        int Counter = 0;
        private void CommandHistory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (ICommand C in CommandHistory)
            {
                if (((ToolCommandViewModel)C) != null)
                {
                    ((ToolCommandViewModel)C).Background = "#00000000";
                }
            }
            if (e.NewItems!=null) foreach (ICommand C in e.NewItems)
            {
                if (((ToolCommandViewModel)C) != null)
                {
                    ((ToolCommandViewModel)C).CommandIndex += ++Counter;
                }
            }
            CommandHistory[CommandHistory.Count - 1].Select();
        }

        public void UndoUpTo(string Tag)
        {
            foreach (ICommand C in CommandHistory)
            {

                ((ToolCommandViewModel)C).Background = "";
                if (((ToolCommandViewModel)C) != null&& ((ToolCommandViewModel)C).CommandIndex == Tag)
                {
                    C.Select();
                    C.UnExecute();
                }
            }
                
        }

        public void CheckIfTooLong()
        {
            if (CommandHistory.Count >= 15)
            {
                CommandHistory.Remove(CommandHistory[0]);
            }
        }

    }
}
