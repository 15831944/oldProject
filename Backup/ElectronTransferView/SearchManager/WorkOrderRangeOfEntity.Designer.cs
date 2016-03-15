namespace ElectronTransferView.SearchManager
{
    partial class WorkOrderRangeOfEntity
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
            this.Btn_Analysis = new System.Windows.Forms.Button();
            this.Btn_ClearColor = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvFeatureInLtt = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_Analysis
            // 
            this.Btn_Analysis.Location = new System.Drawing.Point(6, 20);
            this.Btn_Analysis.Name = "Btn_Analysis";
            this.Btn_Analysis.Size = new System.Drawing.Size(154, 23);
            this.Btn_Analysis.TabIndex = 1;
            this.Btn_Analysis.Text = "查看编辑设备";
            this.Btn_Analysis.UseVisualStyleBackColor = true;
            this.Btn_Analysis.Click += new System.EventHandler(this.Btn_Analysis_Click);
            // 
            // Btn_ClearColor
            // 
            this.Btn_ClearColor.Location = new System.Drawing.Point(186, 20);
            this.Btn_ClearColor.Name = "Btn_ClearColor";
            this.Btn_ClearColor.Size = new System.Drawing.Size(154, 23);
            this.Btn_ClearColor.TabIndex = 2;
            this.Btn_ClearColor.Text = "还原颜色";
            this.Btn_ClearColor.UseVisualStyleBackColor = true;
            this.Btn_ClearColor.Click += new System.EventHandler(this.Btn_ClearColor_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lvFeatureInLtt);
            this.groupBox1.Location = new System.Drawing.Point(5, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(634, 442);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工单锁定设备";
            // 
            // lvFeatureInLtt
            // 
            this.lvFeatureInLtt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFeatureInLtt.FullRowSelect = true;
            this.lvFeatureInLtt.Location = new System.Drawing.Point(3, 17);
            this.lvFeatureInLtt.MultiSelect = false;
            this.lvFeatureInLtt.Name = "lvFeatureInLtt";
            this.lvFeatureInLtt.Size = new System.Drawing.Size(628, 422);
            this.lvFeatureInLtt.TabIndex = 1;
            this.lvFeatureInLtt.UseCompatibleStateImageBehavior = false;
            this.lvFeatureInLtt.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFeatureInLtt_MouseDoubleClick);
            this.lvFeatureInLtt.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFeatureInLtt_ColumnClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.Btn_Analysis);
            this.groupBox2.Controls.Add(this.Btn_ClearColor);
            this.groupBox2.Location = new System.Drawing.Point(5, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(634, 55);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "编辑设备";
            // 
            // WorkOrderRangeOfEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 520);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WorkOrderRangeOfEntity";
            this.Text = "锁定设备列表";
            this.Load += new System.EventHandler(this.WorkOrderRangeOfEntity_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkOrderRangeOfEntity_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WorkOrderRangeOfEntity_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_Analysis;
        private System.Windows.Forms.Button Btn_ClearColor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvFeatureInLtt;
    }
}