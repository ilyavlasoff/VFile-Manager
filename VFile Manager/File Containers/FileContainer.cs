using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VFile_Manager.File_Containers
{
    class FileContainer
    {
        public FileObjects.Dir StoredDirectory { get; private set; }

        public List<FileObjects.IFileObject> DirContent { get; private set; }

        public FileContainer(String _path)
        {
            if (Directory.Exists(_path))
            {
                StoredDirectory = new FileObjects.Dir(_path);
            }
        }

        public FileContainer()
        {
            DirContent = new List<FileObjects.IFileObject>();
        }

        public void GoToDirectory(String _directoryName)
        {
            FileObjects.Dir dinfo = new FileObjects.Dir(_directoryName);
            GoToDirectory(dinfo);
        }

        public void GoToDirectory(FileObjects.Dir _directory)
        {
            if (_directory.Exists())
            {
                StoredDirectory = _directory;
                DirContent.Clear();
                try
                {
                    IEnumerable<FileObjects.File> files = StoredDirectory.GetFiles();
                    DirContent.AddRange(files);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    IEnumerable<FileObjects.Dir> dirs = StoredDirectory.GetDirectories();
                    DirContent.AddRange(dirs);
                }
                catch (Exception ex)
                {

                }

            }
        }

    }
}
