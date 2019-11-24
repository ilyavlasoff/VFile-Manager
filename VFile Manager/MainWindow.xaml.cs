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
            if (_s == FileOperator.Side.Left)
            {
                someView = leftView;
                someRootComboBox = rootComboBoxL;
                somePathRow = pathRowL;
                someStatusStr = StatusStrL;
            }
            else if (_s == FileOperator.Side.Right)
            {
                someView = rightView;
                someRootComboBox = rootComboBoxR;
                somePathRow = pathRowR;
                someStatusStr = StatusStrR;
            }
            someView.ItemsSource = null;
            someView.ItemsSource = FileOperator.GetDirContainsList(_s);
            List<String> drives = FileOperator.GetAllLogicalDrives().ToList();
            String logicalDrive = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info.LogicalDrive;
            someRootComboBox.ItemsSource = drives;
            someRootComboBox.SelectedItem = drives.Find((item) => logicalDrive.Equals(item));
            somePathRow.Text = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info.FullName;
            someStatusStr.DataContext = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.Info;
            MakeDirActive(_s);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileOperator.InitilalDirs();
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
            CreateFileOrDir crDial = new CreateFileOrDir(false);
            if (crDial.ShowDialog() == true)
                UpdateView(FileOperator.ActiveDirectory);
        }

        private void mkdirBut_Click(object sender, RoutedEventArgs e)
        {
            CreateFileOrDir crDial = new CreateFileOrDir(true);
            if (crDial.ShowDialog() == true)
            {
                UpdateView(FileOperator.ActiveDirectory);
            }
        }

        private void copyBut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void movBut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
