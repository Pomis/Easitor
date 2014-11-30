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
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {
        Random R = new Random();
        public event EditorModel.EasterEggEventHandler LogoClicked;
        public About()
        {
            InitializeComponent();
            LogoClicked += EditorModel.StaticModel.ShowEasterEgg;
        }

        public void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (R.NextDouble()<=0.4)
                LogoClicked();
        }

        private void StaticModel_LogoClicked()
        {
            
        }
    }
}
