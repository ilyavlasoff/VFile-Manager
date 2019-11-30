using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFile_Manager.FileObjects.FileDirInfo
{
    public interface IFileDirInfo
    {
        String ShortName { get; }
        public String FullName { get; }
        public String Parent { get; }
        public String LogicalDrive { get; }
        public String Size { get; }
        public String Extension { get; }
        public String NumberOfFilesInside { get; }
        public String NumberOfDirsInside { get; }
        public bool IsReadonly { get; }
        public bool IsArchive { get; }
        public bool IsSystemFile { get; }
        public bool IsHidden { get; }
        public bool IsDirectory { get; }
        public bool IsTemp { get; }
        public bool IsCompressed { get; }
        public bool IsEncrypted { get; }
        public DateTime CreateTime { get; }
        public DateTime LastWriteTime { get; }
        public DateTime LastUsedTime { get; }
        public String IconPath { get; }
        public String Time { get; }
        public String Extensions { get; }
    }
}
