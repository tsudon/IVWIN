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



        //image browser option
        LoadOption loadOption;


        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // IVWIN
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(803, 480);
            this.DoubleBuffered = true;
            this.Name = "IVWIN";
            this.Text = "IVWIN";
            this.Load += new System.EventHandler(this.IVWIN_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.IVWIN_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.IVWIN_DragEnter);
            this.ResumeLayout(false);

        }

        private void IVWIN_Load(object sender, EventArgs e)
        {
        }

        #endregion
    }
}

