namespace ElectronTransferView.ContextMenuManager
{
    partial class SHBMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SHBMap));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddSHBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MoveSHBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "户表蓝.bmp");
            this.imageList1.Images.SetKeyName(1, "户表绿.bmp");
            this.imageList1.Images.SetKeyName(2, "SHB2.jpg");
            this.imageList1.Images.SetKeyName(3, "SHB.jpg");
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改ToolStripMenuItem,
            this.DelMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(95, 48);
            this.contextMenuStrip2.MouseLeave += new System.EventHandler(this.contextMenuStrip2_MouseLeave);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.EditSHBItem_Click);
            // 
            // DelMenuItem
            // 
            this.DelMenuItem.Name = "DelMenuItem";
            this.DelMenuItem.Size = new System.Drawing.Size(94, 22);
            this.DelMenuItem.Text = "删除";
            this.DelMenuItem.Click += new System.EventHandler(this.DelSHBItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddSHBItem,
            this.MoveSHBItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 48);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // AddSHBItem
            // 
            this.AddSHBItem.Name = "AddSHBItem";
            this.AddSHBItem.Size = new System.Drawing.Size(118, 22);
            this.AddSHBItem.Text = "增加";
            this.AddSHBItem.Click += new System.EventHandler(this.AddSHBItem_Click);
            // 
            // MoveSHBItem
            // 
            this.MoveSHBItem.Name = "MoveSHBItem";
            this.MoveSHBItem.Size = new System.Drawing.Size(118, 22);
            this.MoveSHBItem.Text = "户表迁移";
            this.MoveSHBItem.Click += new System.EventHandler(this.MoveSHBItem_Click);
            // 
            // SHBMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 498);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SHBMap";
            this.Text = "散户表管理";
            this.Load += new System.EventHandler(this.SHBMap_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SHBMap_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SHBMap_Paint);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SHBMap_FormClosed);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SHBMap_MouseDown);
            this.Resize += new System.EventHandler(this.SHBMap_Resize);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SHBMap_MouseMove);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SHBView_KeyDown);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DelMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AddSHBItem;
        private System.Windows.Forms.ToolStripMenuItem MoveSHBItem;
    }
}