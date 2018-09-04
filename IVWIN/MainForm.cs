using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IVWIN
{


    public partial class IVWIN : Form
    {
        private String currentFileName;
        private String currentDir;
        private int sortOption = FileSort.SORT_DEAULT;
        private List<String> directryList = new List<String>();
        private List<FileSystemInfo> directryListInfo = new List<FileSystemInfo>();
        private String[] dirFilenames;

        public IVWIN()
        {
            InitializeComponent();
        }

        private void IVWIN_DragDrop(object sender, DragEventArgs e)
        {

            string[] fileName =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (File.Exists(fileName[0])) {
                LogWritter.write("Drop File is exist.");

                currentFileName = Path.GetFileName(fileName[0]);
                currentDir = Path.GetDirectoryName(fileName[0]);

            } else if (Directory.Exists(fileName[0]))
            {
                LogWritter.write("Drop file is directry");
                currentDir = fileName[0];
                currentFileName = null;
            } else
            {
                return;
            }

            directryList.Clear();
            directryListInfo.Clear();

            foreach (String f in Directory.GetFileSystemEntries(currentDir))
            {
                directryList.Add(f);
                directryListInfo.Add(new FileInfo(f));
            }

//            sortOption = FileSort.SORT_BY_DATE_DESC;

            FileSort.Sort(ref directryListInfo, sortOption);

            //            directryList.Sort();
            FileSystemInfo[] infos = directryListInfo.ToArray();

            dirFilenames = new String[infos.Length];
            int i = 0;
            foreach (FileSystemInfo info in infos)
            {
                dirFilenames[i++] = info.Name;
                LogWritter.write(i + " " + info.Name);
            }



            if (currentFileName == null)
            {
                if (dirFilenames.Length > 0)
                {
                    currentFileName = dirFilenames[0];
                } else
                {
                    LogWritter.write("This Directry is empty.");
                    currentFileName = null;
                }
            }
            LogWritter.write("Set current path is " + currentDir);
            LogWritter.write("Set current file is " + currentFileName);
            string imagePath = currentDir + "\\" + currentFileName;
            Image image = Image.FromFile(imagePath);
            this.BackgroundImage = image;
        }



        private void IVWIN_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))

                e.Effect = DragDropEffects.All;
            else

                e.Effect = DragDropEffects.None;
        }
    }
}
