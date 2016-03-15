namespace ElectronTransferView.Upload
{
    partial class UploadCAD
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
            this.Btn_ImportCad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Btn_Select = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn_ExportCad = new System.Windows.Forms.Button();
            this.Btn_PreViewWebGis = new System.Windows.Forms.Button();
            this.Btn_Dxt = new System.Windows.Forms.Button();
            this.Txt_GCID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_ImportCad
            // 
            this.Btn_ImportCad.Location = new System.Drawing.Point(17, 136);
            this.Btn_ImportCad.Name = "Btn_ImportCad";
            this.Btn_ImportCad.Size = new System.Drawing.Size(408, 23);
            this.Btn_ImportCad.TabIndex = 1;
            this.Btn_ImportCad.Text = "导入沿布图";
            this.Btn_ImportCad.UseVisualStyleBackColor = true;
            this.Btn_ImportCad.Click += new System.EventHandler(this.Btn_Up_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Btn_Select
            // 
            this.Btn_Select.Location = new System.Drawing.Point(350, 54);
            this.Btn_Select.Name = "Btn_Select";
            this.Btn_Select.Size = new System.Drawing.Size(75, 23);
            this.Btn_Select.TabIndex = 1;
            this.Btn_Select.Text = "选择文件";
            this.Btn_Select.UseVisualStyleBackColor = true;
            this.Btn_Select.Click += new System.EventHandler(this.Btn_Select_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "文件路径：";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(74, 55);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(270, 21);
            this.textBox1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn_ExportCad);
            this.groupBox1.Controls.Add(this.Btn_PreViewWebGis);
            this.groupBox1.Controls.Add(this.Btn_Dxt);
            this.groupBox1.Controls.Add(this.Txt_GCID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Btn_ImportCad);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.Btn_Select);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 242);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // Btn_ExportCad
            // 
            this.Btn_ExportCad.Location = new System.Drawing.Point(17, 103);
            this.Btn_ExportCad.Name = "Btn_ExportCad";
            this.Btn_ExportCad.Size = new System.Drawing.Size(408, 23);
            this.Btn_ExportCad.TabIndex = 0;
            this.Btn_ExportCad.Text = "导出沿布图";
            this.Btn_ExportCad.UseVisualStyleBackColor = true;
            this.Btn_ExportCad.Click += new System.EventHandler(this.Btn_ExportCad_Click);
            // 
            // Btn_PreViewWebGis
            // 
            this.Btn_PreViewWebGis.Location = new System.Drawing.Point(17, 202);
            this.Btn_PreViewWebGis.Name = "Btn_PreViewWebGis";
            this.Btn_PreViewWebGis.Size = new System.Drawing.Size(408, 23);
            this.Btn_PreViewWebGis.TabIndex = 3;
            this.Btn_PreViewWebGis.Text = "预 览";
            this.Btn_PreViewWebGis.UseVisualStyleBackColor = true;
            this.Btn_PreViewWebGis.Click += new System.EventHandler(this.Btn_PreViewWebGis_Click);
            // 
            // Btn_Dxt
            // 
            this.Btn_Dxt.Location = new System.Drawing.Point(17, 169);
            this.Btn_Dxt.Name = "Btn_Dxt";
            this.Btn_Dxt.Size = new System.Drawing.Size(408, 23);
            this.Btn_Dxt.TabIndex = 2;
            this.Btn_Dxt.Text = "预生成单线图";
            this.Btn_Dxt.UseVisualStyleBackColor = true;
            this.Btn_Dxt.Click += new System.EventHandler(this.Btn_Dxt_Click);
            // 
            // Txt_GCID
            // 
            this.Txt_GCID.Location = new System.Drawing.Point(74, 27);
            this.Txt_GCID.Name = "Txt_GCID";
            this.Txt_GCID.ReadOnly = true;
            this.Txt_GCID.Size = new System.Drawing.Size(270, 21);
            this.Txt_GCID.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "工 程 ID：";
            // 
            // UploadCAD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 262);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadCAD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上传数据";
            this.Load += new System.EventHandler(this.UploadCAD_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_ImportCad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Btn_Select;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox Txt_GCID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_ExportCad;
        private System.Windows.Forms.Button Btn_PreViewWebGis;
        private System.Windows.Forms.Button Btn_Dxt;
    }
}