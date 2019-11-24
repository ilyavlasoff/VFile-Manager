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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileOperator.InitilalDirs();
            leftView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Left);
            rightView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Right);
            List<String> drives = FileOperator.GetAllLogicalDrives().ToList();
            String logicalDriveL = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left).StoredDirectory.Info.LogicalDrive;
            String logicalDriveR = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right).StoredDirectory.Info.LogicalDrive;
            rootComboBoxL.ItemsSource = drives;
            rootComboBoxR.ItemsSource = drives;
            rootComboBoxR.SelectedItem = drives.Find((item) => logicalDriveR.Equals(item));
            rootComboBoxL.SelectedItem = drives.Find((item) => logicalDriveL.Equals(item));
            pathRowL.Text = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left).StoredDirectory.Info.FullName;
            pathRowR.Text = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right).StoredDirectory.Info.FullName;
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
                leftView.ItemsSource = null;
                leftView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Left);
                pathRowL.Text = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left).StoredDirectory.Info.FullName;
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
                rightView.ItemsSource = null;
                rightView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Right);
                pathRowR.Text = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right).StoredDirectory.Info.FullName;
            }
        }

        private void toParentDirButL_Click(object sender, RoutedEventArgs e)
        {
            FileOperator.NavigateToPreviousDirectory(FileOperator.Side.Left);
            leftView.ItemsSource = null;
            leftView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Left);
            pathRowL.Text = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Left).StoredDirectory.Info.FullName;
        }

        private void toParentDirButR_Click(object sender, RoutedEventArgs e)
        {
            FileOperator.NavigateToPreviousDirectory(FileOperator.Side.Right);
            rightView.ItemsSource = null;
            rightView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Right);
            pathRowR.Text = File_Containers.FileDualContainer.ChooseContainer(FileOperator.Side.Right).StoredDirectory.Info.FullName;
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
            leftView.ItemsSource = null;
            leftView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Left);
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
            rightView.ItemsSource = null;
            rightView.ItemsSource = FileOperator.GetDirContainsList(FileOperator.Side.Right);
        }

        private void setBut_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow setWin = new SettingsWindow();
            setWin.ShowDialog();
        }
    }
}
