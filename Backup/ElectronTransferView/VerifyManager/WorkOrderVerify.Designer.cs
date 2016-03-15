namespace ElectronTransferView.VerifyManager
{
    sealed partial class WorkOrderVerify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkOrderVerify));
            this.tabControlVerify = new System.Windows.Forms.TabControl();
            this.tabPageAttribute = new System.Windows.Forms.TabPage();
            this.LvAttribute = new System.Windows.Forms.ListView();
            this.contextMenuStripVerify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemRef = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageTopology = new System.Windows.Forms.TabPage();
            this.LvTopology = new System.Windows.Forms.ListView();
            this.contextMenuStripTopology = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemTopRef = new System.Windows.Forms.ToolStripMenuItem();
            this.btExport = new System.Windows.Forms.Button();
            this.imageListExcel = new System.Windows.Forms.ImageList(this.components);
            this.toolTipExcel = new System.Windows.Forms.ToolTip(this.components);
            this.btRef = new System.Windows.Forms.Button();
            this.bulkModify = new System.Windows.Forms.Button();
            this.tabControlVerify.SuspendLayout();
            this.tabPageAttribute.SuspendLayout();
            this.contextMenuStripVerify.SuspendLayout();
            this.tabPageTopology.SuspendLayout();
            this.contextMenuStripTopology.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlVerify
            // 
            this.tabControlVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlVerify.Controls.Add(this.tabPageAttribute);
            this.tabControlVerify.Controls.Add(this.tabPageTopology);
            this.tabControlVerify.Location = new System.Drawing.Point(0, 0);
            this.tabControlVerify.Name = "tabControlVerify";
            this.tabControlVerify.SelectedIndex = 0;
            this.tabControlVerify.Size = new System.Drawing.Size(792, 475);
            this.tabControlVerify.TabIndex = 0;
            this.tabControlVerify.SelectedIndexChanged += new System.EventHandler(this.tabControlVerify_SelectedIndexChanged);
            // 
            // tabPageAttribute
            // 
            this.tabPageAttribute.Controls.Add(this.LvAttribute);
            this.tabPageAttribute.Location = new System.Drawing.Point(4, 22);
            this.tabPageAttribute.Name = "tabPageAttribute";
            this.tabPageAttribute.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAttribute.Size = new System.Drawing.Size(784, 449);
            this.tabPageAttribute.TabIndex = 1;
            this.tabPageAttribute.Text = "属性校验";
            this.tabPageAttribute.UseVisualStyleBackColor = true;
            // 
            // LvAttribute
            // 
            this.LvAttribute.ContextMenuStrip = this.contextMenuStripVerify;
            this.LvAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LvAttribute.FullRowSelect = true;
            this.LvAttribute.Location = new System.Drawing.Point(3, 3);
            this.LvAttribute.Name = "LvAttribute";
            this.LvAttribute.Size = new System.Drawing.Size(778, 443);
            this.LvAttribute.TabIndex = 0;
            this.LvAttribute.UseCompatibleStateImageBehavior = false;
            this.LvAttribute.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvAttribute_MouseDoubleClick);
            this.LvAttribute.Resize += new System.EventHandler(this.LvAttribute_Resize);
            this.LvAttribute.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.LvAttribute_ItemMouseHover);
            this.LvAttribute.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LvAttribute_ColumnClick);
            // 
            // contextMenuStripVerify
            // 
            this.contextMenuStripVerify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemRef});
            this.contextMenuStripVerify.Name = "contextMenuStripVerify";
            this.contextMenuStripVerify.Size = new System.Drawing.Size(101, 26);
            this.contextMenuStripVerify.Click += new System.EventHandler(this.ToolStripMenuItemRef_Click);
            // 
            // ToolStripMenuItemRef
            // 
            this.ToolStripMenuItemRef.Name = "ToolStripMenuItemRef";
            this.ToolStripMenuItemRef.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemRef.Text = "刷新";
            this.ToolStripMenuItemRef.Click += new System.EventHandler(this.ToolStripMenuItemRef_Click);
            // 
            // tabPageTopology
            // 
            this.tabPageTopology.Controls.Add(this.LvTopology);
            this.tabPageTopology.Location = new System.Drawing.Point(4, 22);
            this.tabPageTopology.Name = "tabPageTopology";
            this.tabPageTopology.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTopology.Size = new System.Drawing.Size(784, 449);
            this.tabPageTopology.TabIndex = 2;
            this.tabPageTopology.Text = "拓扑校验";
            this.tabPageTopology.UseVisualStyleBackColor = true;
            // 
            // LvTopology
            // 
            this.LvTopology.ContextMenuStrip = this.contextMenuStripTopology;
            this.LvTopology.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LvTopology.FullRowSelect = true;
            this.LvTopology.Location = new System.Drawing.Point(3, 3);
            this.LvTopology.Name = "LvTopology";
            this.LvTopology.Size = new System.Drawing.Size(778, 443);
            this.LvTopology.TabIndex = 0;
            this.LvTopology.UseCompatibleStateImageBehavior = false;
            this.LvTopology.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvTopology_MouseDoubleClick);
            this.LvTopology.Resize += new System.EventHandler(this.LvTopology_Resize);
            this.LvTopology.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LvTopology_ColumnClick);
            // 
            // contextMenuStripTopology
            // 
            this.contextMenuStripTopology.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemTopRef});
            this.contextMenuStripTopology.Name = "contextMenuStripTopology";
            this.contextMenuStripTopology.Size = new System.Drawing.Size(101, 26);
            this.contextMenuStripTopology.Click += new System.EventHandler(this.ToolStripMenuItemTopRef_Click);
            // 
            // ToolStripMenuItemTopRef
            // 
            this.ToolStripMenuItemTopRef.Name = "ToolStripMenuItemTopRef";
            this.ToolStripMenuItemTopRef.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemTopRef.Text = "刷新";
            this.ToolStripMenuItemTopRef.Click += new System.EventHandler(this.ToolStripMenuItemTopRef_Click);
            // 
            // btExport
            // 
            this.btExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExport.Location = new System.Drawing.Point(703, 481);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(82, 23);
            this.btExport.TabIndex = 1;
            this.btExport.Text = "导出到EXCEL ";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.MouseLeave += new System.EventHandler(this.btExport_MouseLeave);
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            this.btExport.MouseEnter += new System.EventHandler(this.btExport_MouseEnter);
            // 
            // imageListExcel
            // 
            this.imageListExcel.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListExcel.ImageStream")));
            this.imageListExcel.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListExcel.Images.SetKeyName(0, "导出excl.png");
            this.imageListExcel.Images.SetKeyName(1, "导出excl经过.png");
            // 
            // toolTipExcel
            // 
            this.toolTipExcel.AutoPopDelay = 3000;
            this.toolTipExcel.InitialDelay = 2000;
            this.toolTipExcel.ReshowDelay = 100;
            // 
            // btRef
            // 
            this.btRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRef.Location = new System.Drawing.Point(609, 481);
            this.btRef.Name = "btRef";
            this.btRef.Size = new System.Drawing.Size(88, 23);
            this.btRef.TabIndex = 2;
            this.btRef.Text = "刷新拓扑关系";
            this.btRef.UseVisualStyleBackColor = true;
            this.btRef.Click += new System.EventHandler(this.btRef_Click);
            // 
            // bulkModify
            // 
            this.bulkModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bulkModify.Location = new System.Drawing.Point(515, 481);
            this.bulkModify.Name = "bulkModify";
            this.bulkModify.Size = new System.Drawing.Size(88, 23);
            this.bulkModify.TabIndex = 3;
            this.bulkModify.Text = "批量修改";
            this.bulkModify.UseVisualStyleBackColor = true;
            this.bulkModify.Click += new System.EventHandler(this.bulkModify_Click);
            // 
            // WorkOrderVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(792, 514);
            this.Controls.Add(this.btRef);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.tabControlVerify);
            this.Controls.Add(this.bulkModify);
            this.Name = "WorkOrderVerify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "增量包校验";
            this.Load += new System.EventHandler(this.WorkOrderVerify_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkOrderVerify_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WorkOrderVerify_KeyDown);
            this.tabControlVerify.ResumeLayout(false);
            this.tabPageAttribute.ResumeLayout(false);
            this.contextMenuStripVerify.ResumeLayout(false);
            this.tabPageTopology.ResumeLayout(false);
            this.contextMenuStripTopology.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlVerify;
        private System.Windows.Forms.TabPage tabPageAttribute;
        private System.Windows.Forms.TabPage tabPageTopology;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripVerify;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRef;
        private System.Windows.Forms.ImageList imageListExcel;
        private System.Windows.Forms.ToolTip toolTipExcel;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTopology;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTopRef;
        private System.Windows.Forms.ListView LvAttribute;
        private System.Windows.Forms.ListView LvTopology;
        private System.Windows.Forms.Button btRef;
        private System.Windows.Forms.Button bulkModify;
    }
}