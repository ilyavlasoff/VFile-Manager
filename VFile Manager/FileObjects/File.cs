using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VFile_Manager.FileObjects
{
    class File : IFileObject
    {
        FileInfo CurrentFileInfo;
        public File(FileInfo _finfo)
        {
            if (_finfo.Exists)
            {
                CurrentFileInfo = _finfo;
            }
            else throw new Exception("File not found");
        }
        public String Name
        {
            get
            {
                return CurrentFileInfo.Name;
            }
        }
        public String Path
        {
            get
            {
                return CurrentFileInfo.FullName;
            }
        }
        public bool Exists
        {
            get
            {
                return CurrentFileInfo.Exists;
            }
        }
        public void Open()
        {

        }
        public void Delete()
        {
            if (CurrentFileInfo.Exists)
            {
                CurrentFileInfo.Delete();
            }
        }
        public void Move(String _newPath)
        {
            if (CurrentFileInfo.Exists && System.IO.Directory.Exists(_newPath))
            {
                CurrentFileInfo.MoveTo(_newPath);
            }
        }
        public void Copy(String _newPath)
        {
            if (CurrentFileInfo.Exists && System.IO.Directory.Exists(_newPath))
            {
                CurrentFileInfo.CopyTo(_newPath);
            }
        }
        public void Rename(String _newName)
        {
            if (CurrentFileInfo.Exists)
            {
                CurrentFileInfo.MoveTo(CurrentFileInfo.DirectoryName + _newName);
            }
        }
    }
}
