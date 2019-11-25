using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFile_Manager.FileObjects
{
    interface IFileObject
    {
        FileDirInfo Info { get; }
        bool Exists();
        void Open();
        void Delete();
        void Move(Dir _path);
        void Copy(Dir _path);
        void Rename(String _newName);
    }
}
