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
    /// Логика взаимодействия для CustomFileDialog.xaml
    /// </summary>
    public partial class CustomFileDialog : Window
    {
        CustomFileDialogModel Model = new CustomFileDialogModel();
        public CustomFileDialog(DialogMode Mode)
        {
            InitializeComponent();
            DataContext = Model;
            Model.SelectMode(Mode);
            Model.OpenFolder();
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Model.Click(((Grid)sender).Tag.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Model.ButtonClick();
        }
    }
}
