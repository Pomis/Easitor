using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Easitor
{
    class PixelColor
    {
       public byte Blue;
       public byte Green;
       public byte Red;
       public byte Alpha;
       public PixelColor[,] GetPixels(BitmapSource source)
        {
            if (source.Format != PixelFormats.Bgra32)
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

            int width = source.PixelWidth;
            int height = source.PixelHeight;
            PixelColor[,] result = new PixelColor[width, height];

            source.CopyPixels(result, width * 4, 0);
            return result;
        }
    }
}
