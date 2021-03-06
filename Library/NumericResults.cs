﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
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

        public bool min_flag = false;
        public bool max_flag = false;
        public double min_characteristics
        {
            get
            {
                if (min_flag) return min_ch;
                min_flag = true;
                min_ch = double.MaxValue;
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
            get { return (max_characteristics + min_characteristics) / 2; }
        }
        public double max_characteristics
        {
            get
            {
                if (max_flag) return max_ch;
                max_flag = true;
                max_ch = double.MinValue;
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
