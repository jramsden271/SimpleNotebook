using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleNotebook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileObjectCollection files;
        private FileObject openFile;
        public MainWindow()
        {
            files = new FileObjectCollection(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            //files = new FileObjectCollection(@"C:\Users\jramsden\OneDrive - BuroHappold\Notebook");
            //files = new FileObjectCollection(@"C:\Users\jrams\Desktop\New folder");


            //this.DataContext = files.FileObjects;
            //lb_fileList.DataContext = files;
            DataContext = files;
            InitializeComponent();
            lb_fileList.SelectedItem = files.First();
            lb_fileList.DataContext = files;
            
        }

        private void lb_fileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (openFile != null)
            {
                openFile.UpdateFileContents(tb_TextEditArea.Text, FileObject.UpdateMethod.SaveChangesToDisk);

            }
            if (lb_fileList.SelectedItem != null)
            {
                openFile = (FileObject)lb_fileList.SelectedItem;
                tb_TextEditArea.Text = openFile.GetFileContents();
                lb_Title.Content = openFile.FileName;
                win_MainWindow.Title = openFile.FileName + " - SimpleNotebook";
            }
            
            
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            openFile.UpdateFileContents(tb_TextEditArea.Text, FileObject.UpdateMethod.ForceSaveToDisk);
        }

        private void win_MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            openFile.UpdateFileContents(tb_TextEditArea.Text, FileObject.UpdateMethod.SaveChangesToDisk);
        }

        private void btn_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe",files.CollectionPath);
        }

        private void btn_NewFile_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            string year = dt.Year.ToString().Substring(2);
            string month = dt.Month < 10 ? "0" : ""; month += dt.Month.ToString();
            string day = dt.Day < 10 ? "0" : ""; day += dt.Day.ToString();
            string date = year + month + day;
            files.CreateNewFile(date + ".txt");
        }

        private void btn_Reload_Click(object sender, RoutedEventArgs e)
        {
            files.ReLoadFiles();
            lb_fileList.SelectedItem = files.First();
        }
    }
}
