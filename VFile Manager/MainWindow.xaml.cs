using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using VFile_Manager.FileObjects;
using System.Threading;

namespace VFile_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static System.Threading.Timer timer;
        static object synclock = new object();
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
                someView.ItemsSource = FileOperator.SetActionForSort(FileOperator.GetDirContainsList(_s).ToList(), someSortComboBox.SelectedIndex);
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

        private void UpdateAllViews(object obj)
        {
            lock (synclock)
            {
                UpdateView(FileOperator.Side.Left);
                UpdateView(FileOperator.Side.Right);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileOperator.InitilalDirs();
            rightSortChooseBox.ItemsSource = FileOperator.sortTypes;
            leftSortChooseBox.ItemsSource = FileOperator.sortTypes;
            //timer = new System.Threading.Timer (new TimerCallback(UpdateAllViews), null, 0, 3000);
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
                FileOperator.HandleOpenFileOrDir(new Dir(selectedItemL), FileOperator.Side.Left);
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
                FileOperator.HandleOpenFileOrDir(new Dir(selectedItemR), FileOperator.Side.Right);
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
            if (e.Delta < 0)
            {
                leftScrollViewer.LineDown();
            }
            else
            {
                leftScrollViewer.LineUp();
            }
        }

        private void rightView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Right);
        }

        private void rightView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Right);
            if (e.Delta < 0)
            {
                rightScrollViewer.LineDown();
            }
            else
            {
                rightScrollViewer.LineUp();
            }
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

        private IEnumerable<IFileObject> GetSelectedItemsData()
        {
            ListBox someBox = null;
            if (FileOperator.ActiveDirectory == FileOperator.Side.Left)
                someBox = leftView;
            else if (FileOperator.ActiveDirectory == FileOperator.Side.Right)
                someBox = rightView;
            var selected = someBox.SelectedItems;
            return selected.Cast<IFileObject>();
        }

        private void copyBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IFileObject> selectedItems = GetSelectedItemsData();
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
            IEnumerable<IFileObject> selectedItems = GetSelectedItemsData();
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
            
        }

        private void renBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IFileObject> selectedItem = GetSelectedItemsData();
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
            IEnumerable<IFileObject> selectedItems = GetSelectedItemsData();
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
            IEnumerable<IFileObject> sel = GetSelectedItemsData();
            List<Dir> argumentsToFind = null;
            if (sel.Count() == 0)
            {
                argumentsToFind = new List<Dir> { File_Containers.FileDualContainer.ChooseContainer(FileOperator.ActiveDirectory).StoredDirectory };

            }
            else
            {
                argumentsToFind = sel.ToList().FindAll((item) => item.Info.IsDirectory).Select((item) => item as Dir).ToList();
                if (argumentsToFind == null)
                {
                    MessageBox.Show("No directories selected! Select directories and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (argumentsToFind.Count != sel.Count())
                {
                    if (MessageBox.Show($"Some selected elements dont support find operation. Find in {argumentsToFind.Count}", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        return;
                }
            }
            FindWindow fwindw = new FindWindow(argumentsToFind);
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
