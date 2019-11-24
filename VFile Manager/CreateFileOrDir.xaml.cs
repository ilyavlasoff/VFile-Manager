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
    /// Логика взаимодействия для CreateFileOrDir.xaml
    /// </summary>
    public partial class CreateFileOrDir : Window
    {
        private bool isCreateDir;
        public CreateFileOrDir(Boolean createDir)
        {
            InitializeComponent();
            isCreateDir = createDir;
            Title = createDir ? "Create new directory" : "Create new file";
            titleForStr.Text = createDir ? "Directory name:" : "File name:";
            String creatingStr = createDir ? "NewDirectory" : "NewFile";
            FileObjects.Dir actDir = File_Containers.FileDualContainer.ChooseContainer(FileOperator.ActiveDirectory).StoredDirectory;
            String dirname = actDir.Info.FullName;
            Int32 fileNum = 1;
            while (actDir.GetFiles(creatingStr + fileNum.ToString()).Count() + actDir.GetDirectories(creatingStr + fileNum.ToString()).Count() != 0)
                fileNum++;
            pathRow.Text = dirname + "\\" + creatingStr + fileNum.ToString();
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            String filename = pathRow.Text;
            if (filename != String.Empty)
            {
                try
                {
                    FileOperator.MkDirFile(filename, isCreateDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to create file {filename}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            DialogResult = true;
            Close();
        }
    }
}
