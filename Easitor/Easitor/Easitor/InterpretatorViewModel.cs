using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    public class InterpretatorViewModel:InterpretatorModel
    {
        // Подсветка синтаксиса

        // Вывод ошибок
        ObservableCollection<ErrorViewModel> _ErrorList = new ObservableCollection<ErrorViewModel>();
        public ObservableCollection<ErrorViewModel> ErrorList
        {
            get { return _ErrorList; }
            set { _ErrorList = value; }
        }
    }

    public class ErrorViewModel:INPC
    {
        string _ErrorView;
        public string ErrorView { get { return _ErrorView; } set { _ErrorView = value; OnPropertyChanged("ErrorView"); } }
        
        string _Background;
        public string Background {get {return _Background; } set {_Background = value; OnPropertyChanged("Background");}}
    }
}
