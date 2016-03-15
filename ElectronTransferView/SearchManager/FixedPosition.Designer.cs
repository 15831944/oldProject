namespace ElectronTransferView.SearchManager
{
    partial class FixedPosition
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeViewDevLayer = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LvEntity = new System.Windows.Forms.ListView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(884, 490);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeViewDevLayer);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 490);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设备类别：";
            // 
            // treeViewDevLayer
            // 
            this.treeViewDevLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDevLayer.Location = new System.Drawing.Point(3, 17);
            this.treeViewDevLayer.Name = "treeViewDevLayer";
            this.treeViewDevLayer.Size = new System.Drawing.Size(199, 470);
            this.treeViewDevLayer.TabIndex = 0;
            this.treeViewDevLayer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDevLayer_NodeMouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LvEntity);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(675, 490);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备列表：";
            // 
            // LvEntity
            // 
            this.LvEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LvEntity.FullRowSelect = true;
            this.LvEntity.Location = new System.Drawing.Point(3, 17);
            this.LvEntity.MultiSelect = false;
            this.LvEntity.Name = "LvEntity";
            this.LvEntity.Size = new System.Drawing.Size(669, 470);
            this.LvEntity.TabIndex = 1;
            this.LvEntity.UseCompatibleStateImageBehavior = false;
            this.LvEntity.View = System.Windows.Forms.View.Details;
            this.LvEntity.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvEntity_MouseDoubleClick);
            this.LvEntity.Resize += new System.EventHandler(this.LvEntity_Resize);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(12, 508);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(884, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "帮助：鼠标点击设备列表任意项便可实现定位,滚动鼠标可以加载剩余设备。";
            // 
            // FixedPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 538);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FixedPosition";
            this.Text = "设备快速定位";
            this.Load += new System.EventHandler(this.FixedPosition_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FixedPosition_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FixedPosition_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView LvEntity;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TreeView treeViewDevLayer;
    }
}