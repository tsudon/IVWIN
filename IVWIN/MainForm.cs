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

        System.Timers.Timer clickTimer;
        Loader loader;
        private int startX, startY;
        private bool isMove = false;


        public IVWIN()
        {
            InitializeComponent();
            AddInitialize(null);
        }

        public IVWIN(string imagePath)
        {
            InitializeComponent();
            AddInitialize(imagePath);
        }


        private void AddInitialize(string imagePath)
        {
            loader = new Loader(imagePath);
            loader.SetPictureBox(IVWImage);
            IVWImage.MouseWheel += new
               System.Windows.Forms.MouseEventHandler(this.IVWIN_MouseWheel);
            clickTimer = new System.Timers.Timer(SystemInformation.DoubleClickTime + 10);
            LogWritter.write(SystemInformation.DoubleClickTime.ToString());
            clickTimer.Elapsed += (senderTimer, eTimer) =>
            {
                try
                {
                    clickTimer.Stop();
                    if (!isDobleClicked) loader.NextPiture();
                }
                finally
                {
                    clickTimer.Stop();
                }
            };
        }

        private void IVWIN_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta >= 120 )
            {
                loader.PreviousPiture();
            }
            else if (e.Delta <= -120)
            {
                loader.NextPiture();
            }
        }





        private void IVWIN_DragDrop(object sender, DragEventArgs e)
        {

            string[] fileName =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);

            loader.Load(fileName[0]);
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
                isDobleClicked = true;
                loader.MovePicture(x - startX, y - startY);
            }
        }

        bool isDobleClicked = false;


        private void IVWImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMove && (e.Button == MouseButtons.Left))
            {
                int x = e.Location.X - startX;
                int y = e.Location.Y - startY;
                if (x == 0 && y == 0) { isMove = false; return; }
                loader.MovePicture(x, y);
                loader.AddOffset(x, y);
            }
            isMove = false;
        }


        private void IVWImage_Click(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
//                clickTimer.Start();
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

        private void IVWImage_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            isDobleClicked = true;
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                    this.WindowState = FormWindowState.Normal;
                    loader.RePaintPicture();
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    IVWImage.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    IVWImage.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    loader.RePaintPicture();
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

            loader.ClearAndRePaintPicture();
        }




    }

}
