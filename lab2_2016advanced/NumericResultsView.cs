using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Library;

namespace lab2_2016advanced
{
    class NumericResultsView : INotifyPropertyChanged, IDataErrorInfo
    {
        public NumericResults results { get; set; }
        public NumericResultsImageData image_data;
        public NumericResultsView()
        {
            results = new NumericResults();
            image_data = new NumericResultsImageData(results);
            IsSaved = false;
            IsCompleted = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifityPropetyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public bool IsCompleted
        {
            get;
            set;
        }
        public bool IsSaved
        {
            get; // дописать 
        }

        // для IDataError
        public double propety2
        {
            set { results.propety2 = value; }
            get { return results.propety2; }
        }
        public double propety1
        {
            set { results.propety1 = value; }
            get { return results.propety1; }
        }
        public int count_of_units
        {
            set { results.count_of_units = value; }
            get { return results.count_of_units; }
        }
        public int partition_x
        {
            set { results.partition_x = value; }
            get { return results.partition_x; }
        }
        public int partition_y
        {
            set { results.partition_y = value; }
            get { return results.partition_y;  }
        }
        public DateTime DateOfProcessing
        {
            set { results.DateOfProcessing = value; }
            get { return results.DateOfProcessing;  }
        }
        public string ModelName
        {
            get { return results.ModelName; }
            set { results.ModelName = value; }
        }
        public string Error
        {
            get
            {
                return "Error 128\n";
            }
        }
        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "ModelName":
                        if (results.ModelName == "" || results.ModelName == null)
                            msg = "Name not must be empty";
                        break;
                    case "DateOfProcessing":
                        if (results.DateOfProcessing.Year < 2000 || results.DateOfProcessing.Year > 2015)
                            msg = "Year must be less 2015 and greater 2000";
                        break;
                    case "propety1":
                        if (propety1 <= 0 )
                            msg = "Propety1 must be nonnegative";
                        break;
                    case "propety2":
                        if (propety2 <= 0)
                            msg = "Propety2 must be nonnegative";
                        break;
                    case "count_of_units":
                        if (count_of_units < 1 || count_of_units > 20)
                            msg = "Count of units must be in [0, 20]";
                        break;
                    case "partition_x":
                        if (results.partition_x < 100 || results.partition_x > 1000)
                            msg = "Inccorrect partition X";
                        break;
                    case "partition_y":
                        if (results.partition_y < 100 || results.partition_y > 1000)
                            msg = "Inccorrect partition Y";
                        break;
                    default:
                        break;
                }
                return msg;
            }
         
        }

    }
}
