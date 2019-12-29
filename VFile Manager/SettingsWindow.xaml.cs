using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VFile_Manager
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            SavedDataReader.OpenMode mode = SavedDataReader.Mode;
            if (mode == SavedDataReader.OpenMode.current) usePrHome.IsChecked = true;
            else if (mode == SavedDataReader.OpenMode.lastUsed) usePrev.IsChecked = true;
            else
            {
                useCustomDirs.IsChecked = true;
                List<String> dirs = SavedDataReader.GetSavedStartingDirsFromXml().ToList();
                leftPathTextBox.Text = dirs[0];
                rightPathTextBox.Text = dirs[1];
            }
            ReloadCustomAssociations();
        }

        private void ReloadCustomAssociations()
        {
            ObservableCollection<FileAssociation> fass = new ObservableCollection<FileAssociation> (SavedDataReader.GetCustomFileAssociation());
            pathGrid.ItemsSource = fass;
            
        }

        private void addNewExtensionSetting_Click(object sender, RoutedEventArgs e)
        {
            AddNewHandlerWindow addhandler = new AddNewHandlerWindow();
            if (addhandler.ShowDialog() == true)
            {
                ReloadCustomAssociations();
            }
        }

        private void saveSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void editExtensionSetting_Click(object sender, RoutedEventArgs e)
        {
            Int32 selected_index = pathGrid.SelectedIndex;
            if (selected_index == -1)
                return;
            AddNewHandlerWindow anhw = new AddNewHandlerWindow(pathGrid.Items[selected_index] as FileAssociation, selected_index);
            if (anhw.ShowDialog() == true)
            {
                ReloadCustomAssociations();
            }
        }

        private void removeExtensionSetting_Click(object sender, RoutedEventArgs e)
        {
            Int32 selected_index = pathGrid.SelectedIndex;
            if (selected_index != -1 )
            {
                SavedDataReader.DeleteCustomFileAssociation(selected_index);
            }
            ReloadCustomAssociations();
        }
    }
}
