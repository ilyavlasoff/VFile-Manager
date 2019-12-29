using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using VFile_Manager.FileObjects.FileDirInfo;

namespace VFile_Manager.FileObjects
{
    public class Dir : IFileObject
    {
        private const Int32 MaxDirLen = 255;

        private DirectoryInfo CurrentDirInfo;
        public IFileDirInfo Info { get; private set; }
        public Dir(DirectoryInfo _dinfo)
        {
            if (_dinfo.Exists)
            {
                CurrentDirInfo = _dinfo;
                Info = new DirInfo(CurrentDirInfo);
            }
            else throw new Exception("Directory not found");
        }
        public Dir(String _dpath)
        {
            if (Directory.Exists(_dpath))
            {
                CurrentDirInfo = new DirectoryInfo(_dpath);
                Info = new DirInfo(CurrentDirInfo);
            }
            else throw new Exception("Directory not found");
        }
        public Dir GetParentDirectory()
        {
            try
            {
                DirectoryInfo parentDir = CurrentDirInfo.Parent;
                return new Dir(parentDir);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<File> GetFiles(String _filter = "*")
        {
            return CurrentDirInfo.GetFiles(_filter).Select((item) => new File(item));
        }
        public IEnumerable<Dir> GetDirectories(String _filter = "*")
        {
            return CurrentDirInfo.GetDirectories(_filter).Select((item) => new Dir(item));
        }
        public IEnumerable<IFileObject> GetContent(String _filter = "*")
        {
            List<IFileObject> resobj = new List<IFileObject>();
            resobj.AddRange(this.GetFiles(_filter));
            resobj.AddRange(this.GetDirectories(_filter));
            return resobj;
        }
        public bool Exists()
        {
            return CurrentDirInfo.Exists;
        }

        public override bool Equals (object _other)
        {
            if ((object)this == _other)
                return true;
            Dir _otherDir = _other as Dir;
            if (_otherDir == null)
                return false;
            return _otherDir.CurrentDirInfo.CreationTime == this.CurrentDirInfo.CreationTime && _otherDir.CurrentDirInfo.FullName == this.CurrentDirInfo.FullName;
        }

        public override int GetHashCode()
        {
            return this.CurrentDirInfo.GetHashCode();
        }

        public enum FindMode { Size, Name, Date }

        public IEnumerable<File> Find (FindMode _findmode, List<String> _criteria)
        {
            DirectoryInfo[] containDirs = null;
            try
            {
                containDirs = CurrentDirInfo.GetDirectories();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            List <File> findResInInnderDirs = new List<File>();
            foreach (DirectoryInfo dir in containDirs)
            {
                Dir innerDir = new Dir(dir);
                findResInInnderDirs.AddRange(innerDir.Find(_findmode, _criteria));
            }
            List<FileInfo> containFiles = new List<FileInfo>(CurrentDirInfo.GetFiles());
            Predicate<FileInfo> pred = null;
            if (_findmode == FindMode.Name)
            {
                if (_criteria.Count() != 1)
                    throw new Exception("Wrong arguments count");
                pred = delegate (FileInfo fi) {
                    Regex re = new Regex("\\w*" + _criteria[0] + "\\w*");
                    return re.IsMatch(fi.Name);
                };
            }
            else if (_findmode == FindMode.Size)
            {
                if (_criteria.Count() != 2)
                    throw new Exception("Wrong arguments count");
                pred = delegate (FileInfo fi) {
                    double low, high, fileSize = fi.Length;
                    if (!double.TryParse(_criteria.ToList()[0], out low))
                    {
                        throw new Exception("Bad arguments");
                    }
                    if (!double.TryParse(_criteria.ToList()[1], out high))
                    {
                        throw new Exception("Bad arguments");
                    }
                    return fileSize > low && fileSize < high;
                };
            }
            else if (_findmode == FindMode.Date)
            {
                if (_criteria.Count() != 2)
                    throw new Exception("Wrong arguments count");
                pred = delegate (FileInfo fi)
                {
                    DateTime dt_low = Convert.ToDateTime(_criteria.ToList()[0]);
                    DateTime dt_high = Convert.ToDateTime(_criteria.ToList()[1]);
                    DateTime cr_dt = fi.CreationTime;
                    return cr_dt < dt_high && cr_dt > dt_low;
                };
            }
            else
            {
                throw new Exception("Undefined operation");
            }
            List<FileInfo> finfo = containFiles.FindAll(pred);
            foreach (FileInfo fi in finfo)
            {
                findResInInnderDirs.Add(new File(fi));
            }
            return findResInInnderDirs;
        }

        public bool IsExistsHere(String _filename)
        {
            return this.CurrentDirInfo.GetDirectories().Any((item) => item.Name == _filename) || this.CurrentDirInfo.GetFiles().Any((item) => item.Name == _filename);
        }

        public async Task Copy(Dir _path)
        {
            if (CurrentDirInfo.Exists && _path.Exists()) //&& !_path.Info.FullName.Contains(this.Info.FullName) && !_path.IsExistsHere(this.Info.ShortName))
            {
                try
                {
                    Dir tempdir = FileOperator.MkDirFile(Path.Combine(_path.Info.FullName, this.Info.ShortName), true) as Dir;
                    foreach (File fobj in this.GetFiles())
                    {
                        await Task.Run(() => fobj.Copy(tempdir));
                    }
                    foreach (Dir dobj in this.GetDirectories())
                    {
                        await Task.Run(() => dobj.Copy(tempdir));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Copy directory is unavailable: {ex.Message}");
                }
            }
            else throw new Exception($"Can't copy in directory {_path.Info.ShortName}");
        }

        public  async Task Move(Dir _path)
        {
            if (CurrentDirInfo.Exists && _path.Exists())
            {
                try
                {
                    await Task.Run(() => CurrentDirInfo.MoveTo(Path.Combine(_path.Info.FullName, this.Info.ShortName)));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Can't move {_path.Info.ShortName}");
                }
            }
            else throw new Exception($"Can't move in directory {_path.Info.ShortName}");
        }

        public void Rename(String _name)
        {
            if (_name == this.Info.ShortName)
            {
                throw new Exception($"Old file name can not be same as old name: {_name}");
            }
            if (!(new Dir(this.Info.Parent)).IsExistsHere(_name) && _name.Length < MaxDirLen)
            {
                CurrentDirInfo.MoveTo(Path.Combine(CurrentDirInfo.Parent.FullName, _name));
            }
            else throw new Exception($"Wrong name: {_name}");
        }

        public void Delete()
        {
            if (CurrentDirInfo.Exists)
            {
                foreach (File file in this.GetFiles())
                    file.Delete();
                foreach (Dir dir in this.GetDirectories())
                    dir.Delete();
                CurrentDirInfo.Delete();
            }
        }
    }
}
