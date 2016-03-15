namespace ElectronTransferView.ContextMenuManager
{
    partial class DFMCJLXG
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
            this.button_Change = new System.Windows.Forms.Button();
            this.button_Modify = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_DF_FID = new System.Windows.Forms.TextBox();
            this.textBox_DF_OLDNAME = new System.Windows.Forms.TextBox();
            this.textBox_DF_NEWNAME = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Change
            // 
            this.button_Change.Location = new System.Drawing.Point(285, 30);
            this.button_Change.Name = "button_Change";
            this.button_Change.Size = new System.Drawing.Size(65, 23);
            this.button_Change.TabIndex = 0;
            this.button_Change.Text = "选 择";
            this.button_Change.UseVisualStyleBackColor = true;
            this.button_Change.Click += new System.EventHandler(this.button_Change_Click);
            // 
            // button_Modify
            // 
            this.button_Modify.Enabled = false;
            this.button_Modify.Location = new System.Drawing.Point(214, 127);
            this.button_Modify.Name = "button_Modify";
            this.button_Modify.Size = new System.Drawing.Size(65, 23);
            this.button_Modify.TabIndex = 1;
            this.button_Modify.Text = "修 改";
            this.button_Modify.UseVisualStyleBackColor = true;
            this.button_Modify.Click += new System.EventHandler(this.button_Modify_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "G3E_FID：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "原名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "新名称：";
            // 
            // textBox_DF_FID
            // 
            this.textBox_DF_FID.Location = new System.Drawing.Point(74, 31);
            this.textBox_DF_FID.Name = "textBox_DF_FID";
            this.textBox_DF_FID.ReadOnly = true;
            this.textBox_DF_FID.Size = new System.Drawing.Size(205, 21);
            this.textBox_DF_FID.TabIndex = 5;
            // 
            // textBox_DF_OLDNAME
            // 
            this.textBox_DF_OLDNAME.Location = new System.Drawing.Point(74, 61);
            this.textBox_DF_OLDNAME.Name = "textBox_DF_OLDNAME";
            this.textBox_DF_OLDNAME.ReadOnly = true;
            this.textBox_DF_OLDNAME.Size = new System.Drawing.Size(205, 21);
            this.textBox_DF_OLDNAME.TabIndex = 6;
            // 
            // textBox_DF_NEWNAME
            // 
            this.textBox_DF_NEWNAME.Location = new System.Drawing.Point(74, 91);
            this.textBox_DF_NEWNAME.Name = "textBox_DF_NEWNAME";
            this.textBox_DF_NEWNAME.Size = new System.Drawing.Size(205, 21);
            this.textBox_DF_NEWNAME.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn_Close);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_DF_NEWNAME);
            this.groupBox1.Controls.Add(this.button_Change);
            this.groupBox1.Controls.Add(this.textBox_DF_OLDNAME);
            this.groupBox1.Controls.Add(this.button_Modify);
            this.groupBox1.Controls.Add(this.textBox_DF_FID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(5, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 158);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(285, 127);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(65, 23);
            this.Btn_Close.TabIndex = 8;
            this.Btn_Close.Text = "关 闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // DFMCJLXG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 172);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DFMCJLXG";
            this.Text = "电房名称级联修改";
            this.Load += new System.EventHandler(this.DFMCJLXG_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Change;
        private System.Windows.Forms.Button button_Modify;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_DF_FID;
        private System.Windows.Forms.TextBox textBox_DF_OLDNAME;
        private System.Windows.Forms.TextBox textBox_DF_NEWNAME;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn_Close;
    }
}