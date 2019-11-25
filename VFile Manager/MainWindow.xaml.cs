using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VFile_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MakeDirActive(FileOperator.Side _s)
        {
            FileOperator.ActiveDirectory = _s;
            if (_s == FileOperator.Side.Left)
            {
                leftView.Background = Brushes.AliceBlue;
                rightView.Background = Brushes.White;
            }
            else if (_s == FileOperator.Side.Right)
            {
                rightView.Background = Brushes.AliceBlue;
                leftView.Background = Brushes.White;
            }
        }

        private void UpdateView(FileOperator.Side _s)
        {
            ListBox someView = null;
            ComboBox someRootComboBox = null;
            TextBlock somePathRow = null;
            Grid someStatusStr = null;
            ComboBox someSortComboBox = null;
            if (_s == FileOperator.Side.Left)
            {
                someView = leftView;
                someRootComboBox = rootComboBoxL;
                somePathRow = pathRowL;
                someStatusStr = StatusStrL;
                someSortComboBox = leftSortChooseBox;
            }
            else if (_s == FileOperator.Side.Right)
            {
                someView = rightView;
                someRootComboBox = rootComboBoxR;
                somePathRow = pathRowR;
                someStatusStr = StatusStrR;
                someSortComboBox = rightSortChooseBox;
            }
            someView.ItemsSource = null;
            try
            {
                someView.ItemsSource = FileOperator.SortList(FileOperator.GetDirContainsList(_s), someSortComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot sort items: {ex.Message}. Show unsorted list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                someView.ItemsSource = FileOperator.GetDirContainsList(_s);
            }
            List<String> drives = FileOperator.GetAllLogicalDrives().ToList();
            String logicalDrive = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info.LogicalDrive;
            someRootComboBox.ItemsSource = drives;
            someRootComboBox.SelectedItem = drives.Find((item) => logicalDrive.Equals(item));
            somePathRow.Text = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info.ShortName;
            someStatusStr.DataContext = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info;
            MakeDirActive(_s);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileOperator.InitilalDirs();
            rightSortChooseBox.ItemsSource = FileOperator.sortTypes;
            rightSortChooseBox.SelectedIndex = 0;
            leftSortChooseBox.ItemsSource = FileOperator.sortTypes;
            leftSortChooseBox.SelectedIndex = 0;
            UpdateView(FileOperator.Side.Left);
            UpdateView(FileOperator.Side.Right);
        }

        private void leftViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object obj = (sender as ListBoxItem).DataContext;
            if (obj != null)
            {
                try
                {
                    FileOperator.HandleOpenFileOrDir(obj, FileOperator.Side.Left);
                }
                catch (Exception ex)
                {

                }
                UpdateView(FileOperator.Side.Left);
            }
        }

        private void rightViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object obj = (sender as ListBoxItem).DataContext;
            if (obj != null)
            {
                try
                {
                    FileOperator.HandleOpenFileOrDir(obj, FileOperator.Side.Right);
                }
                catch (Exception ex)
                {

                }
                UpdateView(FileOperator.Side.Right);
            }
        }

        private void toParentDirButL_Click(object sender, RoutedEventArgs e)
        {
            FileOperator.NavigateToPreviousDirectory(FileOperator.Side.Left);
            UpdateView(FileOperator.Side.Left);
        }

        private void toParentDirButR_Click(object sender, RoutedEventArgs e)
        {
            FileOperator.NavigateToPreviousDirectory(FileOperator.Side.Right);
            UpdateView(FileOperator.Side.Right);
        }

        private void rootComboBoxL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedItemL = (sender as ComboBox).SelectedItem.ToString();
            try
            {
                FileOperator.HandleOpenFileOrDir(new FileObjects.Dir(selectedItemL), FileOperator.Side.Left);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Directory {selectedItemL} doesnt exist or unavailable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            UpdateView(FileOperator.Side.Left);
        }

        private void rootComboBoxR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedItemR = (sender as ComboBox).SelectedItem.ToString();
            try
            {
                FileOperator.HandleOpenFileOrDir(new FileObjects.Dir(selectedItemR), FileOperator.Side.Right);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Directory {selectedItemR} doesnt exist or unavailable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            UpdateView(FileOperator.Side.Right);
        }

        private void setBut_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow setWin = new SettingsWindow();
            setWin.ShowDialog();
        }

        private void leftView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Left);
        }

        private void leftView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Left);
        }

        private void rightView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Right);
        }

        private void rightView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Right);
        }

        private void leftViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            object item = (sender as ListBoxItem).DataContext;
            leftView.SelectedIndex = leftView.Items.IndexOf(item);
        }

        private void rightViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            object item = (sender as ListBoxItem).DataContext;
            rightView.SelectedIndex = rightView.Items.IndexOf(item);
        }

        private void mkfileBut_Click(object sender, RoutedEventArgs e)
        {
            CreateFileOrDir crDial = new CreateFileOrDir();
            if (crDial.ShowDialog() == true)
                UpdateView(FileOperator.ActiveDirectory);
        }

        private void mkdirBut_Click(object sender, RoutedEventArgs e)
        {
            CreateFileOrDir crDial = new CreateFileOrDir(null, true);
            if (crDial.ShowDialog() == true)
            {
                UpdateView(FileOperator.ActiveDirectory);
            }
        }

        private IEnumerable<FileObjects.IFileObject> GetSelectedItemsData()
        {
            ListBox someBox = null;
            if (FileOperator.ActiveDirectory == FileOperator.Side.Left)
                someBox = leftView;
            else if (FileOperator.ActiveDirectory == FileOperator.Side.Right)
                someBox = rightView;
            var selected = someBox.SelectedItems;
            return selected.Cast<FileObjects.IFileObject>();
        }

        private void copyBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<FileObjects.IFileObject> selectedItems = GetSelectedItemsData();
            if(selectedItems.Count() == 0)
            {
                MessageBox.Show("No elements selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            File_Containers.FileContainer receiver = FileOperator.ActiveDirectory == FileOperator.Side.Left ? File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right) :
                    File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left);
            if (MessageBox.Show($"{selectedItems.Count()} items will be copied. Continue?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    FileOperator.CopyFiles(selectedItems, receiver);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to copy in this directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            UpdateView(FileOperator.ActiveDirectory == FileOperator.Side.Left ? FileOperator.Side.Right : FileOperator.Side.Left);
        }

        private void movBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<FileObjects.IFileObject> selectedItems = GetSelectedItemsData();
            if(selectedItems.Count() == 0)
            {
                MessageBox.Show("No elements selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            File_Containers.FileContainer receiver = FileOperator.ActiveDirectory == FileOperator.Side.Left ? File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right) :
                    File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left);
            if (MessageBox.Show($"{selectedItems.Count()} items will be moved. Continue?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    FileOperator.MoveFiles(selectedItems, receiver);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to move in this directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                UpdateView(FileOperator.Side.Left);
                UpdateView(FileOperator.Side.Right);
            }
        }

        private void r_Click(object sender, RoutedEventArgs e)
        {
            UpdateView(FileOperator.Side.Left);
            UpdateView(FileOperator.Side.Right);
        }

        private void renBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<FileObjects.IFileObject> selectedItem = GetSelectedItemsData();
            if (selectedItem.Count() == 0)
            {
                MessageBox.Show("No elements selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (selectedItem.Count() > 1)
            {
                MessageBox.Show("Multiple renaming is not supported", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CreateFileOrDir cfd = new CreateFileOrDir(selectedItem.ToList()[0]);
            if (cfd.ShowDialog() == true)
            {
                UpdateView(FileOperator.ActiveDirectory);
            }
        }

        private void delBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<FileObjects.IFileObject> selectedItems = GetSelectedItemsData();
            if(selectedItems.Count() == 0)
            {
                MessageBox.Show("No elements selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show($"{selectedItems.Count()} items will be removed. THIS ACTION CAN NOT BE UNDONE. Continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    FileOperator.Delete(selectedItems);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot delete files : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void findBut_Click(object sender, RoutedEventArgs e)
        {
            FindWindow fwindw = new FindWindow(File_Containers.FileDualContainer.ChooseContainer(FileOperator.ActiveDirectory).StoredDirectory);
            fwindw.ShowDialog();
        }

        private void leftSortChooseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateView(FileOperator.Side.Left);
        }

        private void rightSortChooseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateView(FileOperator.Side.Right);
        }
    }
}
