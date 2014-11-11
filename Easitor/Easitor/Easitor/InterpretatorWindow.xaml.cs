using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Easitor
{
    /// <summary>
    /// Логика взаимодействия для InterpretatorWindow.xaml
    /// </summary>
    public partial class InterpretatorWindow : Window
    {
        string myText;
        InterpretatorViewModel Inter = new InterpretatorViewModel();
        public InterpretatorWindow()
        {
            InitializeComponent();
            myText = new TextRange(CodeField.Document.ContentStart, CodeField.Document.ContentEnd).Text;
            DataContext = Inter;
        }

        private void CodeField_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            
            Inter.CheckSyntax(
                new TextRange(CodeField.Document.ContentStart, CodeField.Document.ContentEnd)
                .Text.ToString()
                             );
        }
    }
}
