using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace lab2_2016advanced
{
    public class ThreadInfo : INotifyPropertyChanged
    {
        public int unit_number
        {
            get; set;
        }
        public bool calculation_status
        {
            get { return calculation_status; }
            set
            {
                calculation_status = value;
                NotifityPropetyChanged();
            }
        }
        public int time
        {
            get;  set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifityPropetyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
