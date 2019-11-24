using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VFile_Manager
{
    static class FileOperator
    {
        public enum Side { Left, Right }
        public static Side ActiveDirectory { get; set; }
        public static bool DirFirst { get; set; }

        public static void InitilalDirs()
        {
            List<String> fileStartPaths = SavedDataReader.GetSavedStartingDirsFromXml().ToList();
            String leftPath = fileStartPaths[0];
            if (!Directory.Exists(leftPath))
            {
                leftPath = Directory.GetCurrentDirectory();
                SavedDataReader.SetSavedStartingDirsToXml(leftPath, Side.Left);
            }
            String rightPath = fileStartPaths[1];
            if (!Directory.Exists(rightPath))
            {
                rightPath = Directory.GetCurrentDirectory();
                SavedDataReader.SetSavedStartingDirsToXml(rightPath, Side.Right);
            }
            File_Containers.FileDualContainer.ChooseContainer(Side.Left).GoToDirectory(leftPath);
            File_Containers.FileDualContainer.ChooseContainer(Side.Right).GoToDirectory(rightPath);
        }

        public static IEnumerable<FileObjects.IFileObject> GetDirContainsList(Side _s)
        {
            return File_Containers.FileDualContainer.ChooseContainer(_s).DirContent;
        }

        public static IEnumerable<String> GetAllLogicalDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            IEnumerable<String> allDrivesName = allDrives.Select((item) => item.Name);
            return allDrivesName;
        }

        public static void HandleOpenFileOrDir(object _file, Side _s)
        {
            if (_file is FileObjects.Dir)
            {
                FileObjects.Dir choosedDir = _file as FileObjects.Dir;
                File_Containers.FileDualContainer.ChooseContainer(_s).GoToDirectory(choosedDir);
            }
            else if (_file is FileObjects.File)
            {
                (_file as FileObjects.File).Open();
            }
            else
            {
                throw new Exception("Unable to convert object to dir or file type");
            }
        }

        public static void NavigateToPreviousDirectory(Side _s)
        {
            try
            {
                FileObjects.Dir parentDir = File_Containers.FileDualContainer.ChooseContainer(_s).StoredDirectory.GetParentDirectory();
                if (parentDir != null)
                {
                    File_Containers.FileDualContainer.ChooseContainer(_s).GoToDirectory(parentDir);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static FileObjects.IFileObject MkDirFile (String _filepath, bool _isDir)
        {
            try
            {
                if (_isDir)
                {
                    DirectoryInfo dirinfo = Directory.CreateDirectory(_filepath);
                    return new FileObjects.Dir(dirinfo);
                }
                else
                {
                    File.Create(_filepath);
                    return new FileObjects.File(new FileInfo(_filepath));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
