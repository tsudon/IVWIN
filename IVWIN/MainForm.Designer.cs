using System;

namespace IVWIN
{
    partial class IVWIN
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード



        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.IVWImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.IVWImage)).BeginInit();
            this.SuspendLayout();
            // 
            // IVWImage
            // 
            this.IVWImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IVWImage.Location = new System.Drawing.Point(0, 0);
            this.IVWImage.Margin = new System.Windows.Forms.Padding(0);
            this.IVWImage.Name = "IVWImage";
            this.IVWImage.Size = new System.Drawing.Size(784, 561);
            this.IVWImage.TabIndex = 0;
            this.IVWImage.TabStop = false;
            this.IVWImage.WaitOnLoad = true;
            this.IVWImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IVWImage_Click);
            this.IVWImage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IVWImage_MouseDoubleClick);
            this.IVWImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IVWImage_MouseDown);
            this.IVWImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.IVWImage_MouseMove);
            this.IVWImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IVWImage_MouseUp);
            // 
            // IVWIN
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.IVWImage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "IVWIN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IVWIN";
            this.Load += new System.EventHandler(this.IVWIN_Load);
            this.SizeChanged += new System.EventHandler(this.IVWIN_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.IVWIN_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.IVWIN_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.IVWImage)).EndInit();
            this.ResumeLayout(false);

        }

        private void IVWIN_Load(object sender, EventArgs e)
        {
        }

        #endregion

        private System.Windows.Forms.PictureBox IVWImage;
    }
}

