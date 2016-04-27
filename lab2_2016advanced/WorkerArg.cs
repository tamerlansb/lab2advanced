using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace lab2_2016advanced
{
    public class WorkerArg
    {
        public NumericResults results { get; set; }
        public ThreadInfo threadinfo { get; set; }
        public int x0 { get; set; }
        public int  x { get; set; }
    }
}
