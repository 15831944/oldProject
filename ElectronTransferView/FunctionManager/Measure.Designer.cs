namespace ElectronTransferView.FunctionManager
{
    partial class Measure
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
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_clean = new System.Windows.Forms.Button();
            this.cb_txtSize = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selColor = new System.Windows.Forms.Button();
            this.curColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ComBox_Unit = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(17, 108);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 0;
            this.btn_ok.Text = "测 量";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_clean
            // 
            this.btn_clean.Location = new System.Drawing.Point(104, 108);
            this.btn_clean.Name = "btn_clean";
            this.btn_clean.Size = new System.Drawing.Size(75, 23);
            this.btn_clean.TabIndex = 1;
            this.btn_clean.Text = "撤 销";
            this.btn_clean.UseVisualStyleBackColor = true;
            this.btn_clean.Click += new System.EventHandler(this.btn_clean_Click);
            // 
            // cb_txtSize
            // 
            this.cb_txtSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_txtSize.FormattingEnabled = true;
            this.cb_txtSize.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.cb_txtSize.Location = new System.Drawing.Point(104, 21);
            this.cb_txtSize.Name = "cb_txtSize";
            this.cb_txtSize.Size = new System.Drawing.Size(75, 20);
            this.cb_txtSize.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "标注大小：";
            // 
            // selColor
            // 
            this.selColor.Location = new System.Drawing.Point(137, 75);
            this.selColor.Name = "selColor";
            this.selColor.Size = new System.Drawing.Size(42, 20);
            this.selColor.TabIndex = 6;
            this.selColor.Text = "选择";
            this.selColor.UseVisualStyleBackColor = true;
            this.selColor.Click += new System.EventHandler(this.selColor_Click);
            // 
            // curColor
            // 
            this.curColor.BackColor = System.Drawing.SystemColors.Control;
            this.curColor.Enabled = false;
            this.curColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.curColor.Location = new System.Drawing.Point(104, 78);
            this.curColor.Name = "curColor";
            this.curColor.Size = new System.Drawing.Size(27, 14);
            this.curColor.TabIndex = 5;
            this.curColor.TabStop = false;
            this.curColor.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "当前颜色：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ComBox_Unit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.selColor);
            this.groupBox1.Controls.Add(this.btn_ok);
            this.groupBox1.Controls.Add(this.curColor);
            this.groupBox1.Controls.Add(this.btn_clean);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cb_txtSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 140);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "测量单位：";
            // 
            // ComBox_Unit
            // 
            this.ComBox_Unit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComBox_Unit.FormattingEnabled = true;
            this.ComBox_Unit.Items.AddRange(new object[] {
            "米",
            "千米"});
            this.ComBox_Unit.Location = new System.Drawing.Point(104, 49);
            this.ComBox_Unit.Name = "ComBox_Unit";
            this.ComBox_Unit.Size = new System.Drawing.Size(75, 20);
            this.ComBox_Unit.TabIndex = 8;
            // 
            // Measure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 154);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Measure";
            this.Text = "测量距离";
            this.Load += new System.EventHandler(this.Measure_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Measure_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Measure_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_clean;
        private System.Windows.Forms.ComboBox cb_txtSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selColor;
        private System.Windows.Forms.Button curColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ComBox_Unit;
        private System.Windows.Forms.Label label3;
    }
}