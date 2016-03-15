namespace ElectronTransferView.FunctionManager
{
    partial class RegionalManagement
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.AddButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnLocation = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.HotTracking = true;
            this.treeView1.Location = new System.Drawing.Point(5, 57);
            this.treeView1.Margin = new System.Windows.Forms.Padding(18);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(430, 251);
            this.treeView1.TabIndex = 0;
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // AddButton
            // 
            this.AddButton.Enabled = false;
            this.AddButton.Location = new System.Drawing.Point(7, 28);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(96, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "添加";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // EditButton
            // 
            this.EditButton.Enabled = false;
            this.EditButton.Location = new System.Drawing.Point(118, 27);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(96, 23);
            this.EditButton.TabIndex = 2;
            this.EditButton.Text = "编辑";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Location = new System.Drawing.Point(226, 27);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(96, 23);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.Text = "删除";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(428, 4);
            this.progressBar1.TabIndex = 4;
            // 
            // btnLocation
            // 
            this.btnLocation.Enabled = false;
            this.btnLocation.Location = new System.Drawing.Point(337, 27);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(96, 23);
            this.btnLocation.TabIndex = 5;
            this.btnLocation.Text = "定位";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(7, 18);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(428, 4);
            this.progressBar2.TabIndex = 6;
            // 
            // RegionalManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 316);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.treeView1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "RegionalManagement";
            this.Text = "支线管理";
            this.Load += new System.EventHandler(this.RegionalManagement_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RegionalManagement_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RegionalManagement_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}