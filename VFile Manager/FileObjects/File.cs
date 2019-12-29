using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using VFile_Manager.FileObjects.FileDirInfo;

namespace VFile_Manager.FileObjects
{
    public class File : IFileObject
    {
        private FileInfo CurrentFileInfo;
        public IFileDirInfo Info { get; private set; }
        public File(FileInfo _finfo)
        {
            if (_finfo.Exists)
            {
                CurrentFileInfo = _finfo;
                Info = new FilesInfo(CurrentFileInfo);
            }
            else throw new Exception("File not found");
        }
        [DllImport("shell32.dll")]
        private static extern int FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);
        public bool Exists()
        {
            return CurrentFileInfo.Exists;
        }
        public bool Open()
        {
            String appName;
            StringBuilder sb = new StringBuilder();
            FileAssociation fass = SavedDataReader
                .GetCustomFileAssociation(this.Info.Extension.Trim('.')).ToList().Find((item) => item.Extension == this.Info.Extension.Trim('.'));
            if (fass == null) {
                if (FindExecutable(Info.FullName, null, sb) < 32)
                    return false;
                else appName = sb.ToString();
            }
            else if (!fass.IsOverrideSystem)
            {
                if (FindExecutable(Info.FullName, null, sb) > 32)
                    appName = sb.ToString();
            }
            Process openProc = new Process();
            openProc.StartInfo.FileName = fass.Filename;
            openProc.StartInfo.Arguments = $"\"{Info.FullName}\"";
            try
            {
                return openProc.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete()
        {
            if (CurrentFileInfo.Exists)
            {
                CurrentFileInfo.Delete();
            }
        }

        public override bool Equals(object _other)
        {
            if ((object)this == _other)
                return true;
            File _otherDir = _other as File;
            if (_otherDir == null)
                return false;
            return _otherDir.CurrentFileInfo.CreationTime == this.CurrentFileInfo.CreationTime &&
                _otherDir.CurrentFileInfo.FullName == this.CurrentFileInfo.FullName && _otherDir.CurrentFileInfo.Length == this.CurrentFileInfo.Length;
        }

        public override int GetHashCode()
        {
            return this.CurrentFileInfo.GetHashCode();
        }

        public async Task Move(Dir _path)
        {
            if (CurrentFileInfo.Exists && _path.Exists())
            {
                await Task.Run(() => CurrentFileInfo.MoveTo(Path.Combine(_path.Info.FullName, CurrentFileInfo.Name)));
            }
            else throw new Exception("File or directory not found");
        }

        public async Task Copy(Dir _path)
        {
            if (_path.IsExistsHere(CurrentFileInfo.Name))
                throw new Exception($"File {CurrentFileInfo.Name} is already exists in {_path}");
            if (CurrentFileInfo.Exists && _path.Exists())
            {
                await Task.Run(() => CurrentFileInfo.CopyTo(Path.Combine(_path.Info.FullName, CurrentFileInfo.Name)));
            }
            else throw new Exception("File or directory not found");

        }

        public void Rename(String _newName)
        {
            if (CurrentFileInfo.Exists)
            {
                CurrentFileInfo.MoveTo(Path.Combine(CurrentFileInfo.DirectoryName, _newName));
            }
        }
    }
}
