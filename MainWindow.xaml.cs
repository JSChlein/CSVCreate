using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace CreateCSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string folderPath { get; set; }
        private string outputPath { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Generate.Click += btn1_Click;
            Select_Folder.Click += btn2_Click;
            Select_Output.Click += btn3_Click;

        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(this.folderPath);
            DRYDataReader dataReader = new DRYDataReader(this.folderPath, this.outputPath);
            Form dlg1 = new Form();
            dlg1.Text = "File has been created";
            dlg1.ShowDialog();
            // do something
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                this.outputPath = openFileDlg.SelectedPath;
            }
            text2.Text += this.outputPath;
            Debug.WriteLine("HEJ");
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                this.folderPath = openFileDlg.SelectedPath;
            }
            text1.Text += this.folderPath;
            Debug.WriteLine("HEJ");
            // do something
        }
    }
}
