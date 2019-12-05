using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFile_Manager.FileObjects.FileDirInfo;

namespace VFile_Manager.FileObjects
{
    public interface IFileObject
    {
        IFileDirInfo Info { get; }
        bool Exists();
        void Delete();
        void Move(Dir _path);
        void Copy(Dir _path);
        void Rename(String _newName);
        
    }
}
