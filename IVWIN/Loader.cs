using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace IVWIN

{
    /*Loaderの本体 ファイル管理は分離する */



    public delegate void Callback(AnimationFrames frames);

    public class AnimationFrame
    {
        public Bitmap bitmap;
        public int delay;
        public AnimationFrame(Bitmap bitmap, int delay)
        {
            this.bitmap = bitmap;
            this.delay = delay;
        }
    }


    public class AnimationFrames
    {

        public int Loop ; // 0 = inifinity
        private int loopCount;
        private System.Timers.Timer timer;
        private Callback callback;
        private int width, height;
        private List<AnimationFrame> frameList;
        public int count,step,delay;
        private AnimationFrame[] frames;

        public AnimationFrames(int width,int height,int steps)
        {
            this.width = width;
            this.height = height;

            frameList = new List<AnimationFrame>();
            this.timer = new System.Timers.Timer(steps);
            callback = null;
            timer.AutoReset = true;
            timer.Enabled = false;
            timer.Elapsed += (sender, e) =>
            {
                DefaultCallback();
            };
            count = 0;
            loopCount = 0;
        }


        public void AddFrame(Bitmap bitmap,int delay)
        {
            AnimationFrame frame = new AnimationFrame(bitmap,delay);
            frameList.Add(frame);
        }

        public void InitializeAnimation()
        {
            loopCount = 0;
            count = 0;
            frames = frameList.ToArray();
        }


        public AnimationFrame GetFrame()
        {
            if (frames == null) InitializeAnimation();
            AnimationFrame frame;
            delay = frames[count].delay;
            frame = frames[count++];
            if (count >= frames.Length) { count = 0; loopCount++; }           
            return frame;
        }

        private void DefaultCallback()
        {
            if (Loop != 0 && loopCount >= Loop) return;
            if (step++ < delay) return;
            step = 0;
            if (callback != null)
            {
                callback(this);
            }
        }

        public void SetCallback(Callback func)
        {
            this.callback = func;
        }


        public void stop()
        {
            timer.Stop();
            count = 0;
            loopCount = 0;
            step = 0;
            delay = 0;
        }

        public void pause()
        {
            timer.Stop();
            step = 0;
            delay = 0;
        }

        public void start()
        {
            timer.Start();
            step = 0;
            delay = 0;
        }

    }



    class Loader
    {
        private String currentFileName;
        private String currentDir;
        private int sortOption = FileSort.SORT_DEAULT;
        private List<string> directryList = new List<String>();
        private List<FileSystemInfo> directryListInfo = new List<FileSystemInfo>();
        private String[] dirFilenames;
        private int currentDirPos;
        private int offsetX = 0, offsetY = 0;
        public LoadOption loadOption;                    //image browser option
        Bitmap bmp, resizeBmp, pbmp;
        System.Windows.Forms.PictureBox pic;
        private int frameCount, framePos = 0;
        private AnimationFrames frames;



        public Loader(String imagePath)
        {

            loadOption = new LoadOption();
            if (imagePath != null)
            {
                SearchDirectry(imagePath);
                PaintPicture(imagePath);
            }
        }

        public void AddOffset(int x, int y)
        {
            offsetX += x;
            offsetY += y;
        }

        public void SetPictureBox(System.Windows.Forms.PictureBox pictureBox)
        {
            this.pic = pictureBox;
        }

        public void Load(string path)
        {
            SearchDirectry(path);
            string imagePath = currentDir + "\\" + currentFileName;
            PaintPicture(imagePath);
        }

        public bool PaintPicture(String imagePath)
        {
            try
            {
                LogWritter.write(imagePath);

                pic.UseWaitCursor = true;
                bmp = (Bitmap)Image.FromFile(imagePath);
                Clear();
                pic.UseWaitCursor = false;
                FrameDimension fd = new FrameDimension(bmp.FrameDimensionsList[0]);
                frameCount = bmp.GetFrameCount(fd);
                framePos = 0;
                if (frameCount > 1 && loadOption.isAnimate)
                {
                    LogWritter.write("This File is Animation GIF.");
                    frames = new AnimationFrames(bmp.Width, bmp.Height, 10); //GIF only
                    Callback callback = new Callback(Animation);
                    frames.SetCallback(callback);
                    byte[] buf = bmp.GetPropertyItem(0x5100).Value; //PropertyTagFrameDelay


                    for (int i = 0; i < frameCount; i++)
                    {
                        int offset = i * 4;
                        int delay = buf[offset] | buf[offset + 1] << 8 | buf[offset + 2] << 16 >> buf[offset + 3] << 24;

                        bmp.SelectActiveFrame(fd, i);
                        Bitmap bitmap = new Bitmap(bmp.Width,bmp.Height);
                        Graphics g = Graphics.FromImage(bitmap);
                        g.DrawImage(bmp,0,0);
                        frames.AddFrame(bitmap, delay);
                    }
                    frames.start();
                } else {
                    RePaintPicture(bmp);
                }


            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Animation(AnimationFrames frames)
        {
            if (frames == null) return;
            AnimationFrame frame = frames.GetFrame();
            if (frame == null) return;
            int count = frames.count;
            if (frame.bitmap != null) RePaintPicture(frame.bitmap,true);
        }


        public void Clear()
        {
        }


        public void ClearAndRePaintPicture()
        {
            Clear();
            RePaintPicture(this.bmp);
        }

        public void RePaintPicture() {
            RePaintPicture(this.bmp);
        }

        public void RePaintPicture(Bitmap bmp)
        {
            RePaintPicture(bmp,false);
        }




        public void RePaintPicture(Bitmap bmp,bool isAnimation)
        {
            int width = pic.Width;
            int height = pic.Height;
            int imgWidth = bmp.Width;
            int imgHeight = bmp.Height;
            int resizeWidth = imgWidth;
            int resizeHeight = imgHeight;


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
            Bitmap rbmp = new Bitmap(resizeWidth, resizeHeight);
            Graphics g = Graphics.FromImage(rbmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Rectangle rect = new Rectangle(0, 0, resizeWidth, resizeHeight);
            g.DrawImage((Image)bmp, rect, 0, 0, imgWidth, imgHeight, GraphicsUnit.Pixel);
            pbmp = new Bitmap(width, height);
            g = Graphics.FromImage(pbmp);
            offsetX = (width - resizeWidth) / 2;
            offsetY = (height - resizeHeight) / 2;
            g.DrawImage((Image)rbmp, offsetX, offsetY, resizeWidth, resizeHeight);
            pic.Image = pbmp;
            pic.Refresh();
            if (resizeBmp != null) {
                Bitmap oldbmp = resizeBmp;
                resizeBmp = rbmp;
                oldbmp.Dispose();
            }
            else
            {
                resizeBmp = rbmp;
            }
        }

        public void MovePicture(int x, int y)
        {
            int width = pic.Width;
            int height = pic.Height;
            int resizeWidth = width;
            int resizeHeight = height;
            if (resizeBmp != null)
            {
                Clear();
                resizeWidth = resizeBmp.Width;
                resizeHeight = resizeBmp.Height;
                pbmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(pbmp);
                g.DrawImage((Image)resizeBmp, offsetX + x, offsetY + y, resizeWidth, resizeHeight);
                pic.Image = pbmp;
            }
        }

        public void NextPiture()
        {
            bool t = false;
            if (dirFilenames == null) return;
            if (frameCount > 1 && loadOption.isAnimate) { frames.stop(); }
            if (framePos < (frameCount -1) && !loadOption.isAnimate)
            {
                framePos++;
                FrameDimension fd = new FrameDimension(bmp.FrameDimensionsList[0]);
                bmp.SelectActiveFrame(fd, framePos);
                RePaintPicture(bmp);
                return;
            }

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


        public void PreviousPiture()
        {
            bool t = false;
            if (dirFilenames == null) return;
            if (frameCount > 1 && loadOption.isAnimate) { frames.stop(); }
            if (framePos > 0 && !loadOption.isAnimate)
            {
                framePos--;
                FrameDimension fd = new FrameDimension(bmp.FrameDimensionsList[0]);
                bmp.SelectActiveFrame(fd, framePos);
                RePaintPicture(bmp);
                return;
            }
            do
            {
                currentDirPos -= 1;
                if (currentDirPos < 0)
                {
                    currentDirPos = dirFilenames.Length - 1;
                }
                currentFileName = dirFilenames[currentDirPos];
                string imagePath = currentDir + "\\" + currentFileName;
                FileInfo info = new FileInfo(imagePath);
                if (info.Exists)
                {
                    t = PaintPicture(imagePath);
                    if (frameCount > 1)
                    {
                        framePos = frameCount - 1;
                        FrameDimension fd = new FrameDimension(bmp.FrameDimensionsList[0]);
                        bmp.SelectActiveFrame(fd, framePos);
                        RePaintPicture(bmp);
                    }

                }
                if (currentDirPos == 0) break; // no Image file in Directory
            } while (!t);

        }

        public void SearchDirectry(String path)
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
    }
}
