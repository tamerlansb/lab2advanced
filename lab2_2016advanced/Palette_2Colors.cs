using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace lab2_2016advanced
{
    public class Palette_2Colors : CustomPalette
    {
        double BorderValue;
        public Palette_2Colors(double BorderValue)
        {
            listOfColors = new List<Color>();
            listOfColors.Add(Colors.Red);
            listOfColors.Add(Colors.Blue);
            this.BorderValue = BorderValue;
        }
        public override int ChooseIndexOfColorInPalette(double param)
        {
            if (param >= BorderValue)
                return 1;
            else 
                return 0;
        }
    }
}
