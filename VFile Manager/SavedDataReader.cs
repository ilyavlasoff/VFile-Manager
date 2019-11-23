using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
namespace VFile_Manager
{
    static class SavedDataReader
    {
        private static String saved_path = "saved_paths.xml";
        public static IEnumerable<String> GetSavedStartingDirsFromXml()
        {
            XDocument savedData = null;
            try
            {
                savedData = XDocument.Load(saved_path);
            }
            catch (FileNotFoundException ex)
            {
                savedData = new XDocument();
                XElement root = new XElement("Paths");
                XElement leftDirPath = new XElement("left");
                leftDirPath.Value = Directory.GetCurrentDirectory();
                XElement rightDirPath = new XElement("right");
                rightDirPath.Value = Directory.GetCurrentDirectory();
                root.Add(leftDirPath, rightDirPath);
                savedData.Add(root);
                savedData.Save(saved_path);
                return new List<String> { Directory.GetCurrentDirectory(), Directory.GetCurrentDirectory() };
            }
            try
            {
                XElement root = savedData.Root;
                String leftPath = root.Element("left").Value;
                String rightPath = root.Element("right").Value;
                return new List<String> { leftPath, rightPath };
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }

        public static void SetSavedStartingDirsToXml(String _path, FileOperator.Side _s)
        {
            if (_path == String.Empty)
                _path = Directory.GetCurrentDirectory();
            try
            {
                XDocument savedData = XDocument.Load(saved_path);
                XElement root = savedData.Root;
                String side = (_s == FileOperator.Side.Left) ? "left" : "right";
                root.Element(side).Value = _path;
                savedData.Add(root);
                savedData.Save(saved_path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
