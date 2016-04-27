using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Library;

namespace lab2_2016advanced
{
    class NumericResultsImageData
    {
        public NumericResults results { get; set; }
        public CustomPalette palette { get; set; }
        public Point point { get; set; }
        public Size size { get; set; }
        public BitmapSource imageSource { get; set; }
        public NumericResultsImageData( NumericResults results)
        {
             this.results = results;
        }
        public void CoordMouseToIndex(ref int x, ref int y)
        {
            x = (int)(point.X * results.partition_x / size.Width);
            y = (int)(point.Y * results.partition_y / size.Height);
        }
        public void createBitmapSource()
        {
            PixelFormat pf = PixelFormats.Indexed8;
            int width = results.partition_x;
            int height = results.partition_y;
            byte[] colour_array = new byte[width * height];

            BitmapPalette plt = new BitmapPalette(palette.listOfColors);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    colour_array[x + y * width] = (byte)palette.ChooseIndexOfColorInPalette(results.characteristics[x, y]);
                }
            BitmapSource bitmap = BitmapSource.Create(width, height, width, height, pf, plt, colour_array, width);
            imageSource = bitmap;
        }
    }
}
