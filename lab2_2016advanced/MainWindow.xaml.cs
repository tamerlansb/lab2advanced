using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using Library;

namespace lab2_2016advanced
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand ExecuteNumericResults = new RoutedCommand("ExecuteNumericResults", typeof(lab2_2016advanced.MainWindow));
        private NumericResultsView DataSource = new NumericResultsView();
        private NumericResultsImageData ImageData = null;
        public ObservableCollectionThreadInfo ThreadsListinfo = new ObservableCollectionThreadInfo();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            execute_button.Command = ExecuteNumericResults;
            this.DataContext = DataSource;
            listBoxCalcultionStatus.ItemsSource = ThreadsListinfo;
            DataTemplate template = this.TryFindResource("Listbox_datatempelate") as DataTemplate;
            if (template != null)
                listBoxCalcultionStatus.ItemTemplate = template;
            /*BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(@"C:\\Users\\Admin\\task3\\lena.bmp", UriKind.Absolute);
            bi3.EndInit();
            // myImage3.Stretch = Stretch.Fill;
            imageCalculationResult.Stretch = Stretch.Fill;
            imageCalculationResult.Source = bi3;*/

        }
        StringBuilder sb = new StringBuilder();
        private void CanExecuteNumericResultsCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            bool flag = true;
            foreach (FrameworkElement child in StackPanelConteiner.Children)
            {
                TextBox element = child as TextBox;
                if (element == null)
                {
                    DatePicker data = child as DatePicker;
                    if (data == null)
                        continue;
                    if (Validation.GetHasError(data))
                    {
                        flag = false;
                        break;
                    }
                    continue;
                }

                if (Validation.GetHasError(element))
                {
                    flag = false;
                    break;
                }
            }
            e.CanExecute = flag;
        }
        private void workerDoWork(object sender,DoWorkEventArgs e)
        {
            WorkerArg arg = e.Argument as WorkerArg;;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            arg.results.calculating_characteristics(arg.x, arg.x0);
            sw.Stop();
            ThreadsListinfo[arg.threadinfo.unit_number].time = (int)( sw.ElapsedMilliseconds / 100.0f);
             ThreadsListinfo[arg.threadinfo.unit_number].calculation_status = true;
            e.Result = arg;
            //   arg.threadinfo.calculation_status = true;

        }
        private void workerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Произошла ошибка");
            }
            else
            {
                WorkerArg res = e.Result as WorkerArg;
                for (int i = res.x0; i < res.x; ++i)
                    for (int j = 0; j < res.results.partition_y; ++j)
                    {
                        DataSource.results.characteristics[i, j] = res.results.characteristics[i, j];
                    }
            }
        }
        private void ExecuteNumericResultsCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ThreadsListinfo = new ObservableCollectionThreadInfo();
            listBoxCalcultionStatus.ItemsSource = ThreadsListinfo;
            DataSource.results.characteristics = new double[DataSource.partition_x, DataSource.partition_y];

            WorkerArg arg = new WorkerArg();
            arg.results = DataSource.results;
            arg.x0 = 0;
            arg.x = DataSource.partition_x /  DataSource.count_of_units;
            for (int i = 0; i < DataSource.count_of_units; ++i)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += workerDoWork;
                worker.RunWorkerCompleted += workerRunWorkerCompleted;

                ThreadInfo threadinfo = new ThreadInfo();
                arg.x0 = i * (DataSource.partition_x / DataSource.count_of_units);
                if (i!=DataSource.count_of_units-1)
                   arg.x = (i + 1) *( DataSource.partition_x / DataSource.count_of_units);
                else
                    arg.x = (i + 1) * (DataSource.partition_x / DataSource.count_of_units) + DataSource.partition_x % DataSource.count_of_units;
                threadinfo.unit_number = i;
                arg.threadinfo = threadinfo;
                ThreadsListinfo.Add(threadinfo);

                worker.RunWorkerAsync(arg);
            }
         /*   bool calc = false;
            do
            {
                foreach (ThreadInfo th in listinfo)
                {
                    calc = calc && th.calculation_status;
                }
            } while (!calc);*/
            DataSource.results = arg.results;
            ImageData = new NumericResultsImageData(DataSource.results);
            if (radioButtonPaletteManyColors.IsChecked == true)
                ImageData.palette = new Palette_ManyColors(DataSource.results.min_characteristics, DataSource.results.max_characteristics);
            if (radioButtonPalette2Colors.IsChecked == true)
                ImageData.palette = new Palette_2Colors((arg.results.min_characteristics + arg.results.max_characteristics)/2);
            ImageData.createBitmapSource();
            imageCalculationResult.Source = ImageData.imageSource;
            DataSource.IsCompleted = true;

            MessageBox.Show("CommandHandler " + DataSource.results.min_characteristics.ToString() + " " + DataSource.results.max_characteristics.ToString());
            /*System.IO.FileStream stream = null;
            stream = new System.IO.FileStream("C:\\Users\\Admin\\Desktop\\log.txt", System.IO.FileMode.Append);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
            for (int j= 0; j< DataSource.partition_y; ++j)
            {
                for (int i = 0; i < DataSource.partition_x;++i)
                {
                    writer.Write("{0} ", DataSource.results.characteristics[i, j]);
                }
                writer.WriteLine();
            }
            writer.Close();
            if (stream != null) stream.Close();*/
        }

        private void CanNewCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteNewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!DataSource.IsSaved) {
                MessageBoxResult rez = MessageBox.Show(" Данные не сохранены. Сохранить ? ", "", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                    ExecuteSaveCommandHandler(sender, e);
            } else
            {
                // очистка
            }
        }

        private void CanOpenCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteOpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            string filename;
            ExecuteSaveCommandHandler(sender, e);
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
                filename = dlg.FileName;
            else
            {
                filename = null;
                return;
            }
            FileStream fileStream = null;
            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binF = new BinaryFormatter();
                DataSource = binF.Deserialize(fileStream) as NumericResultsView;
                DataContext = DataSource;
                //listBox.ItemsSource = StundentsCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (fileStream != null)
                    fileStream.Close();
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (DataSource.IsCompleted && !DataSource.IsSaved)
                e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void ExecuteSaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void radioButtonPalette2Colors_Checked(object sender, RoutedEventArgs e)
        {
            try {
                radioButtonPaletteManyColors.IsChecked = false;
            } catch { }
            if (ImageData != null)
            {
                ImageData.palette = new Palette_2Colors((DataSource.results.min_characteristics + DataSource.results.max_characteristics)/2);
                ImageData.createBitmapSource();
                imageCalculationResult.Source = ImageData.imageSource;
            }
        }

        private void radioButtonPaletteManyColors_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonPalette2Colors.IsChecked = false;
            if (ImageData != null )
            {
                ImageData.palette = new Palette_ManyColors(DataSource.results.min_characteristics, DataSource.results.max_characteristics);
                ImageData.createBitmapSource();
                imageCalculationResult.Source = ImageData.imageSource;
            }
        }

        private void imageCalculationResult_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point CurrentPoint = e.GetPosition(imageCalculationResult);
            ImageData.point = CurrentPoint;
            ImageData.size = imageCalculationResult.RenderSize;
            int x = 0, y  = 0;
            ImageData.CoordMouseToIndex(ref x, ref y);
            DataSource.CurrentPoint = DataSource.results.characteristics[x, y];
            e.Handled = true;
        }


    }
}
