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
                UpdateFileContents(value);
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

        private void SaveFileContents()
        {
            File.WriteAllText(FilePath, _filecontents);
        }

        public void UpdateFileContents(string fileContents, bool saveToDisk = false)
        {
            _filecontents = fileContents;
            if(saveToDisk)
            {
                SaveFileContents();
                UnsavedChanges = false;
            }
            else
            {
                UnsavedChanges = true;
            }
        }

        public void SetSortingIndex()
        {
            SortingIndex = Path.GetFileName(FilePath);
        }
    }


}
