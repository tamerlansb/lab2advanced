using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Library;

namespace lab2_2016advanced
{
    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NumericResults results = value as NumericResults;
            return "Максимальное значение: " + results.max_characteristics.ToString() + "\n" +
                "Среднее значение: " + results.average_characteristics.ToString() + "\n" +
                "Минимальное значение: " + results.min_characteristics.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
