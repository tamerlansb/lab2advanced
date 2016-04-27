using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Model
    {
        public string ModelName{ 
            get; set;
        }
        public DateTime DateOfProcessing
        {
            set;
            get;
        }
        public double propety1 { get; set; } 
        public double propety2 { get; set; }
    }
}
