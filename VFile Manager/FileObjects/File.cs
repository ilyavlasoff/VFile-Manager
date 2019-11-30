using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using VFile_Manager.FileObjects.FileDirInfo;

namespace VFile_Manager.FileObjects
{
    public class File : IFileObject
    {
        private FileInfo CurrentFileInfo;
        public IFileDirInfo Info { get; private set; }
        public File(FileInfo _finfo)
        {
            if (_finfo.Exists)
            {
                CurrentFileInfo = _finfo;
                Info = new FilesInfo(CurrentFileInfo);
            }
            else throw new Exception("File not found");
        }
        public bool Exists()
        {
            return CurrentFileInfo.Exists;
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
        public async void Move(Dir _path)
        {
            if (CurrentFileInfo.Exists && _path.Exists())
            {
                await Task.Run(() => CurrentFileInfo.MoveTo(_path.Info.FullName + "\\" + CurrentFileInfo.Name));
            }
            else throw new Exception("File or dir not found");
        }

        public async void Copy(Dir _path)
        {
            if (CurrentFileInfo.Exists && _path.Exists())
            {
                await Task.Run(() => CurrentFileInfo.CopyTo(_path.Info.FullName + "\\" + CurrentFileInfo.Name));
            }
            else throw new Exception("File or dir not found");
        }

        public void Rename(String _newName)
        {
            if (CurrentFileInfo.Exists)
            {
                CurrentFileInfo.MoveTo(CurrentFileInfo.DirectoryName + "\\" + _newName);
            }
        }
    }
}
