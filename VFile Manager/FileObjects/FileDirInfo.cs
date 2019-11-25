using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VFile_Manager.FileObjects
{
    class FileDirInfo
    {
        public String ShortName { get; private set; }
        public String FullName { get; private set; }
        public String Parent { get; private set; }
        public String LogicalDrive { get; private set; }
        public String Size { get; private set; }
        public String Extension { get; private set; }
        public String NumberOfFilesInside { get; private set; }
        public String NumberOfDirsInside { get; private set; }
        public bool IsReadonly { get; private set; }
        public bool IsArchive { get; private set; }
        public bool IsSystemFile { get; private set; }
        public bool IsHidden { get; private set; }
        public bool IsDirectory { get; private set; }
        public bool IsTemp { get; private set; }
        public bool IsCompressed { get; private set; }
        public bool IsEncrypted { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime LastWriteTime { get; private set; }
        public DateTime LastUsedTime { get; private set; }
        public String IconPath { get; private set; }
        public FileDirInfo(object _info)
        {
            if (_info is DirectoryInfo)
            {
               
                DirectoryInfo source = _info as DirectoryInfo;
                if (!source.Exists)
                    throw new Exception("Unable to find directory");
                ShortName = source.Name;
                FullName = source.FullName;
                try
                {
                    Parent = source.Parent.FullName;
                }
                catch (NullReferenceException)
                {
                    Parent = "No";
                }
                LogicalDrive = source.Root.FullName;
                Size = "?";
                Extension = source.Extension;
                try
                {
                    NumberOfDirsInside = source.GetDirectories().Count().ToString();
                }
                catch (UnauthorizedAccessException)
                {
                    NumberOfDirsInside = "?";
                }
                try
                {
                    NumberOfFilesInside = source.GetFiles().Count().ToString();
                }
                catch (UnauthorizedAccessException)
                {
                    NumberOfFilesInside = "?";
                }
                FileAttributes attr = source.Attributes;
                IsReadonly = (attr & FileAttributes.Hidden) != 0;
                IsArchive = (attr & FileAttributes.Archive) != 0;
                IsSystemFile = (attr & FileAttributes.System) != 0;
                IsHidden = (attr & FileAttributes.Hidden) != 0;
                IsDirectory = (attr & FileAttributes.Directory) != 0;
                IsCompressed = (attr & FileAttributes.Compressed) != 0;
                IsTemp = (attr & FileAttributes.Temporary) != 0;
                IsEncrypted = (attr & FileAttributes.Encrypted) != 0;
                CreateTime = source.CreationTime;
                LastUsedTime = source.LastAccessTime;
                LastWriteTime = source.LastWriteTime;
                IconPath = "/Icons/dir.png";
            }
            else if (_info is FileInfo)
            {
                FileInfo source = _info as FileInfo;
                if (!source.Exists)
                    throw new Exception("File not exists");
                ShortName = source.Name;
                FullName = source.FullName;
                Parent = source.DirectoryName;
                LogicalDrive = source.Directory.Root.FullName;
                Size = source.Length.ToString();
                Extension = source.Extension;
                NumberOfDirsInside = String.Empty;
                NumberOfFilesInside = String.Empty;
                FileAttributes attr = source.Attributes;
                IsReadonly = (attr & FileAttributes.Hidden) != 0;
                IsArchive = (attr & FileAttributes.Archive) != 0;
                IsSystemFile = (attr & FileAttributes.System) != 0;
                IsHidden = (attr & FileAttributes.Hidden) != 0;
                IsDirectory = (attr & FileAttributes.Directory) != 0;
                IsCompressed = (attr & FileAttributes.Compressed) != 0;
                IsTemp = (attr & FileAttributes.Temporary) != 0;
                IsEncrypted = (attr & FileAttributes.Encrypted) != 0;
                CreateTime = source.CreationTime;
                LastUsedTime = source.LastAccessTime;
                LastWriteTime = source.LastWriteTime;
                IconPath = "/Icons/file.png";
            }
        }
    }
}
