using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
//using System.Windows.Forms;

namespace SimpleNotebook
{
    public class FileObjectCollection : IEnumerable<FileObject>, INotifyPropertyChanged
    {
        SettingsFile _settings;
        
        //CONSTRUCTORS
        //public FileObjectCollection() { }

        public FileObjectCollection(string settingsFolder)
        {
            _settings = new SettingsFile(settingsFolder + @"\settings.txt");
            CollectionPath = _settings.StartupPath;
            ReLoadFiles();

        }

        //PRIVATE VARIABLES
        private List<FileObject> _fileObjectCollection = new();

        //PROPERTIES
        public List<FileObject> FileObjects
        {
            get => _fileObjectCollection;
            set
            {
                _fileObjectCollection = value;
                NotifyPropertyChanged(nameof(FileObjects));
            }
        }
        public string CollectionPath { get; private set; }

        //INTERFACE METHODS
        public IEnumerator<FileObject> GetEnumerator()
        {
            return FileObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //METHODS

        /// <summary>
        /// Put the collection in order of the SortingOrder property. 
        /// </summary>
        private void OrderObjects()
        {
            IEnumerable<FileObject> query = FileObjects.OrderBy(_fileObject => _fileObject.SortingIndex).Reverse();
            FileObjects = query.ToList();
            
        }

        private void SetObjectSortingIndexByFileName()
        {
            foreach (FileObject item in FileObjects)
            {
                item.SetSortingIndex();
            }
            
        }

        private bool ValidFile(string path)
        {
            if (new FileInfo(path).Length > _settings.MaxFileObjectSize)
            {
                return false; //50kb limit on file size (can be changed if this value is not suitable)
            }
            //else if (System.IO.Path.GetExtension(path) == @".exe") return false;
            else if (_settings.IncludedFiles != null && !_settings.IncludedFiles.Contains(Path.GetExtension(path)))
            {
                return false;
            }
            //else if (_settings.IgnoredFileExtensions != null && _settings.IgnoredFileExtensions.Contains(Path.GetExtension(path))) return false;
            return true;
        }

        public List<FileObject> GetUnsavedFiles()
        {
            List<FileObject> rtnList = new();
            foreach (FileObject file in FileObjects)
            {
                if (file.UnsavedChanges) rtnList.Add(file);
            }
            return rtnList;
        }

        public void CreateNewFile(string fileName)
        {
            FileObjects.Add(new FileObject(CollectionPath + "\\" + fileName, true));
            //NotifyPropertyChanged(nameof(FileObjects));
        }

        public void ReLoadFiles()
        {
            FileObjects.Clear();

            foreach (string file in Directory.GetFiles(CollectionPath))
            {
                try
                {
                    if (ValidFile(file))
                    {
                        FileObjects.Add(new FileObject(file));
                    }
                }
                catch { }
            }

            SetObjectSortingIndexByFileName();
            OrderObjects();
        }
    }
}
