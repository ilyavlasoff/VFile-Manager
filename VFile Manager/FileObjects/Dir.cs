using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VFile_Manager.FileObjects
{
    class Dir : IFileObject
    {
        private const Int32 MaxDirLen = 255;

        private DirectoryInfo CurrentDirInfo;
        public Dir(DirectoryInfo _dinfo)
        {
            if (_dinfo.Exists)
            {
                CurrentDirInfo = _dinfo;
            }
            else throw new Exception("Directory not found");
        }
        public Dir(String _dpath)
        {
            if (Directory.Exists(_dpath))
            {
                CurrentDirInfo = new DirectoryInfo(_dpath);
            }
            else throw new Exception("Directory not found");
        }
        public Dir GetParentDirectory()
        {
            try
            {
                DirectoryInfo parentDir = CurrentDirInfo.Parent;
                return new Dir(parentDir);
            }
            catch
            {
                return null;
            }
        }
        public FileDirInfo Info
        {
            get
            {
                return new FileDirInfo(CurrentDirInfo);
            }
        }
        public IEnumerable<File> GetFiles(String _filter = "*")
        {
            return CurrentDirInfo.GetFiles(_filter).Select((item) => new File(item));
        }
        public IEnumerable<Dir> GetDirectories(String _filter = "*")
        {
            return CurrentDirInfo.GetDirectories(_filter).Select((item) => new Dir(item));
        }
        public bool Exists()
        {
            return CurrentDirInfo.Exists;
        }
        
        public void Open()
        {

        }

        public void Copy(String _path)
        {
            if (CurrentDirInfo.Exists && Directory.Exists(_path))
            {

            }
        }

        public void Move(String _path)
        {
            if (CurrentDirInfo.Exists && Directory.Exists(_path))
            {
                CurrentDirInfo.MoveTo(_path);
            }
        }

        public void Rename(String _name)
        {
            if (_name.Length < MaxDirLen)
            {
                Move(CurrentDirInfo.Parent + @"\" + _name);
            }
        }

        public void Delete()
        {
            if (CurrentDirInfo.Exists)
            {
                CurrentDirInfo.Delete();
            }
        }
    }
}
