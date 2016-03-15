namespace ElectronTransferView.ContextMenuManager
{
    partial class JLBMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JLBMap));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddJLBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MoveJLBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditJLBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelJLBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TZMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddJLBItem,
            this.MoveJLBItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 48);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // AddJLBItem
            // 
            this.AddJLBItem.Name = "AddJLBItem";
            this.AddJLBItem.Size = new System.Drawing.Size(130, 22);
            this.AddJLBItem.Text = "增加";
            this.AddJLBItem.Click += new System.EventHandler(this.AddJLBItem_Click);
            // 
            // MoveJLBItem
            // 
            this.MoveJLBItem.Name = "MoveJLBItem";
            this.MoveJLBItem.Size = new System.Drawing.Size(130, 22);
            this.MoveJLBItem.Text = "计量表迁移";
            this.MoveJLBItem.Click += new System.EventHandler(this.MoveJLBItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditJLBItem,
            this.DelJLBItem,
            this.TZMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(95, 70);
            this.contextMenuStrip2.MouseLeave += new System.EventHandler(this.contextMenuStrip2_MouseLeave);
            // 
            // EditJLBItem
            // 
            this.EditJLBItem.Name = "EditJLBItem";
            this.EditJLBItem.Size = new System.Drawing.Size(94, 22);
            this.EditJLBItem.Text = "修改";
            this.EditJLBItem.Click += new System.EventHandler(this.EditJLBItem_Click);
            // 
            // DelJLBItem
            // 
            this.DelJLBItem.Name = "DelJLBItem";
            this.DelJLBItem.Size = new System.Drawing.Size(94, 22);
            this.DelJLBItem.Text = "删除";
            this.DelJLBItem.Click += new System.EventHandler(this.DelJLBItem_Click);
            // 
            // TZMenuItem
            // 
            this.TZMenuItem.Name = "TZMenuItem";
            this.TZMenuItem.Size = new System.Drawing.Size(94, 22);
            this.TZMenuItem.Text = "台账";
            this.TZMenuItem.Click += new System.EventHandler(this.TZMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "gg.bmp");
            this.imageList1.Images.SetKeyName(1, "gd.bmp");
            this.imageList1.Images.SetKeyName(2, "gg绿.bmp");
            this.imageList1.Images.SetKeyName(3, "gd绿.bmp");
            this.imageList1.Images.SetKeyName(4, "SHB2.jpg");
            this.imageList1.Images.SetKeyName(5, "SHB.jpg");
            // 
            // JLBMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 523);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "JLBMap";
            this.Text = "计量表管理";
            this.Load += new System.EventHandler(this.JLBMap_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JLBMap_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.JLBMap_Paint);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.JLBMap_FormClosed);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JLBMap_MouseDown);
            this.Resize += new System.EventHandler(this.JLBMap_Resize);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.JLBMap_MouseMove);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.JLBMap_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AddJLBItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem EditJLBItem;
        private System.Windows.Forms.ToolStripMenuItem DelJLBItem;
        private System.Windows.Forms.ToolStripMenuItem TZMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem MoveJLBItem;
    }
}