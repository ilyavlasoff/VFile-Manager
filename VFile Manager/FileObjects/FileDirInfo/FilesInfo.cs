using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VFile_Manager.FileObjects.FileDirInfo
{
    public class FilesInfo: IFileDirInfo
    {
        FileInfo FileProperties;
        public FilesInfo(FileInfo _source)
        {
            if (_source.Exists)
            {
                this.FileProperties = _source;
            }
        }
        
        public String ShortName
        {
            get
            {
                return FileProperties.Name;
            }
        }
        public String FullName
        {
            get
            {
                return FileProperties.FullName;
            }
        }
        public String Parent
        {
            get
            {
                try
                {
                    return FileProperties.Directory.FullName;
                }
                catch (NullReferenceException)
                {
                    return "No";
                }
            }
        }
        public String LogicalDrive
        {
            get
            {
                return FileProperties.Directory.Root.FullName;
            }
        }
        public String Size
        {
            get
            {
                return FileProperties.Length.ToString();
            }
        }
        public String Extension
        {
            get
            {
                return FileProperties.Extension;
            }
        }
        public String NumberOfFilesInside
        {
            get
            {
                return "-1";
            }
        }
        public String NumberOfDirsInside
        {
            get
            {
                return "-1";
            }
        }
        public bool IsReadonly
        {
            get
            {
                return (FileProperties.Attributes & FileAttributes.Hidden) != 0;
            }
        }
        public bool IsArchive
        {
            get
            {

                return (FileProperties.Attributes & FileAttributes.Archive) != 0;

            }
        }
        public bool IsSystemFile
        {
            get
            {
                return (FileProperties.Attributes & FileAttributes.System) != 0;
            }
        }
        public bool IsHidden
        {
            get
            {
                return (FileProperties.Attributes & FileAttributes.Hidden) != 0;
            }
        }
        public bool IsDirectory
        {
            get
            {
                return (FileProperties.Attributes & FileAttributes.Directory) != 0;
            }
        }
        public bool IsTemp
        {
            get
            {
                return (FileProperties.Attributes & FileAttributes.Temporary) != 0;
            }
        }
        public bool IsCompressed
        {
            get
            {
                return (FileProperties.Attributes & FileAttributes.Compressed) != 0;
            }
        }
        public bool IsEncrypted
        {
            get
            {
                return (FileProperties.Attributes & FileAttributes.Encrypted) != 0;
            }
        }
        public DateTime CreateTime
        {
            get
            {
                return FileProperties.CreationTime;
            }
        }
        public DateTime LastWriteTime
        {
            get
            {
                return FileProperties.LastWriteTime;
            }
        }
        public DateTime LastUsedTime
        {
            get
            {
                return FileProperties.LastAccessTime;
            }
        }
        public String IconPath
        {
            get
            {
                return "/Icons/file.png";
            }
        }
        public String Time
        {
            get
            {
                return CreateTime.ToString("dd.MM.yy hh:mm");
            }
        }
        public String Extensions
        {
            get
            {
                String resStr = String.Empty;
                resStr += IsReadonly ? "r" : "-";
                resStr += IsArchive ? "a" : "-";
                resStr += IsSystemFile ? "s" : "-";
                resStr += IsHidden ? "h" : "-";
                resStr += IsDirectory ? "d" : "-";
                resStr += IsTemp ? "t" : "-";
                resStr += IsCompressed ? "c" : "-";
                resStr += IsEncrypted ? "e" : "-";
                return resStr;
            }
        }

    }

}
