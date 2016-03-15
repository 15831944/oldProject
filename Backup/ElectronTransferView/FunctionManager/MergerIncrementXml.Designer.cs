namespace ElectronTransferView.FunctionManager
{
    partial class MergerIncrementXml
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnMerger = new System.Windows.Forms.Button();
            this.Btn_Select = new System.Windows.Forms.Button();
            this.Txt_ZlXml = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatus_MergerXml = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStatusTip = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnMerger);
            this.groupBox1.Controls.Add(this.Btn_Select);
            this.groupBox1.Controls.Add(this.Txt_ZlXml);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // BtnMerger
            // 
            this.BtnMerger.Location = new System.Drawing.Point(289, 65);
            this.BtnMerger.Name = "BtnMerger";
            this.BtnMerger.Size = new System.Drawing.Size(75, 23);
            this.BtnMerger.TabIndex = 3;
            this.BtnMerger.Text = "合 并";
            this.BtnMerger.UseVisualStyleBackColor = true;
            this.BtnMerger.Click += new System.EventHandler(this.BtnMerger_Click);
            // 
            // Btn_Select
            // 
            this.Btn_Select.Location = new System.Drawing.Point(329, 27);
            this.Btn_Select.Name = "Btn_Select";
            this.Btn_Select.Size = new System.Drawing.Size(35, 23);
            this.Btn_Select.TabIndex = 2;
            this.Btn_Select.Text = "…";
            this.Btn_Select.UseVisualStyleBackColor = true;
            this.Btn_Select.Click += new System.EventHandler(this.Btn_Select_Click);
            // 
            // Txt_ZlXml
            // 
            this.Txt_ZlXml.Location = new System.Drawing.Point(68, 28);
            this.Txt_ZlXml.Name = "Txt_ZlXml";
            this.Txt_ZlXml.ReadOnly = true;
            this.Txt_ZlXml.Size = new System.Drawing.Size(255, 21);
            this.Txt_ZlXml.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "CAD增量：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatus_MergerXml,
            this.ToolStatusTip});
            this.statusStrip1.Location = new System.Drawing.Point(0, 98);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(376, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripStatus_MergerXml
            // 
            this.ToolStripStatus_MergerXml.Name = "ToolStripStatus_MergerXml";
            this.ToolStripStatus_MergerXml.Size = new System.Drawing.Size(0, 17);
            // 
            // ToolStatusTip
            // 
            this.ToolStatusTip.Name = "ToolStatusTip";
            this.ToolStatusTip.Size = new System.Drawing.Size(0, 17);
            // 
            // MergerIncrementXml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 120);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MergerIncrementXml";
            this.Text = "数据合并";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn_Select;
        private System.Windows.Forms.TextBox Txt_ZlXml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnMerger;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatus_MergerXml;
        private System.Windows.Forms.ToolStripStatusLabel ToolStatusTip;
    }
}