namespace ElectronTransferView.ContextMenuManager
{
    partial class SHBView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SHBView));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddSHBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditSHBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelSHBItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddSHBItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // AddSHBItem
            // 
            this.AddSHBItem.Name = "AddSHBItem";
            this.AddSHBItem.Size = new System.Drawing.Size(94, 22);
            this.AddSHBItem.Text = "增加";
            this.AddSHBItem.Click += new System.EventHandler(this.AddSHBItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "SHB2.jpg");
            this.imageList1.Images.SetKeyName(1, "SHB.jpg");
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditSHBItem,
            this.DelSHBItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(95, 48);
            // 
            // EditSHBItem
            // 
            this.EditSHBItem.Name = "EditSHBItem";
            this.EditSHBItem.Size = new System.Drawing.Size(94, 22);
            this.EditSHBItem.Text = "修改";
            this.EditSHBItem.Click += new System.EventHandler(this.EditSHBItem_Click);
            // 
            // DelSHBItem
            // 
            this.DelSHBItem.Name = "DelSHBItem";
            this.DelSHBItem.Size = new System.Drawing.Size(94, 22);
            this.DelSHBItem.Text = "删除";
            this.DelSHBItem.Click += new System.EventHandler(this.DelSHBItem_Click);
            // 
            // SHBView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(592, 573);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Enabled = false;
            this.Name = "SHBView";
            this.Text = "散户表管理";
            this.Load += new System.EventHandler(this.SHBView_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SHBView_MouseUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SHBView_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AddSHBItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem DelSHBItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem EditSHBItem;
    }
}