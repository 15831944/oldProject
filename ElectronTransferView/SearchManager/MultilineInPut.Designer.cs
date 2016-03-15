namespace ElectronTransferView.SearchManager
{
    partial class MultilineInPut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultilineInPut));
            this.btAccept = new System.Windows.Forms.Button();
            this.imageListRef = new System.Windows.Forms.ImageList(this.components);
            this.toolTipRef = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFilterBefore = new System.Windows.Forms.ListBox();
            this.AddItem = new System.Windows.Forms.Button();
            this.SubItem = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbFilterAfter = new System.Windows.Forms.ListBox();
            this.btEdit = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbEdit = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(418, 445);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(60, 23);
            this.btAccept.TabIndex = 1;
            this.btAccept.Text = "确定";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // imageListRef
            // 
            this.imageListRef.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListRef.ImageStream")));
            this.imageListRef.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListRef.Images.SetKeyName(0, "确定.png");
            this.imageListRef.Images.SetKeyName(1, "确定经过.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbFilterBefore);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 356);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "过滤字段备选项";
            // 
            // lbFilterBefore
            // 
            this.lbFilterBefore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFilterBefore.FormattingEnabled = true;
            this.lbFilterBefore.ItemHeight = 12;
            this.lbFilterBefore.Location = new System.Drawing.Point(3, 17);
            this.lbFilterBefore.Name = "lbFilterBefore";
            this.lbFilterBefore.Size = new System.Drawing.Size(188, 328);
            this.lbFilterBefore.TabIndex = 0;
            this.lbFilterBefore.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFilterBefore_KeyDown);
            // 
            // AddItem
            // 
            this.AddItem.Location = new System.Drawing.Point(212, 116);
            this.AddItem.Name = "AddItem";
            this.AddItem.Size = new System.Drawing.Size(75, 23);
            this.AddItem.TabIndex = 3;
            this.AddItem.Text = "---->";
            this.AddItem.UseVisualStyleBackColor = true;
            this.AddItem.Click += new System.EventHandler(this.AddItem_Click);
            // 
            // SubItem
            // 
            this.SubItem.Location = new System.Drawing.Point(212, 226);
            this.SubItem.Name = "SubItem";
            this.SubItem.Size = new System.Drawing.Size(75, 23);
            this.SubItem.TabIndex = 4;
            this.SubItem.Text = "<----";
            this.SubItem.UseVisualStyleBackColor = true;
            this.SubItem.Click += new System.EventHandler(this.SubItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lbFilterAfter);
            this.groupBox2.Location = new System.Drawing.Point(293, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(194, 356);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "当前设备过滤项";
            // 
            // lbFilterAfter
            // 
            this.lbFilterAfter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFilterAfter.FormattingEnabled = true;
            this.lbFilterAfter.ItemHeight = 12;
            this.lbFilterAfter.Location = new System.Drawing.Point(3, 17);
            this.lbFilterAfter.Name = "lbFilterAfter";
            this.lbFilterAfter.Size = new System.Drawing.Size(188, 328);
            this.lbFilterAfter.TabIndex = 0;
            this.lbFilterAfter.SelectedIndexChanged += new System.EventHandler(this.lbFilterAfter_SelectedIndexChanged);
            this.lbFilterAfter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFilterAfter_KeyDown);
            // 
            // btEdit
            // 
            this.btEdit.Location = new System.Drawing.Point(403, 20);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(60, 23);
            this.btEdit.TabIndex = 7;
            this.btEdit.Text = "添加";
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox3.Location = new System.Drawing.Point(15, 374);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(472, 2);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbEdit);
            this.groupBox4.Controls.Add(this.btEdit);
            this.groupBox4.Location = new System.Drawing.Point(15, 382);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(469, 57);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "新增备注内容";
            // 
            // tbEdit
            // 
            this.tbEdit.Location = new System.Drawing.Point(6, 22);
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(391, 21);
            this.tbEdit.TabIndex = 8;
            // 
            // MultilineInPut
            // 
            this.AcceptButton = this.SubItem;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(496, 479);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.SubItem);
            this.Controls.Add(this.AddItem);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultilineInPut";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "过滤项";
            this.Load += new System.EventHandler(this.MultilineInPut_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.ToolTip toolTipRef;
        private System.Windows.Forms.ImageList imageListRef;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbFilterBefore;
        private System.Windows.Forms.Button AddItem;
        private System.Windows.Forms.Button SubItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbFilterAfter;
        private System.Windows.Forms.Button btEdit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbEdit;
    }
}