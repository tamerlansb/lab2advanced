using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace lab2_2016advanced
{
    public class Palette_ManyColors:CustomPalette
    {
        double min_val = 0;
        double max_val = 0;
        public Palette_ManyColors(double min_val , double max_val)
        {
            this.max_val = max_val;
            this.min_val = min_val;
            listOfColors = new List<Color>();
            listOfColors.Add(Colors.Red);
            listOfColors.Add(Colors.Orange);
            listOfColors.Add(Colors.Yellow);
            listOfColors.Add(Colors.Green);
            listOfColors.Add(Colors.DeepSkyBlue);
            listOfColors.Add(Colors.Blue);
            listOfColors.Add(Colors.Purple);
        }
        public override int ChooseIndexOfColorInPalette(double param)
        {
            return (int)(7*((param - min_val)/ (max_val - min_val)));
        }
    }
}
