using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ElectronTransferView.ViewManager
{
    partial class LayerManage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new Container();
            this.treeView1 = new TreeView();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.ToolSM_Refresh = new ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = BorderStyle.FixedSingle;
            this.treeView1.CheckBoxes = true;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.Indent = 16;
            this.treeView1.ItemHeight = 16;
            this.treeView1.Location = new Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new Size(253, 683);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.MouseUp += new MouseEventHandler(this.treeView1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
            this.ToolSM_Refresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(119, 26);
            // 
            // ToolSM_Refresh
            // 
            this.ToolSM_Refresh.Name = "ToolSM_Refresh";
            this.ToolSM_Refresh.Size = new Size(118, 22);
            this.ToolSM_Refresh.Text = "刷 新(&R)";
            this.ToolSM_Refresh.Click += new EventHandler(this.ToolSM_Refresh_Click);
            // 
            // LayerManage
            // 
            this.AutoScaleDimensions = new SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.treeView1);
            this.Name = "LayerManage";
            this.Size = new Size(253, 683);
            this.Load += new EventHandler(this.LayerManage_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView treeView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem ToolSM_Refresh;
    }
}