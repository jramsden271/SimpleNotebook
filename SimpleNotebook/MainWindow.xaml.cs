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
        SimpleNotebook.FileObjectCollection files;
        FileObject openFile;
        public MainWindow()
        {
            files = new FileObjectCollection(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

            //this.DataContext = files.FileObjects;
            //lb_fileList.DataContext = files;
            DataContext = files;
            InitializeComponent();
            lb_fileList.SelectedItem = files.First();
        }

        private void lb_fileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(openFile!=null) openFile.FileContents = tb_TextEditArea.Text;
            openFile = (FileObject)lb_fileList.SelectedItem;
            tb_TextEditArea.Text = openFile.FileContents;
            lb_Title.Content = openFile.FileName;
            //asdf
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            openFile.UpdateFileContents(tb_TextEditArea.Text, true);
        }
    }
}
