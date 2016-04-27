using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_2016advanced
{
    public abstract class CustomPalette
    {
        public List<System.Windows.Media.Color> listOfColors;
        public abstract int ChooseIndexOfColorInPalette(double param);
    }
}
