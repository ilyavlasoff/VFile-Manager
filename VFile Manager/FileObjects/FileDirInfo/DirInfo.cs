using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VFile_Manager.FileObjects.FileDirInfo
{
    public class DirInfo : IFileDirInfo
    {
        DirectoryInfo DirProperties;

        public DirInfo (DirectoryInfo _source)
        {
            if (_source.Exists)
            {
                this.DirProperties = _source;
            }
        }
        public String ShortName {
            get
            {
                return DirProperties.Name;
            }
        }
        public String FullName
        {
            get
            {
                return DirProperties.FullName;
            }
        }
        public String Parent
        {
            get
            {
                try
                {
                    return DirProperties.Parent.FullName;
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
                return DirProperties.Root.FullName;
            }
        }
        public String Size
        {
            get
            {
                return "-1";
            }
        }
        public String Extension
        {
            get
            {
                return DirProperties.Extension;
            }
        }
        public String NumberOfFilesInside
        {
            get
            {
                try
                {
                    return DirProperties.GetDirectories().Count().ToString();
                }
                catch (UnauthorizedAccessException)
                {
                    return "-1";
                }
            }
        }
        public String NumberOfDirsInside
        {
            get
            {
                try
                {
                    return DirProperties.GetFiles().Count().ToString();
                }
                catch (UnauthorizedAccessException)
                {
                    return "-1";
                }
            }
        }
        public bool IsReadonly
        {
            get
            {
                return (DirProperties.Attributes & FileAttributes.Hidden) != 0;
            }
        }
        public bool IsArchive
        {
            get
            {
                
                return (DirProperties.Attributes & FileAttributes.Archive) != 0;

            }
        }
        public bool IsSystemFile
        {
            get
            {
                return (DirProperties.Attributes & FileAttributes.System) != 0;
            }
        }
        public bool IsHidden
        {
            get
            {
                return (DirProperties.Attributes & FileAttributes.Hidden) != 0;
            }
        }
        public bool IsDirectory
        {
            get
            {
                return (DirProperties.Attributes & FileAttributes.Directory) != 0;
            }
        }
        public bool IsTemp
        {
            get
            {
                return (DirProperties.Attributes & FileAttributes.Temporary) != 0;
            }
        }
        public bool IsCompressed
        {
            get
            {
                return (DirProperties.Attributes & FileAttributes.Compressed) != 0;
            }
        }
        public bool IsEncrypted
        {
            get
            {
                return (DirProperties.Attributes & FileAttributes.Encrypted) != 0;
            }
        }
        public DateTime CreateTime
        {
            get
            {
                return DirProperties.CreationTime;
            }
        }
        public DateTime LastWriteTime
        {
            get
            {
                return DirProperties.LastWriteTime;
            }
        }
        public DateTime LastUsedTime
        {
            get
            {
                return DirProperties.LastAccessTime;
            }
        }
        public String IconPath
        {
            get
            {
                return "/Icons/dir.png";
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
