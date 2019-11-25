﻿using System;
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

        public static String[] sortTypes = { "Name asc.", "Name desc.", "Size asc.", "Size desc.", "Date asc.", "Date desc.", "Folders first", "Files first" };
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

        public static void CopyFiles(IEnumerable<FileObjects.IFileObject> _files, File_Containers.FileContainer _dir)
        {
            FileObjects.Dir receiver = _dir.StoredDirectory;
            try
            {
                foreach (FileObjects.IFileObject obj in _files)
                {
                    obj.Copy(receiver);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MoveFiles(IEnumerable<FileObjects.IFileObject> _files, File_Containers.FileContainer _dir)
        {
            FileObjects.Dir receiver = _dir.StoredDirectory;
            try
            {
                foreach (FileObjects.IFileObject obj in _files)
                {
                    obj.Move(receiver);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Rename(FileObjects.IFileObject _file, String _newname)
        {
            try
            {
                _file.Rename(_newname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(IEnumerable<FileObjects.IFileObject> _files)
        {
            try
            {
                foreach (FileObjects.IFileObject fobj in _files)
                {
                    //fobj.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<FileObjects.IFileObject> SortList (IEnumerable<FileObjects.IFileObject> _mas, Int32 _index)
        {
            switch (_index)
            {
                case 0:
                    _mas.ToList().Sort((first, second) => String.Compare(first.Info.ShortName,second.Info.ShortName));
                    break;
                case 1:
                    _mas.ToList().Sort((first, second) => String.Compare(second.Info.ShortName, first.Info.ShortName));
                    break;
                case 2:
                    _mas.ToList().Sort((first, second) => String.Compare(first.Info.Size, second.Info.Size));
                    break;
                case 3:
                    _mas.ToList().Sort((first, second) => String.Compare(second.Info.Size, first.Info.Size));
                    break;
                case 4:
                    _mas.ToList().Sort((first, second) => DateTime.Compare(first.Info.CreateTime, second.Info.CreateTime));
                    break;
                case 5:
                    _mas.ToList().Sort((first, second) => DateTime.Compare(second.Info.CreateTime, first.Info.CreateTime));
                    break;
                case 6:
                    _mas.ToList().Sort((first, second) => Convert.ToInt32(first.Info.IsDirectory));
                    break;
                case 7:
                    _mas.ToList().Sort((first, second) => Convert.ToInt32(!first.Info.IsDirectory));
                    break;
                default:
                    throw new Exception("Out of range mas");
            }
            return _mas;
        }

    }
}
