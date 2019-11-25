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
        private FileObjects.IFileObject prototype;
        public CreateFileOrDir(FileObjects.IFileObject _prototype = null, Boolean createDir = false)
        {
            InitializeComponent();
            isCreateDir = createDir;
            prototype = _prototype;
            Title = _prototype == null ? (createDir ? "Create new directory" : "Create new file") : (createDir ? "Rename directory" : "Rename file");
            titleForStr.Text = createDir ? "Directory name:" : "File name:";
            if (_prototype == null)
            {
                String creatingStr = createDir ? "NewDirectory" : "NewFile";
                FileObjects.Dir actDir = File_Containers.FileDualContainer.ChooseContainer(FileOperator.ActiveDirectory).StoredDirectory;
                String dirname = actDir.Info.FullName;
                Int32 fileNum = 1;
                while (actDir.GetFiles(creatingStr + fileNum.ToString()).Count() + actDir.GetDirectories(creatingStr + fileNum.ToString()).Count() != 0)
                    fileNum++;
                pathRow.Text = dirname + "\\" + creatingStr + fileNum.ToString();
            }
            else
            {
                pathRow.Text = String.Empty;
            }
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            String filename = pathRow.Text;
            if (filename != String.Empty)
            {
                if (prototype == null)
                {
                    try
                    {
                        FileOperator.MkDirFile(filename, isCreateDir);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to create file {filename}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (!prototype.Exists())
                    {
                        MessageBox.Show($"File {prototype.Info.ShortName} is not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (MessageBox.Show($"Rename {prototype.Info.ShortName} to {filename}", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            FileOperator.Rename(prototype, filename);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unable to rename file {filename}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            DialogResult = true;
            Close();
        }
    }
}
