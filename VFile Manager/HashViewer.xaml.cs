using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace VFile_Manager
{
    /// <summary>
    /// Логика взаимодействия для HashViewer.xaml
    /// </summary>
    public partial class HashViewer : Window
    {
        private SHA256 Sha256 = SHA256.Create();
        private MD5 Md5 = MD5.Create();
        private byte[] sha256HashSum, md5HashSum;

        private byte[] GetSha256Sum(String _file)
        {
            FileStream fs = File.OpenRead(_file);
            return Sha256.ComputeHash(fs);
        }
        private byte[] GetMD5Sum(String _file)
        {
            FileStream fs = File.OpenRead(_file);
            return Md5.ComputeHash(fs);
        }

        private void Calculate()
        {
            String path = fileNameBox.Text;
            if (!File.Exists(path))
            {
                MessageBox.Show($"File {path} is not exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sha256HashSum = GetSha256Sum(path);
            md5HashSum = GetMD5Sum(path);
        }

        public HashViewer(FileObjects.IFileObject _file)
        {
            InitializeComponent();
            this.fileNameBox.Text = _file.Info.FullName;
            Calculate();
            this.md5Box.Text = System.Text.Encoding.UTF8.GetString(md5HashSum);
            this.shaBox.Text = System.Text.Encoding.UTF8.GetString(sha256HashSum);
        }

        private void testBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (testBox.Text == String.Empty)
            {
                Color transparentColor = Color.FromArgb(100, 118, 210, 117);
                md5Box.Background = new SolidColorBrush(transparentColor);
                shaBox.Background = new SolidColorBrush(transparentColor);
            }
            else
            {
                shaBox.Background = new SolidColorBrush( testBox.Text == shaBox.Text ? Color.FromArgb(50, 118, 210, 117) : Color.FromArgb(50, 229, 57, 53));
                md5Box.Background = new SolidColorBrush(testBox.Text == md5Box.Text ? Color.FromArgb(50, 118, 210, 117) : Color.FromArgb(50, 229, 57, 53));
            }
        }

        private void closeBut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void copyMd5HashButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(md5Box.Text);
        }

        private void copyShaHashButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(shaBox.Text);
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }
    }
}
