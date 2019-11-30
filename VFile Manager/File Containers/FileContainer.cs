using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VFile_Manager.FileObjects;

namespace VFile_Manager.File_Containers
{
    class FileContainer
    {
        public Dir StoredDirectory { get; private set; }

        public List<IFileObject> DirContent
        {
            get
            {
                List<IFileObject> content = new List<FileObjects.IFileObject>();
                try
                {
                    IEnumerable<FileObjects.File> files = StoredDirectory.GetFiles();
                    content.AddRange(files);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    IEnumerable<Dir> dirs = StoredDirectory.GetDirectories();
                    content.AddRange(dirs);
                }
                catch (Exception ex)
                {

                }
                return content;
            }
        }

        public FileContainer(String _path)
        {
            if (Directory.Exists(_path))
            {
                StoredDirectory = new Dir(_path);
            }
        }

        public FileContainer() {}

        public void GoToDirectory(String _directoryName)
        {
            FileObjects.Dir dinfo = new Dir(_directoryName);
            GoToDirectory(dinfo);
        }

        public void GoToDirectory(Dir _directory)
        {
            if (_directory.Exists())
            {
                StoredDirectory = _directory;
            }
        }

    }
}
