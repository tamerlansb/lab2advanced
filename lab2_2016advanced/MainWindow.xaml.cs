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
        private void ExecuteNumericResultsCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ThreadsListinfo = new ObservableCollectionThreadInfo();
            DataSource.results.characteristics = new double[DataSource.results.partition_x,DataSource.results.partition_y];
            DataSource.results.min_flag = false;
            DataSource.results.max_flag = false;
            for (int i = 0; i < DataSource.count_of_units; ++i)
            {
                WorkerArg arg = new WorkerArg();
                arg.results = DataSource.results;
                arg.x0 = i * (arg.results.partition_x / arg.results.count_of_units);
                arg.x = (i + 1) * (arg.results.partition_x / arg.results.count_of_units) + (i == DataSource.count_of_units - 1 ? arg.results.partition_x % arg.results.count_of_units : 0);
                arg.threadinfo.unit_number = i;
                arg.threadinfo.time = Environment.TickCount;
                ThreadsListinfo.Add(arg.threadinfo);
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += workerDoWork;
                worker.RunWorkerCompleted += workerRunWorkerCompleted;
                

                worker.RunWorkerAsync(arg);
            }
            listBoxCalcultionStatus.ItemsSource = ThreadsListinfo;
            bool flag = false;
            while (!flag)
            {
                flag = true;
                foreach (ThreadInfo thread in ThreadsListinfo)
                {
                    flag = flag && thread.calculation_status;
                }
            }
            //DataSource.results = arg.results;
            /*************Отрисовка*************************/
            Binding bd = new Binding();
            bd.Source = DataSource;
            bd.Path = new PropertyPath("results");
            bd.Converter = new Converter();
            MAMResult.SetBinding(TextBlock.TextProperty, bd);
            ImageData = new NumericResultsImageData(DataSource.results);
            if (radioButtonPaletteManyColors.IsChecked == true)
                ImageData.palette = new Palette_ManyColors(DataSource.results.min_characteristics, DataSource.results.max_characteristics);
            if (radioButtonPalette2Colors.IsChecked == true)
                ImageData.palette = new Palette_2Colors(DataSource.results.average_characteristics);
            ImageData.createBitmapSource();
            imageCalculationResult.Source = ImageData.imageSource;
            /***********************************************/
            DataSource.IsCompleted = true;
        }
        private void workerDoWork(object sender, DoWorkEventArgs e)
        {
            WorkerArg arg = e.Argument as WorkerArg;
            arg.results.calculating_characteristics(arg.x, arg.x0);
            arg.threadinfo.time = Environment.TickCount - arg.threadinfo.time;
            arg.threadinfo.calculation_status = true;
            e.Result = arg;
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
            }
        }
        private void radioButtonPalette2Colors_Checked(object sender, RoutedEventArgs e)
        {
            try {
                radioButtonPaletteManyColors.IsChecked = false;
            } catch { }
            if (ImageData != null)
            {
                ImageData.palette = new Palette_2Colors(DataSource.results.average_characteristics);
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
        private void CanNewCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteNewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!DataSource.IsSaved && DataSource.IsCompleted)
            {
                MessageBoxResult rez = MessageBox.Show(" Данные не сохранены. Сохранить ? ", "", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                    ExecuteSaveCommandHandler(sender, e);
            }
            DataSource = new NumericResultsView();
            DataContext = DataSource;
            ThreadsListinfo = new ObservableCollectionThreadInfo();
            ImageData = null;
            listBoxCalcultionStatus.ItemsSource = ThreadsListinfo;
            MAMResult.Text = "";
            imageCalculationResult.Source = null;
        }

        private void CanOpenCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteOpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            string filename;
            if (!DataSource.IsSaved && DataSource.IsCompleted)
            {
                MessageBoxResult rez = MessageBox.Show(" Данные не сохранены. Сохранить ? ", "", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                    ExecuteSaveCommandHandler(sender, e);
            }
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
                NumericResults results = binF.Deserialize(fileStream) as NumericResults;
                DataSource = new NumericResultsView();
                DataSource.results = results;
                DataSource.image_data = new NumericResultsImageData(results);
                DataContext = DataSource;
                Binding bd = new Binding();
                bd.Source = DataSource;
                bd.Path = new PropertyPath("results");
                bd.Converter = new Converter();
                MAMResult.SetBinding(TextBlock.TextProperty, bd);
                ImageData = new NumericResultsImageData(DataSource.results);
                if (radioButtonPaletteManyColors.IsChecked == true)
                    ImageData.palette = new Palette_ManyColors(DataSource.results.min_characteristics, DataSource.results.max_characteristics);
                if (radioButtonPalette2Colors.IsChecked == true)
                    ImageData.palette = new Palette_2Colors(DataSource.results.average_characteristics);
                ImageData.createBitmapSource();
                imageCalculationResult.Source = ImageData.imageSource;
                ThreadsListinfo = new ObservableCollectionThreadInfo();
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
            string filename;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
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
                fileStream = File.Create(filename);
                BinaryFormatter binF = new BinaryFormatter();
                binF.Serialize(fileStream, DataSource.results);
                DataSource.IsSaved = true;
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
        private void Window_Closed(object sender, EventArgs e)
        {
            if (!DataSource.IsSaved && DataSource.IsCompleted)
            {
                MessageBoxResult rez = MessageBox.Show(" Данные не сохранены. Сохранить ? ", "", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                    ExecuteSaveCommandHandler(sender, null);
            }
        }
    }
}
