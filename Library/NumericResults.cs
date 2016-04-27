using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class NumericResults : Model
    {
        public int partition_x { set; get; }
        public int partition_y { set; get; }
        public int count_of_units { set; get; }
        public double[,] characteristics { set; get; }
        public void calculating_characteristics(int x, int x0)
        {
            double deltaX = 1.0 / (double)partition_x,
                deltaY = 1.0 / (double)partition_y;
            for (int i = x0; i < x; ++i)
                for (int j = 0; j < partition_y; ++j)
                {
                    characteristics[i, j] =propety1*Math.Sin((i + 1)* propety2 * (j + 1)) * Math.Exp( Math.Pow((i  - partition_x / 2)*deltaX,2) + Math.Pow((j - partition_y / 2) * deltaY, 2)); 
                }
        }
        private double min_ch = double.MaxValue;
        private double max_ch = double.MinValue;
        private double aver_ch = double.MaxValue;
        public double min_characteristics
        {
            get
            { 
                for (int i = 0; i < partition_x; ++i)
                    for (int j = 0; j < partition_y; ++j)
                    {
                        if (characteristics[i, j] < min_ch)
                            min_ch = characteristics[i, j];
                    }
                return min_ch;
            }
        }
        public double average_characteristics
        {
            get { return (max_ch + min_ch) / 2; }
        }
        public double max_characteristics
        {
            get
            {
                for(int i = 0; i < partition_x; ++i)
                    for (int j = 0; j < partition_y; ++j)
                {
                    if (characteristics[i, j] > max_ch)
                        max_ch = characteristics[i, j];
                }
                return max_ch;
            }
        }
    }
}
