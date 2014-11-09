using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
namespace Easitor
{
    public class PickerModel : CommonToolModel, ITool
    {
        bool IsPainting = false;
        MainWindow win;
        public PickerModel()
        {
            Name = "PickerTool";
            Description = "Пипеточка";
            Icon = "UI/PickerToolDark.png";
            Color = GRAY_2;
        }

        public void StartAction(EditorModel _Model, MainWindow W, MouseButtonEventArgs e)
        {
            win = W;
            Model = EditorModel.StaticModel;
            // Сначала делал через передачу ссылки на основной класс в аргумент, но потом дошло,
            // что можно сделать через статику. На работу не влияет, но со статикой меньше лишнего кода

            // Рендерим все слои
            RenderTargetBitmap Renderer = new RenderTargetBitmap(1500, 1500, 96, 96, PixelFormats.Default);
            Renderer.Render(W.PaintArea);
            WriteableBitmap BM = new WriteableBitmap(Renderer);

            // Получаем массив всех цветов в выбранном пикселе
            byte[] pixel = new byte[1 * 1 * BM.Format.BitsPerPixel / 8]; //bgra
            int stride = (BM.PixelWidth * BM.Format.BitsPerPixel) / 8; //Длина строки изображения в байтах. 
            BM.CopyPixels(new System.Windows.Int32Rect(
                (int)e.GetPosition(W.PaintArea).X + (int)Model.LeftColumnWidth,
                (int)e.GetPosition(W.PaintArea).Y, 
                1, 1), pixel, stride, 0);

            // Переводим в 16-систему и отправляем модели цвет
            string R = Convert.ToString(pixel[2], 16);
            string G = Convert.ToString(pixel[1], 16);
            string B = Convert.ToString(pixel[0], 16);
            Model.sliderColorView = "#" + R + G + B;
        }
        public void FinishAction(){}
        public void MouseMove(MouseEventArgs e){}
    }
}
