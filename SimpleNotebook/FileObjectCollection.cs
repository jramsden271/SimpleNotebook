using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace SimpleNotebook
{
    public class FileObjectCollection : IEnumerable<FileObject>
    {
        SettingsFile _settings;
        
        //CONSTRUCTORS
        //public FileObjectCollection() { }

        public FileObjectCollection(string settingsFolder)
        {
            _settings = new SettingsFile(settingsFolder + @"\settings.txt");
            CollectionPath = _settings.StartupPath;
            foreach (string file in Directory.GetFiles(CollectionPath))
            {
                if (ValidFile(file))
                {
                    FileObjects.Add(new FileObject(file));
                }
            }
            SetObjectSortingIndexByFileName();
            OrderObjects();
        }

        //PRIVATE VARIABLES
        private List<FileObject> _fileObjectCollection = new List<FileObject>();

        //PROPERTIES
        public List<FileObject> FileObjects
        {
            get => _fileObjectCollection;
            set
            {
                _fileObjectCollection = value;
            }
        }
        public string CollectionPath { get; private set; }

        //INTERFACE METHODS
        public IEnumerator<FileObject> GetEnumerator()
        {
            return _fileObjectCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //METHODS

        /// <summary>
        /// Put the collection in order of the SortingOrder property. 
        /// </summary>
        private void OrderObjects()
        {
            var query = _fileObjectCollection.OrderBy(_fileObject => _fileObject.SortingIndex).Reverse();
            _fileObjectCollection = query.ToList();
            
        }

        private void SetObjectSortingIndexByFileName()
        {
            foreach (var item in _fileObjectCollection)
            {
                item.SetSortingIndex();
            }
            
        }

        private bool ValidFile(string path)
        {
            if (new FileInfo(path).Length > _settings.MaxFileObjectSize) return false; //50kb limit on file size (can be changed if this value is not suitable)
            //else if (System.IO.Path.GetExtension(path) == @".exe") return false;
            else if (_settings.IgnoredFileExtensions.Contains(Path.GetExtension(path))) return false;
            return true;
        }

        public List<FileObject> GetUnsavedFiles()
        {
            var rtnList = new List<FileObject>();
            foreach (var file in FileObjects)
            {
                if (file.UnsavedChanges) rtnList.Add(file);
            }
            return rtnList;
        }


    }
}
