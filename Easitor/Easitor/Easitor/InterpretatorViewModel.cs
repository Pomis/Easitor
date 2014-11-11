using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    public class InterpretatorViewModel:InterpretatorModel
    {
        string _ErrorView;
        public string ErrorView { get { return _ErrorView; } set { _ErrorView = value; OnPropertyChanged("ErrorView"); } }
        // Подсветка синтаксиса

        // Вывод ошибок
    }
}
