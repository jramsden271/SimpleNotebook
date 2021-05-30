using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.ComponentModel;

namespace SimpleNotebook
{
    public class FileObject
    {
        public FileObject(string filepath, bool createNew = false)
        {
            if (File.Exists(filepath))
            {
                _filecontents = File.ReadAllText(filepath);
                FilePath = filepath;
            }
            else if(!createNew)
            {
                throw new IOException("The following path does not exist: " + filepath);
            }
            else if(createNew)
            {
                File.CreateText(filepath);
                FilePath = filepath;
            }
        }

        private string _filecontents;

        public string FilePath { get; }

        public string GetFileContents()
        {
            return _filecontents;
        }

        public void SetFileContents(string value)
        {
            UpdateFileContents(value, UpdateMethod.NoSaveToDisk);
        }


        public string SortingIndex { get; set; }
        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }
        public bool UnsavedChanges { get; private set; }

        private void LoadFileContentsFromDisk()
        {
            _filecontents = File.ReadAllText(FilePath);
        }

        private void SaveFileContentsToDisk()
        {
            File.WriteAllText(FilePath, _filecontents);
        }

        public void UpdateFileContents(string fileContents, UpdateMethod updateMethod)
        {
            UnsavedChanges = GetFileContents() != fileContents;
            _filecontents = fileContents;
            if(updateMethod == UpdateMethod.ForceSaveToDisk)
            {
                SaveFileContentsToDisk();
                UnsavedChanges = false;
            }
            else if(updateMethod == UpdateMethod.SaveChangesToDisk && UnsavedChanges)
            {
                SaveFileContentsToDisk();
                UnsavedChanges = false;
            }
        }

        public void SetSortingIndex()
        {
            SortingIndex = Path.GetFileName(FilePath);
        }

        public enum UpdateMethod
        {
            NoSaveToDisk,
            SaveChangesToDisk,
            ForceSaveToDisk
        }

        public override string ToString()
        {
            return FilePath;
        }
    }


}
