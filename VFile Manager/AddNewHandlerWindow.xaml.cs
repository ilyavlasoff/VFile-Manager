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

namespace VFile_Manager
{
    /// <summary>
    /// Логика взаимодействия для AddNewHandlerWindow.xaml
    /// </summary>
    public partial class AddNewHandlerWindow : Window
    {
        public AddNewHandlerWindow()
        {
            InitializeComponent();
        }

        static Int32 index = -1;

        public AddNewHandlerWindow(FileAssociation _prototype, Int32 _index)
        {
            InitializeComponent();
            index = _index;
            typeBox.Text = _prototype.Filename;
            handlerBox.Text = _prototype.Filename;
            overrideCheckbox.IsChecked = _prototype.IsOverrideSystem;
        }


        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            String filetype = typeBox.Text;
            String handler = handlerBox.Text;
            if (filetype == String.Empty || handler == String.Empty || Char.IsDigit(filetype[0]) || Char.IsDigit(handler[0]))
            {
                MessageBox.Show("File type or handler name cant starts with digit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Boolean isOverride = overrideCheckbox.IsChecked ?? false;
            if (filetype == String.Empty || handler == String.Empty)
            {
                MessageBox.Show("Type or handler value can't be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (index != -1)
                {
                    SavedDataReader.EditCustomFileAssociation(index, filetype, handler, isOverride);
                }
                else
                {
                    SavedDataReader.SetCustomFileAssociation(filetype, handler, isOverride);
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Association can not be set: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                return;
            }
            this.Close();
        }
    }
}
