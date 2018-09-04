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
        private List<string> directryList = new List<String>();
        private List<FileSystemInfo> directryListInfo = new List<FileSystemInfo>();
        private String[] dirFilenames;
        private int currentDirPos;
        public LoadOption loadOption;                    //image browser option
        Bitmap bmp,resizeBmp, pbmp;
        private int offsetX = 0, offsetY = 0,startX,startY;
        private bool isMove = false;
        System.Timers.Timer clickTimer;

        public IVWIN()
        {
            InitializeComponent();
            AddInitialize();
            loadOption = new LoadOption();
            bmp = new Bitmap(this.Width, this.Height);
            resizeBmp = bmp;
            pbmp = bmp;
        }

        public IVWIN(string imagePath)
        {
            InitializeComponent();
            AddInitialize();
            loadOption = new LoadOption();
            SearchDirectry(imagePath);
            PaintPicture(imagePath);
        }


        private void AddInitialize()
        {
            clickTimer = new System.Timers.Timer(SystemInformation.DoubleClickTime + 10);
            clickTimer.Elapsed += (senderTimer, eTimer) =>
            {
                try
                {
                    clickTimer.Stop();
                    if (!isDobleClicked) NextPiture();
                }
                finally
                {
                    clickTimer.Stop();
                }
            };
        }

        private void SearchDirectry(String path)
        {
            if (File.Exists(path))
            {
                LogWritter.write("Drop File is exist.");

                currentFileName = Path.GetFileName(path);
                currentDir = Path.GetDirectoryName(path);

            }
            else if (Directory.Exists(path))
            {
                LogWritter.write("Drop file is directry");
                currentDir = path;
                currentFileName = null;
            }
            else
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
                if (info.Name.Equals(currentFileName))
                {
                    currentDirPos = i;
                }
                dirFilenames[i++] = info.Name;
                LogWritter.write(i + " " + info.Name);
            }



            if (currentFileName == null)
            {
                currentDirPos = 0;
                if (dirFilenames.Length > 0)
                {
                    currentFileName = dirFilenames[0];
                }
                else
                {
                    LogWritter.write("This Directry is empty.");
                    currentFileName = null;
                }
            }
            LogWritter.write("Set current path is " + currentDir);
            LogWritter.write("Set current file is " + currentFileName);

        }



        private void IVWIN_DragDrop(object sender, DragEventArgs e)
        {

            string[] fileName =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);

            SearchDirectry(fileName[0]);
            string imagePath = currentDir + "\\" + currentFileName;
            PaintPicture(imagePath);
          }

        private void IVWImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMove = true;
                startX = e.Location.X;
                startY = e.Location.Y;
            }
        }


        private void IVWImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove && (e.Button == MouseButtons.Left))
            {
                int x = e.Location.X;
                int y = e.Location.Y;

                MovePicture(x - startX, y - startY);
            }
        }

        bool isDobleClicked = false;

        private void IVWImage_Click(object sender, EventArgs e)
        {
            if (e.GetType().ToString().Equals("System.Windows.Forms.MouseEventArgs"))
            {

                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    isDobleClicked = false;
                    clickTimer.Start();
                }
                else if (me.Button == MouseButtons.Middle)
                {
                    // nothing
                }
                else
                {
                    // menu
                }
            }
        }

        private void NextPiture()
        {
            bool t = false;
            if (dirFilenames == null) return;
            do
            {

                currentDirPos += 1;
                if (currentDirPos >= dirFilenames.Length)
                {
                    currentDirPos = 0;
                }
                currentFileName = dirFilenames[currentDirPos];
                string imagePath = currentDir + "\\" + currentFileName;
                FileInfo info = new FileInfo(imagePath);
                if (info.Exists)
                {
                    t = PaintPicture(imagePath);
                }
                if (currentDirPos + 1 == dirFilenames.Length) break; // no Image file in Directory
            } while (!t);

        }


        private void IVWImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMove && (e.Button == MouseButtons.Left))
            {
                int x = e.Location.X - startX;
                int y = e.Location.Y - startY;
                if (x == 0 && y == 0) { isMove = false; return; }
                MovePicture(x, y);
                offsetX += x;
                offsetY += y;
            }
            isMove = false;
        }


        private void IVWImage_DoubleClick(object sender, EventArgs e)
        {
            if (e.GetType().ToString().Equals("System.Windows.Forms.MouseEventArgs"))
            {
                isDobleClicked = true;
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                        this.WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        this.FormBorderStyle = FormBorderStyle.None;
                        this.WindowState = FormWindowState.Maximized;

                    }
                }
            }
        }

        private void IVWIN_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))

                e.Effect = DragDropEffects.All;
            else

                e.Effect = DragDropEffects.None;
        }

        private void IVWIN_SizeChanged(object sender, EventArgs e)
        {
            RePaintPicture();
        }


        private bool PaintPicture(String imagePath)
        {
            try
            {
                LogWritter.write(imagePath);
                bmp = (Bitmap)Image.FromFile(imagePath);
                RePaintPicture();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void RePaintPicture ()
        {
            int width = IVWImage.Width;
            int height = IVWImage.Height;
            int imgWidth = bmp.Width;
            int imgHeight = bmp.Height;
            int resizeWidth = imgWidth;
            int resizeHeight = imgHeight;

            LogWritter.write("Original:" + imgWidth + "," + imgHeight + ">" + width + "," + height);

            loadOption.drawMode = DrawMode.DEFALT;
            IVWImage.SizeMode = PictureBoxSizeMode.CenterImage;

            switch (loadOption.drawMode)
            {
                case DrawMode.DEFALT:
                    if (width < imgWidth)
                    {
                        resizeWidth = width;
                        resizeHeight = (int)((double)imgHeight * ((double)width / (double)imgWidth));
                    }
                    if (height < resizeHeight)
                    {
                        resizeHeight = height;
                        resizeWidth = (int)((double)imgWidth * ((double)height / (double)imgHeight));
                    }
                    break;
                case DrawMode.HEIGHT_MATCH:
                    if (height < imgHeight)
                    {
                        resizeHeight = height;
                        resizeWidth = (int)((double)imgWidth * ((double)height / (double)imgHeight));
                    }
                    break;
                case DrawMode.FRAME_MATCH:
                    resizeWidth = width;
                    resizeHeight = height;
                    break;

                case DrawMode.WIDTH_MATCH:
                    if (width < imgWidth)
                    {
                        resizeWidth = width;
                        resizeHeight = (int)((double)imgHeight * ((double)width / (double)imgWidth));
                    }
                    break;

                case DrawMode.ORIGINAL: // do nothing
                    break;
            }

            resizeBmp = new Bitmap(resizeWidth, resizeHeight);
            Graphics g = Graphics.FromImage(resizeBmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Rectangle rect = new Rectangle(0, 0, resizeWidth, resizeHeight);
            g.DrawImage((Image)bmp, rect, 0, 0, imgWidth, imgHeight, GraphicsUnit.Pixel);
            pbmp = new Bitmap(width, height);
            g = Graphics.FromImage(pbmp);
            offsetX = (width - resizeWidth) / 2;
            offsetY = (height - resizeHeight) / 2;
            LogWritter.write(offsetX + "," + offsetY);
            g.DrawImage((Image)resizeBmp, offsetX, offsetY, resizeWidth, resizeHeight);
            IVWImage.Image = pbmp;
        }

        private void MovePicture (int x,int y)
        {
            int width = IVWImage.Width;
            int height = IVWImage.Height;
            int resizeWidth = resizeBmp.Width;
            int resizeHeight = resizeBmp.Height;
            pbmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(pbmp);
            g.DrawImage((Image)resizeBmp, offsetX + x, offsetY + y, resizeWidth, resizeHeight);
            IVWImage.Image = pbmp;
        }

    }

}
