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
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Text;

namespace VFile_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                Dispatcher.Invoke(() => leftView.Background = Brushes.AliceBlue);
                Dispatcher.Invoke(() => rightView.Background = Brushes.White);
            }
            else if (_s == FileOperator.Side.Right)
            {
                Dispatcher.Invoke(() => rightView.Background = Brushes.AliceBlue);
                Dispatcher.Invoke(() => leftView.Background = Brushes.White);
            }
        }

        private void UpdateView(FileOperator.Side _s, IEnumerable<IFileObject> _files = null, bool isMakeActive = true)
        {
            ListBox someView = null;
            ComboBox someRootComboBox = null;
            TextBlock somePathRow = null;
            Grid someStatusStr = null;
            ComboBox someSortComboBox = null;
            if (_s == FileOperator.Side.Left)
            {
                Dispatcher.Invoke(() => someView = leftView);
                Dispatcher.Invoke(() => someRootComboBox = rootComboBoxL);
                Dispatcher.Invoke(() => somePathRow = pathRowL);
                Dispatcher.Invoke(() => someStatusStr = StatusStrL);
                Dispatcher.Invoke(() => someSortComboBox = leftSortChooseBox);
            }
            else if (_s == FileOperator.Side.Right)
            {
                Dispatcher.Invoke(() => someView = rightView);
                Dispatcher.Invoke(() => someRootComboBox = rootComboBoxR);
                Dispatcher.Invoke(() => somePathRow = pathRowR);
                Dispatcher.Invoke(() => someStatusStr = StatusStrR);
                Dispatcher.Invoke(() => someSortComboBox = rightSortChooseBox);
            }
            lock (synclock)
            {
                Dispatcher.Invoke(() => someView.ItemsSource = null);
                List<IFileObject> listOfContFiles = null;
                try
                {
                    listOfContFiles = (_files == null) ? FileOperator.GetDirContainsList(_s).ToList() : _files.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Files list can not be get", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    Dispatcher.Invoke(() => someView.ItemsSource = FileOperator.SetActionForSort(listOfContFiles, someSortComboBox.SelectedIndex));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot sort items: {ex.Message}. Show unsorted list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Dispatcher.Invoke(() => someView.ItemsSource = listOfContFiles);
                }
                List<String> drives = FileOperator.GetAllLogicalDrives().ToList();
                String logicalDrive = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info.LogicalDrive;
                try
                {
                    Dispatcher.Invoke(() => someRootComboBox.ItemsSource = drives);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("This disk is not availbale. Chack other disk", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Dispatcher.Invoke(() => someRootComboBox.SelectedItem = drives.Find((item) => logicalDrive.Equals(item)));
                Dispatcher.Invoke(() => somePathRow.Text = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info.ShortName);
                Dispatcher.Invoke(() => someStatusStr.DataContext = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info);
                if (isMakeActive)
                    MakeDirActive(_s);
            }
        }

        async Task UpdateAllViews()
        {
            List<IFileObject> leftList = FileOperator.GetDirContainsList(FileOperator.Side.Left).ToList();
            List<IFileObject> rightList = FileOperator.GetDirContainsList(FileOperator.Side.Right).ToList();
            while (true)
            {
                await Task.Delay(1000);
                //FileOperator.Side s = FileOperator.ActiveDirectory;
                List<IFileObject> leftNewList = FileOperator.GetDirContainsList(FileOperator.Side.Left).ToList();
                List<IFileObject> rightNewList = FileOperator.GetDirContainsList(FileOperator.Side.Right).ToList();
                List<String> drives = FileOperator.GetAllLogicalDrives().ToList();
                if (!CompareSortedFileLists(leftList, leftNewList) || !CompareSortedFileLists(rightList, rightNewList) || drives.Count != (rootComboBoxL.ItemsSource.Cast<string>().Count()))
                {
                    //lock (synclock)
                    {
                        await Task.Run(() => UpdateView(FileOperator.Side.Left, leftNewList, false));
                        await Task.Run(() => UpdateView(FileOperator.Side.Right, rightNewList, false));
                    }
                    leftList = leftNewList;
                    rightList = rightNewList;
                }
            }
        }

        private bool CompareSortedFileLists(List<IFileObject> _first, List<IFileObject> _second)
        {
            if (_first.Count != _second.Count)
                return false;
            for (Int32 i = 0; i!= _first.Count; ++i)
            {
                if (!_first[i].Equals(_second[i]))
                    return false;
            }
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileOperator.InitilalDirs();
            rightSortChooseBox.ItemsSource = FileOperator.sortTypes;
            leftSortChooseBox.ItemsSource = FileOperator.sortTypes;
            UpdateAllViews();
        }

        async private void leftViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                    MessageBox.Show($"This item can not be opened. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                await Task.Run(() => UpdateView(FileOperator.Side.Left));
            }
        }

        async private void rightViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                await Task.Run(() => UpdateView(FileOperator.Side.Right));
            }
        }

        async private void toParentDirButL_Click(object sender, RoutedEventArgs e)
        {
            FileOperator.NavigateToPreviousDirectory(FileOperator.Side.Left);
            await Task.Run(() => UpdateView(FileOperator.Side.Left));
        }

        async private void toParentDirButR_Click(object sender, RoutedEventArgs e)
        {
            FileOperator.NavigateToPreviousDirectory(FileOperator.Side.Right);
            await Task.Run(() => UpdateView(FileOperator.Side.Right));
        }

        async private void rootComboBoxL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedItemL = null;
            try
            {
                selectedItemL = (sender as ComboBox).SelectedItem.ToString();
                FileOperator.HandleOpenFileOrDir(new Dir(selectedItemL), FileOperator.Side.Left);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Directory {selectedItemL} doesnt exist or unavailable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await Task.Run(() => UpdateView(FileOperator.Side.Left));
        }

        async private void rootComboBoxR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedItemR = null;
            try
            {
                selectedItemR = (sender as ComboBox).SelectedItem.ToString();
                FileOperator.HandleOpenFileOrDir(new Dir(selectedItemR), FileOperator.Side.Right);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Directory {selectedItemR} doesnt exist or unavailable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await Task.Run(() => UpdateView(FileOperator.Side.Right));
        }

        private void setBut_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow setWin = new SettingsWindow();
            setWin.ShowDialog();
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

        private void leftViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            object item = (sender as ListBoxItem).DataContext;
            Int32 selIndex = leftView.Items.IndexOf(item);
            if (selIndex == -1) return;
            object clickedItem = leftView.Items.GetItemAt(selIndex);
            if (leftView.SelectedItems.Contains(clickedItem))
            {
                leftView.SelectedItems.Remove(clickedItem);
            }
            else
            {
                leftView.SelectedItems.Add(clickedItem);
            }

        }

        private void rightViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            object item = (sender as ListBoxItem).DataContext;
            Int32 selIndex = rightView.Items.IndexOf(item);
            if (selIndex == -1) return;
            object clickedItem = rightView.Items.GetItemAt(selIndex);
            if (rightView.SelectedItems.Contains(clickedItem))
            {
                rightView.SelectedItems.Remove(clickedItem);
            }
            else
            {
                rightView.SelectedItems.Add(clickedItem);
            }

        }

        async private void mkfileBut_Click(object sender, RoutedEventArgs e)
        {
            CreateFileOrDir crDial = new CreateFileOrDir();
            if (crDial.ShowDialog() == true)
                await Task.Run(() => UpdateView(FileOperator.ActiveDirectory));
        }

        async private void mkdirBut_Click(object sender, RoutedEventArgs e)
        {
            CreateFileOrDir crDial = new CreateFileOrDir(null, true);
            if (crDial.ShowDialog() == true)
            {
                await Task.Run(() => UpdateView(FileOperator.ActiveDirectory));
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

        private void DeletePreviouslyCreated(File_Containers.FileContainer receiver, IEnumerable<IFileObject> selectedItems)
        {
            foreach (IFileObject obj in selectedItems)
            {
                if (receiver.StoredDirectory.IsExistsHere(obj.Info.ShortName))
                {
                    MessageBoxResult res = MessageBox.Show($"File {obj.Info.ShortName} is already exists in {receiver.StoredDirectory.Info.ShortName}. Replace it?",
                        "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        String path = Path.Combine(receiver.StoredDirectory.Info.FullName, obj.Info.ShortName);
                        List<VFile_Manager.FileObjects.File> filesInDir = receiver.StoredDirectory.GetFiles().ToList();
                        List<Dir> dirsInDir = receiver.StoredDirectory.GetDirectories().ToList();
                        IFileObject fileobjToDelete = null;
                        fileobjToDelete = filesInDir.Find((item) => item.Info.ShortName == obj.Info.ShortName);
                        if (fileobjToDelete == null)
                            fileobjToDelete = dirsInDir.Find((item) => item.Info.ShortName == obj.Info.ShortName);
                        if (fileobjToDelete != null)
                        {
                            fileobjToDelete.Delete();
                        }
                    }
                }
            }
        }

        private void NotifyAboutFileOperations(String _notification)
        {
            operationTextBlock.Visibility = Visibility.Visible;
            fileInfo.Visibility = Visibility.Collapsed;
            operationTextBlock.Text = _notification;
        }

        private void ClearNotifyAboutFileOperations()
        {
            operationTextBlock.Visibility = Visibility.Collapsed;
            fileInfo.Visibility = Visibility.Visible;
        }

        async private void copyBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IFileObject> selectedItems = GetSelectedItemsData();
            if(selectedItems.Count() == 0)
            {
                MessageBox.Show("No elements selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            File_Containers.FileContainer receiver = FileOperator.ActiveDirectory == FileOperator.Side.Left ? File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right) :
                    File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left);
            Task delTask = Task.Run(() => DeletePreviouslyCreated(receiver, selectedItems));
            delTask.Wait();
            if (MessageBox.Show($"{selectedItems.Count()} items will be copied. Continue?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    NotifyAboutFileOperations($"Copy files to {receiver}");
                    await FileOperator.CopyFiles(selectedItems, receiver);
                    ClearNotifyAboutFileOperations();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to copy in this directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            await Task.Run(() => UpdateView(FileOperator.ActiveDirectory == FileOperator.Side.Left ? FileOperator.Side.Right : FileOperator.Side.Left));
        }

        async private void movBut_Click(object sender, RoutedEventArgs e)
        {
            List<IFileObject> selectedItems = GetSelectedItemsData().ToList();
            if(selectedItems.Count() == 0)
            {
                MessageBox.Show("No elements selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            File_Containers.FileContainer receiver = FileOperator.ActiveDirectory == FileOperator.Side.Left ? File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right) :
        File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left);
            DeletePreviouslyCreated(receiver, selectedItems); 
            if (MessageBox.Show($"{selectedItems.Count()} items will be moved. Continue?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    NotifyAboutFileOperations($"Move files to {receiver.StoredDirectory.Info.ShortName}");
                    await FileOperator.MoveFiles(selectedItems, receiver);
                    ClearNotifyAboutFileOperations();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to move in {receiver.StoredDirectory.Info.ShortName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    await Task.Run(() => UpdateView(FileOperator.Side.Left));
                    await Task.Run(() => UpdateView(FileOperator.Side.Right));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to renew views: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        async private void renBut_Click(object sender, RoutedEventArgs e)
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
                try
                {
                    await Task.Run(() => UpdateView(FileOperator.ActiveDirectory));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to renew views: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
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
                    //await Task.Run(() => UpdateView(FileOperator.ActiveDirectory));
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

        async private void leftSortChooseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Task.Run (() => UpdateView(FileOperator.Side.Left));
        }

        async private void rightSortChooseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Task.Run (() => UpdateView(FileOperator.Side.Right));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            object obj = (sender as ListBoxItem).DataContext;
        }

        private void cmdHere_Click(object sender, RoutedEventArgs e)
        {
            if (!FileOperator.StartCmd(File_Containers.FileDualContainer.ChooseContainer(FileOperator.ActiveDirectory).StoredDirectory))
            {
                MessageBox.Show("Error opening cmd from this directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void leftView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Left);
        }

        private void rightView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeDirActive(FileOperator.Side.Right);
        }

        private ListBox dragSource = null;

        #region GetDataFromListBox(ListBox,Point)
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

        #endregion

        private void leftView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object rawdata = GetDataFromListBox(dragSource, e.GetPosition(parent));
            if (rawdata == null)
                return;
            object str = ((IFileObject)rawdata).Info.FullName;
            if (str != null)
            {
                DragDrop.DoDragDrop(parent, str, DragDropEffects.Copy);
            }
        }

        private void rightView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object rawdata = GetDataFromListBox(dragSource, e.GetPosition(parent));
            if (rawdata == null)
                return;
            object str = ((IFileObject)rawdata).Info.FullName;
            if (str != null)
            {
                DragDrop.DoDragDrop(parent, str, DragDropEffects.Copy);
            }
        }

        async private Task DropHandler(object _sender, object _e, FileOperator.Side _side)
        {
            ListBox parent = (ListBox)_sender;
            if (parent == dragSource)
                return;
            String data = (String)(((DragEventArgs)_e).Data.GetData(typeof(String)));
            List<IFileObject> objToMove = ((IList<IFileObject>)dragSource.ItemsSource).Where((item) => item.Info.FullName == data).ToList();
            File_Containers.FileContainer receiver = File_Containers.FileDualContainer.ChooseContainer(_side);
            try
            {
                await Task.Run(() => FileOperator.CopyFiles(objToMove, receiver));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to copy in this directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ((IList<IFileObject>)dragSource.ItemsSource).Remove(objToMove[0]);
            await Task.Run(() => UpdateView(_side));
        }

        async private void leftView_Drop(object sender, DragEventArgs e)
        {
            await DropHandler(sender, e, FileOperator.Side.Left);
        }

        async private void rightView_Drop(object sender, DragEventArgs e)
        {
            await DropHandler(sender, e, FileOperator.Side.Right);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SavedDataReader.SetSavedStartingDirsToXml( SavedDataReader.OpenMode.lastUsed, File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left).StoredDirectory.Info.FullName, FileOperator.Side.Left);
            SavedDataReader.SetSavedStartingDirsToXml(SavedDataReader.OpenMode.lastUsed, File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right).StoredDirectory.Info.FullName, FileOperator.Side.Right);
            SavedDataReader.SaveUserDefinedFileAssociations();
        }

        private void compHash_Click(object sender, RoutedEventArgs e)
        {
            List<IFileObject> selectedItem = GetSelectedItemsData().ToList();
            if (selectedItem.Count() == 0)
            {
                MessageBox.Show("No elements selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (selectedItem.Count() > 1)
            {
                MessageBox.Show("Multiple hashsum calculating is not supported", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            HashViewer hw = new HashViewer(selectedItem[0]);
            hw.ShowDialog();
        }
    }
}
