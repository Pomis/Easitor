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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Easitor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimeredActions Model = new TimeredActions();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Model;
        }

        //выход
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //левая панель
        private void Grid_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            Model.MoveLeftPanel();
        }

        //правая панель
        private void Grid_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            Model.MoveRightPanel();
        }

        //новый слой
        private void Grid_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {
            Model.NewLayer();
        }

        //открыть изображение
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Model.ImportImage();
        }

        //нажатие на инстумент
        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Model.SelectTool(((Grid)sender).ToolTip.ToString());
        }

        //тык по холсту
        private void Grid_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            if (Model.SelectedTool != null) Model.SelectedTool.StartAction(Model,this,e);
        }

        //движение мышью по холсту
        private void PaintArea_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (Model.SelectedTool != null) Model.SelectedTool.MouseMove(e);
        }

        //затык по холсту
        private void PaintArea_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (Model.SelectedTool != null) Model.SelectedTool.FinishAction();
        }

        //размытие гаусса
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Model.Blur();
        }

        //выбор слоя в панельке
        private void Grid_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            Model.ChooseLayer(((Grid)sender).ToolTip.ToString());
        }

        //перемещение слайдера
        private void Grid_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            Model.MoveSlider((double)e.GetPosition(sender as Grid).X / 200, (sender as Grid).ToolTip.ToString());
        }

        //движение мыши по канвасу для определения Visual-объекта для рендера по завершении рисования кистью
        //private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        //{
            
        //}

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            Model.CreateGridList((sender as Grid));
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Model.RunAutomator();
        }
    }
}
