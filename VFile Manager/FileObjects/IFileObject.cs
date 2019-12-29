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
        Task Move(Dir _path);
        Task Copy(Dir _path);
        void Rename(String _newName);
        
    }
}
