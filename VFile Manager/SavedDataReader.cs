using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace VFile_Manager
{
    public class FileAssociation
    {
        public String Extension { get; set; }
        public String Filename { get; set; }
        public Boolean IsOverrideSystem { get; set; }
        public FileAssociation(String _ext, String _filename, String _is_override)
        {
            Extension = _ext;
            Filename = _filename;
            IsOverrideSystem = _is_override == "0" ? false : true;
        }
    }
    static class SavedDataReader
    {
        private static String SavedPath = "settings.xml";
        private static List<FileAssociation> UserDefinedAssociations = new List<FileAssociation>();

        public enum OpenMode
        {
            lastUsed = 0,
            current = 1,
            userDefined = 2
        }
        
        private static XDocument LoadSettingsDocument()
        {
            XDocument savedData = null;
            try
            {
                savedData = XDocument.Load(SavedPath);
            }
            catch (FileNotFoundException ex)
            {
                CreateNewSettingsFile(savedData);
                savedData = XDocument.Load(SavedPath);
            }
            return savedData;
        }

        public static OpenMode Mode
        {
            get
            {
                XDocument savedData = null;
                try
                {
                    savedData = XDocument.Load(SavedPath);
                }
                catch (FileNotFoundException ex)
                {
                    CreateNewSettingsFile(savedData);
                }
                return (OpenMode)Int32.Parse(savedData.Root.Element("openDirMode").Value);
            }

            set
            {
                XDocument savedData = null;
                try
                {
                    savedData = XDocument.Load(SavedPath);
                }
                catch (FileNotFoundException ex)
                {
                    CreateNewSettingsFile(savedData);
                }
                savedData.Root.Element("openDirMode").Value = (value.ToString());
            }
        }

        private static void CreateNewSettingsFile(XDocument savedData)
        {
            savedData = new XDocument();
            XElement root = new XElement("settings");

            XElement openDirMode = new XElement("openDirMode");
            openDirMode.Value = "0";

            XElement lastuserpathnames = new XElement("lastUsedDirs");
            XElement leftLastUsedDirPath = new XElement("left");
            leftLastUsedDirPath.Value = Directory.GetCurrentDirectory();
            XElement rightLastUsedDirPath = new XElement("right");
            rightLastUsedDirPath.Value = Directory.GetCurrentDirectory();
            lastuserpathnames.Add(leftLastUsedDirPath, rightLastUsedDirPath);

            XElement userDefinedpathnames = new XElement("userDefinedPaths");
            XElement leftUserDefinedDirPath = new XElement("left");
            leftUserDefinedDirPath.Value = Directory.GetCurrentDirectory();
            XElement rightUserDefinedDirPath = new XElement("right");
            rightUserDefinedDirPath.Value = Directory.GetCurrentDirectory();
            userDefinedpathnames.Add(leftUserDefinedDirPath, rightUserDefinedDirPath);

            XElement customFileAssociations = new XElement("association");
            root.Add(openDirMode, lastuserpathnames, userDefinedpathnames, customFileAssociations);
            savedData.Add(root);
            savedData.Save(SavedPath);
        }

        public static IEnumerable<String> GetSavedStartingDirsFromXml()
        {
            XDocument savedData = LoadSettingsDocument();
            try
            {
                OpenMode mode = (OpenMode)Int32.Parse(savedData.Root.Element("openDirMode").Value);
                if (mode == OpenMode.lastUsed)
                {
                    XElement paths = savedData.Root.Element("lastUsedDirs");
                    String leftPath = paths.Element("left").Value;
                    String rightPath = paths.Element("right").Value;
                    return new List<String> { leftPath, rightPath };
                }
                else if (mode == OpenMode.current)
                {
                    return new List<String> { Directory.GetCurrentDirectory(), Directory.GetCurrentDirectory() };
                }
                else if (mode == OpenMode.userDefined)
                {
                    XElement paths = savedData.Root.Element("userDefinedPaths");
                    String leftPath = paths.Element("left").Value;
                    String rightPath = paths.Element("right").Value;
                    return new List<String> { leftPath, rightPath };
                }
                else
                    throw new Exception("Incorrect mode");
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }

        public static void SetSavedStartingDirsToXml(OpenMode _mode, String _path, FileOperator.Side _s)
        {
            if (_path == String.Empty)
                _path = Directory.GetCurrentDirectory();
            XDocument savedData = LoadSettingsDocument();
            try
            {
                String modeDefiner = null;
                if (_mode == OpenMode.userDefined)
                    modeDefiner = "userDefinedPaths";
                else if (_mode == OpenMode.lastUsed)
                    modeDefiner = "lastUsedDirs";
                else return;
                XElement paths = savedData.Root.Element(modeDefiner);
                String side = (_s == FileOperator.Side.Left) ? "left" : "right";
                paths.Element(side).Value = _path;
                savedData.Save(SavedPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadUserDefinedFileAssociations()
        {
            XDocument savedData = LoadSettingsDocument();
            XElement root = savedData.Root;
            UserDefinedAssociations = root.Element("association").Elements()
                .Select((item) => new FileAssociation(item.Name.ToString(), item.Value, item.Attribute("override_system").Value)).ToList();
        }


        public static void SaveUserDefinedFileAssociations()
        {
            XDocument savedData = LoadSettingsDocument();
            foreach (FileAssociation fass in UserDefinedAssociations)
            {
                XElement savedass = savedData.Root.Element("association");
                if (savedass.Element(fass.Extension) == null)
                {
                    XElement newAssociation = new XElement(fass.Extension, fass.Filename);
                    XAttribute attr = new XAttribute("override_system", (fass.IsOverrideSystem ? "1" : "0"));
                    newAssociation.Add(attr);
                    savedass.Add(newAssociation);
                }
                else if (savedass.Element(fass.Extension).Value != fass.Filename || savedass.Element(fass.Extension)
                    .Attribute("override_system").Value != (fass.IsOverrideSystem ? "1" : "0"))
                {
                    savedass.Element(fass.Extension).Value = fass.Filename;
                    savedass.Element(fass.Extension).Attribute("override_system").Value = (fass.IsOverrideSystem ? "1" : "0");
                }
            }
            savedData.Save(SavedPath);
        }

        public static IEnumerable<FileAssociation> GetCustomFileAssociation(String _extension = null)
        {
            if (_extension != null)
            {
                return new List<FileAssociation> { UserDefinedAssociations.Find((item) => item.Extension == _extension) };
            }
            else return UserDefinedAssociations;
        }

        public static IEnumerable<FileAssociation> SetCustomFileAssociation(String _extension, String _filename, Boolean _is_override_system)
        {
            if (UserDefinedAssociations.FindAll((item) => item.Extension == _extension).Count > 0)
                throw new Exception($"User defined association for {_extension} is already exists. Multi-defined associations are restricted!");
            UserDefinedAssociations.Add(new FileAssociation(_extension, _filename, _is_override_system ? "1" : "0"));
            return UserDefinedAssociations;
        }

        public static IEnumerable<FileAssociation> EditCustomFileAssociation (Int32 _index, String _extension, String _filename, bool? _is_override_system) {
            if (UserDefinedAssociations.Count > _index)
            {
                if (_extension != null)
                    UserDefinedAssociations[_index].Extension = _extension;
                if (_filename != null)
                    UserDefinedAssociations[_index].Filename = _filename;
                if (_is_override_system != null)
                    UserDefinedAssociations[_index].IsOverrideSystem = _is_override_system ?? false;
            }
            return UserDefinedAssociations;
        }

        public static IEnumerable<FileAssociation> DeleteCustomFileAssociation(Int32 _index)
        {
            if (UserDefinedAssociations.Count > _index)
            {
                UserDefinedAssociations.RemoveAt(_index);
            }
            return UserDefinedAssociations;
        }



    }
}
