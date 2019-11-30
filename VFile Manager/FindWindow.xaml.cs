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
using System.Windows.Shapes;
using VFile_Manager.FileObjects;

namespace VFile_Manager
{
    /// <summary>
    /// Логика взаимодействия для FindWindow.xaml
    /// </summary>
    public partial class FindWindow : Window
    {
        IEnumerable<Dir> DirsToFind;

        public FindWindow(IEnumerable<Dir> _initDir)
        {
            InitializeComponent();
            DirsToFind = _initDir;
            foreach (IFileObject dir in _initDir)
                dirsToFindBox.AppendText(dir.Info.FullName + "\n");
        }

        private void startSearchBut_Click(object sender, RoutedEventArgs e)
        {
            if (byNameRadio.IsChecked == true)
            {
                String searchConditionsAll = condByNameStr.Text;
                String[] searchConditions = searchConditionsAll.Split(',');
                try
                {
                    IEnumerable<IFileObject> foundedFiles = FileOperator.Find(DirsToFind, Dir.FindMode.Name, searchConditions);
                    resultBox.ItemsSource = foundedFiles;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search can not be executed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                } 
            }
            else if (byDateRadio.IsChecked == true)
            {
                DateTime startDate = lowDate.SelectedDate.Value;
                DateTime endDate = highDate.SelectedDate.Value;
                if (DateTime.Compare(startDate, endDate) > 0)
                {
                    MessageBox.Show("Start date is later than end date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                List<String> conditionsDate = new List<string> { startDate.ToString(), endDate.ToString() };
                try
                {
                    IEnumerable<IFileObject> foundedFiles = FileOperator.Find(DirsToFind, Dir.FindMode.Date, conditionsDate);
                    resultBox.ItemsSource = foundedFiles;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search can not be executed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (bySizeRadio.IsChecked == true)
            {
                String sizeAll = sizeSearchValue.Text;
                String[] searchSizes = sizeAll.Split('-');
                String modeSearch = ((ComboBoxItem)modeSelectionSize.SelectedItem).Content.ToString();
                String sizeSearch = ((ComboBoxItem)sizeSearchSize.SelectedItem).Content.ToString();
                double multCoef;
                if (sizeSearch == "KB")
                    multCoef = 1024;
                else if (sizeSearch == "MB")
                    multCoef = Math.Pow(1024, 2);
                else if (sizeSearch == "GB")
                    multCoef = Math.Pow(1024, 3);
                else
                {
                    throw new Exception("Undefined size mode");
                }
                List<String> conditions = new List<String>();
                List<double> sizes = new List<double>();
                foreach (String size in searchSizes)
                {
                    double outVal;
                    if (double.TryParse(size, out outVal))
                    {
                        outVal *= multCoef;
                        sizes.Add(outVal);
                    }
                    else
                    {
                        MessageBox.Show("Require digit parameter", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if (modeSearch == "more" && sizes.Count == 1)
                {
                    conditions.Add(sizes[0].ToString());
                    conditions.Add(Int32.MaxValue.ToString());
                }
                else if (modeSearch == "less" && sizes.Count == 1)
                {
                    conditions.Add(Int32.MinValue.ToString());
                    conditions.Add(sizes[0].ToString());
                }
                else if (modeSearch == "equals" && sizes.Count == 1)
                {
                    conditions.Add(sizes[0].ToString());
                    conditions.Add(sizes[0].ToString());
                }
                else if (modeSearch == "between" && sizes.Count == 2)
                {
                    conditions.Add(sizes[0].ToString());
                    conditions.Add(sizes[1].ToString());
                }
                else
                {
                    MessageBox.Show("These conditions are not achievable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    IEnumerable<IFileObject> foundedFiles = FileOperator.Find(DirsToFind, Dir.FindMode.Size, conditions);
                    resultBox.ItemsSource = foundedFiles;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search can not be executed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
