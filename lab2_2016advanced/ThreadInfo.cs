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
        private bool status = false;
        public bool calculation_status
        {
            get { return status; }
            set
            {
                status = value;
                NotifityPropetyChanged();
            }
        }
        private int TimeWork;
        public int time
        {
            get { return TimeWork; }
            set {
                TimeWork = value;
                NotifityPropetyChanged();
            }
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
