namespace ElectronTransferView.ContextMenuManager
{
    partial class ZXLabelForm
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn_Select = new System.Windows.Forms.Button();
            this.TXT_DYSBFID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Btn_Add = new System.Windows.Forms.Button();
            this.TXT_BZNR = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ComBox_BZLX = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ComBox_SSDW = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn_Select);
            this.groupBox1.Controls.Add(this.TXT_DYSBFID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Btn_Add);
            this.groupBox1.Controls.Add(this.TXT_BZNR);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ComBox_BZLX);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ComBox_SSDW);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 195);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // Btn_Select
            // 
            this.Btn_Select.AutoSize = true;
            this.Btn_Select.Location = new System.Drawing.Point(256, 22);
            this.Btn_Select.Name = "Btn_Select";
            this.Btn_Select.Size = new System.Drawing.Size(41, 23);
            this.Btn_Select.TabIndex = 9;
            this.Btn_Select.Text = "…";
            this.Btn_Select.UseVisualStyleBackColor = true;
            this.Btn_Select.Click += new System.EventHandler(this.Btn_Select_Click);
            // 
            // TXT_DYSBFID
            // 
            this.TXT_DYSBFID.Location = new System.Drawing.Point(75, 23);
            this.TXT_DYSBFID.Name = "TXT_DYSBFID";
            this.TXT_DYSBFID.ReadOnly = true;
            this.TXT_DYSBFID.Size = new System.Drawing.Size(178, 21);
            this.TXT_DYSBFID.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "父设备：";
            // 
            // Btn_Add
            // 
            this.Btn_Add.AutoSize = true;
            this.Btn_Add.Location = new System.Drawing.Point(217, 163);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(80, 23);
            this.Btn_Add.TabIndex = 6;
            this.Btn_Add.Tag = "add";
            this.Btn_Add.Text = "添 加(&A)";
            this.Btn_Add.UseVisualStyleBackColor = true;
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // TXT_BZNR
            // 
            this.TXT_BZNR.AcceptsReturn = true;
            this.TXT_BZNR.AcceptsTab = true;
            this.TXT_BZNR.Location = new System.Drawing.Point(75, 100);
            this.TXT_BZNR.Multiline = true;
            this.TXT_BZNR.Name = "TXT_BZNR";
            this.TXT_BZNR.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TXT_BZNR.Size = new System.Drawing.Size(222, 57);
            this.TXT_BZNR.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "标注内容：";
            // 
            // ComBox_BZLX
            // 
            this.ComBox_BZLX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComBox_BZLX.FormattingEnabled = true;
            this.ComBox_BZLX.Location = new System.Drawing.Point(75, 74);
            this.ComBox_BZLX.Name = "ComBox_BZLX";
            this.ComBox_BZLX.Size = new System.Drawing.Size(222, 20);
            this.ComBox_BZLX.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "标注类型：";
            // 
            // ComBox_SSDW
            // 
            this.ComBox_SSDW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComBox_SSDW.FormattingEnabled = true;
            this.ComBox_SSDW.Location = new System.Drawing.Point(75, 49);
            this.ComBox_SSDW.Name = "ComBox_SSDW";
            this.ComBox_SSDW.Size = new System.Drawing.Size(222, 20);
            this.ComBox_SSDW.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "所属单位：";
            // 
            // ZXLabelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ZXLabelForm";
            this.Size = new System.Drawing.Size(309, 201);
            this.Load += new System.EventHandler(this.ZXLabelForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TXT_BZNR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ComBox_BZLX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComBox_SSDW;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_Add;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TXT_DYSBFID;
        private System.Windows.Forms.Button Btn_Select;
    }
}
