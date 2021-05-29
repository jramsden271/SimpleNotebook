using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace SimpleNotebook
{
    public class FileObject
    {
        public FileObject(string filepath)
        {
            if (File.Exists(filepath))
            {
                _filecontents = File.ReadAllText(filepath);
                _filepath = filepath;
            }
            else
            {
                throw new IOException("The following path does not exist: " + filepath);
            }
        }

        private string _filecontents;
        private string _filepath;

        public string FilePath { get => _filepath; }
        public string FileContents 
        { 
            get => _filecontents;
            set
            {
                UpdateFileContents(value, UpdateMethod.NoSaveToDisk);
            }
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
            UnsavedChanges = FileContents != fileContents;
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
    }


}
